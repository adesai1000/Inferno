using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager I { get; private set; }

    [Header("Data")]
    [SerializeField] private LevelRegistry registry;  // Drag LevelRegistry.asset here

    [Header("UI")]
    [SerializeField] private LoadingUI loadingUI;     // Drag LoadingPanel here

    public int CurrentIndex { get; private set; } = -1;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    // Called from MainMenuUI when clicking Play
    public void StartGameAt(int index = 0)
    {
        if (registry == null || registry.levels.Length == 0)
        {
            Debug.LogError("[LevelManager] LevelRegistry is not assigned or empty!");
            return;
        }

        index = Mathf.Clamp(index, 0, registry.levels.Length - 1);
        CurrentIndex = index;
        StartCoroutine(LoadLevelRoutine(index, true));
    }

    // Called by LevelExit trigger at the end of each level
    public void LoadNextLevel()
    {
        int next = CurrentIndex + 1;

        if (next >= registry.levels.Length)
        {
            Debug.Log("[LevelManager] Last level reached — returning to Main Menu.");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        CurrentIndex = next;
        StartCoroutine(LoadLevelRoutine(next, true));
    }

    // Reload current level (optional)
    public void ReloadCurrent()
    {
        if (CurrentIndex < 0)
        {
            Debug.LogError("[LevelManager] No current level to reload.");
            return;
        }

        StartCoroutine(LoadLevelRoutine(CurrentIndex, false));
    }

    public void CompleteLevelAndLoadNext(float completeCardSeconds = 1.5f)
{
    if (registry == null || registry.levels == null || registry.levels.Length == 0)
    {
        Debug.LogError("[LevelManager] Registry not set.");
        return;
    }

    // Safety for index
    int idx = Mathf.Clamp(CurrentIndex, 0, registry.levels.Length - 1);
    var info = registry.levels[idx];

    StartCoroutine(Co_CompleteThenNext(info.title, completeCardSeconds));
}

private IEnumerator Co_CompleteThenNext(string justFinishedTitle, float holdSeconds)
{
    // 1) Show "Level X Completed!"
    if (loadingUI) loadingUI.ShowCompleted(justFinishedTitle);
    yield return new WaitForSecondsRealtime(holdSeconds);

    // 2) Load the NEXT level with your normal flow
    int next = CurrentIndex + 1;

    if (next >= registry.levels.Length)
    {
        // No next level – go to Main Menu or stay here
        if (loadingUI) loadingUI.ShowEntering("All Levels Complete!");
        yield return new WaitForSecondsRealtime(1.2f);
        if (loadingUI) loadingUI.Hide();
        SceneManager.LoadScene("MainMenu");
        yield break;
    }

    CurrentIndex = next;
    // Reuse your existing loader: shows "Loading …" and then "Entering: …"
    StartCoroutine(LoadLevelRoutine(next, showEntering: true));
}


    // -------------------------------------------------------
    // 📦 The main coroutine that loads scenes with progress
    // -------------------------------------------------------
    private IEnumerator LoadLevelRoutine(int idx, bool showEntering)
    {
        if (registry == null)
        {
            Debug.LogError("[LevelManager] Missing LevelRegistry reference!");
            yield break;
        }

        if (idx < 0 || idx >= registry.levels.Length)
        {
            Debug.LogError("[LevelManager] Invalid level index " + idx);
            yield break;
        }

        var info = registry.levels[idx];
        if (string.IsNullOrWhiteSpace(info.scene))
        {
            Debug.LogError("[LevelManager] Scene name is empty in LevelRegistry!");
            yield break;
        }

        // Check if scene is in Build Profiles
        if (!Application.CanStreamedLevelBeLoaded(info.scene))
        {
            Debug.LogError($"[LevelManager] Scene '{info.scene}' not found in Build Profiles!");
            yield break;
        }

        // Show loading screen
        if (loadingUI != null)
            loadingUI.ShowLoading($"Loading {info.title}...");

        // Begin async loading
        AsyncOperation op = SceneManager.LoadSceneAsync(info.scene, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        // Fake minimum load time for smoother visuals
        float timer = 0f;
        float minTime = 0.5f;

        while (!op.isDone)
        {
            timer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            if (loadingUI != null)
                loadingUI.SetProgress(progress);

            if (op.progress >= 0.9f && timer >= minTime)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }

        // After scene load
        if (showEntering && loadingUI != null)
        {
            loadingUI.ShowEntering(info.title);
            yield return new WaitForSecondsRealtime(1.2f);
        }

        if (loadingUI != null)
            loadingUI.Hide();
    }
}
