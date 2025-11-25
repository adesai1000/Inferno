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

    void OnTriggerStay(Collider other)
    {
        if (timer > 0f) return;
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null) return;

        health.ApplyContactDamage(contactDamage);
        timer = damageCooldown;
    }
}
