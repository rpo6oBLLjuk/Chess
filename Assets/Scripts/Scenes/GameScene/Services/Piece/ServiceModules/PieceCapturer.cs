using UnityEngine;
using Zenject;

public class PieceCapturer
{
    [Inject] GameController gameController;
    [SerializeField] private Canvas canvas;

    public void Init()
    {
        canvas = gameController.GetComponentInParent<Canvas>();
    }

    public void CapturePiece(CellHandler cellHandler)
    {
        if (gameController.Board[cellHandler.Index] == 0)
            return;

        gameController.Board[cellHandler.Index] = 0;
        gameController.Pieces[cellHandler.Index].gameObject.transform.SetParent(canvas.transform);

        cellHandler?.PieceRemoved();
    }
}