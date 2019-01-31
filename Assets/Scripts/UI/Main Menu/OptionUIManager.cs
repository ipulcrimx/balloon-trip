using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUIManager : MonoBehaviour
{
    public GameObject buttonGroup;

    [Header("Buttons")]
    public Button toggleButton;
    public Button musicButton;
    public Button sfxButton;

    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        _soundManager = SoundManager.instance;
        buttonGroup.SetActive(false);

        toggleButton.onClick.AddListener(() => { OnToggleClicked(); });
        musicButton.onClick.AddListener(() => { OnMusicButtonClicked(); });
        sfxButton.onClick.AddListener(() => { OnSFXButtonClicked(); });
    }

    private void OnSFXButtonClicked()
    {
        _soundManager.PlaySFX();
        _soundManager.ToggleSFX();
    }

    private void OnMusicButtonClicked()
    {
        _soundManager.PlaySFX();
        _soundManager.ToggleMusic();
    }

    private void OnToggleClicked()
    {
        _soundManager.PlaySFX();
        buttonGroup.SetActive(!buttonGroup.activeInHierarchy);
    }
}
