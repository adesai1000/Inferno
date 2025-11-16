using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuUI : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;           // expose a float param named "MasterVolume"
    [SerializeField] private string mixerVolumeParam = "MasterVolume";
    [SerializeField] private Slider volumeSlider;             // range 0..1

    [Header("Video")]
    [SerializeField] private TMP_Dropdown resolutionDropdown; // TMP dropdown
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Navigation")]
    [SerializeField] private GameObject selfPanel;            // This panel (for Back button)
    [SerializeField] private MainMenuUI mainMenu;             // Reference to MainMenuUI for returning to main screen

    private Resolution[] _resolutions;

    private const string PREF_VOL = "pref_master_vol";
    private const string PREF_FS  = "pref_fullscreen";
    private const string PREF_RES = "pref_res_index";

    private void Awake()
    {
        // Populate resolutions list
        _resolutions = Screen.resolutions;
        if (resolutionDropdown)
        {
            resolutionDropdown.ClearOptions();
            var options = new List<string>(_resolutions.Length);
            foreach (var r in _resolutions)
            {
                float hz = (float)r.refreshRateRatio.numerator / r.refreshRateRatio.denominator;
                options.Add($"{r.width} x {r.height} @ {hz:0.#}Hz");
            }
            resolutionDropdown.AddOptions(options);

            int savedIndex = Mathf.Clamp(PlayerPrefs.GetInt(PREF_RES, GetCurrentResolutionIndex()), 0, _resolutions.Length - 1);
            resolutionDropdown.SetValueWithoutNotify(savedIndex);
        }

        // Fullscreen toggle state
        bool fsSaved = PlayerPrefs.GetInt(PREF_FS, Screen.fullScreen ? 1 : 0) == 1;
        if (fullscreenToggle) fullscreenToggle.SetIsOnWithoutNotify(fsSaved);

        // Volume
        float volSaved = PlayerPrefs.GetFloat(PREF_VOL, 0.8f);
        if (volumeSlider) volumeSlider.SetValueWithoutNotify(volSaved);
        ApplyVolume(volSaved);

        // Apply saved resolution and fullscreen
        ApplyResolution(PlayerPrefs.GetInt(PREF_RES, GetCurrentResolutionIndex()), fsSaved);
    }

    // --- UI Event Hooks ---

    public void SetVolume(float linear01)
    {
        PlayerPrefs.SetFloat(PREF_VOL, Mathf.Clamp01(linear01));
        ApplyVolume(linear01);
    }

    public void SetResolution(int index)
    {
        PlayerPrefs.SetInt(PREF_RES, Mathf.Clamp(index, 0, _resolutions.Length - 1));
        ApplyResolution(index, fullscreenToggle ? fullscreenToggle.isOn : Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt(PREF_FS, isFullscreen ? 1 : 0);
        int idx = resolutionDropdown ? resolutionDropdown.value : GetCurrentResolutionIndex();
        ApplyResolution(idx, isFullscreen);
    }

    public void OnBack()
    {
        PlayerPrefs.Save();

        // Hide Options panel
        if (selfPanel) selfPanel.SetActive(false);

        // Notify Main Menu UI to show again
        if (mainMenu != null)
            mainMenu.OnOptionsClosed();
        else
            Debug.LogWarning("[OptionsMenuUI] MainMenuUI reference not assigned!");
    }

    // --- Internal Helpers ---

    private void ApplyVolume(float linear01)
    {
        if (!audioMixer || string.IsNullOrEmpty(mixerVolumeParam)) return;

        // Map [0..1] to decibels safely
        float x = Mathf.Clamp(linear01, 0.0001f, 1f);
        float dB = Mathf.Log10(x) * 20f; // 1 -> 0 dB, 0.5 -> -6 dB, etc.
        audioMixer.SetFloat(mixerVolumeParam, dB);
    }

    private void ApplyResolution(int index, bool fullscreen)
    {
        if (_resolutions == null || _resolutions.Length == 0) return;

        index = Mathf.Clamp(index, 0, _resolutions.Length - 1);
        var r = _resolutions[index];
        var rr = r.refreshRateRatio;

        // Unity 6 uses the new RefreshRate struct
        Screen.SetResolution(
            r.width,
            r.height,
            fullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed,
            new RefreshRate { numerator = rr.numerator, denominator = rr.denominator }
        );
    }

    private int GetCurrentResolutionIndex()
    {
        var cur = Screen.currentResolution;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            var r = _resolutions[i];
            if (r.width == cur.width &&
                r.height == cur.height &&
                r.refreshRateRatio.numerator == cur.refreshRateRatio.numerator &&
                r.refreshRateRatio.denominator == cur.refreshRateRatio.denominator)
            {
                return i;
            }
        }
        return Mathf.Clamp(_resolutions.Length - 1, 0, int.MaxValue);
    }
}
