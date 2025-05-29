public class DBDataSaver : OneFileBaseSaver<SerializableDBData>
{

}

public struct SerializableDBData
{
    public string Username;
    public int PlayerId;

    public SerializableDBData(string nickname, int playerId)
    {
        this.Username = nickname;
        this.PlayerId = playerId;
    }
}