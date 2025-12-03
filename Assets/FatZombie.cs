using UnityEngine;
using UnityEngine.AI;

public class FatZombie : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;

    const string PLAYER_TAG = "Player";

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        if (player == null)
        {
            Debug.LogWarning("Player not found! Make sure your player GameObject has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (!player) return;

        agent.SetDestination(player.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            // enemyHealth.SelfDestruct();
        }
    }
}
