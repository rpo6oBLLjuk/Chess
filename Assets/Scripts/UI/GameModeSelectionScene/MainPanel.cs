using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GameModeSelectionScene.Panel
{
    public class MainPanel : AnimatedPanel
    {
        [Inject] GameData gameData;
        [Inject] SceneLoader sceneLoader;
        [Inject] NotificationService notificationService;

        [Header("References")]
        [SerializeField] DeskLoaderUI DeskLoaderUI;

        [Header("Scene Buttons")]
        [SerializeField] Button pvpButton;
        [SerializeField] Button pveButton;
        [SerializeField] Button constructorButton;

        [Header("Board buttons")]
        [SerializeField] Button defaultBoardButton;
        [SerializeField] Button customBoardButton;


        private void OnEnable()
        {
            pvpButton.onClick.AddListener(PvPOnClick);
            pveButton.onClick.AddListener(PvEOnClick);
            constructorButton.onClick.AddListener(ConstructorOnClick);

            defaultBoardButton.onClick.AddListener(DefaultBoardOnClick);
            customBoardButton.onClick.AddListener(DeskLoaderUI.AnimShow);
        }

        private void PvPOnClick() => sceneLoader.LoadGameScene();
        private void PvEOnClick() => sceneLoader.LoadGameScene();
        private void ConstructorOnClick() => sceneLoader.LoadConstructorScene();

        private void DefaultBoardOnClick()
        {
            gameData.SetDefaultBoard();
            notificationService.ShowPopup("Default board loaded", popupType: PopupType.Info);
        }
    }
}