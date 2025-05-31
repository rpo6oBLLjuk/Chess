using Cysharp.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using Zenject;

public class SignInController
{
    [Inject] DBService dbService;
    [Inject] NotificationService notificationService;


    public async UniTask<(bool success, string nickname, int playerId)> SignInAsync(string login, string password)
    {
        string log = string.Empty;
        PopupType popupType = PopupType.None;

        // Проверка входных данных
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            notificationService.ShowPopup("Логин и пароль обязательны", "Sing In", PopupType.Info);
            return (false, null, -1);
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
                        log = "Пользователь не найден";
                        popupType = PopupType.Warning;
                        return (false, null, -1);
                    }

                    await reader.ReadAsync();

                    string storedHash = reader.GetString("password_hash");
                    bool isPasswordValid = VerifyPassword(password, storedHash);

                    if (!isPasswordValid)
                    {
                        log = "Неверный пароль";
                        popupType = PopupType.Warning;
                        return (false, null, -1);
                    }

                    string nickname = reader.GetString("nickname");
                    int playerId = reader.GetInt32("player_id");
                    return (true, nickname, playerId);
                }
            }
        }
        catch (MySqlException ex)
        {
            log = ex.Message;
            popupType = PopupType.Error;
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

    private bool VerifyPassword(string inputPassword, string storedHash) => storedHash == dbService.HashPassword(inputPassword);
}
