using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CellEffectHandler : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [SerializeField] private Image targetImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image possibleMoveImage;
    [SerializeField] private Image captureImage;
    [SerializeField] private Image previousMoveImage;
    [SerializeField] private Image hoverImage;


    public void Init(CellsSkinData cellsSkinData)
    {
        targetImage.sprite = cellsSkinData.TargetCellData.Sprite;
        selectImage.sprite = cellsSkinData.SelectCellData.Sprite;
        possibleMoveImage.sprite = cellsSkinData.PossibleMoveCellData.Sprite;
        captureImage.sprite = cellsSkinData.CaptureCellData.Sprite;
        previousMoveImage.sprite = cellsSkinData.PreviousMoveCellData.Sprite;
        hoverImage.sprite = cellsSkinData?.HoverCellData.Sprite;
    }

    public void ChangeAll(bool enableTarget = false, bool enableSelected = false, bool enablePossibleMove = false, bool enableCaptured = false, bool enablePreviousMove = false, bool enableHovered = false)
    {
        (enableTarget ? (Action)EnableTarget : DisableTarget)();
        (enableSelected ? (Action)EnableSelect : DisableSelect)();
        (enablePossibleMove ? (Action)EnablePossibleMove : DisablePossibleMove)();
        (enableCaptured ? (Action)EnableCapture : DisableCapture)();
        (enablePreviousMove ? (Action)EnablePreviousMove : DisablePreviousMove)();
        (enableHovered ? (Action)EnableHover : DisableHover)();
    }
    public void ChangeAllAnim(bool enableTarget = false, bool enableSelected = false, bool enablePossibleMove = false, bool enableCaptured = false, bool enablePreviousMove = false, bool enableHovered = false)
    {
        (enableTarget ? (Action)EnableAnimTarget : DisableAnimTarget)();
        (enableSelected ? (Action)EnableAnimSelect : DisableAnimSelect)();
        (enablePossibleMove ? (Action)EnableAnimPossibleMove : DisableAnimPossibleMove)();
        (enableCaptured ? (Action)EnableAnimCapture : DisableAnimCapture)();
        (enablePreviousMove ? (Action)EnableAnimPreviousMove : DisableAnimPreviousMove)();
        (enableHovered ? (Action)EnableAnimHover : DisableAnimHover)();
    }

    public void EnableAnimTarget() => SetAnimImageColor(gameManager.CellsSkinData.TargetCellData.Color, gameManager.CellsSkinData.AnimationData.TargetAnimationData.Duration, targetImage);
    public void EnableAnimSelect() => SetAnimImageColor(gameManager.CellsSkinData.SelectCellData.Color, gameManager.CellsSkinData.AnimationData.SelectAnimationData.Duration, selectImage);
    public void EnableAnimPossibleMove() => SetAnimImageColor(gameManager.CellsSkinData.PossibleMoveCellData.Color, gameManager.CellsSkinData.AnimationData.PossibleMoveAnimationData.Duration, possibleMoveImage);
    public void EnableAnimCapture() => SetAnimImageColor(gameManager.CellsSkinData.CaptureCellData.Color, gameManager.CellsSkinData.AnimationData.CaptureAnimationData.Duration, captureImage);
    public void EnableAnimPreviousMove() => SetAnimImageColor(gameManager.CellsSkinData.PreviousMoveCellData.Color, gameManager.CellsSkinData.AnimationData.PreviousMoveAnimationData.Duration, previousMoveImage);
    public void EnableAnimHover() => SetAnimImageColor(gameManager.CellsSkinData.HoverCellData.Color, gameManager.CellsSkinData.AnimationData.HoverAnimationData.Duration, hoverImage);

    public void DisableAnimTarget() => DisableAnimImage(targetImage);
    public void DisableAnimSelect() => DisableAnimImage(selectImage);
    public void DisableAnimPossibleMove() => DisableAnimImage(possibleMoveImage);
    public void DisableAnimCapture() => DisableAnimImage(captureImage);
    public void DisableAnimPreviousMove() => DisableAnimImage(previousMoveImage);
    public void DisableAnimHover() => DisableAnimImage(hoverImage);

    public void EnableTarget() => SetImageColor(gameManager.CellsSkinData.TargetCellData.Color, targetImage);
    public void EnableSelect() => SetImageColor(gameManager.CellsSkinData.SelectCellData.Color, selectImage);
    public void EnablePossibleMove() => SetImageColor(gameManager.CellsSkinData.PossibleMoveCellData.Color, possibleMoveImage);
    public void EnableCapture() => SetImageColor(gameManager.CellsSkinData.CaptureCellData.Color, captureImage);
    public void EnablePreviousMove() => SetImageColor(gameManager.CellsSkinData.PreviousMoveCellData.Color, previousMoveImage);
    public void EnableHover() => SetImageColor(gameManager.CellsSkinData.HoverCellData.Color, hoverImage);

    public void DisableTarget() => SetImageColor(default, targetImage);
    public void DisableSelect() => SetImageColor(default, selectImage);
    public void DisablePossibleMove() => SetImageColor(default, possibleMoveImage);
    public void DisableCapture() => SetImageColor(default, captureImage);
    public void DisablePreviousMove() => SetImageColor(default, previousMoveImage);
    public void DisableHover() => SetImageColor(default, hoverImage);


    private void Awake()
    {
        DisableTarget();
        DisableSelect();
        DisablePossibleMove();
        DisableCapture();
        DisablePreviousMove();
        DisableHover();
    }

    private void SetAnimImageColor(Color color, float duration, Image imageComponent)
    {
        KillAnim(imageComponent);

        SetImageColor(color, imageComponent);

        Sequence colorSequence = DOTween.Sequence(imageComponent);
        colorSequence
            .Append(imageComponent.DOFade(0, duration / 2)
                                  .SetEase(Ease.OutQuad))
            .Append(imageComponent.DOFade(color.a, duration / 2)
                                  .SetEase(Ease.InQuad))
            .SetLoops(-1)
            .Play();
    }
    private void SetImageColor(Color color, Image imageComponent) => imageComponent.color = color;

    private void KillAnim(Image image) => image.DOKill();

    private void DisableAnimImage(Image image)
    {
        KillAnim(image);
        SetImageColor(default, image);
    }
}
