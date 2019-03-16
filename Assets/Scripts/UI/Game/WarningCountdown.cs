using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningCountdown : MonoBehaviour
{
    public string suffixText = " seconds more, you ballon will popped";
    public Text countDownText;

    private bool _isOpen = false;

    public void Init()
    {
        _isOpen = false;
        gameObject.SetActive(false);
    }


    public void SetCountdown(float sec)
    {
        countDownText.text = sec.ToString("0.0") + suffixText;
    }

    public void SetCountdown(string sec)
    {
        countDownText.text = sec + suffixText;
    }

    public void ToggleWarning()
    {
        _isOpen = !_isOpen;
        gameObject.SetActive(_isOpen);
    }
}
