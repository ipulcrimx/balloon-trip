using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Tooltip("Get json data from link")]
    public string jsonLink = "";
    [Tooltip("If json link is empty, will search json from this directory")]
    public string jsonDirectory = "";

    [Space]
    public ControlManager controlManager;
    public AsteroidManager asteroidManager;
    public EnemyPooler enemyPooler;

    public UnityAction OnLevelInit = delegate { };
    public UnityAction OnGoToNextLevel = delegate { };

    private string _json;
    private int _levelIndex;
    private LevelArray _levels;

    private Level _currentLevel
    {
        get
        {
            return _levels.level[_levelIndex];
        }
    }

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

    private void Awake()
    {
        _instance = this;

        DontDestroyOnLoad(gameObject);

        OnLevelInit += OnInitLevel;
        OnGoToNextLevel += GoToNextLevel;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(jsonLink))
        {

        }
        else if (!string.IsNullOrEmpty(jsonDirectory))
        {
            TextAsset txt = Resources.Load(jsonDirectory) as TextAsset;
            _json = txt.text;
        }
        else
        {
            Debug.LogError("JSON link and JSON directory is empty! Please fill it so the game can obtain level info");
            return;
        }

        // need confirmation...!
        _levels = JsonUtility.FromJson(_json, typeof(LevelArray)) as LevelArray;
    }

    public void SetLevel(int lvl)
    {
        _levelIndex = lvl;
    }

    private void GoToNextLevel()
    {
        _levelIndex++;

        SceneManager.LoadScene("Game");
        OnInitLevel();
    }

    public void OnInitLevel()
    {
        enemyPooler = EnemyPooler.instance;
        controlManager = ControlManager.instance;
        asteroidManager = AsteroidManager.instance;

        enemyPooler.InitEnemy(_currentLevel.totalEnemy, _currentLevel.minEnemySpeed, _currentLevel.maxEnemySpeed);
        controlManager.InitControlLevel(_currentLevel.moveSpeed, _currentLevel.jumpPower);
        asteroidManager.InitLevelAsteroid(_currentLevel.minAsteroidInterval, _currentLevel.maxAsteroidInterval, _currentLevel.minScale, _currentLevel.maxScale);
    }

    public class LevelArray
    {
        public Level[] level;
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
    public float minScale;
    public float maxScale;

    [Header("Control")]
    public float jumpPower;
    public float moveSpeed;

    [Header("Background")]
    [Tooltip("Resources directory of background\n(if want to set background per level, let it empty if do not)")]
    public GameObject parallaxBackground;
}