using UnityEngine;
using UnityEngine.InputSystem;

public class Level4MusicController : MonoBehaviour
{
    public AudioSource heartbeat;
    public AudioSource industrialPulse;

    public float minPulseVolume = 0.1f;
    public float maxPulseVolume = 0.4f;

    float currentSpeed;

    void Update()
    {
        if (Keyboard.current == null) return;

        // Player is moving
        bool moving =
            Keyboard.current.wKey.isPressed ||
            Keyboard.current.aKey.isPressed ||
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.dKey.isPressed;

        float target = moving ? maxPulseVolume : minPulseVolume;

        industrialPulse.volume = Mathf.Lerp(
            industrialPulse.volume,
            target,
            Time.deltaTime * 2f
        );
    }
}