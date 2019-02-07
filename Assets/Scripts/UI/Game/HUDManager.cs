using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text coinText;
    public Text scoreText;
    public Text killText;

    private GameManager _gameManager = null;

    // Use this for initialization
    void Start()
    {
        _gameManager = GameManager.instance;

        if(PlayerManager.instance) coinText.text = PlayerManager.instance.coin.ToString();
        _gameManager.OnScoreChanged += OnScoreChanged;
        _gameManager.OnKillChanged += OnKillValueChanged;
    }

    private void OnKillValueChanged()
    {
        killText.text = _gameManager.kill.ToString();
    }

    private void OnScoreChanged()
    {
        scoreText.text = _gameManager.score.ToString();
    }

    private void OnDestroy()
    {
        _gameManager.OnScoreChanged -= OnScoreChanged;
        _gameManager.OnKillChanged -= OnKillValueChanged;
    }
}
