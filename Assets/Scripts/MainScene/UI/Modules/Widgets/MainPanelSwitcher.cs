using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelSwitcher : MonoBehaviour
{
    public AnimatedPanel mainPanel;

    [Serializable]
    private class PanelContainer
    {
        public AnimatedPanel panel;
        public Button showButton;
    }
    [SerializeField] private List<PanelContainer> widgets;


    private void OnEnable()
    {
        mainPanel.Initialize();

        foreach (PanelContainer panelContainer in widgets)
        {
            if (panelContainer.panel == null)
                continue;

            panelContainer.showButton.onClick.AddListener(() =>
            {
                mainPanel.Hide();
                panelContainer.panel.Show();
            });

            panelContainer.panel.HideButton.onClick.AddListener(mainPanel.ForceShow);

            panelContainer.panel.Initialize();
            panelContainer.panel.ForceHide();
        }

        mainPanel.Show();
    }
}


