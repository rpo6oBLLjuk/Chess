using UnityEngine;
using Zenject;

public class PiecesEffector : MonoBehaviour
{
    [Inject] GameController gameController;

    void OnEnable()
    {
        gameController.PieceSpawned += PieceSpawned;
        gameController.PieceCaptured += PieceCaptured;
        gameController.PieceDestroyed += PieceDestroyed;
    }

    void OnDisable()
    {
        gameController.PieceDestroyed -= PieceSpawned;
        gameController.PieceCaptured -= PieceCaptured;
        gameController.PieceDestroyed -= PieceDestroyed;
    }


    public void PieceSpawned(PieceHandler pieceHandler, CellHandler cellHandler) => pieceHandler.PieceEffectController.OnInitialized(gameController.PiecesSkinData.AnimationData.showTime);
    public void PieceCaptured(PieceHandler capturerPiece, PieceHandler capturedPiece, CellHandler cellHandler) => capturedPiece.PieceEffectController.Destroy(gameController.PiecesSkinData.AnimationData.destroyTime);
    public void PieceDestroyed(PieceHandler destroyedHandler, CellHandler cellHandler) => destroyedHandler.PieceEffectController.Destroy(gameController.PiecesSkinData.AnimationData.destroyTime);
}
