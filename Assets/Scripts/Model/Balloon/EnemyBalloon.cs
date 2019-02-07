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
        if(enemyParent) enemyParent.OnBallonDestroyed += OnBalloonDestroyed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("Balloon destroyed");
            if(enemyParent) enemyParent.OnBallonDestroyed();
        }
        else
        {
            if(enemyParent) enemyParent.OnCollideWithObstacle(col.transform.position);
        }
    }

    private void OnBalloonHit()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if(enemyParent) enemyParent.OnBallonDestroyed -= OnBalloonDestroyed;
        OnBalloonDestroyed = null;
    }
}
