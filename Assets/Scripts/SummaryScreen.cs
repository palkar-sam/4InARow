using System;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScreen : MonoBehaviour
{
    [SerializeField] private Text statusText;
    [SerializeField] private string wintext = "You Win!";
    [SerializeField] private string losetext = "You Lose!";
    [SerializeField] private string drawtext = "Match Draw!";

    public event Action<GameStatus> OnScreenClosed;

    public void ActivateScreen(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.WIN:
                statusText.text = wintext;
                break;
            case GameStatus.LOSE:
                statusText.text = losetext;
                break;
            case GameStatus.DRAW:
                statusText.text = drawtext;
                break;
            default:
                statusText.text = "No Case Found!";
                break;
        }

        SetVisibility(true);
    }

    public void PlayAgain()
    {
        SetVisibility(false);
        OnScreenClosed?.Invoke(GameStatus.PLAY_AGAIN);
    }

    public void GoToHome()
    {
        SetVisibility(false);
        OnScreenClosed?.Invoke(GameStatus.GO_TO_HOME);
    }

    public void SetVisibility(bool flag)
    {
        gameObject.SetActive(flag);
    }
}
