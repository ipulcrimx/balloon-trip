using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PauseButton : MonoBehaviour
{
    public PausePopUpManager pausePopUp;
    private Button _btn;

    // Start is called before the first frame update
    void Start()
    {
        _btn = GetComponent<Button>();

        _btn.onClick.AddListener(() =>
        {
            if(SoundManager.instance) SoundManager.instance.PlaySFX();
            pausePopUp.TogglePopUp();
        });
    }
}
