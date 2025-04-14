using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CapturedPiecesWidget : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [SerializeField] Transform whitePiecesContainer;
    [SerializeField] Transform blackPiecesContainer;

    [SerializeField] GameObject defaultPiece;


    private void OnEnable()
    {
        gameManager.PieceCaptured += PieceCaptured;
    }

    private void OnDisable()
    {
        gameManager.PieceCaptured -= PieceCaptured;
    }

    private void Awake()
    {
        defaultPiece.SetActive(false);
    }

    private void PieceCaptured(PieceHandler _, PieceHandler capturedPiece, byte capturedPieceData, CellHandler __)
    {
        GameObject instance = Instantiate(defaultPiece, PiecePacker.IsEqualColor(capturedPieceData, PieceColor.White) ? whitePiecesContainer : blackPiecesContainer);
        instance.SetActive(true);
        
        instance.GetComponent<Image>().sprite = gameManager.PiecesSkinData.Get(capturedPieceData);
    }
}
