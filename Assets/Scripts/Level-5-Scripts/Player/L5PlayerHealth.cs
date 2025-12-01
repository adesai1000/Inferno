using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class L5PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int startingHealth = 5;
    [SerializeField] CinemachineVirtualCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] shieldBars;
    [SerializeField] GameObject gameOverContainer;

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

    void PlayerGameOver()
    {
        weaponCamera.parent = null;
        deathVirtualCamera.Priority = gameOverVirtualCameraPriority;
        gameOverContainer.SetActive(true);
        StarterAssetsInputs starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        starterAssetsInputs.SetCursorState(false);
        Destroy(this.gameObject);
    }

    void AdjustShieldUI()
    {
        for (int i = 0; i < shieldBars.Length; i++)
        {
            if (i < currentHealth) 
            {
                shieldBars[i].gameObject.SetActive(true);
            }
            else 
            {
                shieldBars[i].gameObject.SetActive(false);
            }
        }
    }

    public void BossKillPlayer()
    {
        // Same as normal game over but without showing Game Over UI
        weaponCamera.parent = null;
        deathVirtualCamera.Priority = gameOverVirtualCameraPriority;

        // Turn off normal game-over screen
        gameOverContainer.SetActive(false);

        // Tell GameManager to show YOU WIN — YOU MUST RETURN
        FindFirstObjectByType<GameManager>().PlayerKilledByBoss();

        // Disable player input and remove player
        StarterAssetsInputs inputs = FindFirstObjectByType<StarterAssetsInputs>();
        inputs.SetCursorState(false);

        Destroy(this.gameObject);
    }
}
