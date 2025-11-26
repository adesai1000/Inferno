using System.Collections;
using UnityEngine;

public class L5Boss : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform turretHead;
    [SerializeField] Transform playerTargetPoint;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float fireRate = 2f;
    [SerializeField] int damage = 2;
    [SerializeField] float bossProjectileSpeed = 100f;

    

    L5PlayerHealth player;
    GameManager gameManager;

    void Awake()
    {
        // Start boss hidden visually (NOT deactivated)
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        player = FindFirstObjectByType<L5PlayerHealth>();

        // Boss counts as enemy
        gameManager.AdjustEnemiesLeft(1);

        // Wait until all turrets die
        gameManager.OnAllTurretsDestroyed += ActivateBoss;
    }

    void Update()
    {
        if (player != null)
            turretHead.LookAt(playerTargetPoint);
    }

    void ActivateBoss()
    {
        // Show boss visuals and colliders
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = true;

        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = true;

        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            // Stop if player is gone
            if (player == null || playerTargetPoint == null)
                yield break;

            Projectile newProjectile = Instantiate(
                projectilePrefab,
                projectileSpawnPoint.position,
                Quaternion.identity
            ).GetComponent<Projectile>();

            newProjectile.transform.LookAt(playerTargetPoint);
            newProjectile.Init(damage);

            // NEW: mark it as a boss projectile
            newProjectile.SetBossProjectile(true);

            // keep speed override
            newProjectile.SetSpeed(bossProjectileSpeed);
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
