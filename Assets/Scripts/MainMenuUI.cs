using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("UI/Main Menu UI")]
public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject optionsPanel;   // Assign the OptionsPanel (root of options UI)

    [Header("Main Menu Root (everything to hide)")]
    [SerializeField] private GameObject mainMenuRoot;   // Parent that contains TitleText + Buttons + Background, etc.

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "Game"; // Exact scene name in Build Settings

    private void Awake()
    {
        // Start with Options hidden and Main Menu shown
        if (optionsPanel != null) optionsPanel.SetActive(false);
        ShowMainMenu(true);

        // (Optional) Make sure Options is on top if both are active in editor
        if (optionsPanel != null)
            optionsPanel.transform.SetAsLastSibling();
    }

    // --- Buttons ---

    // Play
    public void OnPlay()
    {
        if (string.IsNullOrWhiteSpace(gameSceneName))
        {
            Debug.LogError("[MainMenuUI] gameSceneName is empty. Set it in the Inspector.");
            return;
        }

        // Make sure the scene is added to Build Settings
        SceneManager.LoadScene(gameSceneName);
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
        else Debug.LogWarning("[MainMenuUI] mainMenuRoot not assigned. Assign a parent that contains Title/Buttons/Background.");
    }
}
