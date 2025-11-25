using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // New Input System
#endif

public class NooseInteract : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;            // Optional: auto-filled from trigger if left null (tag Player)
    public UIFader fader;               // Drag the UIFader on your full-screen black Image
    public GameObject pressEPrompt;     // Optional TMP "Press E" text object

    [Header("Timing")]
    [Tooltip("Seconds to fade to black before showing 'Completed' card.")]
    public float fadeTime = 1.2f;
    [Tooltip("Seconds to hold the 'Level Completed' card before loading screen kicks in.")]
    public float completedCardSeconds = 1.5f;

    [Header("Disable during fade (optional)")]
    [Tooltip("Movement / input scripts to disable during the fade so the player can't move.")]
    public MonoBehaviour[] disableWhileFading;

    private bool inRange;
    private bool consumed;

    private void OnTriggerEnter(Collider other)
    {
        if (!player && other.CompareTag("Player")) player = other.transform;
        if (other.transform == player)
        {
            inRange = true;
            if (pressEPrompt) pressEPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            inRange = false;
            if (pressEPrompt) pressEPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (!inRange || consumed) return;

        bool pressed = false;
        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null) pressed = Keyboard.current.eKey.wasPressedThisFrame;
        #else
        pressed = Input.GetKeyDown(KeyCode.E);
        #endif

        if (pressed)
        {
            consumed = true; // prevent double-activation
            StartCoroutine(FadeAndComplete());
        }
    }

    private IEnumerator FadeAndComplete()
    {
        // Hide prompt & lock controls
        if (pressEPrompt) pressEPrompt.SetActive(false);
        foreach (var m in disableWhileFading) if (m) m.enabled = false;

        // Fade to black (covers transition from gameplay)
        if (fader) fader.FadeToBlack(fadeTime);
        yield return new WaitForSecondsRealtime(fadeTime + 0.05f);

        // IMPORTANT: hand off to LevelManager (shows 'Completed', then Loading + Entering + loads next)
        if (LevelManager.I != null)
        {
            // Clear the black immediately so the Completed/Loading UI is visible above gameplay
            if (fader) fader.SetClearImmediate();

            LevelManager.I.CompleteLevelAndLoadNext(completedCardSeconds);
        }
        else
        {
            Debug.LogError("[NooseInteract] LevelManager.I is null. Make sure LevelManager exists in MainMenu and persists (DontDestroyOnLoad).");
        }
    }
}
