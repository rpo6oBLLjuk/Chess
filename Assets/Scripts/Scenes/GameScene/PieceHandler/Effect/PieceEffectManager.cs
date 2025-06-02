using DG.Tweening;
using ModestTree;
using UnityEngine;
using Zenject;

public class PieceEffectManager
{
    [Inject] GameManager gameManager;
    [Inject] SkinData skinData;


    public void Init()
    {
        gameManager.OnKingChecked += KingChecked;
        gameManager.OnCheckResolved += CheckResolved;

        gameManager.GameEnded += GameEnd;

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

        gameManager.GameEnded -= GameEnd;

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
            pieceHandler.transform.DOScale(Vector3.one * skinData.pieceAnimationData.scaleMultiplier, skinData.pieceAnimationData.scaleDuration);
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

    public void PieceSpawned(PieceHandler pieceHandler, CellHandler cellHandler) => pieceHandler.PieceEffectController.OnInitialized(skinData.pieceAnimationData.showTime);
    public void PieceCaptured(PieceHandler capturerPiece, PieceHandler capturedPiece, byte capturedPieceData, CellHandler cellHandler) => capturedPiece.PieceEffectController.Destroy(skinData.pieceAnimationData.destroyTime, skinData.pieceAnimationData.destroyCurve);
    public void PieceDestroyed(PieceHandler destroyedHandler, CellHandler cellHandler) => destroyedHandler.PieceEffectController.Destroy(skinData.pieceAnimationData.destroyTime, skinData.pieceAnimationData.destroyCurve);

    public void KingChecked(PieceHandler pieceHandler) => pieceHandler.PieceEffectController.Check(skinData.piecesSkinData.gradationPreset);
    public void CheckResolved(PieceHandler pieceHandler) => pieceHandler.PieceEffectController.ResolveCheck(skinData.piecesSkinData.defaultPreset);

    public void GameEnd(PieceColor pieceColor, bool pat)
    {
        if (pat)
            FindKing(pieceColor).PieceEffectController.Burn(skinData.piecesSkinData.burnPreset);

        FindKing(pieceColor.Invert()).PieceEffectController.Burn(skinData.piecesSkinData.burnPreset); //burn oppenent king
    }

    private void MovePieceToCell(PieceHandler pieceHandler, CellHandler cellHandler)
    {
        pieceHandler.transform.SetParent(pieceHandler.GetComponentInParent<Canvas>().transform);
        pieceHandler.transform.DOMove(cellHandler.transform.position, skinData.pieceAnimationData.magnetToCellDuration)
            .OnComplete(() => pieceHandler.transform.SetParent(cellHandler.transform));

        pieceHandler.transform.DOScale(Vector3.one, skinData.pieceAnimationData.scaleDuration);
    }
    private bool IsDragable(int index)
    {
        if (skinData.pieceEffectData.DragInactivePieces || gameManager.PossibleMoves[index].Count > 0)
            return true;

        return false;
    }

    private PieceHandler FindKing(PieceColor pieceColor)
    {
        for (int i = 0; i < gameManager.Board.Length; i++)
        {
            byte pieceData = gameManager.Board[i];
            if (PiecePacker.IsEqualType(pieceData, PieceType.King) && PiecePacker.IsEqualColor(pieceData, pieceColor))
            {
                return gameManager.Pieces[i];
            }
        }

        return null;
    }
}
