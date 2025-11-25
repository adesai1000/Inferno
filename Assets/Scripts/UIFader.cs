using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class UIFader : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("Default duration for fade in/out in seconds.")]
    public float fadeDuration = 1f;

    private Image img;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        img = GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("[UIFader] Missing Image component on this object!");
            return;
        }

        // Ensure the image covers the whole screen and starts transparent
        var color = img.color;
        color.a = Mathf.Clamp01(color.a);
        img.color = color;
    }

    /// <summary>
    /// Instantly make the screen black (no animation)
    /// </summary>
    public void SetBlackImmediate()
    {
        if (img == null) return;
        var c = img.color;
        c.a = 1f;
        img.color = c;
    }

    /// <summary>
    /// Instantly make the screen fully transparent (no animation)
    /// </summary>
    public void SetClearImmediate()
    {
        if (img == null) return;
        var c = img.color;
        c.a = 0f;
        img.color = c;
    }

    /// <summary>
    /// Fade from transparent → black.
    /// </summary>
    public void FadeToBlack(float duration = -1f)
    {
        StartFade(1f, duration);
    }

    /// <summary>
    /// Fade from black → transparent.
    /// </summary>
    public void FadeFromBlack(float duration = -1f)
    {
        StartFade(0f, duration);
    }

    private void StartFade(float targetAlpha, float duration)
    {
        if (duration < 0f)
            duration = fadeDuration;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, duration));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        if (img == null)
            yield break;

        float startAlpha = img.color.a;
        float time = 0f;
        Color color = img.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            img.color = color;
            yield return null;
        }

        // Ensure final value is exact
        color.a = targetAlpha;
        img.color = color;
        fadeCoroutine = null;
    }
}
