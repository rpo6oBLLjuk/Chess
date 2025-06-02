using Cysharp.Threading.Tasks;
using ModestTree;
using MySqlConnector;
using UnityEngine;
using Zenject;

public class AutoAuthController
{
    [Inject] private DBService dbService;
    [Inject] private NotificationService notificationService;


    public async UniTask<bool> TryAutoLogin()
    {
        if (dbService.Data.PlayerId < 0)
        {
            return false;
        }

        var (dbSuccess, connection) = await dbService.TryGetConnection();
        if (!dbSuccess)
            return false;

        try
        {
            await using (connection)
            {
                string query = "SELECT player_id, nickname FROM players WHERE player_id = @playerId AND password_hash = @passwordHash LIMIT 1";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@playerId", dbService.Data.PlayerId);
                    cmd.Parameters.AddWithValue("@passwordHash", dbService.Data.PasswordHash);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            await dbService.UpdateLastLogin(dbService.Data.PlayerId);
                            return true;
                        }
                    }
                }
            }
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
        }


        return false;
    }
}
