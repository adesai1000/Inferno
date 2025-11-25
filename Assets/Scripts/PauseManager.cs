using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Assign Pause Panel Here")]
    public GameObject pausePanel;

    private bool isPaused = false;

    // Stores all player movement / camera scripts
    private MonoBehaviour[] controlScripts;

    void Start()
    {
        // Auto-find every script in the scene (new Unity API)
        controlScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);   // Hide panel on start
        }
        else
        {
            Debug.LogError("❌ PauseManager ERROR: pausePanel is NOT assigned in the Inspector!");
        }

        // Hide & lock cursor at start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ESC toggles pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    // -------------------------
    // 🚀 PAUSE THE GAME
    // -------------------------
    public void PauseGame()
    {
        if (pausePanel == null)
        {
            Debug.LogError("❌ PauseManager: pausePanel not assigned!");
            return;
        }

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        DisableControls();
    }

    // -------------------------
    // ▶ RESUME GAME
    // -------------------------
    public void ResumeGame()
    {
        if (pausePanel == null)
        {
            Debug.LogError("❌ PauseManager: pausePanel not assigned!");
            return;
        }

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EnableControls();
    }

    // -------------------------
    // 🔄 RESTART LEVEL
    // -------------------------
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // -------------------------
    // 🚪 QUIT TO MAIN MENU
    // -------------------------
    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // -------------------------
    // ❌ DISABLE MOVEMENT/LOOK SCRIPTS
    // -------------------------
    void DisableControls()
    {
        foreach (MonoBehaviour script in controlScripts)
        {
            if (script == null) continue;

            string n = script.GetType().Name;

            // Detect common movement/camera scripts intelligently
            if (n.Contains("Look") || n.Contains("Camera")
             || n.Contains("Move") || n.Contains("Player")
             || n.Contains("Controller"))
            {
                script.enabled = false;
            }
        }
    }

    // -------------------------
    // ✔ ENABLE MOVEMENT/LOOK SCRIPTS
    // -------------------------
    void EnableControls()
    {
        foreach (MonoBehaviour script in controlScripts)
        {
            if (script == null) continue;

            string n = script.GetType().Name;

            if (n.Contains("Look") || n.Contains("Camera")
             || n.Contains("Move") || n.Contains("Player")
             || n.Contains("Controller"))
            {
                script.enabled = true;
            }
        }
    }
}
