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

    [Space]
    [SerializeField] bool useLoopAnim;
    [SerializeField] float loopDuration = 1.0f;
    [SerializeField] AnimationData loopAnimEndData;

    [SerializeField] private AnimationData defaultData = new();
    [HideInInspector] Sequence loopSequence;


    public void Initialize()
    {
        rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();

        defaultData.position = rectTransform.position;
        defaultData.rotation = rectTransform.localRotation;
        defaultData.scale = rectTransform.localScale;
    }

    public virtual Tween Show(float showDuration = 0, bool forceShow = false)
    {
        Tween showTween = GetAnim(rectTransform,
            defaultData.position + animStartData.position, defaultData.position,
            animStartData.rotation, defaultData.rotation,
            animStartData.scale, defaultData.scale,
            animStartData.easeType,
            (forceShow) ? 0 : showDuration,
            (forceShow) ? 0 : animStartData.delay);

        if (useLoopAnim)
            showTween.OnComplete(PlayLoopAnim);

        showTween.Play();

        return showTween;
    }
    public virtual Tween Hide(float hideDuration = 0, bool forceHide = false)
    {
        //if (useLoopAnim)
        //    rectTransform.DOKillAllTweens();

        return GetAnim(rectTransform,
            defaultData.position, defaultData.position + animEndData.position,
            defaultData.rotation, animEndData.rotation,
            defaultData.scale, animEndData.scale,
            animEndData.easeType,
            (forceHide) ? 0 : hideDuration,
            (forceHide) ? 0 : animEndData.delay).Play();
    }

    private Tween GetAnim(RectTransform rectTransform, Vector3 fromPosition, Vector3 toPosition, Quaternion fromRotation, Quaternion toRotation, Vector3 fromScale, Vector3 toScale, Ease easeType, float duration, float delay = 0)
    {
        Sequence sequence = DOTween.Sequence(rectTransform);

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
        return sequence;
    }

    private void PlayLoopAnim()
    {
        if (useLoopAnim)
        {
            loopSequence?.Kill();
            loopSequence = DOTween.Sequence(rectTransform);

            loopSequence.Append(GetAnim(rectTransform,
                defaultData.position, defaultData.position + loopAnimEndData.position,
                defaultData.rotation, loopAnimEndData.rotation,
                defaultData.scale, loopAnimEndData.scale,
                loopAnimEndData.easeType, loopDuration, loopAnimEndData.delay));

            loopSequence.Append(GetAnim(rectTransform,
                loopAnimEndData.position + defaultData.position, defaultData.position,
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
        Gizmos.DrawWireCube(rectTransform.position + animStartData.position, rectTransform.rect.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rectTransform.position + animEndData.position, rectTransform.rect.size);
    }
}
