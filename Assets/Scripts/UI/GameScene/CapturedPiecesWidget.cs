using Coffee.UIEffects;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CapturedPiecesWidget : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] SkinData skinData;

    [SerializeField] Transform whitePiecesContainer;
    [SerializeField] Transform blackPiecesContainer;

    [Serializable]
    private class DefaultPieceData
    {
        public Transform Container;
        public GameObject DefaultPiece;
        public UIEffectPreset DefaiultUIEffectPreset;
    }
    [SerializeField] DefaultPieceData whitePieceData;
    [SerializeField] DefaultPieceData blackPieceData;


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
        whitePieceData.DefaultPiece.SetActive(false);
        blackPieceData.DefaultPiece.SetActive(false);
    }

    private void PieceCaptured(PieceHandler _, PieceHandler capturedPiece, byte capturedPieceData, CellHandler __)
    {
        DefaultPieceData pieceData = PiecePacker.IsEqualColor(capturedPieceData, PieceColor.White) ? whitePieceData : blackPieceData;
        
        GameObject instance = Instantiate(pieceData.DefaultPiece, pieceData.Container);
        instance.SetActive(true);

        instance.GetComponent<Image>().sprite = skinData.piecesSkinData.Get(capturedPieceData);
        instance.GetComponent<UIEffect>().LoadPreset(pieceData.DefaiultUIEffectPreset);
    }
}
