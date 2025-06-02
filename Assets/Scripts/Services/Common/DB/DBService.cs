using Cysharp.Threading.Tasks;
using MySqlConnector; // Используем MySqlConnector вместо MySql.Data
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using Zenject;

public class DBService : MonoService
{
    [Inject] private NotificationService notificationService;

    [field: SerializeField] public DBMainData Data { get; private set; }
    [SerializeField] private DBDataSaver dBDataSaver;

    public SignInController SignInController { get; private set; }
    public SignUpController SignUpController { get; private set; }
    public AutoAuthController AutoAuthController { get; private set; }

    public AnalyticsController AnalyticsController { get; private set; }


    public override void Initialize()
    {
        SignInController = container.Instantiate<SignInController>();
        SignUpController = container.Instantiate<SignUpController>();
        AutoAuthController = container.Instantiate<AutoAuthController>();

        AnalyticsController = container.Instantiate<AnalyticsController>();

        Data.SetUserData(dBDataSaver.Load());
    }

    public async UniTask<MySqlConnection> GetConnectionAsync()
    {
        var connection = new MySqlConnection(Data.ConnectionString);

        await UniTask.SwitchToThreadPool();
        try
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(Data.timeoutSeconds));
            await connection.OpenAsync(cts.Token).ConfigureAwait(false);
            return connection;
        }
        catch (MySqlException ex)
        {
            connection.Dispose();
            HandleMySqlError(ex);
            return null;
        }
        catch (Exception ex)
        {
            connection.Dispose();
            notificationService.ShowPopup($"Connection error: {ex.Message}", "Error", PopupType.Error);
            return null;
        }
    }
    public async UniTask<(bool success, MySqlConnection connection)> TryGetConnection()
    {
        var connection = await GetConnectionAsync();
        return (connection != null, connection);
    }

    public async UniTask UpdateLastLogin(int playerId)
    {
        var (success, connection) = await TryGetConnection();
        if (!success)
            return;

        await using (connection)
        {
            string query = "UPDATE players SET last_login_date = NOW() WHERE player_id = @playerId";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@playerId", playerId);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] salt = Encoding.UTF8.GetBytes("your_salt_here");
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));

        byte[] hash = sha256.ComputeHash(passwordBytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public void SaveData(int playerId, string nickname, string passwordHash)
    {
        Data.SetUserData(playerId, nickname, passwordHash);
        dBDataSaver.Save(new SerializableDBData(playerId, nickname, passwordHash));
    }

    public void LogOut()
    {
        dBDataSaver.DeleteSave();
        Data.SetUserData(-1, "", "");
    }

    private void HandleMySqlError(MySqlException ex)
    {
        string errorMessage = ex.ErrorCode switch
        {
            MySqlErrorCode.UnableToConnectToHost => "Сервер MySQL недоступен",
            MySqlErrorCode.AccessDenied => "Неверный логин или пароль",
            _ => ex.Message
        };

        notificationService.ShowPopup(errorMessage, "Database Error", PopupType.Error);
    }
}