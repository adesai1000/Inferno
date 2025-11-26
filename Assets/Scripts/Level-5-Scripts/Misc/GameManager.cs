using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text enemiesLeftText;
    [SerializeField] GameObject youWinText;

    int enemiesLeft = 0;

    const string ENEMIES_LEFT_STRING = "Enemies Left: ";

    // Event triggered when all turrets are destroyed (only boss left)
    public System.Action OnAllTurretsDestroyed;

    public void AdjustEnemiesLeft(int amount)
    {
        enemiesLeft += amount;
        enemiesLeftText.text = ENEMIES_LEFT_STRING + enemiesLeft.ToString();

        // If only the boss is left, trigger event
        if (enemiesLeft == 1)
        {
            OnAllTurretsDestroyed?.Invoke();
        }

        // Win condition (should trigger only after boss defeat if you add that later)
        if (enemiesLeft <= 0)
        {
            youWinText.SetActive(true);
        }
    }

    public void RestartLevelButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void QuitButton()
    {
        Debug.LogWarning("Does not work in the Unity Editor!  You silly goose!");
        Application.Quit();
    }

    public void PlayerKilledByBoss()
    {
        youWinText.SetActive(true);   // This already references your UI
    }
}
