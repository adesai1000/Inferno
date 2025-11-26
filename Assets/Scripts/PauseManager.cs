using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PauseManager : MonoBehaviour
{
    [Header("Assign the Pause Panel (Parent of Resume/Restart/Quit)")]
    public GameObject pausePanel;

    private bool isPaused = false;

    // Scripts we disable during pause
    private MonoBehaviour[] gameplayScripts;

    // NavMesh agents (enemies) to freeze
    private NavMeshAgent[] navAgents;

    void Start()
    {
        if (pausePanel == null)
        {
            Debug.LogError("❌ PauseManager ERROR: pausePanel is not assigned!");
            return;
        }

        pausePanel.SetActive(false);

        // Auto-find all scripts in scene
        gameplayScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        // Auto-find all navmesh agents (enemy movement)
        navAgents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);

        // Start with hidden cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    // -------------------------------
    // PAUSE GAME
    // -------------------------------
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        DisableGameplayScripts();
        FreezeNavAgents();
    }

    // -------------------------------
    // RESUME GAME
    // -------------------------------
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EnableGameplayScripts();
        UnfreezeNavAgents();
    }

    // -------------------------------
    // RESTART LEVEL
    // -------------------------------
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // -------------------------------
    // QUIT TO MAIN MENU
    // -------------------------------
    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // -------------------------------
    // DISABLE ALL NON-UI SCRIPTS
    // -------------------------------
    private void DisableGameplayScripts()
    {
        foreach (var script in gameplayScripts)
        {
            if (script == null) continue;

            // Skip UI scripts
            if (script is UnityEngine.UI.Graphic ||
                script is UnityEngine.EventSystems.UIBehaviour)
                continue;

            string name = script.GetType().Name;

            // Disable common gameplay classes
            if (name.Contains("Move") ||
                name.Contains("Look") ||
                name.Contains("Shoot") ||
                name.Contains("Attack") ||
                name.Contains("Gun") ||
                name.Contains("Weapon") ||
                name.Contains("Player") ||
                name.Contains("Enemy") ||
                name.Contains("AI") ||
                name.Contains("Controller") ||
                name.Contains("Input"))
            {
                script.enabled = false;
            }
        }
    }

    // -------------------------------
    // ENABLE GAMEPLAY SCRIPTS
    // -------------------------------
    private void EnableGameplayScripts()
    {
        foreach (var script in gameplayScripts)
        {
            if (script == null) continue;

            string name = script.GetType().Name;

            if (name.Contains("Move") ||
                name.Contains("Look") ||
                name.Contains("Shoot") ||
                name.Contains("Attack") ||
                name.Contains("Gun") ||
                name.Contains("Weapon") ||
                name.Contains("Player") ||
                name.Contains("Enemy") ||
                name.Contains("AI") ||
                name.Contains("Controller") ||
                name.Contains("Input"))
            {
                script.enabled = true;
            }
        }
    }

    // -------------------------------
    // FREEZE NAVMESH ENEMIES
    // -------------------------------
    private void FreezeNavAgents()
    {
        foreach (var agent in navAgents)
        {
            if (agent == null) continue;
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }
    }

    // -------------------------------
    // UNFREEZE NAVMESH ENEMIES
    // -------------------------------
    private void UnfreezeNavAgents()
    {
        foreach (var agent in navAgents)
        {
            if (agent == null) continue;
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.isStopped = false;
        }
    }
}
