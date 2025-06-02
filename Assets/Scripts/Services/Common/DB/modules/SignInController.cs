using Cysharp.Threading.Tasks;
using MySqlConnector;
using Zenject;

public class SignInController
{
    [Inject] DBService dbService;
    [Inject] NotificationService notificationService;


    public async UniTask<bool> SignInAsync(string login, string password)
    {
        string log = string.Empty;
        PopupType popupType = PopupType.None;

        int playerId = -1;
        string nickname = "";
        string passwordHash = "";

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            notificationService.ShowPopup("Логин и пароль обязательны", "Sing In", PopupType.Info);
            return false;
        }

        var (success, connection) = await dbService.TryGetConnection();
        if (!success)
            return false;

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
                        return false;
                    }

                    await reader.ReadAsync();

                    string storedHash = reader.GetString("password_hash");
                    bool isPasswordValid = VerifyPassword(password, storedHash);

                    if (!isPasswordValid)
                    {
                        log = "Неверный пароль";
                        popupType = PopupType.Warning;
                        return false;
                    }
                    else
                    {
                        passwordHash = dbService.HashPassword(password);
                    }

                    nickname = reader.GetString("nickname");
                    playerId = reader.GetInt32("player_id");

                    await dbService.UpdateLastLogin(playerId);
                    return true;
                }
            }
        }
        catch (MySqlException ex)
        {
            log = ex.Message;
            popupType = PopupType.Error;
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

    private bool VerifyPassword(string inputPassword, string storedHash) => storedHash == dbService.HashPassword(inputPassword);
}
