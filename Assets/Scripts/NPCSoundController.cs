using UnityEngine;
using UnityEngine.InputSystem;  // Required for new Input System

public class NPCSoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource loopSource;      // continuous background loop
    public AudioSource oneShotSource;   // gunshot or short sound

    [Header("Audio Clips")]
    public AudioClip idleLoop;          // background/ambient sound
    public AudioClip oneShotClip;       // the sound that plays when spacebar is pressed

    void Start()
    {
        // Start playing the background loop (optional)
        if (loopSource && idleLoop)
        {
            loopSource.clip = idleLoop;
            loopSource.loop = true;
            loopSource.Play();
        }
    }

    void Update()
    {
        // Listen for SPACEBAR press
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PlayGunshot();
        }
    }

    void PlayGunshot()
    {
        if (oneShotSource && oneShotClip)
        {
            oneShotSource.PlayOneShot(oneShotClip);
            Debug.Log("Gunshot played!"); // check Console to confirm trigger
        }
        else
        {
            Debug.LogWarning("Missing Audio Source or Clip on NPCSoundController!");
        }
    }
}