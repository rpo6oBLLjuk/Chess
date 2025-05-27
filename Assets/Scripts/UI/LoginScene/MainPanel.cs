using UnityEngine;
using UnityEngine.UI;

namespace UI.LoginScene
{
    public class MainPanel : MonoBehaviour
    {
        [SerializeField] Button swapButton;

        [SerializeField] AnimatedPanel singinPanel;
        [SerializeField] AnimatedPanel singupPanel;
        private bool singinPanelActive = true;


        private void OnEnable() => swapButton.onClick.AddListener(SwapPanels);

        private void SwapPanels()
        {
            singinPanelActive = !singinPanelActive;
            if (singinPanelActive)
            {
                singinPanel.AnimShow();
                singupPanel.AnimHide();
            }
            else
            {
                singinPanel.AnimHide();
                singupPanel.AnimShow();
            }
        }
    }
}
