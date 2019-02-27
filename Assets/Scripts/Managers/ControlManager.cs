using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ControlManager : MonoBehaviour
{
    [Range(0,1)]
    public float minimalThreshold;
    public float playerJumpPower;
    public float maximumSpeed;
    public float currentSpeed;
    public Vector2 inputValue;
    [Space]
    [Range(0, 10)]
    public float slowDownSpeed;

    public Player player;
    public Transform world;


    private Joystick _joystick;
    private bool _isInput;
    
    #region Instance
    private static ControlManager _instance = null;

    public static ControlManager instance
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
        if(_instance)
        {
            Debug.LogWarning("Duplicate of ControlManager detected....\nDestroying itslef");
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        _joystick = Joystick.instance;

        if(InputSelector.instance)
        {
            if(InputSelector.instance.inputType != InputType.Button)
            {
                if (_joystick)
                {
                    _joystick.OnStartInput += delegate ()
                    {
                        _isInput = true;
                    };

                    _joystick.OnStopInput += delegate ()
                    {
                        _isInput = false;
                    };
                }
            }
        }
    }

    private void FixedUpdate()
    {
        inputValue = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), 0);
        bool isJump = CrossPlatformInputManager.GetButton("Jump");

        if(inputValue.sqrMagnitude >= minimalThreshold)
        {
            RotateTheWorld();
        }

        if(isJump)
        {
            isJump = false;
            PlayerJump();
        }

        if(!_isInput)
        {
            inputValue = Vector2.zero;
            SlowTheWorldDown();
        }
    }

    private void PlayerJump()
    {
        player.DoJump(playerJumpPower);
    }

    private void RotateTheWorld()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, maximumSpeed * inputValue.x, Time.deltaTime);
        world.Rotate(new Vector3(0, 0, 1), currentSpeed);
    }

    private void SlowTheWorldDown()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime / slowDownSpeed);
        world.Rotate(new Vector3(0, 0, 1), currentSpeed);
    }
}
