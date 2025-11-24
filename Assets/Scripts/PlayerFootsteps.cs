using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip[] footstepClips;

    [Header("Timing")]
    public float stepInterval = 0.45f;   // time between steps
    public float minSpeed = 0.1f;       // ignore tiny movement

    CharacterController controller;
    float stepTimer;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller == null || footstepSource == null || footstepClips.Length == 0)
            return;

        // Ignore vertical movement for footsteps
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
        float speed = horizontalVelocity.magnitude;

        if (speed > minSpeed)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayStep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // reset timer so it does not immediately fire on next move
            stepTimer = 0.1f;
        }
    }

    void PlayStep()
    {
        int index = Random.Range(0, footstepClips.Length);
        footstepSource.PlayOneShot(footstepClips[index], 0.9f);
    }
}