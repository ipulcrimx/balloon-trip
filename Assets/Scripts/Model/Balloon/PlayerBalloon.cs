using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerBalloon : MonoBehaviour
{
    public Player player;
    public UnityAction OnBalloonPoppedUp;

    // Use this for initialization
    void Start()
    {
        if (!player)
        {
            player = transform.parent.GetComponent<Player>();

            if (!player)
            {
                Debug.LogError("Player is not found!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemies")
        {
            Debug.Log("Balloon destroyed");
            if (player) player.OnBallonDestroyed();
        }
    }
}