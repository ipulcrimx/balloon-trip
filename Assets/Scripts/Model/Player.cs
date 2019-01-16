using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.15f;
    public float jumpPower = 0.5f;

    public GameObject[] balloons;
    private Rigidbody2D _rigidBody2d;

    private void Awake()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            MoveHorizontal(-moveSpeed);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            MoveHorizontal(moveSpeed);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
#elif UNITY_ANDROID

#endif

    }

    #region Input Methods
    private void MoveHorizontal(float speed)
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0);
    }

    private void Jump()
    {
        _rigidBody2d.AddForce(Vector2.up * jumpPower);
    }
    #endregion
}
