using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSelector : MonoBehaviour
{
    [Header("IS DEBUG")]
    public bool isDebug = true;
    public Transform selectorParent;
    public Vector2 offPosition;
    public Vector2 onPosition;

    [Space]
    public Button toggleButton;
    public Dropdown dropdown;

    public InputType inputType = InputType.Analog;

    private bool _isOpen = false;

    #region Instance
    private static InputSelector _instance = null;

    public static InputSelector instance
    {
        get
        {
            if (!_instance)
            {
                Debug.LogWarning("Instance haven't initiated yet....");
            }

            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        if(!isDebug)
        {
            Debug.Log("------INPUT SELECTOR DISABLED...------");
            Destroy(gameObject);
            Destroy(selectorParent.gameObject);

            return;
        }

        if (_instance)
        {
            Debug.LogWarning("There's already Ads Manager Instance here...\nDestroying other instance...");
            Destroy(_instance.gameObject);
        }

        _instance = this;

        int input = PlayerPrefs.GetInt(Constant.INPUT_TYPE_KEY, 0);
        inputType = (InputType)input;
        dropdown.value = input;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        toggleButton.onClick.AddListener(OnToggleClicked);

        Debug.Log(selectorParent.position);
    }

    private void OnDropdownValueChanged(int inputIndex)
    {
        inputType = (InputType)inputIndex;
    }

    private void OnToggleClicked()
    {
        _isOpen = !_isOpen;

        selectorParent.position = _isOpen ? onPosition : offPosition;
        PlayerPrefs.SetInt(Constant.INPUT_TYPE_KEY, (int)inputType);
    }
}

public enum InputType
{
    none = -1,
    Analog = 0,
    Slider = 1,
    Button = 2
}