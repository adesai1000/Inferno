using UnityEngine;

public class BlackScreenController : MonoBehaviour
{
    public GameObject salvationText;   // Assign your TMP text here

    void OnEnable()
    {
        // Hide text when black screen turns on
        if (salvationText != null)
            salvationText.SetActive(false);

        // Show the text after 1.5 seconds
        Invoke(nameof(ShowText), 1.5f);
    }

    void ShowText()
    {
        if (salvationText != null)
            salvationText.SetActive(true);
    }
}
