using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public GameObject balloon;

    public UnityAction OnBallonDestroyed = delegate { };
    public UnityAction OnEnemyKilled = delegate { };

    private bool _hasBalloon = true;
    private Rigidbody2D _rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _rigidBody2D.mass = 15;
        _rigidBody2D.gravityScale = 0;

        OnEnemyKilled += Boom;
        OnBallonDestroyed += BallDestroyed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player pl = col.gameObject.GetComponent<Player>();

            if (pl)
            {
                if (_hasBalloon)
                {
                }
                else
                {
                    OnEnemyKilled();
                }
            }
            else
            {
                Debug.LogWarning("There's no Player component on " + col.gameObject.name + " gameObject.");
            }
        }
    }

    private void BallDestroyed()
    {
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

    private void OnDestroy()
    {
        OnBallonDestroyed = null;
        OnEnemyKilled = null;
    }
}