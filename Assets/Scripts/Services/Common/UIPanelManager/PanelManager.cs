using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

public class PanelManager : MonoService
{
    [Inject] NotificationService notificationService;
    [Inject] SceneLoader sceneLoader;

    public event Action QiutRequest;

    [SerializeField] InputActionReference _closeAction;
    [SerializeField] List<AnimatedPanel> pool = new();

    private int currentSceneIndex = -1;


    public void PanelShowed(AnimatedPanel panel) //May be need add listener to panel event
    {
        if (panel.Hideable)
        {
            pool.Add(panel);
            Debug.Log("Panel Showed");
        }
    }

    public void PanelHided(AnimatedPanel panel) //May be need add listener to panel event
    {
        if (panel.Hideable)
        {
            pool.Remove(panel);
        }
    }

    private void OnEnable()
    {
        _closeAction.action.canceled += CloseLastPanel;
        sceneLoader.SceneLoaded += ClearPoolAfterLoadingScene;
    }

    private void OnDisable()
    {
        _closeAction.action.canceled -= CloseLastPanel;
        sceneLoader.SceneLoaded -= ClearPoolAfterLoadingScene;
    }

    private void CloseLastPanel(InputAction.CallbackContext _)
    {
        if (pool.Count > 0)
        {
            AnimatedPanel panel = pool.Last();

            if (panel.Hideable) //Always come true
            {
                panel.HideButton.onClick.Invoke(); //Call PanelHided(panel), removing this panel from pool
            }
        }
        else
        {
            if (currentSceneIndex != sceneLoader.MainScene)
            {
                QiutRequest?.Invoke();
                notificationService.ShowDialog(
                    (bool closeScene) =>
                    {
                        if (closeScene)
                            sceneLoader.LoadMainScene(inverseLoadScreen: true);
                    },
                    "Quit?",
                    "Return to Main",
                    DialogType.OkCancel);
                this.FastLog("Quit?");
            }
            else
            {
                QiutRequest?.Invoke();
                notificationService.ShowDialog(
                    (bool closeApplication) =>
                    {
                        if (closeApplication)
                            Application.Quit();
                    },
                    "Quit?",
                    "Close application",
                    DialogType.OkCancel);
                this.FastLog("Quit?");
            }
        }
    }

    private void ClearPoolAfterLoadingScene(int sceneIndex)
    {
        currentSceneIndex = sceneIndex;
        pool.Clear();
    }
}
