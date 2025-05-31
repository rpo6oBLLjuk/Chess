using Cysharp.Threading.Tasks;
using MySql.Data.MySqlClient;
//using MySqlConnector; // Используем MySqlConnector вместо MySql.Data
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
    public DBDataSaver DBDataSaver => dBDataSaver;

    //private MySqlConnectionStringBuilder _connectionBuilder;

    public override void Initialize()
    {
        SignInController = container.Instantiate<SignInController>();
        SignUpController = container.Instantiate<SignUpController>();

        //_connectionBuilder = new MySqlConnectionStringBuilder(Data.ConnectionString)
        //{
        //    // Оптимальные настройки для Android
        //    Pooling = false, // Пулинг может вызывать проблемы на мобильных устройствах
        //    AllowUserVariables = true,
        //    ConnectionTimeout = (uint)Data.timeoutSeconds,
        //    SslMode = MySqlSslMode.Disabled // Для Android лучше отключать SSL
        //};

        Data.SetUserData(dBDataSaver.Load());
    }

    public async UniTask<MySqlConnection> GetConnectionAsync()
    {
        var connection = new MySqlConnection(Data.ConnectionString);

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

    private void HandleMySqlError(MySqlException ex)
    {
        string errorMessage = ex.ErrorCode switch
        {
            //MySqlErrorCode.UnableToConnectToHost => "Сервер MySQL недоступен",
            //MySqlErrorCode.AccessDenied => "Неверный логин или пароль",
            _ => ex.Message
        };

        notificationService.ShowPopup(errorMessage, "Database Error", PopupType.Error);
    }

    public string HashPassword(string password)
    {
        // Более безопасный вариант с солью
        using var sha256 = SHA256.Create();
        byte[] salt = Encoding.UTF8.GetBytes("your_salt_here"); // Замените на уникальную соль
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));

        byte[] hash = sha256.ComputeHash(passwordBytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public void SaveData(string nickname, int playerId)
    {
        Data.SetUserData(nickname, playerId);
        dBDataSaver.Save(new SerializableDBData(nickname, playerId));
    }
}