using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float NormalizedHealth => maxHealth > 0f ? currentHealth / maxHealth : 0f;

    public NPCSoundController soundController;

    void Start()
    {
        currentHealth = maxHealth;

        if (soundController == null)
        {
            soundController = GetComponent<NPCSoundController>();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP left: {currentHealth}");

        if (soundController != null && currentHealth > 0f)
        {
            soundController.PlayHit();
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (soundController != null)
        {
            soundController.PlayDeath();
        }

        Destroy(gameObject, 2f);   // small delay so scream can play
    }
}