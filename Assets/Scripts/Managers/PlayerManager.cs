using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public int coin { private set; get; }
    #region Instance
    private static PlayerManager _instance = null;

    public static PlayerManager instance
    {
        get
        {
            if (!_instance)
            {
                Debug.LogWarning("Instance haven't initiated yet....");
            }

            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        coin = PlayerPrefs.GetInt(Constant.COIN_KEY, 0);
    }

    // Use this for initialization
    void Start()
    {

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(Constant.COIN_KEY, coin);
    }
}
