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
    [SerializeField]
    internal Alien.AreaBoundary interactableArea;
    [Space]
    public float outOfScreenThreshold;
    public GameObject warningPopUp;

    private bool _isDead = false;
    private bool _isOutOfScreen = false;
    private Vector2 _initialPosition;
    private Rigidbody2D _rigidBody2d;
    private CustomGravity _custGravity;
    [Space]
    [Range(0, 5)]
    public float heighThreshold = 2;
    public float decceleratePower = 3;

    private float _outOfScreenTimer = 0;

    [SerializeField]
    private Alien.AreaType _areaType;

    public UnityAction OnBallonDestroyed = delegate { };
    public UnityAction OnPlayerHit = delegate { };
    public UnityAction OnEnterBlackHole = delegate { };
    public UnityAction OnExitBlackHole = delegate { };

    public float distanceFromInitialPosition
    {
        get
        {
            return ((Vector2)transform.position - _initialPosition).magnitude;
        }
    }

    internal Alien.AreaType areaType
    {
        get { return _areaType; }
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

        OnEnterBlackHole += delegate ()
        {
            _rigidBody2d.gravityScale = 0.15f;
        };

        OnExitBlackHole += delegate ()
        {
            _rigidBody2d.gravityScale = 1.7f;
        };
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
                transform.position = Vector2.Lerp
                (
                    transform.position,
                    new Vector2 (_initialPosition.x, transform.position.y),
                    Time.deltaTime / backToPositionSpeed
                );
        }

        if(_isOutOfScreen)
        {
            _outOfScreenTimer += Time.deltaTime;
            _rigidBody2d.gravityScale = 3.5f;

            if(!warningPopUp.activeInHierarchy)
            {
                warningPopUp.SetActive(true);
            }

            if(_outOfScreenTimer >= outOfScreenThreshold)
            {
                _outOfScreenTimer = 0;

                OnBallonDestroyed();
            }
        }
        else
        {
            _outOfScreenTimer = 0;
            _rigidBody2d.gravityScale = 1.7f;
            if (warningPopUp.activeInHierarchy)
            {
                warningPopUp.SetActive(false);
            }
        }

        if(_custGravity.distanceFromCenter <= 50)
        {
            Destroy(gameObject);
        }

        if (_custGravity.distanceFromCenter + heighThreshold >= interactableArea.above)
        {
            _rigidBody2d.velocity = Vector2.Lerp(_rigidBody2d.velocity, Vector2.zero, Time.deltaTime * decceleratePower);
        }

        UpdateArea();

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
        //if (_isOutOfScreen)
        //    return;

        if (TotalBalloon <= 0)
        {
            _isDead = true;
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            OnBallonDestroyed();
        }
    }

    protected void UpdateArea()
    {
        float dist = _custGravity.distanceFromCenter;
        if (dist <= interactableArea.below)
        {
            _areaType = Alien.AreaType.Bellow;
        }
        else if (dist >= interactableArea.above)
        {
            _areaType = Alien.AreaType.Above;
        }
        else
        {
            _areaType = Alien.AreaType.SafeArea;
        }
    }

    private void OnBecameInvisible()
    {
        _isOutOfScreen = true;
    }

    private void OnBecameVisible()
    {
        _isOutOfScreen = false;
    }
}