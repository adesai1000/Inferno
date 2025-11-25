using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public CanvasGroup canvasGroup;

    bool isVisible;

    void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    void OnEnable()
    {
        if (playerHealth != null) playerHealth.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        if (playerHealth != null) playerHealth.OnDeath -= HandleDeath;
    }

    void HandleDeath()
    {
        isVisible = true;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!isVisible) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }
}
