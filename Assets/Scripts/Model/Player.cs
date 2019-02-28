using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float jumpPower = 0.5f;
    public List<GameObject> balloons = new List<GameObject>();
    [Space]
    public float thresholdPosition;
    public float backToPositionSpeed;

    private bool _isDead = false;
    private Vector2 _initialPosition;
    private Rigidbody2D _rigidBody2d;
    private CustomGravity _custGravity;

    public UnityAction OnBallonDestroyed = delegate { };
    public UnityAction OnPlayerHit = delegate { };

    public float distanceFromInitialPosition
    {
        get
        {
            return ((Vector2)transform.position - _initialPosition).magnitude;
        }
    }

    public int TotalBalloon
    {
        get
        {
            int temp = 0;
            foreach (Transform t in transform)
            {
                if (t.gameObject.activeInHierarchy)
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
        _custGravity = GetComponent<CustomGravity>();
    }

    // Start is called before the first frame update
    void Start()
    {
        OnPlayerHit += PlayerHit;
        OnBallonDestroyed += BallonHit;
    }

    // Update is called once per frame
    void Update()
    {
        if (TotalBalloon <= 0 && !_isDead)
        {
            _isDead = true;
            GetComponent<Collider2D>().isTrigger = true;
        }

        if (!_custGravity.isDisturbed && Mathf.Abs(transform.position.x - _initialPosition.x) > thresholdPosition)
        {
                transform.position = Vector2.Lerp(
                transform.position,
                new Vector2
                (_initialPosition.x, transform.position.y),
                Time.deltaTime / backToPositionSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemies")
        {
            Enemy en = col.gameObject.GetComponent<Enemy>();

            if (en)
            {
                OnPlayerHit();
            }
            else
            {
                Debug.LogWarning("There's no enemy component on collided object!");
            }
        }
        else if(col.gameObject.tag == "Asteroid")
        {
            OnPlayerHit();
        }
    }

    public void DoJump(float jumpPower)
    {
        _rigidBody2d.AddForce(Vector2.up * jumpPower);
    }

    private void BallonHit()
    {
        int index = 0;
        GameObject bal = null;
        for (index = 0; index < balloons.Count; index++)
        {
            if (balloons[index].activeInHierarchy)
            {
                bal = balloons[index];
                break;
            }
        }

        if (bal)
        {
            bal.transform.SetParent(null);
            balloons.RemoveAt(index);
            bal.SetActive(false);
        }
        else
        {
            PlayerHit();
        }
    }

    [ContextMenu("Player Hit")]
    private void PlayerHit()
    {
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