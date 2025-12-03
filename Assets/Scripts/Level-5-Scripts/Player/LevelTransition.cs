using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public Animator fadeAnimator;   // Assign Black Screen Animator
    public string nextLevelName;    // Assign next level in Inspector
    public float fadeDelay = 10f;

    public void FadeAndLoadNext()
    {
        fadeAnimator.Play("FadeIn");
        Invoke(nameof(LoadNext), fadeDelay);
    }

    void LoadNext()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
