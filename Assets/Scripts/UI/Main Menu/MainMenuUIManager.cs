using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button playButton;
    public Button leaderBoardButton;
    public Button creditButton;

    [Header("Pop ups")]
    public CreditPopUpManager creditPopUp;
    public ShopPopUpManager shopPopUp;
    public SkinPopUpManager skinPopUp;

    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        _soundManager = SoundManager.instance;

        // Lazy init here...
        creditPopUp.Init();
        shopPopUp.Init();
        skinPopUp.Init();

        // button listener initiation here...
        playButton.onClick.AddListener(() => { OnPlayButtonClicked(); });
        leaderBoardButton.onClick.AddListener(() => { OnLeaderboardButtonClicked(); });
        creditButton.onClick.AddListener(() => { OnCreditButtonClicked(); });
    }

    public void OnPlayButtonClicked()
    {
        _soundManager.PlaySFX();
        SceneManager.LoadScene("Game");
    }

    public void OnLeaderboardButtonClicked()
    {
        _soundManager.PlaySFX();

        // TODO: open leaderboard...
    }

    public void OnCreditButtonClicked()
    {
        _soundManager.PlaySFX();
        creditPopUp.TogglePopUp();
    }
}
