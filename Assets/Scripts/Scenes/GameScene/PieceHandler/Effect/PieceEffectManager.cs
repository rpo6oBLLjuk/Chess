using DG.Tweening;
using ModestTree;
using UnityEngine;
using Zenject;

public class PieceEffectManager
{
    [Inject] GameManager gameManager;

    [SerializeField] PieceEffectManagerData data;


    public void Init(PieceEffectManagerData data)
    {
        this.data = data;

        gameManager.OnKingChecked += KingChecked;
        gameManager.OnCheckResolved += CheckResolved;

        gameManager.PieceSpawned += PieceSpawned;
        gameManager.PieceCaptured += PieceCaptured;
        gameManager.PieceDestroyed += PieceDestroyed;

        gameManager.PieceDragStarted += PieceDragStart;
        gameManager.PieceDragged += PieceDragged;

        gameManager.PieceMoved += PieceMoved;
        gameManager.PieceMoveBlocked += PieceMoveBlocked;
    }
    public void OnDisable()
    {
        gameManager.OnKingChecked -= KingChecked;
        gameManager.OnCheckResolved -= CheckResolved;

        gameManager.PieceDestroyed -= PieceSpawned;
        gameManager.PieceCaptured -= PieceCaptured;
        gameManager.PieceDestroyed -= PieceDestroyed;

        gameManager.PieceDragStarted -= PieceDragStart;
        gameManager.PieceDragged -= PieceDragged;

        gameManager.PieceMoved -= PieceMoved;
        gameManager.PieceMoveBlocked -= PieceMoveBlocked;
    }

    public void PieceDragStart(PieceHandler pieceHandler)
    {
        if (IsDragable(gameManager.Pieces.IndexOf(pieceHandler)))
        {
            pieceHandler.transform.DOScale(Vector3.one * gameManager.PiecesSkinData.AnimationData.scaleMultiplier, gameManager.PiecesSkinData.AnimationData.scaleDuration);
            pieceHandler.transform.SetParent(pieceHandler.GetComponentInParent<Canvas>().transform);
        }
    }
    public void PieceDragged(PieceHandler pieceHandler, Vector3 position, CellHandler cellHandler)
    {
        if (IsDragable(gameManager.Pieces.IndexOf(pieceHandler)))
        {
            pieceHandler.transform.position = position;
        }
    }
    
    public void PieceMoved(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => MovePieceToCell(pieceHandler, endCell);
    public void PieceMoveBlocked(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell) => MovePieceToCell(pieceHandler, startCell);

    public void PieceSpawned(PieceHandler pieceHandler, CellHandler cellHandler) => pieceHandler.PieceEffectController.OnInitialized(gameManager.PiecesSkinData.AnimationData.showTime);
    public void PieceCaptured(PieceHandler capturerPiece, PieceHandler capturedPiece, byte capturedPieceData, CellHandler cellHandler) => capturedPiece.PieceEffectController.Destroy(gameManager.PiecesSkinData.AnimationData.destroyTime, gameManager.PiecesSkinData.AnimationData.destroyCurve);
    public void PieceDestroyed(PieceHandler destroyedHandler, CellHandler cellHandler) => destroyedHandler.PieceEffectController.Destroy(gameManager.PiecesSkinData.AnimationData.destroyTime, gameManager.PiecesSkinData.AnimationData.destroyCurve);

    public void KingChecked(PieceHandler pieceHandler) => pieceHandler.PieceEffectController.Check(gameManager.PiecesSkinData.gradationPreset);
    public void CheckResolved(PieceHandler pieceHandler) => pieceHandler.PieceEffectController.ResolveCheck(gameManager.PiecesSkinData.defaultPreset);

    private void MovePieceToCell(PieceHandler pieceHandler, CellHandler cellHandler)
    {
        pieceHandler.transform.SetParent(pieceHandler.GetComponentInParent<Canvas>().transform);
        pieceHandler.transform.DOMove(cellHandler.transform.position, gameManager.PiecesSkinData.AnimationData.magnetToCellDuration)
            .OnComplete(() => pieceHandler.transform.SetParent(cellHandler.transform));

        pieceHandler.transform.DOScale(Vector3.one, gameManager.PiecesSkinData.AnimationData.scaleDuration);
    }
    private bool IsDragable(int index)
    {
        if (data.DragInactivePieces || gameManager.PossibleMoves[index].Count > 0)
            return true;

        return false;
    }
}
