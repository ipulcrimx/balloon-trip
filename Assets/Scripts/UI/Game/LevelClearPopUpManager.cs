using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelClearPopUpManager : MonoBehaviour
{
    public Button continueButton;
    public Button menuButton;
    public Button quitButton;

    [Header("Texts")]
    public Text coinText;
    public Text scoreText;

    private bool _isOpen = false;
    private SoundManager _soundManager;
    private GameManager _gameManager;

    public void Init()
    {
        _soundManager = SoundManager.instance;
        _gameManager = GameManager.instance;

        continueButton.onClick.AddListener(() => { ContinueButtonClicked(); });
        menuButton.onClick.AddListener(() => { OnMenuClicked(); });
        quitButton.onClick.AddListener(() => { OnQuitClicked(); });
    }


    [ContextMenu("Toggle Level Clear")]
    public void TogglePopUp()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            if (coinText) coinText.text = _gameManager.obtainedCoin + "";
            if (scoreText) scoreText.text = _gameManager.score + "";
        }

        Time.timeScale = _isOpen ? 0 : 1;
        gameObject.SetActive(_isOpen);
    }

    private void ContinueButtonClicked()
    {
        if (_soundManager) _soundManager.PlaySFX();

        LevelManager.instance.OnGoToNextLevel();
    }

    private void OnMenuClicked()
    {
        if (_soundManager) _soundManager.PlaySFX();
        SceneManager.LoadScene("Main Menu");
    }

    private void OnQuitClicked()
    {
        if (_soundManager) _soundManager.PlaySFX();
        Application.Quit();
    }
}