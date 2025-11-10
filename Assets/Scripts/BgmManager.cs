using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // looped, playOnAwake = false
    [SerializeField] private AudioClip lobbyClip;
    [SerializeField] private float defaultVolume = 0.6f;

    private const string PREF_MUTED = "bgm_muted";
    private const string PREF_VOL   = "bgm_vol";

    public bool IsMuted { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!audioSource) audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        IsMuted = PlayerPrefs.GetInt(PREF_MUTED, 0) == 1;
        float vol = PlayerPrefs.GetFloat(PREF_VOL, defaultVolume);
        audioSource.volume = vol;
        audioSource.mute = IsMuted;

        if (lobbyClip && !audioSource.isPlaying)
        {
            audioSource.clip = lobbyClip;
            audioSource.Play();
        }
    }

    public void ToggleMute()
    {
        IsMuted = !IsMuted;
        audioSource.mute = IsMuted;
        PlayerPrefs.SetInt(PREF_MUTED, IsMuted ? 1 : 0);
        PlayerPrefs.Save();
        OnMuteChanged?.Invoke(IsMuted);
    }

    public void SetVolume(float v)
    {
        v = Mathf.Clamp01(v);
        audioSource.volume = v;
        PlayerPrefs.SetFloat(PREF_VOL, v);
        PlayerPrefs.Save();
    }

    public System.Action<bool> OnMuteChanged;
}
