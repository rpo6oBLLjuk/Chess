using Cysharp.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using Zenject;

public class SignInController
{
    [Inject] DBService dbService;
    [Inject] NotificationService notificationService;


    public async UniTask<(bool success, string nickname, int playerId)> SignInAsync(string login, string password)
    {
        int playerId = -1;
        string nickname = string.Empty;

        // Проверка входных данных
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            notificationService.ShowPopup("Логин и пароль обязательны", "SignIn Error", PopupType.Info);
            return  (false, null, -1);
        }

        var (success, connection) = await dbService.TryGetConnection();
        if (!success)
            return (false, null, -1);

        try
        {
            string query = "SELECT player_id, password_hash, nickname FROM players WHERE login = @username LIMIT 1";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", login);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        notificationService.ShowPopup("Пользователь не найден", "SignIn Error", PopupType.Error);
                        return (false, null, -1);
                    }

                    await reader.ReadAsync();

                    string storedHash = reader.GetString("password_hash");
                    bool isPasswordValid = VerifyPassword(password, storedHash);

                    if (!isPasswordValid)
                    {
                        notificationService.ShowPopup("Неверный пароль", "SignIn Error", PopupType.Warning);
                        return (false, null, -1);
                    }

                    nickname = reader.GetString("nickname");
                    playerId = reader.GetInt32("player_id");

                    return (true, nickname, playerId);
                }
            }
        }
        catch (MySqlException ex)
        {
            notificationService.ShowPopup(ex.Message, "SignIn Error", PopupType.Error);
            return (false, null, -1);
        }
        finally
        {
            connection?.Close();
            await UniTask.SwitchToMainThread();
        }
    }

    private bool VerifyPassword(string inputPassword, string storedHash) => storedHash == dbService.HashPassword(inputPassword);
}
