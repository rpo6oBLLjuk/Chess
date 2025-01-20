using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnimatedWidgetElement : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    [Serializable]
    private class AnimatedWidgetElementData
    {

        public Vector2 animStartPosition;
        public Quaternion animStartRotation;
        public Vector3 animStartScale;
        public Ease animStartEaseType;

        public Vector2 animEndPosition;
        public Quaternion animEndRotation;
        public Vector3 animEndScale;
        public Ease animEndEaseType;

        [HideInInspector] public Vector2 basePosition;
        [HideInInspector] public Quaternion baseRotation;
        [HideInInspector] public Vector3 baseScale = Vector3.one;
    }
    [SerializeField] private AnimatedWidgetElementData data;


    public void Initialize()
    {
        rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();

        data.basePosition = rectTransform.anchoredPosition;
        data.baseRotation = rectTransform.localRotation;
        data.baseScale = rectTransform.localScale;
    }

    public void Show(float showDuration)
    {
        PlayAnim(rectTransform,
                data.basePosition,
                data.animStartPosition,
                data.baseRotation,
                data.animStartRotation,
                data.baseScale,
                data.animStartScale,
                data.animStartEaseType,
                showDuration);
    }
    public void Hide(float hideDuration)
    {
        PlayAnim(rectTransform,
                data.animEndPosition,
                data.basePosition,
                data.animEndRotation,
                data.baseRotation,
                data.animEndScale,
                data.baseScale,
                data.animEndEaseType,
                hideDuration);
    }

    private void PlayAnim(RectTransform rectTransform, Vector2 position, Vector2 fromPosition, Quaternion rotation, Quaternion fromRotation, Vector3 scale, Vector3 fromScale, Ease easeType, float duration)
    {
        Sequence tween = DOTween.Sequence();

        tween.Join(
            rectTransform.DOAnchorPos(position, duration)
                .From(fromPosition)
                .SetEase(Ease.OutQuad)
        );

        tween.Join(
            rectTransform.DORotateQuaternion(rotation, duration)
                .From(fromRotation)
                .SetEase(Ease.OutQuad)
        );

        tween.Join(
            rectTransform.DOScale(scale, duration)
                .From(fromScale)
                .SetEase(Ease.OutQuad)
        );

        tween.Play();
    }

    private void Reset() => rectTransform = GetComponent<RectTransform>();
    private void OnDrawGizmos()
    {
        if (rectTransform == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(rectTransform.anchoredPosition + data.animStartPosition, rectTransform.rect.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rectTransform.anchoredPosition + data.animEndPosition, rectTransform.rect.size);
    }
}
