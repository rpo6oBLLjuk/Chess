using Coffee.UIEffects;
using CustomInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConstructorUI : MonoBehaviour
{
    [Inject] NotificationService notificationService;
    [Inject] GameManager gameManager;
    [Inject] SkinData skinData;

    [SerializeField, Dictionary] ReorderableDictionary<PieceType, Button> pieceButtons;
    [SerializeField] Button colorChangeButton;
    [SerializeField] Button destroyButton;
    [SerializeField] Button clearBoardButton;

    [SerializeField] Button saveButton;
    [SerializeField] Button loadButton;


    [SerializeField] private DeskSaverUI saverUI;
    [SerializeField] private DeskLoaderUI loaderUI;

    private bool destroyerIsActive;
    private PieceColor currectPieceColor = PieceColor.White;


    private void Awake()
    {
        destroyerIsActive = false;

        currectPieceColor = PieceColor.White;
        UpdatePieceButtonSprites();

        foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
        {
            if (pieceType != PieceType.None && pieceType != PieceType.Other)
                AddCallback(pieceButtons[pieceType], () => SpawnButtonCallback(pieceType));
        }
        AddCallback(colorChangeButton, ColorChangeButtonCallback);
        AddCallback(destroyButton, DestroyButtonCallback);
        AddCallback(clearBoardButton, gameManager.ClearBoard);

        AddCallback(saveButton, saverUI.AnimShow);
        AddCallback(loadButton, loaderUI.AnimShow);
    }

    private void AddCallback(Button button, Action callback) => button.onClick.AddListener(() => callback.Invoke());

    private void SpawnButtonCallback(PieceType pieceType)
    {
        var foundIndex = Array.FindIndex(gameManager.Board, piece => PiecePacker.IsEqualType(piece, PieceType.None));

        if (foundIndex == -1)
            notificationService.ShowPopup("Board full", "Spawner", PopupType.Error);
        else
            gameManager.SpawnPiece(PiecePacker.PackPiece(pieceType, currectPieceColor), gameManager.Cells[foundIndex]);
    }

    private void ColorChangeButtonCallback()
    {
        currectPieceColor = (currectPieceColor == PieceColor.White) ? PieceColor.Black : PieceColor.White;
        UpdatePieceButtonSprites();
    }
    private void DestroyButtonCallback()
    {
        destroyerIsActive = !destroyerIsActive;

        if (destroyerIsActive)
        {
            gameManager.CellClicked += DestroyPiece;
            destroyButton.GetComponentInChildren<UIEffect>().enabled = true;
        }
        else
        {
            gameManager.CellClicked -= DestroyPiece;
            destroyButton.GetComponentInChildren<UIEffect>().enabled = false;
        }
    }

    private void DestroyPiece(CellHandler cellHandler)
    {
        if (!PiecePacker.IsEqualType(gameManager.Board[cellHandler.Index], PieceType.None))
            gameManager.DestroyPiece(cellHandler);
    }
    private void UpdatePieceButtonSprites()
    {
        foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
        {
            if (pieceType != PieceType.None && pieceType != PieceType.Other)
            {
                pieceButtons[pieceType].transform.GetComponentInChildrenOnly<Image>().sprite = skinData.piecesSkinData.Get(pieceType, currectPieceColor);
            }
        }
    }
}
