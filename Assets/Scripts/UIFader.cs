using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class UIFader : MonoBehaviour
{
    public float fadeDuration = 1f;
    Image img; Coroutine co;

    void Awake() { img = GetComponent<Image>(); }
    public void FadeToBlack(float dur = -1f)  { StartFade(1f, dur); }
    public void FadeFromBlack(float dur = -1f){ StartFade(0f, dur); }

    void StartFade(float target, float dur)
    {
        if (dur < 0f) dur = fadeDuration;
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(FadeRoutine(target, dur));
    }

    IEnumerator FadeRoutine(float to, float dur)
    {
        float from = img.color.a, t = 0f;
        var c = img.color;
        while (t < dur) { t += Time.deltaTime; c.a = Mathf.Lerp(from, to, t/dur); img.color = c; yield return null; }
        c.a = to; img.color = c; co = null;
    }
}