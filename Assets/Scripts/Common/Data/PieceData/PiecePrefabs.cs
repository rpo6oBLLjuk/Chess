using UnityEngine;

[CreateAssetMenu(fileName = "PiecePrefabs", menuName = "Scriptable Objects/Piece/Prefabs")]
public class PiecePrefabs : ScriptableObject
{
    [field: SerializeField]
    public GameObject Piece { get; private set; }
}
