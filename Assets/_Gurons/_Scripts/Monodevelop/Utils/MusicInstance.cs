using UnityEngine;

public class MusicInstance : MonoBehaviour
{
    private static MusicInstance _instance;
    public static MusicInstance instance
    {
        get { return _instance; }
    }

    [SerializeField] private AudioSource _source;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool _IsPlaying
    {
        get { return _source.isPlaying; }
    }

    public void _Pause()
    {
        _source.Stop();
    }

    public void _Play()
    {
        _source.Play();
    }
}
