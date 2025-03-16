using UnityEngine;
using Zenject;

public class PieceEffectManager : MonoBehaviour
{
    [Inject] GameManager gameManager;

    void OnEnable()
    {
        gameManager.PieceSpawned += PieceSpawned;
        gameManager.PieceCaptured += PieceCaptured;
        gameManager.PieceDestroyed += PieceDestroyed;
    }

    void OnDisable()
    {
        gameManager.PieceDestroyed -= PieceSpawned;
        gameManager.PieceCaptured -= PieceCaptured;
        gameManager.PieceDestroyed -= PieceDestroyed;
    }


    public void PieceSpawned(PieceHandler pieceHandler, CellHandler cellHandler) => pieceHandler.PieceEffectController.OnInitialized(gameManager.PiecesSkinData.AnimationData.showTime);
    public void PieceCaptured(PieceHandler capturerPiece, PieceHandler capturedPiece, CellHandler cellHandler) => capturedPiece.PieceEffectController.Destroy(gameManager.PiecesSkinData.AnimationData.destroyTime);
    public void PieceDestroyed(PieceHandler destroyedHandler, CellHandler cellHandler) => destroyedHandler.PieceEffectController.Destroy(gameManager.PiecesSkinData.AnimationData.destroyTime);
}
