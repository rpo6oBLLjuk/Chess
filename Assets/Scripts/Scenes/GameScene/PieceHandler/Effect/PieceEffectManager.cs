using DG.Tweening;
using ModestTree;
using UnityEngine;
using Zenject;

public class PieceEffectManager : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [SerializeField] PieceEffectManagerData data;

    void OnEnable()
    {
        gameManager.PieceSpawned += PieceSpawned;
        gameManager.PieceCaptured += PieceCaptured;
        gameManager.PieceDestroyed += PieceDestroyed;

        gameManager.PieceDragStarted += PieceDragStart;
        gameManager.PieceDragEnded += PieceEndDrag;
        gameManager.PieceDragged += PieceDragged;
    }

    void OnDisable()
    {
        gameManager.PieceDestroyed -= PieceSpawned;
        gameManager.PieceCaptured -= PieceCaptured;
        gameManager.PieceDestroyed -= PieceDestroyed;

        gameManager.PieceDragStarted -= PieceDragStart;
        gameManager.PieceDragEnded -= PieceEndDrag;
        gameManager.PieceDragged -= PieceDragged;
    }

    public void PieceDragStart(PieceHandler pieceHandler)
    {
        if (IsDragable(gameManager.Pieces.IndexOf(pieceHandler)))
        {
            pieceHandler.transform.DOScale(Vector3.one * gameManager.PiecesSkinData.AnimationData.scaleMultiplier, gameManager.PiecesSkinData.AnimationData.scaleDuration);
        }
    }

    public void PieceDragged(PieceHandler pieceHandler, Vector3 position, CellHandler cellHandler)
    {
        if (IsDragable(gameManager.Pieces.IndexOf(pieceHandler)))
        {
            pieceHandler.transform.position = position;
        }
    }

    public void PieceEndDrag(PieceHandler pieceHandler, Transform parent)
    {
        pieceHandler.transform.DOMove(parent.position, gameManager.PiecesSkinData.AnimationData.magnetToCellDuration)
            .OnComplete(() => transform.SetParent(parent));

        pieceHandler.transform.DOScale(Vector3.one, gameManager.PiecesSkinData.AnimationData.scaleDuration);
    }

    public void PieceSpawned(PieceHandler pieceHandler, CellHandler cellHandler) => pieceHandler.PieceEffectController.OnInitialized(gameManager.PiecesSkinData.AnimationData.showTime);
    public void PieceCaptured(PieceHandler capturerPiece, PieceHandler capturedPiece, byte capturedPieceData, CellHandler cellHandler) => capturedPiece.PieceEffectController.Destroy(gameManager.PiecesSkinData.AnimationData.destroyTime);
    public void PieceDestroyed(PieceHandler destroyedHandler, CellHandler cellHandler) => destroyedHandler.PieceEffectController.Destroy(gameManager.PiecesSkinData.AnimationData.destroyTime);


    private bool IsDragable(int index)
    {
        if (data.DragInactivePieces || gameManager.Moves[index].Count > 0)
            return true;

        return false;
    }
}
