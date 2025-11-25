using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("UI/Main Menu UI")]
public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject optionsPanel;   // Assign the OptionsPanel (root of options UI)

    [Header("Main Menu Root (everything to hide)")]
    [SerializeField] private GameObject mainMenuRoot;   // Parent that contains TitleText + Buttons + Background, etc.

    [Header("Fallback (only used if LevelManager is missing)")]
    [SerializeField] private string fallbackGameSceneName = "Level1_Limbo"; // Exact scene name in Build Profiles

    private void Awake()
    {
        // Start with Options hidden and Main Menu shown
        if (optionsPanel != null) optionsPanel.SetActive(false);
        ShowMainMenu(true);

        // Ensure Options is on top if both are active in editor
        if (optionsPanel != null)
            optionsPanel.transform.SetAsLastSibling();

        if (mainMenuRoot == null)
            Debug.LogWarning("[MainMenuUI] mainMenuRoot not assigned. Assign a parent that contains Title/Buttons/Background.");
    }

    // --- Buttons ---

    // Play → start at the first configured level (index 0)
    public void OnPlay()
    {
        // Prefer LevelManager flow (with loading screen & titles)
        if (LevelManager.I != null)
        {
            // Hide menu UI before switching
            ShowMainMenu(false);
            if (optionsPanel) optionsPanel.SetActive(false);

            LevelManager.I.StartGameAt(0); // first level in LevelRegistry
            return;
        }

        // Fallback: direct scene load (no loading UI)
        if (string.IsNullOrWhiteSpace(fallbackGameSceneName))
        {
            Debug.LogError("[MainMenuUI] No LevelManager found and fallbackGameSceneName is empty. Set one in the Inspector.");
            return;
        }

        Debug.LogWarning("[MainMenuUI] LevelManager not found. Using fallback direct scene load.");
        SceneManager.LoadScene(fallbackGameSceneName);
    }

    // Options
    public void OnOptions()
    {
        if (optionsPanel == null)
        {
            Debug.LogError("[MainMenuUI] optionsPanel not assigned.");
            return;
        }

        // Ensure Options renders above other UI and blocks interactions
        optionsPanel.transform.SetAsLastSibling();
        optionsPanel.SetActive(true);

        // Hide the entire main menu (title, buttons, background, etc.)
        ShowMainMenu(false);
    }

    // Quit
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // --- Called by OptionsMenuUI.OnBack() ---
    public void OnOptionsClosed()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        ShowMainMenu(true);
    }

    // --- Helpers ---

    private void ShowMainMenu(bool show)
    {
        if (mainMenuRoot != null) mainMenuRoot.SetActive(show);
    }
}
