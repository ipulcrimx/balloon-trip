using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostGamePopUpManager : MonoBehaviour
{
    public Button restartButton;
    public Button menuButton;
    public Button quitButton;

    private bool _isOpen = false;
    private SoundManager _soundManager;

    public void Init()
    {
        _soundManager = SoundManager.instance;

        restartButton.onClick.AddListener(() => { OnRestartClicked(); });
        menuButton.onClick.AddListener(() => { OnMenuClicked(); });
        quitButton.onClick.AddListener(() => { OnQuitClicked(); });
    }

    [ContextMenu("Toggle Post Game")]
    public void TogglePopUp()
    {
        _isOpen = !_isOpen;

        Time.timeScale = _isOpen ? 0 : 1;
        gameObject.SetActive(_isOpen);
    }

    private void OnRestartClicked()
    {
        if (_soundManager) _soundManager.PlaySFX();
        SceneManager.LoadScene("Game");
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
