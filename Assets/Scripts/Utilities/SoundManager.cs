using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public AudioClip[] musics;
    public AudioClip sfx;

    private bool _isMusicPlay = true;
    private bool _isSFXPlay = true;
    private int _currentBGMPlaying = -1;
    private AudioSource _source;

    private static SoundManager _instance = null;

    public static SoundManager instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogWarning("Sound Manager instance is null!");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            Debug.LogWarning("There's already Ads Manager Instance here...\nDestroying itself...");
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        _isMusicPlay = PlayerPrefs.GetInt(Constant.MUSIC_KEY, 1) == 1;
        _isSFXPlay = PlayerPrefs.GetInt(Constant.SFX_KEY, 1) == 1;

        _source = GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        if (!_isSFXPlay)
            return;

        if (!sfx)
            return;

        _source.PlayOneShot(sfx);
    }

    public void PlayBGM(int index)
    {
        if (!_isMusicPlay)
            return;

        if (_source.isPlaying)
            return;

        if (index == _currentBGMPlaying)
            return;

        if (musics.Length <= 0)
            return;

        _source.clip = musics[index];
        _source.Play();

        _currentBGMPlaying = index;
    }

    public void ToggleSFX()
    {
        _isSFXPlay = !_isSFXPlay;
        PlayerPrefs.SetInt(Constant.SFX_KEY, _isSFXPlay ? 1 : 0);
    }

    public void ToggleMusic()
    {
        _isMusicPlay = !_isMusicPlay;
        PlayerPrefs.SetInt(Constant.MUSIC_KEY, _isMusicPlay ? 1 : 0);
    }

    public void BalloonHitSound()
    {
        if (!_isSFXPlay)
            return;
    }

    public void EnemyKilledSound()
    {
        if (!_isSFXPlay)
            return;
    }

    public void GoToPitfallSound()
    {
        if (!_isSFXPlay)
            return;
    }
}
