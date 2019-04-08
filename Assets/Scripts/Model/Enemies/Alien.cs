using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CustomGravity))]
public class Alien : MonoBehaviour
{
    #region public variables
    public GameObject balloon;

    [Space]
    public bool isFacingRight = true;
    public float moveSpeed;
    [Range(0,3.5f)]
    public float verticalMoveSpeed;

    [Space]
    public float invincibleDuration = 0.75f;
    public float inflatingDelay = 1.5f;
    public float inflatingBalloonDuration = 10;

    [Header("Move Direction Parameters")]
    public Vector2 moveDirection = Vector2.zero;
    public Vector2 nextMoveDirection;
    [Space]
    public float duration;
    public float minDuration;
    public float maxDuration;
    [SerializeField]
    internal AreaType _areaType = AreaType.None;
    [SerializeField]
    internal AreaBoundary _boundary = new AreaBoundary();

    [Space]
    public PhysicsMaterial2D bounceMaterial;
    public PhysicsMaterial2D defaultMaterial;

    [Header("Distance")]
    public float distanceToCenter;

    public UnityAction OnBallonDestroyed = delegate { };
    public UnityAction OnDead = delegate { };
    public UnityAction<Vector2> OnCollideWithObstacle = delegate { };
    #endregion
    #region protected and private variables
    protected bool _hasBalloon = true;
    protected float _moveTimer;
    protected float _inflatingTimer = 0;
    protected float _invincibleTimer = 0;

    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;
    protected CustomGravity _customGrav;
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _customGrav = GetComponent<CustomGravity>();

        _rigidbody.mass = 8;
        _rigidbody.gravityScale = 0;
        _collider.sharedMaterial = bounceMaterial;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnDead += Dead;
        OnBallonDestroyed += BallDestroyed;
        OnCollideWithObstacle += ChangeDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasBalloon)
        {
            if (_moveTimer >= duration || !IsWIthinArea())
            {
                ChangeDuration();
                ChangeDirection(Vector2.zero);
            }
            else
            {
                _moveTimer += Time.deltaTime;
            }

            moveDirection = Vector2.Lerp(moveDirection, nextMoveDirection, (Time.deltaTime * 2.5f) / duration);
            transform.Translate(moveDirection * Time.deltaTime);
        }
        else
        {
            Inflating();
        }

        UpdateArea();
        if (_invincibleTimer <= invincibleDuration)
        {
            _invincibleTimer += Time.deltaTime;
        }
    }
    #region Collide and Collision Method
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag =="Player")
        {
            if (_invincibleTimer <= invincibleDuration)
                return;

            Player pl = col.gameObject.GetComponent<Player>();
            if (pl)
            {
                if (_hasBalloon)
                {
                    pl.OnBallonDestroyed();
                }
                else
                {
                    OnDead();
                }
            }
            else
            {
                Debug.LogWarning("There's no Player component on " + col.gameObject.name + " gameObject.");
            }
        }
        else if(col.gameObject.tag == "Players Balloon")
        {
            PlayerBalloon balloon = col.gameObject.GetComponent<PlayerBalloon>();

            if (balloon)
            {
                balloon.OnBalloonPoppedUp();
            }
            else
            {
                Debug.LogWarning("There's no Player Balloon component on " + col.gameObject.name + " gameObject.");
            }
        }
    }
    #endregion
    #region extended method in update
    protected void Inflating()
    {
        _inflatingTimer += Time.deltaTime;
        if (!balloon.activeInHierarchy)
        {
            balloon.transform.localScale = Vector3.zero;
            balloon.SetActive(true);
        }

        if (_inflatingTimer <= 2)
        {
            // TODO: do nothing?
        }
        else if (_inflatingTimer <= inflatingBalloonDuration + inflatingDelay)
        {
            balloon.transform.localScale = Vector3.one * (_inflatingTimer - inflatingDelay) / inflatingBalloonDuration;
        }
        else
        {
            _hasBalloon = true;
        }
    }

    protected void UpdateArea()
    {
        distanceToCenter = _customGrav.distanceFromCenter;
        if (distanceToCenter <= _boundary.below)
        {
            _areaType = AreaType.Bellow;
        }
        else if (distanceToCenter >= _boundary.above)
        {
            _areaType = AreaType.Above;
        }
        else
        {
            _areaType = AreaType.SafeArea;
        }
    }

    private void ChangeDuration()
    {
        _moveTimer = 0;
        duration = Random.Range(minDuration, maxDuration);
    }

    private void ChangeDirection(Vector2 dir)
    {
        float rndX = 0;
        float rndY = 0;

        switch (_areaType)
        {
            case AreaType.SafeArea: rndY = Random.Range(-verticalMoveSpeed, verticalMoveSpeed); break;
            case AreaType.Bellow: rndY = Random.Range(0, verticalMoveSpeed); break;
            case AreaType.Above: rndY = Random.Range(-verticalMoveSpeed, 0); break;
            default: rndY = Random.Range(-verticalMoveSpeed, verticalMoveSpeed); break;
        }

        if (isFacingRight)
            rndX = Random.Range(0, moveSpeed);
        else
            rndX = Random.Range(-moveSpeed, 0);

        nextMoveDirection = new Vector2(rndX, rndY);
        //Debug.Log("Change direction to " + moveDirection);
    }

    /// <summary>
    /// Check if Alien is still within save area
    /// </summary>
    /// <returns></returns>
    protected bool IsWIthinArea()
    {
        return _customGrav.distanceFromCenter > _boundary.below &&
            _customGrav.distanceFromCenter < _boundary.above;
    }
    #endregion
    #region subscribed method
    private void BallDestroyed()
    {
        _hasBalloon = false;
        balloon.SetActive(false);

        _rigidbody.mass = 1;
        _rigidbody.gravityScale = 1;
        _collider.sharedMaterial = defaultMaterial;
    }

    private void Dead()
    {
        // TODO: write something here when enemy killed...
        Debug.Log("Boom!");

        transform.position = Vector3.one * -10;
        gameObject.SetActive(false);
    }
    #endregion

    private void OnDestroy()
    {
        OnDead = null;
        OnBallonDestroyed = null;
        OnCollideWithObstacle = null;
    }

    [System.Serializable]
    internal class AreaBoundary
    {
        public float below = 65f;
        public float above = 83f;
    }

    internal enum AreaType
    {
        None = -1,
        SafeArea = 0,
        Bellow = 1,
        Above = 2
    }
}
