using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public GameObject balloon;

    [Space]
    public float moveSpeed = 3;
    public Vector2 moveDirection = Vector2.zero;

    [Space]
    public float duration;
    public float minDurationChange;
    public float maxDurationChange;


    public UnityAction OnBallonDestroyed = delegate { };
    public UnityAction OnEnemyKilled = delegate { };
    public UnityAction<Vector2> OnCollideWithObstacle = delegate { };

    private bool _hasBalloon = true;
    private float _timer;
    private Rigidbody2D _rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _rigidBody2D.mass = 15;
        _rigidBody2D.gravityScale = 0;

        OnEnemyKilled += Boom;
        OnBallonDestroyed += BallDestroyed;
        OnCollideWithObstacle += ChangeDirection;
    }

    private void Update()
    {
        if(_hasBalloon)
        {
            if(_timer >= duration)
            {
                _timer = 0;
                ChangeDuration();
                ChangeDirection(Vector2.zero);
            }
            else
            {
                _timer += Time.deltaTime;
            }

            transform.position += (Vector3)moveDirection * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player pl = col.gameObject.GetComponent<Player>();

            if (pl)
            { 
                OnEnemyKilled();
            }
            else
            {
                Debug.LogWarning("There's no Player component on " + col.gameObject.name + " gameObject.");
            }
        }
        else
        {
            OnCollideWithObstacle(col.transform.position);
        }
    }

    private void BallDestroyed()
    {
        Debug.Log("Asu!");

        _hasBalloon = false;
        balloon.SetActive(false);

        _hasBalloon = false;

        _rigidBody2D.mass = 1;
        _rigidBody2D.gravityScale = 1;
    }

    private void Boom()
    {
        // TODO: write something here when enemy killed...
        Debug.Log("Boom!");
    }

#if UNITY_EDITOR
    [ContextMenu("Change Direction")]
    public void Change()
    {
        ChangeDirection(Vector2.zero);
    }
#endif

    private void ChangeDirection(Vector2 collidePosition)
    {
        float rndAngle;
        Vector2 pos = transform.position;

        // if enemy didn't collide with something
        if (collidePosition == Vector2.zero)
        {
            rndAngle = Random.Range(0f, 360f);
            Debug.Log("Random Angle: " + rndAngle);
        }
        else
        {
            Vector2 colPos = collidePosition - pos;
            float angle = GetAngleBetween(collidePosition, pos);

            rndAngle = Random.Range(angle - 120, angle + 120) + 180;

            Debug.Log("Random Angle after collide with something: " + rndAngle);
        }

        float x = Mathf.Sin(rndAngle);
        float y = Mathf.Cos(rndAngle);

        moveDirection = new Vector2(x, y) * moveSpeed;
    }

    private float GetAngleBetween(Vector2 pos1, Vector2 pos2)
    {
        // TODO: calculate angle here...
        return 0;
    }

    private void ChangeDuration()
    {
        duration = Random.Range(minDurationChange, maxDurationChange);
    }

    private void OnDestroy()
    {
        OnBallonDestroyed = null;
        OnEnemyKilled = null;
    }
}