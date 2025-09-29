using UnityEngine;

public class NooseInteract : MonoBehaviour
{
    public UIFader fader;
    bool inside;

    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) inside = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) inside = false; }

    void Update()
    {
        if (inside && Input.GetKeyDown(KeyCode.E))
        {
            if (fader) fader.FadeToBlack();
            Destroy(gameObject);
        }
    }
}
