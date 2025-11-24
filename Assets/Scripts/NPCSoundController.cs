using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NPCSoundController : MonoBehaviour
{
    [Header("Footsteps")]
    public AudioClip[] footstepClips;
    public float stepInterval = 0.6f;
    public float minSpeed = 0.1f;
    public UnityEngine.AI.NavMeshAgent agent;

    [Header("Combat SFX")]
    public AudioClip hitClip;      // grunt
    public AudioClip deathClip;    // scream

    [Header("Idle Loop (breathing, moan, etc.)")]
    public AudioSource loopSource; 
    public AudioClip idleLoop;

    AudioSource oneShotSource;
    float stepTimer;

    void Awake()
    {
        oneShotSource = GetComponent<AudioSource>();

        if (agent == null)
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (loopSource != null && idleLoop != null)
        {
            loopSource.clip = idleLoop;
            loopSource.loop = true;
            loopSource.volume = 0.35f;   // softer so footsteps + hits are audible
            loopSource.spatialBlend = 1f;
            loopSource.Play();
        }
    }

    void Update()
    {
        if (agent == null || footstepClips == null || footstepClips.Length == 0)
            return;

        Vector3 horizontalVel = new Vector3(agent.velocity.x, 0f, agent.velocity.z);
        float speed = horizontalVel.magnitude;

        if (speed > minSpeed)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0.1f;
        }
    }

    void PlayFootstep()
    {
        int index = Random.Range(0, footstepClips.Length);
        oneShotSource.PlayOneShot(footstepClips[index], 0.8f);
    }

    public void PlayHit()
    {
        if (hitClip != null)
            oneShotSource.PlayOneShot(hitClip, 1.0f);
    }

    public void PlayDeath()
    {
        if (deathClip != null)
            oneShotSource.PlayOneShot(deathClip, 1.0f);

        if (loopSource != null)
            loopSource.Stop();
    }
}