using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnimatedElement : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    [Serializable]
    private class AnimationData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale = Vector3.one;

        public Ease easeType = Ease.OutQuad;

        public float delay;
    }
    [SerializeField] private AnimationData animStartData;
    [SerializeField] private AnimationData animEndData;

    [HideInInspector] private AnimationData defaultData = new();


    public void Initialize()
    {
        rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();

        defaultData.position = rectTransform.position;
        defaultData.rotation = rectTransform.localRotation;
        defaultData.scale = rectTransform.localScale;
    }

    public virtual Tween Show(float showDuration = 0, float delay = 0, bool forceShow = false)
    {
        return PlayAnim(rectTransform,
                defaultData.position,
                defaultData.position + animStartData.position,
                defaultData.rotation,
                animStartData.rotation,
                defaultData.scale,
                animStartData.scale,
                animStartData.easeType,
                showDuration,
                (forceShow) ? 0 : animStartData.delay + delay);
    }
    public virtual Tween Hide(float hideDuration = 0, float delay = 0, bool forceHide = false)
    {
        return PlayAnim(rectTransform,
                defaultData.position + animEndData.position,
                defaultData.position,
                animEndData.rotation,
                defaultData.rotation,
                animEndData.scale,
                defaultData.scale,
                animEndData.easeType,
                hideDuration,
                (forceHide) ? 0 : animEndData.delay + delay);
    }

    private Tween PlayAnim(RectTransform rectTransform, Vector3 toPosition, Vector3 fromPosition, Quaternion toRotation, Quaternion fromRotation, Vector3 toScale, Vector3 fromScale, Ease easeType, float duration, float delay = 0)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Join(
            rectTransform.DOMove(toPosition, duration)
                .From(fromPosition)
                .SetEase(easeType)
        );

        sequence.Join(
            rectTransform.DORotateQuaternion(toRotation, duration)
                .From(fromRotation)
                .SetEase(easeType)
        );

        sequence.Join(
            rectTransform.DOScale(toScale, duration)
                .From(fromScale)
                .SetEase(easeType)
        );

        sequence.SetDelay(delay);
        sequence.Play();
        return sequence;
    }

    private void Reset() => rectTransform = GetComponent<RectTransform>();
    private void OnDrawGizmosSelected()
    {
        if (rectTransform == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(rectTransform.position + animStartData.position, rectTransform.rect.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rectTransform.position + animEndData.position, rectTransform.rect.size);
    }
}
