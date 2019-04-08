using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public PausePopUpManager pausePopUp;
    public PostGamePopUpManager postGamePopUp;
    public LevelClearPopUpManager levelClear;
    public WarningCountdown warningCountdown;

    // Start is called before the first frame update
    void Start()
    {
        // Lazy init...
        pausePopUp.Init();
        postGamePopUp.Init();
        warningCountdown.Init();

        GameManager.instance.OnGameOver += OnGameOver;
        GameManager.instance.OnGameClear += OnGameClear;
    }

    private void OnGameOver()
    {
        if (!postGamePopUp.gameObject.activeInHierarchy)
        {
            postGamePopUp.TogglePopUp();
            // Play game over sound...
        }
    }

    private void OnGameClear()
    {
        if(!levelClear.gameObject.activeInHierarchy)
        {
            levelClear.TogglePopUp();
        }
    }
}