using Cysharp.Threading.Tasks;
using MySqlConnector;
using System;
using UnityEngine;
using Zenject;

public class SignUpController
{
    [Inject] DBService dbService;
    [Inject] NotificationService notificationService;


    public async UniTask<bool> SignUpAsync(string login, string password, string nickname)
    {
        string log = string.Empty;
        PopupType popupType = PopupType.None;

        int playerId = -1;
        string passwordHash = "";

        // Валидация входных данных
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nickname))
        {
            notificationService.ShowPopup("Все поля должны быть заполнены!", "SignUp", PopupType.Info);
            return false;
        }

        var (success, connection) = await dbService.TryGetConnection();

        if (!success)
            return false;

        try
        {
            if (IsUserExists(connection, login, nickname))
            {
                log = "Логин или никнейм уже заняты!";
                popupType = PopupType.Warning;
                return false;
            }

            passwordHash = dbService.HashPassword(password);

            // Разделяем запросы и явно получаем ID
            string insertPlayerQuery = @"
                INSERT INTO players (login, password_hash, nickname)
                VALUES (@login, @passwordHash, @nickname);
                SELECT LAST_INSERT_ID();";

            playerId = -1;

            using (var cmd = new MySqlCommand(insertPlayerQuery, connection))
            {
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                cmd.Parameters.AddWithValue("@nickname", nickname);

                playerId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            if (playerId <= 0)
            {
                Debug.LogError("Не удалось получить ID нового игрока");
                return false;
            }

            string statsQuery = "INSERT INTO player_stats (player_id) VALUES (@playerId);";
            using (var cmd = new MySqlCommand(statsQuery, connection))
            {
                cmd.Parameters.AddWithValue("@playerId", playerId);
                await cmd.ExecuteNonQueryAsync();
            }

            Debug.Log($"Игрок {nickname} (ID: {playerId}) успешно зарегистрирован!");
            await dbService.UpdateLastLogin(playerId);
            return true;
        }
        catch (MySqlException ex)
        {
            notificationService.ShowPopup(ex.Message, "SignUp Error", PopupType.Error);
            return false;
        }
        finally
        {
            connection?.Close();
            await UniTask.SwitchToMainThread();

            if (popupType != PopupType.None)
                notificationService.ShowPopup(log, "Sing In", popupType);
            else
                dbService.SaveData(playerId, nickname, passwordHash);
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
