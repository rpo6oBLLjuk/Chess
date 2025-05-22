using System;
using System.Collections.Generic;
using Zenject;

public class GameSaver : OneFileBaseSaver<GameSave>
{
    [Inject] PanelManager panelManager;
    [Inject] GameManager gameManager;

    bool needSave = true;

    public void ForceSave() => SaveWrapper();

    private void OnEnable()
    {
        panelManager.QuitReuqest += QuitListener;
        gameManager.GameEnded += GameEnded;
    }

    private void OnDisable()
    {
        panelManager.QuitReuqest -= QuitListener;
    }

    private void OnApplicationQuit() => SaveWrapper();

    private void QuitListener(bool qiut)
    {
        if (qiut)
            SaveWrapper();
    }
    private void GameEnded(PieceColor _, bool __)
    {
        needSave = false;
        DeleteSave();
    }

    private void SaveWrapper()
    {
        if (needSave)
        {
            GameSave gameSave = new(gameManager.Board, gameManager.Moves, gameManager.Captures, gameManager.GameTurnController.TurnColor == PieceColor.White);
            Save(gameSave);
        }
    }
}

[Serializable]
public struct GameSave
{
    public byte[] Board;
    public List<Move> Moves;
    public List<Capture> Captures;

    public bool IsWhiteMove;

    public GameSave(byte[] board, List<Move> moves, List<Capture> captures, bool isWhiteMove)
    {
        Board = board;
        Moves = moves;
        Captures = captures;
        IsWhiteMove = isWhiteMove;
    }
}
