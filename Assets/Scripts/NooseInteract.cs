using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class NooseInteract : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;
    public UIFader fader;
    public GameObject pressEPrompt;

    [Header("Transition UI")]
    public GameObject transitionCanvas;
    public TMPro.TextMeshProUGUI transitionText;
    public string nextLevelName;

    [Header("Timing")]
    public float fadeTime = 1.2f;
    public float panelShowSeconds = 3f;

    [Header("Disable during fade")]
    public MonoBehaviour[] disableWhileFading;

    private bool inRange;
    private bool consumed;

    private void OnTriggerEnter(Collider other)
    {
        if (!player && other.CompareTag("Player"))
            player = other.transform;

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
        if (Keyboard.current != null)
            pressed = Keyboard.current.eKey.wasPressedThisFrame;
#else
        pressed = Input.GetKeyDown(KeyCode.E);
#endif

        if (pressed)
        {
            consumed = true;
            StartCoroutine(DoTransition());
        }
    }

    private IEnumerator DoTransition()
    {
        if (pressEPrompt) pressEPrompt.SetActive(false);

        foreach (var m in disableWhileFading)
            if (m) m.enabled = false;

        // Fade to black
        if (fader) fader.FadeToBlack(fadeTime);
        yield return new WaitForSecondsRealtime(fadeTime + 0.05f);

        // SAFETY CHECKS (prevents crash)
        if (transitionCanvas == null)
        {
            Debug.LogError("❌ No Transition Canvas assigned!");
            SceneManager.LoadScene(nextLevelName);
            yield break;
        }

        if (transitionText == null)
        {
            Debug.LogError("❌ No Transition Text assigned!");
            SceneManager.LoadScene(nextLevelName);
            yield break;
        }

        if (string.IsNullOrEmpty(nextLevelName))
        {
            Debug.LogError("❌ nextLevelName is EMPTY!");
            yield break;
        }

        // Show the panel
        transitionCanvas.SetActive(true);
        transitionText.text = "Entering Next Level      " + nextLevelName + "...";

        yield return new WaitForSecondsRealtime(panelShowSeconds);

        SceneManager.LoadScene(nextLevelName);
    }
}
