using DG.Tweening;
using UnityEngine;

public static class DOTweenExtensions
{
    public static void DOKillAllTweens(this Transform parent)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
            child.DOKill();
    }
}
