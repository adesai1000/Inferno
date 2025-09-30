using UnityEngine;

public class ProximityMusicController : MonoBehaviour
{
    [Header("Scene refs")]
    public Transform player;      
    public Transform target;      

    [Header("Radii (meters)")]
    public float outerRadius = 8f;   
    public float innerRadius = 1.5f; 

    [Header("Audio Sources")]
    public AudioSource ambientSource; // limbo_ambient
    public AudioSource intenseSource; // hanging_intense

    [Header("Smoothing & Curve")]
    public float smoothTime = 0.15f; 
    public AnimationCurve intensityCurve = AnimationCurve.Linear(0, 0, 1, 1);

    float current = 0f, velocity = 0f;

    void Start()
    {
        if (player == null && Camera.main != null) player = Camera.main.transform;
        if (ambientSource != null) { ambientSource.loop = true; if (!ambientSource.isPlaying) ambientSource.Play(); ambientSource.volume = 1f; }
        if (intenseSource != null) { intenseSource.loop = true; if (!intenseSource.isPlaying) intenseSource.Play(); intenseSource.volume = 0f; }
    }

    void Update()
    {
        if (player == null || target == null || ambientSource == null || intenseSource == null) return;

        float d = Vector3.Distance(player.position, target.position);
        // 0 when far (>= outer), 1 when near (<= inner)
        float t = Mathf.InverseLerp(outerRadius, innerRadius, d);
        t = Mathf.Clamp01(t);
        t = intensityCurve.Evaluate(t);

        
        current = Mathf.SmoothDamp(current, t, ref velocity, smoothTime);

        intenseSource.volume = current;      // ramp up as you approach center
        ambientSource.volume = 1f - current; // ramp down
    }

    void OnDrawGizmosSelected()
    {
        if (target == null) return;
        Gizmos.color = new Color(0f, 1f, 1f, 0.35f);
        Gizmos.DrawWireSphere(target.position, outerRadius);
        Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
        Gizmos.DrawWireSphere(target.position, innerRadius);
    }
}