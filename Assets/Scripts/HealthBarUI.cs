using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider slider;

    void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += HandleHealthChanged;
            HandleHealthChanged(playerHealth.currentHealth, playerHealth.maxHealth);
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    void HandleHealthChanged(float current, float max)
    {
        if (slider == null) return;
        slider.minValue = 0f;
        slider.maxValue = max;
        slider.value = current;
    }
}
