using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryPanel : AnimatedPanel
{
    [Inject] SkinData skinData;

    [Serializable]
    private class SkinContainer
    {
        public string name;
        public PiecesSkinData piecesSkinData;
        public CellsSkinData cellsSkinData;
    }
    [SerializeField] List<SkinContainer> skinsData;

    [SerializeField] Image currentSkinImage;
    [SerializeField] TMP_Text currentSkinName;
    [SerializeField] List<Button> inventorySlots;


    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < skinsData.Count; i++)
        {
            SkinContainer skinContainer = skinsData[i];

            inventorySlots[i].transform.GetComponentInChildrenOnly<Image>().sprite = skinContainer.piecesSkinData.Get(PiecePacker.PackPiece(PieceType.Pawn, PieceColor.White));
            if (skinData.piecesSkinData == skinContainer.piecesSkinData)
            {
                UpdateUI(skinContainer.name);
            }
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            int index = i;
            inventorySlots[i].onClick.AddListener(() => SetNewSkin(index));
        }
    }

    private void SetNewSkin(int skinIndex)
    {
        skinData.SetNewSkin(skinsData[skinIndex].piecesSkinData, skinsData[skinIndex].cellsSkinData);
        UpdateUI(skinsData[skinIndex].name);
    }

    private void UpdateUI(string name)
    {
        currentSkinImage.sprite = skinData.piecesSkinData.Get(PiecePacker.PackPiece(PieceType.Pawn, PieceColor.White));
        currentSkinName.text = name;
    }
}
