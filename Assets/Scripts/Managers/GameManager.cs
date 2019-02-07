using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy[] enemies;

    private GamePhase _phase;
    private AnalyticsManager _analytics = null;

    public UnityAction OnGameOver = delegate { };
    public UnityAction OnGameClear = delegate { };

    #region Instance
    private static GameManager _instance = null;

    public static GameManager instance
    {
        get
        {
            if(!_instance)
            {
                Debug.LogWarning("Instance haven't initiated yet....");
            }

            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        _instance = this;

        _phase = GamePhase.PreGame;
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        _analytics = AnalyticsManager.instance;
        _phase = GamePhase.InGame;

        OnGameOver += OnGameIsOver;
        OnGameClear += OnLevelIsClear;
    }

    // Update is called once per frame
    void Update()
    {
        if (_phase == GamePhase.GameOver)
        {
            OnGameOver();
        }
        else if (_phase == GamePhase.GameClear)
        {
            OnGameClear();
        }

        if (player == null || player.TotalBalloon <= 0)
        {
            _phase = GamePhase.GameOver;
        }

        if (GetActiveEnemyCount() <= 0)
        {
            _phase = GamePhase.GameClear;
        }
    }

    private void OnGameIsOver()
    {
    }

    private void OnLevelIsClear()
    {

    }

    private int GetActiveEnemyCount()
    {
        int temp = 0;
        foreach(Enemy e in enemies)
        {
            if(e.gameObject.activeInHierarchy)
            {
                temp++;
            }
        }

        return temp;
    }

    private void OnDestroy()
    {
        if(_analytics) _analytics.DonePlaying(0);
    }

    private void OnApplicationQuit()
    {
        if(_analytics) _analytics.DonePlaying(0);
    }

    internal enum GamePhase
    {
        None = -1,
        PreGame = 0,
        InGame = 1,
        GameOver = 2,
        GameClear = 3
    }
}
