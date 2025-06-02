using UnityEngine;
using Zenject;

public class GameAnalyticsService : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] DBService dbService;
    [Inject] GameAnalyticsData gameAnalyticsData;


    private void OnEnable() => gameManager.GameEnded += GameEnd;
    private void OnDisable() => gameManager.GameEnded -= GameEnd;

    private async void GameEnd(PieceColor pieceColor, bool pat)
    {
        if (!pat)
        {
            bool isWin = pieceColor == PieceColor.White;
            if (isWin)
                gameAnalyticsData.winsCount++;
            else
                gameAnalyticsData.defeatsCount++;

            bool updated = await dbService.AnalyticsController.UpdateBattleStats(isWin);

            Debug.Log($"Game data updated: {updated}");
        }
    }
}
