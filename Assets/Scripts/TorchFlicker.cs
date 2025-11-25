using UnityEngine;

public class TorchFlicker : MonoBehaviour
{
    private Light torchLight;
    public float minIntensity = 2f;
    public float maxIntensity = 3f;
    public float flickerSpeed = 10f;

    void Start() => torchLight = GetComponent<Light>();

    void Update()
    {
        if (!torchLight) return;
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        torchLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}