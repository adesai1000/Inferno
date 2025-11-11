using UnityEngine;

public class LustProximityMusicController : MonoBehaviour
{
    [Header("Scene Refs")]
    public Transform player;
    public Transform target;

    [Header("Radii (meters)")]
    public float outerRadius = 60f;
    public float innerRadius = 15f;

    [Header("Audio Sources")]
    public AudioSource ambientSource; // Lust_Ambient
    public AudioSource intenseSource; // Lust_Intense

    [Header("Smoothing")]
    public float smoothTime = 0.2f;
    public AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    float current = 0f, velocity = 0f;

    void Start()
    {
        if (player == null && Camera.main != null) player = Camera.main.transform;

        if (ambientSource != null)
        {
            ambientSource.loop = true;
            if (!ambientSource.isPlaying) ambientSource.Play();
            ambientSource.volume = 0.3f;
        }

        if (intenseSource != null)
        {
            intenseSource.loop = true;
            if (!intenseSource.isPlaying) intenseSource.Play();
            intenseSource.volume = 0f;
        }
    }

    void Update()
    {
        if (player == null || target == null) return;

        float d = Vector3.Distance(player.position, target.position);
        float t = Mathf.InverseLerp(outerRadius, innerRadius, d);
        t = Mathf.Clamp01(t);
        t = intensityCurve.Evaluate(t);
        current = Mathf.SmoothDamp(current, t, ref velocity, smoothTime);

        // Blend softly
        if (ambientSource != null)
            ambientSource.volume = Mathf.Lerp(0.25f, 0.15f, current);
        if (intenseSource != null)
            intenseSource.volume = Mathf.Lerp(0.0f, 0.25f, current);
    }
}