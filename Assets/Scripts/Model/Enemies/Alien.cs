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
    public float moveSpeed;

    [Space]
    public float invincibleDuration = 0.75f;
    public float inflatingDelay = 1.5f;
    public float inflatingBalloonDuration = 10;

    [Header("Move Direction Parameters")]
    public Vector2 moveDirection = Vector2.zero;
    [Space]
    public float duration;
    public float minDuration;
    public float maxDuration;

    [Space]
    public PhysicsMaterial2D bounceMaterial;
    public PhysicsMaterial2D defaultMaterial;

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
            if (_moveTimer >= duration)
            {
                ChangeDuration();
                ChangeDirection(Vector2.zero);
            }
            else
            {
                _moveTimer += Time.deltaTime;
            }

            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            //_rigidbody.AddForce(moveDirection);

        }
        else
        {
            Inflating();
        }

        //Debug.Log(_customGrav.distanceFromCenter);
        if (_invincibleTimer < invincibleDuration)
        {
            _invincibleTimer += Time.deltaTime;
        }
    }

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

    private void ChangeDuration()
    {
        _moveTimer = 0;
        duration = Random.Range(minDuration, maxDuration);
    }

    private void ChangeDirection(Vector2 dir)
    {
        float rnd = Random.Range(-1f, 1f);

        moveDirection = new Vector2(1, rnd);
        Debug.Log("Change direction to " + moveDirection);
    }

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

    private void OnDestroy()
    {
        OnDead = null;
        OnBallonDestroyed = null;
        OnCollideWithObstacle = null;
    }
}
