using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Invulnerability")]
    public float iFrameDuration = 0.2f;   // brief grace after a hit (seconds)
    private float _iFrameTimer = 0f;

    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action OnDeath;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (_iFrameTimer > 0f) _iFrameTimer -= Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;
        if (_iFrameTimer > 0f) return; // still invulnerable

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
        if (amount <= 0f || currentHealth <= 0f) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        // TODO: disable input / show game over / respawn
        OnDeath?.Invoke();
        Debug.Log("Player died.");
    }

    // Optional: simple API enemies can call
    public void ApplyContactDamage(float amount) => TakeDamage(amount);
}
