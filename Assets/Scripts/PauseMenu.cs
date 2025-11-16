using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel; // disabled by default
    [Header("Settings")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private bool _paused;

    void Start()
    {
        SetPaused(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            TogglePause();
    }

    public void TogglePause() => SetPaused(!_paused);

    public void OnResume() => SetPaused(false);

    public void OnRestartLevel()
    {
        Time.timeScale = 1f;
        _paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetPaused(bool value)
    {
        _paused = value;
        if (pausePanel) pausePanel.SetActive(_paused);
        Time.timeScale = _paused ? 0f : 1f;
        // lock cursor only if your gameplay needs it
        Cursor.visible = _paused;
        Cursor.lockState = _paused ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
