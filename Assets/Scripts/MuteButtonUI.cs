using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MuteButtonUI : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;                 // drag your mixer
    [SerializeField] private string volumeParam = "MasterVolume"; // exposed parameter name

    [Header("UI (optional)")]
    [SerializeField] private Button button;                   // auto-fills if left empty
    [SerializeField] private TextMeshProUGUI label;           // shows Mute/Unmute
    [SerializeField] private Image iconOn;                    // shown when unmuted
    [SerializeField] private Image iconOff;                   // shown when muted

    [Header("Settings")]
    [SerializeField, Range(0f,1f)] private float defaultVolume = 0.8f; // used if no saved volume
    [SerializeField] private float muteDb = -80f;             // how “silent” mute is in dB

    const string PREF_MUTED = "pref_muted";
    const string PREF_VOL   = "pref_master_vol";

    bool isMuted;
    float lastLinearVol = 0.8f;

    void Awake()
    {
        // Auto-wire missing UI refs to avoid NullReferenceException
        if (!button) button = GetComponent<Button>();
        if (!label)  label  = GetComponentInChildren<TextMeshProUGUI>(true);

        if (!mixer)
        {
            Debug.LogError("[MuteButtonUI] AudioMixer not assigned. Please drag your mixer.");
            enabled = false;
            return;
        }

        if (!button)
        {
            Debug.LogError("[MuteButtonUI] Button component not found. Put this script on a UI Button.");
            enabled = false;
            return;
        }

        button.onClick.AddListener(ToggleMute);
    }

    void Start()
    {
        // Load saved state
        isMuted = PlayerPrefs.GetInt(PREF_MUTED, 0) == 1;

        // Load last volume to restore after unmute
        lastLinearVol = Mathf.Clamp01(PlayerPrefs.GetFloat(PREF_VOL, defaultVolume));

        ApplyState();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt(PREF_MUTED, isMuted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyState();
    }

    void ApplyState()
    {
        if (isMuted)
        {
            // store current volume (if any) before muting
            if (mixer.GetFloat(volumeParam, out float currentDb))
            {
                float lin = Mathf.Pow(10f, currentDb / 20f);
                if (lin > 0.0001f) lastLinearVol = lin;
            }

            mixer.SetFloat(volumeParam, muteDb);
            UpdateUI("Unmute", false);
        }
        else
        {
            // restore last volume
            float clamped = Mathf.Clamp01(lastLinearVol);
            float db = Mathf.Log10(Mathf.Max(clamped, 0.0001f)) * 20f;
            mixer.SetFloat(volumeParam, db);
            PlayerPrefs.SetFloat(PREF_VOL, clamped);
            UpdateUI("Mute", true);
        }
    }

    void UpdateUI(string labelText, bool soundOn)
    {
        if (label) label.text = labelText;
        if (iconOn)  iconOn.enabled  = soundOn;
        if (iconOff) iconOff.enabled = !soundOn;
    }
}
