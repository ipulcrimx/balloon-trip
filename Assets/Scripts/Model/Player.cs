using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.15f;
    public float jumpPower = 0.5f;
    public float slowDownModifier = 2f;

    public GameObject[] balloons;
    private Rigidbody2D _rigidBody2d;

    private bool _isInput = false;

    public int TotalBalloon
    {
        get
        {
            int temp = 0;
            foreach(Transform t in transform)
            {
                if(t.GetComponent<PlayerBalloon>())
                {
                    temp++;
                }
            }

            return temp;
        }
    }

    private void Awake()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(()=> Joystick.instance != null);

        Joystick.instance.OnStartInput += () =>
        {
            _isInput = true;
        };

        Joystick.instance.OnStopInput += () =>
         {
             _isInput = false;
         };
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            //_isInput = true;
            MoveHorizontal(-moveSpeed);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            //_isInput = true;
            MoveHorizontal(moveSpeed);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
#endif
        Vector2 move = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
        bool isJump = CrossPlatformInputManager.GetButton("Jump");

        if (move.sqrMagnitude >= 0.15f)
        {
            MoveHorizontal(move);
        }

        if(isJump)
        {
            isJump = false;
            Jump();
        }

        if(!_isInput)
        {
            _rigidBody2d.velocity = Vector2.Lerp(_rigidBody2d.velocity, Vector2.zero, Time.deltaTime / slowDownModifier);
        }
    }

    #region Input Methods
    private void MoveHorizontal(float speed)
    {
        //transform.position += new Vector3(speed * Time.deltaTime, 0);
        _rigidBody2d.AddForce(Vector2.right*speed);
    }

    private void MoveHorizontal(Vector2 direction)
    {
        //transform.position += moveSpeed * Time.deltaTime * (Vector3)direction;
        _rigidBody2d.AddForce(direction * moveSpeed);
    }

    private void Jump()
    {
        //_rigidBody2d.velocity = Vector2.zero;
        _rigidBody2d.AddForce(Vector2.up * jumpPower);
    }
    #endregion
}
