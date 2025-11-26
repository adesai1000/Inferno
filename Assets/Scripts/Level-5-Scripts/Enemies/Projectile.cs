using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] GameObject projectileHitVFX;

    bool isBossProjectile = false;
    Rigidbody rb;

    int damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    public void Init(int damage) 
    {
        this.damage = damage;
    }


    public void SetBossProjectile(bool value)
    {
        isBossProjectile = value;
    }

    void OnTriggerEnter(Collider other)
{
    L5PlayerHealth playerHealth = other.GetComponent<L5PlayerHealth>();

    if (playerHealth != null)
    {
        if (isBossProjectile)
        {
            playerHealth.BossKillPlayer();
        }
        else
        {
            playerHealth.TakeDamage(damage);
        }

        Instantiate(projectileHitVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
        return;
    }

    // Environment hit → still destroy projectile
    Instantiate(projectileHitVFX, transform.position, Quaternion.identity);
    Destroy(gameObject);
}

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
