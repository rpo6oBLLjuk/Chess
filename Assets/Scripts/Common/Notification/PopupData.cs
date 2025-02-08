using UnityEngine;

[CreateAssetMenu(fileName = "PopupData", menuName = "Scriptable Objects/PopupData")]
public class PopupData : ScriptableObject
{
    [Header("References")]
    public GameObject popup;
    public Sprite infoSprite;
    public Color infoColor = Color.white;
    public Sprite warningSprite;
    public Color warningColor = Color.white;
    public Sprite errorSprite;
    public Color errorColor = Color.white;

    [Header("Values")]
    public float duration = 2f;
    public float showTime = 0.25f;
    public float hideTime = 0.25f;

    [Header("Other settings")]
    public bool horizontalScale = true;
    public bool verticalScale = false;

    [Header("PopupList")]
    public bool useList;
    [Tooltip("Only if useList")] public int listSize = 3;
}
