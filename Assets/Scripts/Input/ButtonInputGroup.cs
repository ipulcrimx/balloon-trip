using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ButtonInputGroup : MonoBehaviour
{
    public string jump_key = "Jump";
    public bool isInput { private set; get; }

    private string horizontalAxisName = "Horizontal";
    private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input

    private void Awake()
    {
        isInput = false;
    }

    private void OnEnable()
    {
        m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
    }

    private void Update()
    {
        if(!isInput)
        {
            m_HorizontalVirtualAxis.Update(0);
        }
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

    public void OnLeftButtonDown()
    {
        isInput = true;
        m_HorizontalVirtualAxis.Update(-1);
    }

    public void OnLeftButtonUp()
    {
        isInput = false;
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

    public void OnRightButtonDown()
    {
        isInput = true;
        m_HorizontalVirtualAxis.Update(1);
    }

    public void OnRightButtonUp()
    {
        isInput = false;
    }

    public void OnJumpButtonUp()
    {
        CrossPlatformInputManager.SetButtonUp(jump_key);
    }

    public void OnJumButtonDown()
    {
        CrossPlatformInputManager.SetButtonDown(jump_key);
    }

    private void OnDisable()
    {
        m_HorizontalVirtualAxis.Remove();
    }
}
