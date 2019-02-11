using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class InputSelectorManager : MonoBehaviour
{
    [Header("Joystick parameter")]
    public Joystick joystick;
    public Sprite analogSprite;
    public Sprite sliderSprite;
    [Space]
    public Vector2 analogMaxAnchor;
    public Vector2 sliderMaxAnchor;

    [Space]
    public GameObject joystickParent;
    public GameObject buttonParent;

    #region Instance
    private static InputSelectorManager _instance = null;

    public static InputSelectorManager instance
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
        _instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!joystickParent || !buttonParent)
        {
            Debug.LogWarning("One of parent input is null!");
            return;
        }


        if (InputSelector.instance !=null)
        {
            InputType type = InputSelector.instance.inputType;
            switch(type)
            {
                case InputType.Analog:
                    joystick.axesToUse = Joystick.AxisOption.Both;
                    joystick.ResetVirtualAxes(Joystick.AxisOption.Both);
                    joystick.GetComponent<Image>().sprite = analogSprite;
                    joystick.GetComponent<RectTransform>().anchorMax = analogMaxAnchor;
                    buttonParent.SetActive(false);
                    break;
                case InputType.Slider:
                    joystick.axesToUse = Joystick.AxisOption.OnlyHorizontal;
                    joystick.ResetVirtualAxes(Joystick.AxisOption.OnlyHorizontal);
                    joystick.GetComponent<Image>().sprite = sliderSprite;
                    joystick.GetComponent<RectTransform>().anchorMax = sliderMaxAnchor;
                    buttonParent.SetActive(false);
                    break;
                case InputType.Button:
                    joystickParent.SetActive(false);
                    buttonParent.SetActive(true);
                    break;
                default:
                    joystick.axesToUse = Joystick.AxisOption.Both;
                    joystick.GetComponent<Image>().sprite = analogSprite;
                    buttonParent.SetActive(false);
                    break;
            }
        }
        else
        {
            Debug.LogWarning("There's no input selector. Using default input type...");
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}
