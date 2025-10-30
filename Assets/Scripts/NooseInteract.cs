using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // New Input System
#endif
using UnityEngine.SceneManagement;

public class NooseInteract : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;            // drag your player root here (or tag Player)
    public UIFader fader;               // drag the UIFader on FadeImage
    public GameObject pressEPrompt;     // optional TMP "Press E" text

    [Header("Behavior")]
    public float fadeTime = 1.2f;
    public bool loadNextScene = false;
    public string nextSceneName = "Level2_Lust";
    public MonoBehaviour[] disableWhileFading; // drag your movement scripts here

    bool inRange;

    void OnTriggerEnter(Collider other)
    {
        if (!player && other.CompareTag("Player")) player = other.transform;
        if (other.transform == player) { inRange = true; if (pressEPrompt) pressEPrompt.SetActive(true); }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player) { inRange = false; if (pressEPrompt) pressEPrompt.SetActive(false); }
    }

    void Update()
    {
        if (!inRange) return;
        bool pressed = false;
        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null) pressed = Keyboard.current.eKey.wasPressedThisFrame;
        #else
        pressed = Input.GetKeyDown(KeyCode.E);
        #endif
        if (pressed) StartCoroutine(FadeAndAct());
    }

    IEnumerator FadeAndAct()
    {
        inRange = false;
        if (pressEPrompt) pressEPrompt.SetActive(false);
        foreach (var m in disableWhileFading) if (m) m.enabled = false;

        if (fader) fader.FadeToBlack(fadeTime);
        yield return new WaitForSeconds(fadeTime + 0.05f);

        if (loadNextScene && !string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        // else stay black / trigger cutscene here
    }
}