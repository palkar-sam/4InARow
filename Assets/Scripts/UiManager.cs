using System.Collections;
using System.Collections.Generic;
using board;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private Board board;
    [SerializeField] private SummaryScreen summaryScreen;
    [SerializeField] private ModeSelectionScreen modeSelectionScreen;

    private bool _isRoundStarted;

    private void Start()
    {
        RegisterEvents(true);
        modeSelectionScreen.ActivateScreen(true);
        board.SetVisibility(false);
        summaryScreen.SetVisibility(false);
        inputController.ActivateInput(false);
    }

    private void RegisterEvents(bool flag)
    {
        if (flag)
        {
            inputController.OnClick += OnClick;
            board.OnBoardFull += OnBoardFull;
            board.OnMatchFound += OnMatchFound;
            summaryScreen.OnScreenClosed += OnSummaryScreenClosed;
            modeSelectionScreen.OnModeSelect += OnModeSelect;
        }
        else
        {
            inputController.OnClick -= OnClick;
            board.OnBoardFull -= OnBoardFull;
            board.OnMatchFound -= OnMatchFound;
            summaryScreen.OnScreenClosed -= OnSummaryScreenClosed;
            modeSelectionScreen.OnModeSelect -= OnModeSelect;
        }
    }

    private void OnClick(GameObject obj)
    {
        if (!_isRoundStarted) return;

        CellBase cellBase = obj.GetComponent<CellBase>();

        if (cellBase != null)
            board.PlayRound(cellBase);
    }

    private void OnBoardFull()
    {
        summaryScreen.ActivateScreen(GameStatus.DRAW);
        inputController.ActivateInput(false);
    }

    private void OnMatchFound(bool isPlayer)
    {
        _isRoundStarted = false;
        summaryScreen.ActivateScreen(isPlayer ? GameStatus.WIN : GameStatus.LOSE);
        inputController.ActivateInput(false);
    }

    private void OnSummaryScreenClosed(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.PLAY_AGAIN:
                _isRoundStarted = true;
                board.RestartGame();
                inputController.ActivateInput(true, 0.5f);
                break;
            case GameStatus.GO_TO_HOME:
                _isRoundStarted = false;
                board.RestartGame(false);
                board.SetVisibility(false);
                modeSelectionScreen.ActivateScreen(true);
                inputController.ActivateInput(false);
                break;
        }
    }

    private void OnModeSelect(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.SINGLE_PLAYER:
                _isRoundStarted = true;
                board.StartRound(status);
                inputController.ActivateInput(true, 0.5f);
                break;
            case GameStatus.MULTI_PLAYER:
                Debug.Log("Multiplayer is in WIP------------------");
            break;
        }


    }
}
