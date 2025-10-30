using UnityEngine;

public class ProximityMusicController : MonoBehaviour
{
    [Header("Scene refs")]
    public Transform player;
    public Transform target;

    [Header("Radii (meters)")]
    public float outerRadius = 14f;
    public float innerRadius = 3.5f;

    [Header("Audio Sources")]
    public AudioSource ambientSource;
    public AudioSource intenseSource;

    [Header("Smoothing & Curve")]
    public float smoothTime = 0.25f;
    // very gentle, never reaches 1.0
    public AnimationCurve intensityCurve = new AnimationCurve(
        new Keyframe(0.00f, 0.00f),
        new Keyframe(0.40f, 0.06f),
        new Keyframe(0.70f, 0.20f),
        new Keyframe(1.00f, 0.35f)
    );

    [Header("Overall Loudness")]
    [Range(0f,1f)] public float globalMusicGain = 0.18f; // super soft bed
    [Range(0f,1f)] public float maxSourceVolume = 0.25f; // hard ceiling per track
    public bool forceMusic2D = true; // keeps music non-spatial so SFX 3D win

    [Header("Ducking (SFX priority)")]
    [Range(0f,1f)] public float musicFloor = 0.08f; // almost silent when ducked
    public float duckAttack = 0.02f;  // fast drop
    public float duckRelease = 0.60f; // slow recovery

    float duckLevel = 1f, duckTarget = 1f, duckVel;
    float current = 0f, velocity = 0f;

    public static ProximityMusicController Instance { get; private set; }
    void Awake() => Instance = this;

    void Start()
    {
        if (!player && Camera.main) player = Camera.main.transform;

        if (ambientSource)
        {
            ambientSource.loop = true;
            if (forceMusic2D) ambientSource.spatialBlend = 0f; // 2D
            ambientSource.volume = 0f;
            if (!ambientSource.isPlaying) ambientSource.Play();
        }
        if (intenseSource)
        {
            intenseSource.loop = true;
            if (forceMusic2D) intenseSource.spatialBlend = 0f; // 2D
            intenseSource.volume = 0f;
            if (!intenseSource.isPlaying) intenseSource.Play();
        }
    }

    void Update()
    {
        if (!player || !target || !ambientSource || !intenseSource) return;

        // distance -> [0..1] then softened curve (max ~0.35)
        float d = Vector3.Distance(player.position, target.position);
        float t = Mathf.InverseLerp(outerRadius, innerRadius, d);
        t = Mathf.Clamp01(t);
        t = intensityCurve.Evaluate(t);
        current = Mathf.SmoothDamp(current, t, ref velocity, smoothTime);

        // base blend
        float baseIntense = current;
        float baseAmbient = 1f - current;

        // duck envelope
        float ducked = duckLevel; // 1 normal, musicFloor ducked
        float vIntense = baseIntense * ducked;
        float vAmbient = baseAmbient * ducked;

        // global gain + caps (keeps bed very soft)
        vIntense = Mathf.Min(vIntense * globalMusicGain, maxSourceVolume);
        vAmbient = Mathf.Min(vAmbient * globalMusicGain, maxSourceVolume);

        intenseSource.volume = vIntense;
        ambientSource.volume = vAmbient;

        // envelope follow
        float tc = (duckTarget < duckLevel) ? duckAttack : duckRelease;
        duckLevel = Mathf.SmoothDamp(duckLevel, duckTarget, ref duckVel, Mathf.Max(0.0001f, tc));
    }

    public void Duck(float strength = 1f, float hold = 0.25f)
    {
        float targetLevel = Mathf.Lerp(1f, Mathf.Clamp01(musicFloor), Mathf.Clamp01(strength));
        duckTarget = targetLevel;
        CancelInvoke(nameof(ReleaseDuck));
        Invoke(nameof(ReleaseDuck), Mathf.Max(0f, hold));
    }
    void ReleaseDuck() => duckTarget = 1f;

    public static void DuckGlobal(float strength = 1f, float hold = 0.25f)
    {
        if (Instance) Instance.Duck(strength, hold);
    }
}