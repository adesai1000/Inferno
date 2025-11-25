using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3FoodGoalManager : MonoBehaviour
{
    public int requiredGoodItems = 5;
    public int collectedGoodItems = 0;
    public string nextSceneName = "Anger&Violence";
    public bool autoLoadNextScene = true;

    public void RegisterGoodPickup()
    {
        collectedGoodItems++;
        if (autoLoadNextScene && collectedGoodItems >= requiredGoodItems && !string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
