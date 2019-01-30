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

    protected bool _hasBalloon = true;
    protected float _timer;
    protected Rigidbody2D _rigidBody2D;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _rigidBody2D.mass = 8;
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
                ChangeDuration();
                ChangeDirection(Vector2.zero);
            }
            else
            {
                _timer += Time.deltaTime;
            }

            //transform.position += (Vector3)moveDirection * Time.deltaTime;
            _rigidBody2D.AddForce(moveDirection);
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
        else if(col.gameObject.tag == "Players Balloon")
        {
            PlayerBalloon balloon = col.gameObject.GetComponent<PlayerBalloon>();

            if(balloon)
            {
                balloon.OnBalloonPoppedUp();
            }
            else
            {
                Debug.LogWarning("There's no Player Balloon component on " + col.gameObject.name + " gameObject.");
            }
        }
        else
        {
            OnCollideWithObstacle(col.contacts[0].point);
        }
    }

    protected void BallDestroyed()
    {
        _hasBalloon = false;
        balloon.SetActive(false);

        _hasBalloon = false;

        _rigidBody2D.mass = 1;
        _rigidBody2D.gravityScale = 1;
    }

    protected void Boom()
    {
        // TODO: write something here when enemy killed...
        Debug.Log("Boom!");
    }

    protected void ChangeDirection(Vector2 collidePosition)
    {
        float rndAngle;
        Vector2 pos = transform.position;

        // if enemy didn't collide with something
        if (collidePosition == Vector2.zero)
        {
            rndAngle = Random.Range(0f, 360f);
            //Debug.Log("Random Angle: " + rndAngle);
        }
        else
        {
            Vector2 colPos = collidePosition - pos;
            float angle = GetAngleBetween(colPos);

            rndAngle = Random.Range(angle - 60, angle + 60) + 180;
            Debug.Log("Random Angle after collide with something: " + rndAngle);

            _timer = 0;
            ChangeDuration();
        }

        float x = Mathf.Sin(rndAngle);
        float y = Mathf.Cos(rndAngle);

        moveDirection = new Vector2(x, y) * moveSpeed;
    }

    protected float GetAngleBetween(Vector2 pos)
    {
        float angle = Vector2.Angle(pos, Vector2.up);
        Debug.LogFormat("Position: {0}, collide position: {1} - ({3}), angle: {2}", transform.position, pos, angle, transform.position + (Vector3)pos);

        //Debug.DrawLine(transform.position, (Vector3)pos + transform.position);
        //Debug.Break();

        return angle;
    }

    protected virtual void ChangeDuration()
    {
        _timer = 0;
        duration = Random.Range(minDurationChange, maxDurationChange);
    }

    private void OnDestroy()
    {
        OnBallonDestroyed = null;
        OnEnemyKilled = null;
    }
}