using Cysharp.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using Zenject;

public class DBService : MonoService
{
    [Inject] NotificationService notificationService;

    [field: SerializeField] public DBMainData Data { get; private set; }

    [SerializeField] public DBDataSaver dBDataSaver;

    public SignInController SignInController { get; private set; }
    public SignUpController SignUpController { get; private set; }

    public DBDataSaver DBDataSaver { get; private set; }


    public override void Initialize()
    {
        SignInController = container.Instantiate<SignInController>();
        SignUpController = container.Instantiate<SignUpController>();

        Data.SetUserData(dBDataSaver.Load());
    }

    public async UniTask<MySqlConnection> GetConnectionAsync()
    {
        await UniTask.SwitchToThreadPool();
        MySqlConnection connection = new MySqlConnection(Data.ConnectionString);
        try
        {
            // Асинхронное подключение с таймаутом
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(Data.timeoutSeconds));
            await connection.OpenAsync(cts.Token);

            return connection;
        }
        catch (MySqlException mySqlEx)
        {
            string errorMessage = mySqlEx.Number switch
            {
                1042 => "Сервер MySQL недоступен",
                1045 => "Неверный логин или пароль",
                _ => mySqlEx.Message
            };
            notificationService.ShowPopup(errorMessage, "Connection error", PopupType.Error);
            return null;
        }
    }

    public async UniTask<(bool success, MySqlConnection connection)> TryGetConnection()
    {
        var connection = await GetConnectionAsync();
        return (connection != null, connection);
    }

    public string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2")); // hex format
            }
            return builder.ToString();
        }
    }

    public void SaveData(string nickname, int playerId)
    {
        Data.SetUserData(nickname, playerId);
        dBDataSaver.Save(new(nickname, playerId));
    }
}
