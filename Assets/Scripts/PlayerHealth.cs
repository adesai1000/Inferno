using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Invulnerability")]
    public float iFrameDuration = 0.2f;
    private float _iFrameTimer = 0f;

    [Header("Death UI")]
    public GameObject pausePanel;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    bool isDead;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isDead = false;
    }

    void Update()
    {
        if (_iFrameTimer > 0f) _iFrameTimer -= Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;
        if (_iFrameTimer > 0f) return;
        if (isDead) return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        _iFrameTimer = iFrameDuration;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        if (currentHealth <= 0f) return;
        if (isDead) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        OnDeath?.Invoke();
        Debug.Log("Player died.");

        if (pausePanel != null) pausePanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ApplyContactDamage(float amount) => TakeDamage(amount);
}
