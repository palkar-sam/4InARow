using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelectionScreen : MonoBehaviour
{
    public event Action<GameStatus> OnModeSelect;

   public void ActivateScreen(bool flag)
   {
        gameObject.SetActive(flag);
   }

   public void StartSinglePlayerGame()
   {
        OnModeSelect?.Invoke(GameStatus.SINGLE_PLAYER);
        gameObject.SetActive(false);
   }

   public void StartMultiPlayerGame()
   {
        OnModeSelect?.Invoke(GameStatus.MULTI_PLAYER);
        gameObject.SetActive(false);
   }
}
