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

    [SerializeField] private float mainPanelShowDuration = 0.1f;
    [SerializeField] private float mainPanelHideDuration = 0.1f;

    [SerializeField] private float showDuration = 0.2f;
    [SerializeField] private float hideDuration = 0.2f;


    private void OnEnable()
    {
        mainPanel.Initialize();

        foreach (PanelContainer panelContainer in widgets)
        {
            if (panelContainer.panel == null)
                continue;

            panelContainer.showButton.onClick.AddListener(() =>
            {
                mainPanel.Hide(mainPanelHideDuration);
                panelContainer.panel.Show(showDuration, mainPanelHideDuration);
            });
            panelContainer.panel.HideButton?.onClick.AddListener(() =>
            {
                panelContainer.panel.Hide(hideDuration);
                mainPanel.ForceShow();
            });

            panelContainer.panel.Initialize();
            panelContainer.panel.ForceHide();
        }

        mainPanel.Show(mainPanelShowDuration);
    }
}


