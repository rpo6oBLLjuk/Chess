public class DBDataSaver : OneFileBaseSaver<SerializableDBData>
{
    public override void Save(SerializableDBData obj)
    {
        var hashedData = new SerializableDBData(
            XorEncrypt(obj.PlayerId),
            obj.Username,
            obj.PasswordHash
        );

        base.Save(hashedData);
    }

    public override SerializableDBData Load()
    {
        SerializableDBData loadedData = base.Load();

        return new SerializableDBData(
            XorEncrypt(loadedData.PlayerId),
            loadedData.Username,
            loadedData.PasswordHash
        );
    }

    private int XorEncrypt(int value, uint key = 0xDEADBEEF) => value ^ unchecked((int)key);
}

public struct SerializableDBData
{
    public int PlayerId;
    public string Username;
    public string PasswordHash;

    public SerializableDBData(int playerId, string username, string passwordHash)
    {
        this.PlayerId = playerId;
        this.Username = username;
        this.PasswordHash = passwordHash;
    }
}