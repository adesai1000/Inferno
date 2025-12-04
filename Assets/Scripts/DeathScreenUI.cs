using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public CanvasGroup canvasGroup;

    bool isVisible;

    void Awake()
    {
        // Auto-grab CanvasGroup if not set in Inspector
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        HideImmediate();
    }

    void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnDeath -= HandleDeath;
    }

    void HandleDeath()
    {
        Debug.Log("DeathScreenUI: HandleDeath called");

        isVisible = true;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        Time.timeScale = 0f;                 // pause game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideImmediate()
    {
        isVisible = false;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
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
