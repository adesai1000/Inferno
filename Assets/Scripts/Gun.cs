using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Empty child at the muzzle (in front of the barrel).")]
    public Transform bulletSpawnPoint;
    [Tooltip("Prefab with Sphere (MeshRenderer), SphereCollider (IsTrigger ON), Rigidbody, Bullet.cs")]
    public GameObject bulletPrefab;

    [Header("Firing")]
    [Tooltip("Units per second applied to the bullet along spawnPoint.forward")]
    public float bulletSpeed = 40f;
    [Tooltip("Shots per second (0 = no limit)")]
    public float fireRate = 0f;

    private float _cooldown;
    private Collider _gunCollider; // so we can ignore the first collision with the gun

    void Awake()
    {
        _gunCollider = GetComponent<Collider>();
        if (!bulletSpawnPoint) Debug.LogError("[Gun] Missing bulletSpawnPoint.");
        if (!bulletPrefab) Debug.LogError("[Gun] Missing bulletPrefab.");
    }

    void Update()
    {
        if (_cooldown > 0f) _cooldown -= Time.deltaTime;

        // Click to fire once
        if (Input.GetMouseButtonDown(0)) TryFire();

        // Hold-to-fire if fireRate > 0
        if (fireRate > 0f && Input.GetMouseButton(0)) TryFire();
    }

    private void TryFire()
    {
        if (_cooldown > 0f) return;
        Fire();
        if (fireRate > 0f) _cooldown = 1f / fireRate;
    }

    private void Fire()
    {
        if (!bulletSpawnPoint || !bulletPrefab) return;

        // Spawn bullet at the muzzle
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Ignore first collision with the gun
        var bcol = bullet.GetComponent<Collider>();
        if (bcol && _gunCollider) Physics.IgnoreCollision(bcol, _gunCollider, true);

        // Propel the bullet forward
        var rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.linearVelocity = bulletSpawnPoint.forward * bulletSpeed; // <-- fixed
            GetComponent<GunRecoil>()?.ApplyRecoil();
        }
        else
        {
            Debug.LogError("[Gun] Bullet prefab needs a Rigidbody.");
        }
    }
}
