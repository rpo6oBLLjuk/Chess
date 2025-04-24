using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanelManager : MonoService
{
    public event Action QiutRequest;

    [SerializeField] InputActionReference _closeAction;

    [SerializeField] AnimatedPanel mainPanel;
    [SerializeField] List<AnimatedPanel> pool = new();


    public void PanelShowed(AnimatedPanel panel)
    {
        if (panel != mainPanel)
            pool.Add(panel);
    }

    private void OnEnable() => _closeAction.action.canceled += CloseLastPanel;
    private void OnDisable() => _closeAction.action.canceled -= CloseLastPanel;

    private void CloseLastPanel(InputAction.CallbackContext _)
    {
        if (pool.Count > 0)
        {
            AnimatedPanel panel = pool.Last();

            panel.HideButton.onClick.Invoke();
            pool.Remove(panel);
        }
        else
        {
            QiutRequest?.Invoke();
            this.FastLog("Quit?");
        }

    }
}
