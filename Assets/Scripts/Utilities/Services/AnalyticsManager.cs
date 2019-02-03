using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{

    #region Instance Region
    private static AnalyticsManager _instace = null;

    public static AnalyticsManager instance
    {
        get
        {
            if (!_instace)
            {
                Debug.LogWarning("AdsManager's instance is null!");
            }

            return _instace;
        }
    }
    #endregion

    /// <summary>
    /// How many games played within a session
    /// </summary>
    private int _timesPlayed;
    private bool _isPlaying = false;
    private float _playTime = 0;

    private void Awake()
    {
        if (_instace)
        {
            Debug.LogWarning("There's already Analytics Manager Instance here...\nDestroying itself...");
            Destroy(gameObject);
            return;
        }

        _instace = this;
    }

    public void StartPlaying(int coin)
    {
        _timesPlayed++;
        _isPlaying = true;
        Analytics.CustomEvent("Game", new Dictionary<string, object>
        {
            { "played", _timesPlayed },
            { "coin", coin}
        });
    }


    public void LevelClear(int coin, int enemyKilled = -1)
    {
        Analytics.CustomEvent("Game", new Dictionary<string, object>
        {
            {"timePlayed", _playTime },
            {"coin", coin }
        });

        if (enemyKilled > 0)
        {
            Analytics.CustomEvent("Game", new Dictionary<string, object>
            {
                {"enemyKilled", enemyKilled }
            });
        }
    }

    public void DonePlaying(int coin, int enemyKilled = -1)
    {
        _isPlaying = false;
        Analytics.CustomEvent("Game", new Dictionary<string, object>
        {
            {"timePlayed", _playTime },
            {"coin", coin }
        });

        if (enemyKilled > 0)
        {
            Analytics.CustomEvent("Game", new Dictionary<string, object>
            {
                {"enemyKilled", enemyKilled }
            });
        }

        _timesPlayed = 0;
    }

    public void BuyingIAP()
    {

    }

    private void Update()
    {
        if (_isPlaying)
        {
            _playTime += Time.deltaTime;
        }
    }
}