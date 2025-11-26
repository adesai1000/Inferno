using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelEndTrigger : MonoBehaviour
{
    public GameObject pressEText;
    public GameObject transitionCanvas;
    public TextMeshProUGUI transitionText;
    public string nextLevelName;

    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressEText.SetActive(true);
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressEText.SetActive(false);
            playerInside = false;
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        pressEText.SetActive(false);

        transitionCanvas.SetActive(true);
        transitionText.text = "Entering " + nextLevelName + "...";

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(nextLevelName);
    }
}
