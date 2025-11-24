using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TitleSplash : MonoBehaviour
{
    [Header("Timing")]
    [Tooltip("Seconds to keep the title fully visible before fading out.")]
    [SerializeField] private float holdSeconds = 1.25f;

    [Tooltip("Seconds to fade the title to transparent.")]
    [SerializeField] private float fadeSeconds = 1.0f;

    [Header("Behavior")]
    [Tooltip("Allow skipping the splash by any key / click / gamepad button.")]
    [SerializeField] private bool dismissOnAnyInput = true;

    private CanvasGroup _group;

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        _group.alpha = 1f;
        _group.interactable = false;   // don't block gameplay/UI beneath
        _group.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        // Hold phase – wait for timer or input
        float t = 0f;
        while (t < holdSeconds)
        {
            if (dismissOnAnyInput && AnyInputPressed())
                break;

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // Fade-out phase
        float startA = _group.alpha;
        float f = 0f;
        float dur = Mathf.Max(0.0001f, fadeSeconds);
        while (f < dur)
        {
            f += Time.unscaledDeltaTime;
            _group.alpha = Mathf.Lerp(startA, 0f, f / dur);
            yield return null;
        }

        _group.alpha = 0f;
        gameObject.SetActive(false);
    }

    private bool AnyInputPressed()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        // New Input System
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null && kb.anyKey.wasPressedThisFrame) return true;

        var mouse = UnityEngine.InputSystem.Mouse.current;
        if (mouse != null &&
            (mouse.leftButton.wasPressedThisFrame ||
             mouse.rightButton.wasPressedThisFrame ||
             mouse.middleButton.wasPressedThisFrame)) return true;

        var gp = UnityEngine.InputSystem.Gamepad.current;
        if (gp != null &&
            (gp.startButton.wasPressedThisFrame ||
             gp.selectButton.wasPressedThisFrame ||
             gp.buttonSouth.wasPressedThisFrame ||
             gp.buttonNorth.wasPressedThisFrame ||
             gp.buttonEast.wasPressedThisFrame ||
             gp.buttonWest.wasPressedThisFrame)) return true;

        return false;
#else
        // Legacy Input Manager
        return Input.anyKeyDown ||
               Input.GetMouseButtonDown(0) ||
               Input.GetMouseButtonDown(1) ||
               Input.GetMouseButtonDown(2);
#endif
    }
}
