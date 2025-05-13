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


    public void Awake()
    {
        rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();

        defaultData.anchoredPosition = rectTransform.anchoredPosition;
        defaultData.rotation = rectTransform.localRotation;
        defaultData.scale = rectTransform.localScale;
    }

    public virtual Tween Show(float showDuration = 0, bool forceShow = false)
    {
        Tween showTween = GetAnim(rectTransform,
            defaultData.anchoredPosition + animStartData.anchoredPosition, defaultData.anchoredPosition,
            animStartData.rotation, defaultData.rotation,
            animStartData.scale, defaultData.scale,
            animStartData.easeType,
            (forceShow) ? 0 : showDuration,
            (forceShow) ? 0 : animStartData.delay);

        if (useLoopAnim)
            showTween.OnComplete(PlayLoopAnim);

        return showTween.Play();
    }
    public virtual Tween Hide(float hideDuration = 0, bool forceHide = false)
    {
        //if (useLoopAnim)
        //    rectTransform.DOKillAllTweens();

        return GetAnim(rectTransform,
            defaultData.anchoredPosition, defaultData.anchoredPosition + animEndData.anchoredPosition,
            defaultData.rotation, animEndData.rotation,
            defaultData.scale, animEndData.scale,
            animEndData.easeType,
            (forceHide) ? 0 : hideDuration,
            (forceHide) ? 0 : animEndData.delay).Play();
    }

    private Tween GetAnim(RectTransform rectTransform, Vector2 fromAnchoredPosition, Vector2 toAnchoredPosition, Quaternion fromRotation, Quaternion toRotation, Vector3 fromScale, Vector3 toScale, Ease easeType, float duration, float delay = 0)
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

        sequence.SetDelay(delay);
        return sequence;
    }

    private void PlayLoopAnim()
    {
        if (useLoopAnim)
        {
            loopSequence?.Kill();
            loopSequence = DOTween.Sequence(rectTransform);

            loopSequence.Append(GetAnim(rectTransform,
                defaultData.anchoredPosition, defaultData.anchoredPosition + loopAnimEndData.anchoredPosition,
                defaultData.rotation, loopAnimEndData.rotation,
                defaultData.scale, loopAnimEndData.scale,
                loopAnimEndData.easeType, loopDuration, loopAnimEndData.delay));

            loopSequence.Append(GetAnim(rectTransform,
                loopAnimEndData.anchoredPosition + defaultData.anchoredPosition, defaultData.anchoredPosition,
                loopAnimEndData.rotation, defaultData.rotation,
                loopAnimEndData.scale, defaultData.scale,
                loopAnimEndData.easeType, loopDuration, loopAnimEndData.delay));

            loopSequence.SetLoops(-1);
            loopSequence.PlayForward();
        }
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
