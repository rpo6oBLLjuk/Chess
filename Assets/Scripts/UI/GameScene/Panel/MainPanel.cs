using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : AnimatedPanel
{
    [SerializeField] private Button playButton;

    protected override void Start()
    {
        base.Start();
        playButton.onClick.AddListener(LoadGameScene);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
