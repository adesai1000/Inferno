using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text subText;
    [SerializeField] private Slider progress;

    void Awake()
    {
        if (!group) group = GetComponent<CanvasGroup>();
        Hide();
    }

    public void ShowLoading(string title)
    {
        gameObject.SetActive(true);
        group.alpha = 1f;
        group.blocksRaycasts = true;
        if (titleText) titleText.text = title;
        if (subText) subText.text = "Please wait…";
        if (progress) progress.value = 0f;
    }

    public void SetProgress(float t)
    {
        if (progress) progress.value = Mathf.Clamp01(t);
    }

    public void ShowEntering(string levelTitle)
    {
        gameObject.SetActive(true);
        group.alpha = 1f;
        group.blocksRaycasts = true;
        if (titleText) titleText.text = $"Entering: {levelTitle}";
        if (subText) subText.text = "";
        if (progress) progress.value = 1f;
    }

    public void Hide()
    {
        if (!group) return;
        group.alpha = 0f;
        group.blocksRaycasts = false;
        gameObject.SetActive(false);
    }
    public void ShowCompleted(string levelTitle)    
    {
        gameObject.SetActive(true);
        if (!group) group = GetComponent<CanvasGroup>();
        group.alpha = 1f;
        group.blocksRaycasts = true;

        if (titleText) titleText.text = $"{levelTitle} Completed!";
        if (subText)  subText.text  = "Great job!";
        if (progress) progress.value = 1f; // full bar
    }

}
