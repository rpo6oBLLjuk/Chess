using Cysharp.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using UnityEngine;
using Zenject;

public class SignUpController
{
    [Inject] DBService dbService;
    [Inject] NotificationService notificationService;


    public async UniTask<(bool success, string nickname, int playerId)> SignUpAsync(string login, string password, string nickname)
    {
        string log = string.Empty;
        PopupType popupType = PopupType.None;

        // Валидация входных данных
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nickname))
        {
            notificationService.ShowPopup("Все поля должны быть заполнены!", "SignUp", PopupType.Info);
            return (false, null, -1);
        }

        var (success, connection) = await dbService.TryGetConnection();

        if (!success)
            return (false, null, -1);

        try
        {
            if (IsUserExists(connection, login, nickname))
            {
                log = "Логин или никнейм уже заняты!";
                popupType = PopupType.Warning;
                return (false, null, -1);
            }

            string passwordHash = dbService.HashPassword(password);

            // Разделяем запросы и явно получаем ID
            string insertPlayerQuery = @"
                INSERT INTO players (login, password_hash, nickname)
                VALUES (@login, @passwordHash, @nickname);
                SELECT LAST_INSERT_ID();";

            int playerId = -1;

            using (var cmd = new MySqlCommand(insertPlayerQuery, connection))
            {
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                cmd.Parameters.AddWithValue("@nickname", nickname);

                // Явно получаем ID нового игрока
                playerId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            if (playerId <= 0)
            {
                Debug.LogError("Не удалось получить ID нового игрока");
                return (false, null, -1);
            }

            // Добавляем запись в статистику
            string statsQuery = "INSERT INTO player_stats (player_id) VALUES (@playerId);";
            using (var cmd = new MySqlCommand(statsQuery, connection))
            {
                cmd.Parameters.AddWithValue("@playerId", playerId);
                await cmd.ExecuteNonQueryAsync();
            }

            Debug.Log($"Игрок {nickname} (ID: {playerId}) успешно зарегистрирован!");
            return (true, nickname, playerId);
        }
        catch (MySqlException ex)
        {
            notificationService.ShowPopup(ex.Message, "SignUp Error", PopupType.Error);
            return (false, null, -1);
        }
        finally
        {
            connection?.Close();
            await UniTask.SwitchToMainThread();

            if (popupType != PopupType.None)
            {
                notificationService.ShowPopup(log, "Sing In", popupType);
            }
        }
    }


    private bool IsUserExists(MySqlConnection conn, string login, string nickname)
    {
        string query = "SELECT COUNT(*) FROM players WHERE login = @login OR nickname = @nickname";

        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@nickname", nickname);

            long count = (long)cmd.ExecuteScalar();

            if (count > 0)
            {
                return true;
            }
        }
        return false;
    }
}
