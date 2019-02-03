using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopUpManager : MonoBehaviour
{
    public Button closeButton;

    private bool _isOpen = false;

    public void Init()
    {
        closeButton.onClick.AddListener(() => { TogglePopUp(); });
    }

    internal void TogglePopUp()
    {
        SoundManager.instance.PlaySFX();

        _isOpen = !_isOpen;
        gameObject.SetActive(_isOpen);
    }
}
