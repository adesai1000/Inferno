using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    public float contactDamage = 20f;
    public float damageCooldown = 0.5f;

    float timer;

    void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    void OnTriggerStay(Collider other)
    {
        TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        if (timer > 0f) return;
        if (!other.CompareTag("Player")) return;

        
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health == null) return;
        Debug.Log("Hit player, applying contact damage");

        health.ApplyContactDamage(contactDamage);
        timer = damageCooldown;
    }
}
