using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 40f;
    public float fireRate = 10f;
    public AudioSource audioSource;
    public ParticleSystem muzzleFlash;
    public GunRecoil recoil;

    float _cooldown;
    Collider _gunCollider;

    void Awake()
    {
        _gunCollider = GetComponent<Collider>();
        if (!recoil) recoil = GetComponent<GunRecoil>();
    }

    void Update()
    {
        if (_cooldown > 0f) _cooldown -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
            TryFire(true);

        if (fireRate > 0f && Input.GetMouseButton(0))
            TryFire(false);
    }

    void TryFire(bool ignoreCooldown)
    {
        if (!ignoreCooldown && _cooldown > 0f) return;
        Fire();
        if (fireRate > 0f) _cooldown = 1f / fireRate;
    }

    void Fire()
    {
        if (!bulletSpawnPoint || !bulletPrefab) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        var bcol = bullet.GetComponent<Collider>();
        if (bcol && _gunCollider) Physics.IgnoreCollision(bcol, _gunCollider, true);

        var rb = bullet.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        if (recoil) recoil.ApplyRecoil();
        if (audioSource && audioSource.clip) audioSource.PlayOneShot(audioSource.clip);
        if (muzzleFlash) muzzleFlash.Play();
    }
}
