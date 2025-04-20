using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainPanel : AnimatedPanel
{
    [Inject] SceneLoader sceneLoader;

    [SerializeField] private Button playButton;


    protected override void Start()
    {
        base.Start();
        playButton.onClick.AddListener(LoadGameScene);
    }

    private void LoadGameScene()
    {
        sceneLoader.LoadGameModeSelectionScene();
    }
}
