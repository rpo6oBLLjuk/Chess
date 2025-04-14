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


    private void Start()
    {
        foreach (PanelContainer panelContainer in widgets)
        {
            if (panelContainer.panel == null)
                continue;

            panelContainer.showButton?.onClick.AddListener(() =>
            {
                mainPanel.AnimHide();
                panelContainer.panel.AnimShow();
            });

            panelContainer.panel.HideButton.onClick.AddListener(mainPanel.ForceShow);
        }

        mainPanel.AnimShow();
    }
}


