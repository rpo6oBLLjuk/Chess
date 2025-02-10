using UnityEngine;

[CreateAssetMenu(fileName = "PopupData", menuName = "Scriptable Objects/UI/PopupData")]
public class PopupData : ScriptableObject
{
    [Header("References")]
    public GameObject popup;
    public Sprite infoSprite;
    public Color infoColor = Color.white;
    public Sprite warningSprite;
    public Color warningColor = Color.yellow;
    public Sprite errorSprite;
    public Color errorColor = Color.red;

    [Header("Values")]
    public float duration = 1f;
    public float showTime = 0.25f;
    public float hideTime = 0.25f;

    [Header("Other settings")]
    public bool horizontalScale = true;
    public bool verticalScale = false;

    [Header("PopupList")]
    public bool useList;
    [Tooltip("Only if useList"), Min(2)] public int listSize = 3;
}
