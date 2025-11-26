using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public float typingSpeed = 0.03f;
    public float autoCloseTime = 4f;

    public void PlayDialogue(string message)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "";
        StopAllCoroutines();
        StartCoroutine(TypeEffect(message));
    }

    IEnumerator TypeEffect(string message)
    {
        foreach (char c in message)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(autoCloseTime);
        dialoguePanel.SetActive(false);
    }
}
