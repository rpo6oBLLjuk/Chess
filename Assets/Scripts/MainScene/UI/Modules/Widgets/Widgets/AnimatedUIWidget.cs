using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedUIWidget : UIWidget
{
    [Serializable]
    private class AnimatedWidgetData
    {
        public RectTransform rectTransform;

        public Vector2 animPositionOffset;
        public Quaternion animStartRotation;
        public Vector3 animStartScale;

        [HideInInspector] public Vector2 startPosition;
        [HideInInspector] public Quaternion startRotation;
        [HideInInspector] public Vector3 startScale;
    }
    [SerializeField] private List<AnimatedWidgetData> animatedWidgetDatas;


    private void Awake()
    {
        animatedWidgetDatas.ForEach(data =>
        {
            data.startPosition = data.rectTransform.anchoredPosition;
            data.startRotation = data.rectTransform.localRotation;
            data.startScale = data.rectTransform.localScale;
        });
    }

    public override Tween ShowWidget(float showDuration)
    {
        animatedWidgetDatas.ForEach(data =>
        {
            Sequence tween = DOTween.Sequence();

            tween.Join(
                data.rectTransform.DOAnchorPos(data.startPosition, showDuration)
                    .From(data.startPosition + data.animPositionOffset)
                    .SetEase(Ease.OutQuad)
            );

            tween.Join(
                data.rectTransform.DORotateQuaternion(data.startRotation, showDuration)
                    .From(data.animStartRotation)
                    .SetEase(Ease.OutQuad)
            );

            tween.Join(
                data.rectTransform.DOScale(data.startScale, showDuration)
                    .From(data.animStartScale)
                    .SetEase(Ease.OutQuad)
            );

            tween.Play();
        });

        return base.ShowWidget(showDuration);
    }
    public override Tween HideWidget(float hideDuration)
    {
        animatedWidgetDatas.ForEach(data =>
        {
            Sequence tween = DOTween.Sequence();

            tween.Join(
                data.rectTransform.DOAnchorPos(data.startPosition + data.animPositionOffset, hideDuration)
                    .From(data.startPosition)
                    .SetEase(Ease.OutQuad)
            );

            tween.Join(
                data.rectTransform.DORotateQuaternion(data.animStartRotation, hideDuration)
                    .From(data.startRotation)
                    .SetEase(Ease.OutQuad)
            );

            tween.Join(
                data.rectTransform.DOScale(data.animStartScale, hideDuration)
                    .From(data.startScale)
                    .SetEase(Ease.OutQuad)
            );

            tween.Play();
        });

        return base.HideWidget(hideDuration);
    }
}
