using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy[] enemies;

    private bool _hasGameOver = false;
    private int _live;

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

    private void Awake()
    {
        _instance = this;
        _live = Constant.startingLive;

        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        AnalyticsManager.instance.DonePlaying(0);
    }

    private void OnApplicationQuit()
    {
        AnalyticsManager.instance.DonePlaying(0);
    }
}
