using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    public float contactDamage = 10f;
    public float repeatDelay = 0.5f; // prevents rapid multi-hits while overlapping

    private float _timer = 0f;

    void Update()
    {
        if (_timer > 0f) _timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_timer > 0f) return;
        if (!other.CompareTag("Player")) return;

        var health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.ApplyContactDamage(contactDamage);
            _timer = repeatDelay;
        }
    }
}
