using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BlackHole : MonoBehaviour
{
    public float moveSpeed;
    [Space]
    public float suckArea = 15f;
    public float suckPower;

    private GameManager _gameManager;
    private AsteroidManager _asteroidManager;

    private Player _player;
    private Alien[] _aliens;
    private GameObject[] _asteroids
    {
        get
        {
            return _asteroidManager.asteroids;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float move = CrossPlatformInputManager.GetAxis("Horizontal");
        transform.Translate(Vector3.right * move * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {

        }
        else if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemies")
        {

        }
        else if (col.gameObject.tag == "Asteroid")
        {

        }
        else
        {

        }
    }
}