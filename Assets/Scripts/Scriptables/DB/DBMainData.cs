using CustomInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DBMainData", menuName = "Scriptable Objects/DB/MainData")]
public class DBMainData : ScriptableObject
{
    [field: SerializeField, ReadOnly] public string Username { get; private set; }
    [field: SerializeField, ReadOnly] public string Password { get; private set; }

    [field: SerializeField, TextArea(1, 5)] public string ConnectionString { get; private set; }

    //public string ConnectionString => $"server={Server};database={Database};user={Username};password={Password};";


    public void SetUserData(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
