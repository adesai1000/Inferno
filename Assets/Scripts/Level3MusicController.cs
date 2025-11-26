using UnityEngine;

public class Level3MusicController : MonoBehaviour
{
    public AudioSource bgmSource;

    void Start()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
}