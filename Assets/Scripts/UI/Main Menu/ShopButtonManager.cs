using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopButtonManager : MonoBehaviour
{
    public MainMenuUIManager mainMenu;
    private Button _btn;

    // Start is called before the first frame update
    void Start()
    {
        _btn = GetComponent<Button>();

        _btn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySFX();
            mainMenu.shopPopUp.TogglePopUp();
        });
    }
}
