using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.15f;
    public float jumpPower = 0.5f;
    public float slowDownModifier = 2f;
    [Space]
    public float invincibleDuration = 1;

    public List<GameObject> balloons = new List<GameObject>();

    private bool _isDead = false;
    private bool _isInput = false;
    private float _invinTimer = 0;
    private Rigidbody2D _rigidBody2d;

    public UnityAction OnBallonDestroyed = delegate { };
    public UnityAction OnPlayerHit = delegate { };

    public int TotalBalloon
    {
        get
        {
            int temp = 0;
            foreach(Transform t in transform)
            {
                if(t.gameObject.activeInHierarchy)
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
        if (InputSelector.instance)
        {
            if (InputSelector.instance.inputType == InputType.Button)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitUntil(() => Joystick.instance != null);

                Joystick.instance.OnStartInput += StartInput;
                Joystick.instance.OnStopInput += StopInput;
            }
        }

        OnPlayerHit += PlayerHit;
        OnBallonDestroyed += BallonHit;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //_isInput = true;
            MoveHorizontal(-moveSpeed);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //_isInput = true;
            MoveHorizontal(moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
#endif
        if (!_isDead)
        {
            Vector2 move = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
            bool isJump = CrossPlatformInputManager.GetButton("Jump");

            if (move.sqrMagnitude >= 0.15f)
            {
                MoveHorizontal(move);
            }

            if (isJump)
            {
                isJump = false;
                Jump();
            }

            if (!_isInput)
            {
                _rigidBody2d.velocity = Vector2.Lerp(_rigidBody2d.velocity, Vector2.zero, Time.deltaTime / slowDownModifier);
            }
        }

        if(_invinTimer < invincibleDuration)
        {
            _invinTimer +=Time.deltaTime;
        }

        if (TotalBalloon <= 0 && !_isDead)
        {
            _isDead = true;
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemies")
        {
            Enemy en = col.gameObject.GetComponent<Enemy>();

            if(en)
            {
                if(en.hasBallon && _invinTimer >= invincibleDuration)
                {
                    OnPlayerHit();
                }
            }
            else
            {
                Debug.LogWarning("There's no enemy component on collided object!");
            }
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

    private void BallonHit()
    {
        if (_invinTimer < invincibleDuration)
            return;

        int index = 0;
        GameObject bal = null;
        for(index = 0; index < balloons.Count;index++)
        {
            if(balloons[index].activeInHierarchy)
            {
                bal = balloons[index];
                break;
            }
        }

        if(bal)
        {
            Debug.Log("Index: " + index);
            bal.transform.SetParent(null);
            balloons.RemoveAt(index);
            bal.SetActive(false);

            _invinTimer = 0;
        }
        else
        {
            PlayerHit();
        }
    }

    public void StartInput()
    {
        _isInput = true;
    }

    public void StopInput()
    {
        _isInput = false;
    }

    [ContextMenu("Player Hit")]
    private void PlayerHit()
    {
        if (_invinTimer < invincibleDuration)
            return;

        OnBallonDestroyed();

        if (TotalBalloon <= 0)
        {
            _isDead = true;
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
