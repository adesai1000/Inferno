using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3f;
    public float damage = 10f;

    void Awake() => Destroy(gameObject, life);

    void OnTriggerEnter(Collider other)
    {
        // Optional damage:
        var eh = other.GetComponent<EnemyHealth>() ?? other.GetComponentInParent<EnemyHealth>();
        if (eh) eh.TakeDamage(damage);

        Destroy(gameObject);
    }
}
