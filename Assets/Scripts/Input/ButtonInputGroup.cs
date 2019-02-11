using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ButtonInputGroup : MonoBehaviour
{
    public string jump_key = "Jump";
    public bool isInput { private set; get; }

    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input

    private void Awake()
    {
        isInput = false;
    }

    public void OnPointerEnterLeftButton()
    {
        if(isInput)
        {
            m_HorizontalVirtualAxis.Update(-1);
        }
    }

    public void OnPointerExitLeftButton()
    {

    }

    public void OnPointerEnterRightButton()
    {
        if(isInput)
        {
            m_HorizontalVirtualAxis.Update(1);
        }
    }

    public void OnPointerExitRightButton()
    {

    }

    public void OnJumpButtonUp()
    {
        CrossPlatformInputManager.SetButtonUp(jump_key);
    }

    public void OnJumButtonDown()
    {
        CrossPlatformInputManager.SetButtonDown(jump_key);
    }

    public void OnPointerDown()
    {
        isInput = true;
    }

    public void OnPointerUp()
    {
        isInput = false;
    }
}
