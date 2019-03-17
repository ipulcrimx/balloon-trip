using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public PausePopUpManager pausePopUp;
    public PostGamePopUpManager postGamePopUp;
    public WarningCountdown warningCountdown;

    // Start is called before the first frame update
    void Start()
    {
        // Lazy init...
        pausePopUp.Init();
        postGamePopUp.Init();
        warningCountdown.Init();

        GameManager.instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        if (!postGamePopUp.gameObject.activeInHierarchy)
        {
            postGamePopUp.TogglePopUp();
            // Play game over sound...
        }
    }
}