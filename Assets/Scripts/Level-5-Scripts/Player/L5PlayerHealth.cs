using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class L5PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int startingHealth = 5;
    [SerializeField] CinemachineVirtualCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] shieldBars;
    [SerializeField] GameObject gameOverContainer;

    // BLACK SCREEN + NEXT LEVEL
    [SerializeField] GameObject blackScreen;     // Drag your BlackScreen UI here
    [SerializeField] string nextLevelName;       // Type next scene name here

    int currentHealth;
    int gameOverVirtualCameraPriority = 20;

    void Awake()
    {
        currentHealth = startingHealth;
        AdjustShieldUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        AdjustShieldUI();

        if (currentHealth <= 0)
        {
            PlayerGameOver();
        }
    }

    // =====================
    // NORMAL GAME OVER
    // =====================
    void PlayerGameOver()
    {
        weaponCamera.parent = null;
        deathVirtualCamera.Priority = gameOverVirtualCameraPriority;

        gameOverContainer.SetActive(true);

        StarterAssetsInputs starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        starterAssetsInputs.SetCursorState(false);

        Destroy(gameObject);
    }

    // =====================
    // ADJUST SHIELD UI
    // =====================
    void AdjustShieldUI()
    {
        for (int i = 0; i < shieldBars.Length; i++)
        {
            shieldBars[i].gameObject.SetActive(i < currentHealth);
        }
    }

    // =====================
    // BOSS KILL → BLACK SCREEN → NEXT LEVEL
    // =====================
    public void BossKillPlayer()
    {
        // Camera change
        weaponCamera.parent = null;
        deathVirtualCamera.Priority = gameOverVirtualCameraPriority;

        // Hide regular game over
        gameOverContainer.SetActive(false);

        // Enable BLACK SCREEN instantly
        if (blackScreen != null)
            blackScreen.SetActive(true);

        // Disable player input
        StarterAssetsInputs inputs = FindFirstObjectByType<StarterAssetsInputs>();
        inputs.SetCursorState(false);

        // Load next level safely
        Invoke(nameof(LoadNextLevel), 3f);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
