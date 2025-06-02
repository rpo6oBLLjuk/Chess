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
        public Vector2 anchoredPosition;
        public Quaternion rotation;
        public Vector3 scale = Vector3.one;

        public Ease easeType = Ease.OutQuad;

        public float delay;
    }
    [SerializeField] private AnimationData animStartData;
    [SerializeField] private AnimationData animEndData;

    [Space]
    [SerializeField] bool useLoopAnim;
    [SerializeField] float loopDuration = 1.0f;
    [SerializeField] AnimationData loopAnimEndData;

    [SerializeField] private AnimationData defaultData = new();
    [HideInInspector] Sequence loopSequence;

    private Tween previousTween;


    public void Awake()
    {
        rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();

        defaultData.anchoredPosition = rectTransform.anchoredPosition;
        defaultData.rotation = rectTransform.localRotation;
        defaultData.scale = rectTransform.localScale;
    }

    public virtual Tween Show(float showDuration = 0, bool forceShow = false)
    {
        previousTween.Kill(true);
        previousTween = GetAnim(rectTransform,
            defaultData.anchoredPosition + animStartData.anchoredPosition, defaultData.anchoredPosition,
            animStartData.rotation, defaultData.rotation,
            animStartData.scale, defaultData.scale,
            animStartData.easeType,
            (forceShow) ? 0 : showDuration,
            (forceShow) ? 0 : animStartData.delay);

        if (useLoopAnim)
            previousTween.OnComplete(PlayLoopAnim);

        return previousTween.Play();
    }
    public virtual Tween Hide(float hideDuration = 0, bool forceHide = false)
    {
        previousTween.Kill(true);
        previousTween = GetAnim(rectTransform,
            defaultData.anchoredPosition, defaultData.anchoredPosition + animEndData.anchoredPosition,
            defaultData.rotation, animEndData.rotation,
            defaultData.scale, animEndData.scale,
            animEndData.easeType,
            (forceHide) ? 0 : hideDuration,
            (forceHide) ? 0 : animEndData.delay);

        return previousTween.Play();
    }

    private Tween GetAnim(RectTransform rectTransform, Vector2 fromAnchoredPosition, Vector2 toAnchoredPosition, Quaternion fromRotation, Quaternion toRotation, Vector3 fromScale, Vector3 toScale, Ease easeType, float duration, float delay = -1)
    {
        Sequence sequence = DOTween.Sequence(rectTransform);

        sequence.Join(
            rectTransform.DOAnchorPos(toAnchoredPosition, duration)
                .From(fromAnchoredPosition)
                .SetEase(easeType)
        );

        sequence.Join(
            rectTransform.DOLocalRotateQuaternion(toRotation, duration)
                .From(fromRotation)
                .SetEase(easeType)
        );

        sequence.Join(
            rectTransform.DOScale(toScale, duration)
                .From(fromScale)
                .SetEase(easeType)
        );

        if (delay > 0)
            sequence.SetDelay(delay);
        return sequence;
    }

    private void PlayLoopAnim()
    {
        if (!useLoopAnim)
            return;

        loopSequence?.Kill();

        loopSequence = DOTween.Sequence();

        loopSequence.Append(
            rectTransform.DOAnchorPos(defaultData.anchoredPosition + loopAnimEndData.anchoredPosition, loopDuration)
                .From(defaultData.anchoredPosition)
                .SetEase(loopAnimEndData.easeType)
        );
        loopSequence.Join(
            rectTransform.DOLocalRotateQuaternion(loopAnimEndData.rotation, loopDuration)
                .From(defaultData.rotation)
                .SetEase(loopAnimEndData.easeType)
        );
        loopSequence.Join(
            rectTransform.DOScale(loopAnimEndData.scale, loopDuration)
                .From(defaultData.scale)
                .SetEase(loopAnimEndData.easeType)
        );

        loopSequence.Append(
            rectTransform.DOAnchorPos(defaultData.anchoredPosition, loopDuration)
                .SetEase(loopAnimEndData.easeType)
        );
        loopSequence.Join(
            rectTransform.DOLocalRotateQuaternion(defaultData.rotation, loopDuration)
                .SetEase(loopAnimEndData.easeType)
        );
        loopSequence.Join(
            rectTransform.DOScale(defaultData.scale, loopDuration)
                .SetEase(loopAnimEndData.easeType)
        );

        loopSequence.SetLoops(-1);
        loopSequence.Play();
    }


    private void OnDisable() => rectTransform.DOKill();

    private void Reset() => rectTransform = GetComponent<RectTransform>();
    private void OnDrawGizmosSelected()
    {
        if (rectTransform == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(rectTransform.position + (Vector3)animStartData.anchoredPosition, rectTransform.rect.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rectTransform.position + (Vector3)animEndData.anchoredPosition, rectTransform.rect.size);
    }
}
