using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public float fadeDuration = 1f;
    private Image img;

    void Awake() { img = GetComponent<Image>(); }

    void Start() { StartCoroutine(Fade(1, 0)); } // fade from black at start

    public void FadeToBlack() { StartCoroutine(Fade(0, 1)); }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0;
        Color c = img.color;
        c.a = from; img.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / fadeDuration);
            img.color = c;
            yield return null;
        }

        c.a = to; img.color = c;
    }
}
