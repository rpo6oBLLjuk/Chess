using CustomInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupData", menuName = "Scriptable Objects/Notification/PopupData")]
public class PopupData : ScriptableObject
{
    [Serializable]
    public class PopupTypeData
    {
        public Sprite sprite;
        public Color color;
    }

    [Header("References")]
    public GameObject popup;

    [Tab("Data")] public PopupTypeData Info;
    [Tab("Data")] public PopupTypeData Warning;
    [Tab("Data")] public PopupTypeData Error;

    [Tab("Values")] public float duration = 1f;
    [Tab("Values")] public float showTime = 0.25f;
    [Tab("Values")] public float hideTime = 0.25f;

    [Tab("Other settings")] public bool horizontalScale = true;
    [Tab("Other settings")] public bool verticalScale = false;

    [Tab("PopupList")] public bool useList;
    [Tab("PopupList"), ShowIf(nameof(useList)), Min(2)] public int listSize = 3;
}
