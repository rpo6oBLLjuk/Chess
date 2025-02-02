using UnityEngine;

public class WorldCanvasScaler : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private float referenceWidth = 1920f;
    [SerializeField] private float referenceHeight = 1080f;

    private Vector2Int lastScreenSize = new(0, 0);


    private void Start() => ScaleCanvas();
    private void Update() => Refresh();

    private void Refresh()
    {
        Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);
        if (lastScreenSize != screenSize)
            ScaleCanvas();
    }

    private void ScaleCanvas()
    {
        if (canvas.renderMode != RenderMode.WorldSpace)
            return;

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        lastScreenSize = new(screenWidth, screenHeight);

        float scaleX = screenWidth / referenceWidth;
        float scaleY = screenHeight / referenceHeight;

        canvas.transform.localScale = new Vector3(scaleX, scaleY, 0);
    }
}
