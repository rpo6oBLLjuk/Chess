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
        [SerializeField] Button constructorButton;

        private void OnEnable()
        {
            pvpButton.onClick.AddListener(PvPOnClick);
            pveButton.onClick.AddListener(PvEOnClick);
            constructorButton.onClick.AddListener(ConstructorOnClick);
        }


        private void PvPOnClick()
        {
            sceneLoader.LoadGameScene();
        }

        private void PvEOnClick()
        {
            sceneLoader.LoadGameScene();
        }

        private void ConstructorOnClick()
        {
            sceneLoader.LoadConstructorScene();
        }
    }
}