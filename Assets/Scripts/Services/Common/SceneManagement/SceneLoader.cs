using CustomInspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoService
{
    public event Action<int> SceneLoaded;

    [field: SerializeField, Scene, Tab("Scenes")] public int MainScene { get; private set; }
    [field: SerializeField, Scene, Tab("Scenes")] public int GameModeSelectionScene { get; private set; }
    [field: SerializeField, Scene, Tab("Scenes")] public int GameScene { get; private set; }
    [field: SerializeField, Scene, Tab("Scenes")] public int ConstructorScene { get; private set; }

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

    public void LoadMainScene(IProgress<float> progress = null, bool inverseLoadScreen = false) => LoadScene(MainScene, progress, inverseLoadScreen).Forget();
    public void LoadGameModeSelectionScene(IProgress<float> progress = null, bool inverseLoadScreen = false) => LoadScene(GameModeSelectionScene, progress, inverseLoadScreen).Forget();
    public void LoadGameScene(IProgress<float> progress = null, bool inverseLoadScreen = false) => LoadScene(GameScene, progress, inverseLoadScreen).Forget();
    public void LoadConstructorScene(IProgress<float> progress = null, bool inverseLoadScreen = false) => LoadScene(ConstructorScene, progress, inverseLoadScreen).Forget();


    private async UniTask LoadScene(int sceneIndex, IProgress<float> progress = null, bool inverseLoadScreen = false)
    {
        if (isLoading)
            return;

        isLoading = true;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        Tween openTween = GetOpenTween(inverseLoadScreen);

        while (operation.progress < 0.9f)
        {
            progress?.Report(operation.progress);

            await UniTask.Yield();
        }

        if (!openTween.IsComplete())
            await openTween.AsyncWaitForCompletion().AsUniTask();


        operation.allowSceneActivation = true;
        GetCloseTween(inverseLoadScreen);

        isLoading = false;

        SceneLoaded?.Invoke(sceneIndex);
    }

    private Tween GetOpenTween(bool inverse)
    {
        return inverse
            ? loadingImage.DOLocalMoveX(0, startDuration).From(canvasWidth).SetEase(startEaseType)
            : loadingImage.DOLocalMoveX(0, startDuration).From(-canvasWidth).SetEase(startEaseType);
    }
    private Tween GetCloseTween(bool inverse)
    {
        return inverse
            ? loadingImage.DOLocalMoveX(-canvasWidth, endDuration).From(0).SetEase(endEaseType)
            : loadingImage.DOLocalMoveX(canvasWidth, endDuration).From(0).SetEase(endEaseType);
    }
}
