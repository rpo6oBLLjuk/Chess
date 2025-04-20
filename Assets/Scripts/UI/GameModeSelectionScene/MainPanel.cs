using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GameModeSelectionScene.Panel
{
    public class MainPanel : AnimatedPanel
    {
        [Inject] SceneLoader sceneLoader;

        [SerializeField] Button pvpButton;
        [SerializeField] Button pveButton;


        private void OnEnable()
        {
            pvpButton.onClick.AddListener(PvPOnClick);
            pveButton.onClick.AddListener(PvEOnClick);
        }


        private void PvPOnClick()
        {
            sceneLoader.LoadGameScene();
        }

        private void PvEOnClick()
        {
            sceneLoader.LoadGameScene();
        }
    }
}