using UnityEngine;

[CreateAssetMenu(fileName = "DBMainData", menuName = "Scriptable Objects/DB/MainData")]
public class DBMainData : ScriptableObject
{
    [field: SerializeField] public string Username { get; private set; }
    [field: SerializeField] public int PlayerId { get; private set; }

    [field: SerializeField, TextArea(1, 5)] public string ConnectionString { get; private set; }

    [field: SerializeField] public float timeoutSeconds = 2f;

    //public string ConnectionString => $"server={Server};database={Database};user={Username};password={Password};";


    public void SetUserData(string username, int playerId)
    {
        Username = username;
        PlayerId = playerId;
    }
    public void SetUserData(SerializableDBData data)
    {
        Username = data.Username;
        PlayerId = data.PlayerId;
    }
}
