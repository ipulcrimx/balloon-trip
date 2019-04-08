using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class WarningCountdown : MonoBehaviour
{
    public string suffixText = " seconds more, you ballon will popped";
    public Text countDownText;

    private bool _isOpen = false;

    private Player _player;
    private CanvasGroup _canvasGroup;

    public void Init()
    {
        _isOpen = false;

        _canvasGroup = GetComponent<CanvasGroup>();
        _player = GameManager.instance.player;

        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
    }

    private void Update()
    {
        if (_player && _player.TotalBalloon <= 0)
        {
            _isOpen = false;
            ToggleWarning(false);
        }
        else
        {
            if (_isOpen != _player.isOutOfScreen)
            {
                ToggleWarning(_player.isOutOfScreen);
            }
        }

        if(_isOpen)
        {
            SetCountdown(_player.outOfScreenTimer);
        }
    }


    public void SetCountdown(float sec)
    {
        countDownText.text = sec.ToString("0.0") + suffixText;
    }

    public void SetCountdown(string sec)
    {
        countDownText.text = sec + suffixText;
    }

    public void ToggleWarning(bool isActive)
    {
        _isOpen = isActive;

        _canvasGroup.alpha = _isOpen ? 1 : 0;
        _canvasGroup.interactable = _isOpen;
    }
}
