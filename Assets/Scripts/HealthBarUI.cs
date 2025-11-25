using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Slider slider;

    [Header("Smoothing")]
    public float lerpSpeed = 10f;
    private float _targetValue;

    void Start()
    {
        if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth>();
        if (slider == null) slider = GetComponent<Slider>();

        slider.minValue = 0f;
        slider.maxValue = playerHealth.maxHealth;
        slider.value = playerHealth.currentHealth;
        _targetValue = slider.value;

        playerHealth.OnHealthChanged += HandleHealthChanged;
        playerHealth.OnDeath += HandleDeath;
    }

    void Update()
    {
        // Smooth bar
        slider.value = Mathf.Lerp(slider.value, _targetValue, Time.deltaTime * lerpSpeed);
    }

    private void HandleHealthChanged(float current, float max)
    {
        if (slider.maxValue != max) slider.maxValue = max;
        _targetValue = current;
    }

    private void HandleDeath()
    {
        // Optional: show "YOU DIED" etc.
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
            playerHealth.OnDeath -= HandleDeath;
        }
    }
}
