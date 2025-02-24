using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CellEffectController : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Image possibleMoveImage;
    [SerializeField] private Image eatImage;
    [SerializeField] private Image lastMoveImage;


    public void SetAnimTargetColor(Color color, float duration) => SetAnimImageColor(color, duration, targetImage);
    public void SetAnimSelectColor(Color color, float duration) => SetAnimImageColor(color, duration, selectImage);
    public void SetAnimPossibleMoveColor(Color color, float duration) => SetAnimImageColor(color, duration, possibleMoveImage);
    public void SetAnimEatColor(Color color, float duration) => SetAnimImageColor(color, duration, eatImage);
    public void SetAnimLastMoveColor(Color color, float duration) => SetAnimImageColor(color, duration, lastMoveImage);

    public void SetTargetColor(Color color) => SetImageColor(color, targetImage);
    public void SetSelectColor(Color color) => SetImageColor(color, selectImage);
    public void SetPossibleMoveColor(Color color) => SetImageColor(color, possibleMoveImage);
    public void SetEatColor(Color color) => SetImageColor(color, eatImage);
    public void SetLastMoveColor(Color color) => SetImageColor(color, lastMoveImage);

    public void Init(CellsSkinData cellsSkinData)
    {
        targetImage.sprite = cellsSkinData.TargetImage.Sprite;
        selectImage.sprite = cellsSkinData.SelectImage.Sprite;
        possibleMoveImage.sprite = cellsSkinData.PossibleMoveImage.Sprite;
        eatImage.sprite = cellsSkinData.EatImage.Sprite;
        lastMoveImage.sprite = cellsSkinData.LastMoveImage.Sprite;
    }

    private void Awake()
    {
        SetImageColor(default, targetImage);
        SetImageColor(default, selectImage);
        SetImageColor(default, possibleMoveImage);
        SetImageColor(default, eatImage);
        SetImageColor(default, lastMoveImage);
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
    private void SetImageColor(Color color, Image imageComponent)
    {
        imageComponent.color = color;
    }
}
