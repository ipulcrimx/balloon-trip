using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnemyBalloon : MonoBehaviour
{
    public Enemy enemyParent;

    public UnityAction OnBalloonDestroyed = delegate { };

    // Use this for initialization
    void Start()
    {
        OnBalloonDestroyed += OnBalloonHit;
        enemyParent.OnBallonDestroyed += OnBalloonDestroyed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("Balloon destroyed");
            enemyParent.OnBallonDestroyed();
        }
    }

    private void OnBalloonHit()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        enemyParent.OnBallonDestroyed -= OnBalloonDestroyed;
        OnBalloonDestroyed = null;
    }
}
