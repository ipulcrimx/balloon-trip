using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string jsonLink = "";

    [Space]
    public ControlManager controlManager;
    public AsteroidManager asteroidManager;

    #region Instances
    private static LevelManager _instance = null;
    public static LevelManager instance
    {
        get
        {
            if(!_instance)
            {
                Debug.LogWarning("Level manager hasn't initiated yet...");
            }

            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(!string.IsNullOrEmpty(jsonLink))
        {

        }
        else
        {

        }
    }
}

[System.Serializable]
public class Level
{
    [Header("Enemy")]
    public int totalEnemy;
    public float minEnemySpeed;
    public float maxEnemySpeed;

    [Header("Asteroids")]
    public float minAsteroidInterval;
    public float maxAsteroidInterval;

    [Header("Control")]
    public float jumpPower;
    public float moveSpeed;

    [Header("Background")]
    [Tooltip("directory of background\n(if want to set background per level, let it empty if do not)")]
    public GameObject parallaxBackground;
}