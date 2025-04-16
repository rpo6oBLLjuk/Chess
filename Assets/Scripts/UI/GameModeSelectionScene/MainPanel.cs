using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.GameModeSelectionScene.Panel
{
    public class MainPanel : AnimatedPanel
    {
        [SerializeField] Button pvpButton;
        [SerializeField] Button pveButton;


        private void OnEnable()
        {
            pvpButton.onClick.AddListener(PvPOnClick);
            pveButton.onClick.AddListener(PvEOnClick);
        }


        private void PvPOnClick()
        {
            SceneManager.LoadScene(3);
        }

        private void PvEOnClick()
        {
            SceneManager.LoadScene(3);
        }
    }
}