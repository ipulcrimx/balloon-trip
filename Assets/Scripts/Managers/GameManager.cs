using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy[] enemies;

    private GamePhase _phase;
    private AnalyticsManager _analytics = null;

    public long score { private set; get; }
    public int kill { private set; get; }
    public int obtainedCoin
    {
        get
        {
            if(_phase != GamePhase.GameOver)
            {
                Debug.LogWarning("The game is not over...");
                return 0;
            }
            else
            {
                int coin = Mathf.FloorToInt(score * Constant.SCORE_TO_COIN);
                float rndBonus = Random.Range(0, 0.6f);

                return Mathf.RoundToInt(coin * (1 + rndBonus));
            }
        }
    }

    public UnityAction OnScoreChanged = delegate { };
    public UnityAction OnKillChanged = delegate { };
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

        //OnGameOver += OnGameIsOver;
        OnGameClear += OnLevelIsClear;

        foreach(Enemy en in enemies)
        {
            en.OnBallonDestroyed += OnEnemyBalloonPoppedUp;
            en.OnEnemyKilled += OnEnemyKilled;
        }
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

        if (player == null)
        {
            _phase = GamePhase.GameOver;
        }

        if (GetActiveEnemyCount() <= 0)
        {
            _phase = GamePhase.GameClear;
        }
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

    private void OnEnemyBalloonPoppedUp()
    {
        score += Constant.SCORE_PER_BALLON;

        OnScoreChanged();
    }

    private void OnEnemyKilled()
    {
        score += Constant.SCORE_FINISHING_HIT;
        kill++;

        OnScoreChanged();
        OnKillChanged();
    }

    private void OnEnemyEnterPitfall()
    {
        score += Constant.SCORE_PITFALL;
        kill++;

        OnScoreChanged();
        OnKillChanged();
    }

    private void OnDisable()
    {
        if(_analytics) _analytics.DonePlaying(PlayerManager.instance.coin, kill);

        enemies = new Enemy[0];
    }

    private void OnApplicationQuit()
    {
        if (_analytics) _analytics.DonePlaying(PlayerManager.instance.coin, kill);

        enemies = new Enemy[0];
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
