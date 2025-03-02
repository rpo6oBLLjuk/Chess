using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CellEffectController : MonoBehaviour
{
    [Inject] GameController gameController;

    [SerializeField] private Image targetImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image possibleMoveImage;
    [SerializeField] private Image captureImage;
    [SerializeField] private Image lastMoveImage;


    public void SetAnimTargetColor(Color color, float duration) => SetAnimImageColor(color, duration, targetImage);
    public void SetAnimSelectColor(Color color, float duration) => SetAnimImageColor(color, duration, selectImage);
    public void SetAnimPossibleMoveColor(Color color, float duration) => SetAnimImageColor(color, duration, possibleMoveImage);
    public void SetAnimCaptureColor(Color color, float duration) => SetAnimImageColor(color, duration, captureImage);
    public void SetAnimLastMoveColor(Color color, float duration) => SetAnimImageColor(color, duration, lastMoveImage);


    public void EnableTarget() => SetImageColor(gameController.CellsSkinData.TargetImage.Color, targetImage);
    public void DisableTarget() => SetImageColor(default, targetImage);

    public void EnableSelect() => SetImageColor(gameController.CellsSkinData.SelectImage.Color, selectImage);
    public void DisableSelect() => SetImageColor(default, selectImage);

    public void EnablePossibleMove() => SetImageColor(gameController.CellsSkinData.PossibleMoveImage.Color, possibleMoveImage);
    public void DisablePossibleMove() => SetImageColor(default, possibleMoveImage);

    public void EnableCapture() => SetImageColor(gameController.CellsSkinData.CaptureImage.Color, captureImage);
    public void DisableCapture() => SetImageColor(default, captureImage);

    public void EnableLastMove() => SetImageColor(gameController.CellsSkinData.LastMoveImage.Color, lastMoveImage);
    public void DisableLastMove() => SetImageColor(default, lastMoveImage);


    public void Init(CellsSkinData cellsSkinData)
    {
        targetImage.sprite = cellsSkinData.TargetImage.Sprite;
        selectImage.sprite = cellsSkinData.SelectImage.Sprite;
        possibleMoveImage.sprite = cellsSkinData.PossibleMoveImage.Sprite;
        captureImage.sprite = cellsSkinData.CaptureImage.Sprite;
        lastMoveImage.sprite = cellsSkinData.LastMoveImage.Sprite;
    }

    private void Awake()
    {
        DisableTarget();
        DisableSelect();
        DisablePossibleMove();
        DisableCapture();
        DisableLastMove();
    }

    private void SetAnimImageColor(Color color, float duration, Image imageComponent)
    {
        SetImageColor(color, imageComponent);

        imageComponent.DOKill();

        Sequence colorSequence = DOTween.Sequence();
        colorSequence.Append(imageComponent.DOColor(new Color(color.r, color.g, color.b, 0), duration / 2))
            .Append(imageComponent.DOColor(color, duration / 2))
            .SetLoops(-1)
            .Play();
    }
    private void SetImageColor(Color color, Image imageComponent) => imageComponent.color = color;
}
