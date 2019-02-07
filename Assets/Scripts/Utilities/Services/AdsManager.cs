using UnityEngine.Monetization;
using UnityEngine;
using System.Collections;

public class AdsManager : MonoBehaviour
{
    public int frequencyAds = 2;

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

    private int _callCounter = 0;

    private void Awake()
    {
        if(_instace)
        {
            Debug.LogWarning("There's already Ads Manager Instance here...\nDestroying itself...");
            Destroy(gameObject);
            return;
        }

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
    public void ShowAds(AdsType type = AdsType.Random)
    {
        _callCounter++;
        if (_callCounter % frequencyAds != 0)
            return;

        switch(type)
        {
            case AdsType.Random:
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
                break;
            case AdsType.Skipable:
                StartCoroutine(ShowSkipableVideoWhenReady());
                Debug.Log("Showing skipable video");
                break;
            case AdsType.Rewarded:
                StartCoroutine(ShowRewardedVideoWhenReady());
                Debug.Log("Showing Rewarded Video");
                break;
            default:
                StartCoroutine(ShowSkipableVideoWhenReady());
                Debug.Log("Showing skipable video");
                break;
        }
    }
}

public enum AdsType
{
    None = -1,
    Random = 0,
    Skipable = 1,
    Rewarded = 2
}