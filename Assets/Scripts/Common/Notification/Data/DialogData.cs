using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Scriptable Objects/Notification/DialogData")]
public class DialogData : ScriptableObject
{
    [Header("References")]
    [field: SerializeField] public GameObject OkCancelDialog { get; private set; }

    [Header("Values")]
    [field: SerializeField] public AnimatedPanelData AnimatedPanelData { get; private set; }
}
