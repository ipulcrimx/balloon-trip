using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePopUpManager : MonoBehaviour
{
    [Header("Button")]
    public Button resumeButton;
    public Button restartButton;
    public Button menuButton;
    public Button quitButton;

    [Space]
    public Button musicButton;
    public Button sfxButton;
    public Button adsButton;

    private bool _isOpen = false;
    private SoundManager _soundManager;

    public void Init()
    {
        _soundManager = SoundManager.instance;

        resumeButton.onClick.AddListener(() => { OnResumeClicked(); });
        restartButton.onClick.AddListener(() => { OnRestartClicked(); });
        menuButton.onClick.AddListener(() => { OnMenuClicked(); });
        quitButton.onClick.AddListener(() => { OnQuitClicked(); });

        musicButton.onClick.AddListener(() => { OnMusicClicked(); });
        sfxButton.onClick.AddListener(() => { OnSFXClicked(); });
        adsButton.onClick.AddListener(() => { OnAdsClicked(); });
    }

    public void TogglePopUp()
    {
        _isOpen = !_isOpen;

        Time.timeScale = _isOpen ? 0 : 1;
        gameObject.SetActive(_isOpen);
    }

    private void OnResumeClicked()
    {
        if(_soundManager) _soundManager.PlaySFX();
        TogglePopUp();
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

    private void OnAdsClicked()
    {
        if (_soundManager) _soundManager.PlaySFX();

        // TODO: Call video ads
    }

    private void OnSFXClicked()
    {
        if (_soundManager) _soundManager.PlaySFX();
        if (_soundManager) _soundManager.ToggleSFX();
    }

    private void OnMusicClicked()
    {
        if (_soundManager) _soundManager.PlaySFX();
        if (_soundManager) _soundManager.ToggleMusic();
    }
}
