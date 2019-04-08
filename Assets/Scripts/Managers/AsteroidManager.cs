using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Header("Random Interval Parameter")]
    public float nextSpawn;
    public float minInterval;
    public float maxInterval;

    [Header("Random scale Parameter")]
    public float minScale;
    public float maxScale;

    public GameObject asteroidClone;
    public GameObject[] asteroids;

    [Space]
    public Transform asteroidParent;
    public Transform[] spawPositions;

    private int _positionIndex;
    private float _intervalTimer;
    internal static AsteroidManager instance;

    public void InitLevelAsteroid(float minInterval, float maxInterval, float minScale, float maxScale)
    {
        this.minInterval = minInterval;
        this.maxInterval = maxInterval;
        this.minScale = minScale;
        this.maxScale = maxScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(_intervalTimer <= nextSpawn)
        {
            _intervalTimer += Time.deltaTime;
        }
        else
        {
            CallAsteroid();
        }
    }

    [ContextMenu("Call Asteroid")]
    public void CallAsteroid()
    {
        Asteroid asteroid = null;

        foreach(GameObject go in asteroids)
        {
            if(!go.activeInHierarchy)
            {
                asteroid = go.GetComponent<Asteroid>();
                break;
            }
        }

        if (!asteroid)
        {
            GameObject temp = Instantiate(asteroidClone);
            asteroid = temp.GetComponent<Asteroid>();

            asteroid.OnStartShoot += OnStartShot;
        }

        asteroid.Shoot();
        RandomizeNextInterval();
    }

    private void OnStartShot(Transform tr)
    {
        tr.parent = asteroidParent;
        tr.position = spawPositions[_positionIndex].position;
        tr.localScale = Vector3.one * Random.Range(minScale, maxScale);
    }

    private void RandomizeNextInterval()
    {
        _positionIndex = Random.Range(0, spawPositions.Length);
        nextSpawn = Random.Range(minInterval, maxInterval);
        _intervalTimer = 0;
    }
}
