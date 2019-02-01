﻿using UnityEngine.Monetization;
using UnityEngine;
using System.Collections;

public class AdsManager : MonoBehaviour
{
    [SerializeField]
#if UNITY_IOS
    private string gameId = "3027967";
#elif UNITY_ANDROID
    private string gameId = "3027966";
#endif
    public bool testMode = true;

    #region Instance Region
    private static AdsManager _instace = null;

    public static AdsManager instance
    {
        get
        {
            if(!_instace)
            {
                Debug.LogWarning("AdsManager's instance is null!");
            }

            return _instace;
        }
    }
#endregion

    private void Awake()
    {
        _instace = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Monetization.Initialize(gameId, testMode);
    }

    private IEnumerator ShowSkipableVideoWhenReady()
    {
        while (!Monetization.IsReady("video"))
        {
            yield return new WaitForSeconds(0.25f);
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent("video") as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show();
        }
    }

    private IEnumerator ShowRewardedVideoWhenReady()
    {
        while (!Monetization.IsReady("rewardedVideo"))
        {
            yield return new WaitForSeconds(0.25f);
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent("rewardedVideo") as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show(OnRewardedFinished);
        }
    }

    void OnRewardedFinished(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            // Reward the player
        }
    }

    [ContextMenu("Show Ads")]
    public void ShowAds()
    {
        int rnd = Random.Range(0, 100);

        if (rnd % 2 == 0)
        {
            StartCoroutine(ShowRewardedVideoWhenReady());
            Debug.Log("Showing Rewarded Video");
        }
        else
        {
            StartCoroutine(ShowSkipableVideoWhenReady());
            Debug.Log("Showing skipable video");
        }
    }
}
