using CustomInspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoService
{
    [SerializeField, Scene, Tab("Scenes")] int mainScene;
    [SerializeField, Scene, Tab("Scenes")] int gameModeSelectionScene;
    [SerializeField, Scene, Tab("Scenes")] int gameScene;
    [SerializeField, Scene, Tab("Scenes")] int constructorScene;

    [SerializeField, Tab("References")] RectTransform loadingImage;

    [SerializeField, Tab("Settings")] Ease startEaseType;
    [SerializeField, Tab("Settings")] float startDuration = 0.25f;
    [SerializeField, Tab("Settings")] Ease endEaseType;
    [SerializeField, Tab("Settings")] float endDuration = 0.25f;

    private float canvasWidth;
    
    private bool isLoading = false;



    public void Start()
    {
        canvasWidth = GetComponentInChildren<Canvas>().GetComponent<RectTransform>().rect.width;

        loadingImage.localPosition = new Vector3(-canvasWidth, loadingImage.localPosition.y, loadingImage.localPosition.z);
    }

    public void LoadMainScene(IProgress<float> progress = null) => LoadScene(mainScene, progress).Forget();
    public void LoadGameModeSelectionScene(IProgress<float> progress = null) => LoadScene(gameModeSelectionScene, progress).Forget();
    public void LoadGameScene(IProgress<float> progress = null) => LoadScene(gameScene, progress).Forget();
    public void LoadConstructorScene(IProgress<float> progress = null) => LoadScene(constructorScene, progress).Forget();


    private async UniTask LoadScene(int sceneIndex, IProgress<float> progress = null)
    {
        if (isLoading)
            return;

        isLoading = true;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        Tween openTween = loadingImage.DOLocalMoveX(0, startDuration)
            .From(-canvasWidth)
            .SetEase(startEaseType);

        while (operation.progress < 0.9f)
        {
            progress?.Report(operation.progress);

            await UniTask.Yield();
        }

        if (!openTween.IsComplete())
            await openTween.AsyncWaitForCompletion().AsUniTask();

        operation.allowSceneActivation = true;
        loadingImage.DOLocalMoveX(canvasWidth, endDuration)
            .From(0)
            .SetEase(endEaseType);

        isLoading = false;
    }
}
