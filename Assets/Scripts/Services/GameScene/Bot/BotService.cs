using CustomInspector;
using Zenject;

public class BotService : MonoService
{
    [Inject] GameManager gameManager;


    private bool botEnabled = false;


    public void EnableBot()
    {
        botEnabled = true;
        gameManager.PieceMoved += GenerateMoves;
    }
    private void OnDisable()
    {
        gameManager.PieceMoved -= GenerateMoves;
    }

    private void GenerateMoves(PieceHandler pieceHandler, CellHandler startCell, CellHandler endCell)
    {
    }
}
