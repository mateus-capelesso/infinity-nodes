using UnityEngine;

public class SoundController : MonoBehaviour
{
    #region SINGLETON

    private static SoundController _instance;

    public static SoundController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SoundController>();
            }

            return _instance;
        }
    }

    #endregion
    
    [SerializeField] public AudioSource source;
    public AudioClip click;
    public AudioClip win;

    private AudioClip _currentClip;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = false;
        
        GameManager.Instance.OnPuzzleOver += WinSound;
    }

    public void PieceSound()
    {
        if (!source.isPlaying)
        {
            _currentClip = click;
            Play();
        }
    }

    public void WinSound()
    {
        _currentClip = win;
        Play();
    }

    private void Play()
    {
        source.loop = false;
        source.clip = _currentClip;
        source.Play();
    }
}
