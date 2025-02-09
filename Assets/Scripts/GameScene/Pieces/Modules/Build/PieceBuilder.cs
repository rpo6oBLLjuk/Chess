using UnityEngine;

public class PieceBuilder : MonoBehaviour
{
    [SerializeField] private BoardBuilder boardCellBuilder;
    [SerializeField] private PieceFactory pooler = new();
    [SerializeField] private PiecesSkinData PiecesSkinData;

    private Transform[,] PiecesInstance;


    public void SetupPieces(DeskData data)
    {
        data.PieceData = new PieceData[boardCellBuilder.BoardSize.x, boardCellBuilder.BoardSize.y];
        PiecesInstance = new Transform[boardCellBuilder.BoardSize.x, boardCellBuilder.BoardSize.y];

        for (int y = 0; y < boardCellBuilder.BoardSize.y; y++)
        {
            for (int x = 0; x < boardCellBuilder.BoardSize.x; x++)
            {
                if (data.PieceData[x, y] != null)
                {
                    GameObject instance = Instantiate(data.PieceData[x, y], boardCellBuilder.Cells[x, y]);
                    PiecesInstance[x, y] = instance.transform;
                }
            }
        }
    }

    private GameObject Instantiate(PieceData PieceData, Transform boardCell)
    {
        GameObject Piece = Instantiate(pooler.Get(PieceData.Type), boardCell);
        Piece.GetComponentInChildren<SpriteRenderer>().sprite = PiecesSkinData.Get(PieceData.Type, PieceData.Color);
        return Piece;
    }
}
