using UnityEngine;

public class BedWake : MonoBehaviour
{
    public GameObject cover;
    bool inside;

    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) inside = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) inside = false; }

    void Update()
    {
        if (inside && Input.GetKeyDown(KeyCode.E))
        {
            if (cover) cover.SetActive(false); // removes blanket
            Destroy(gameObject); // remove trigger so it’s one-time
        }
    }
}
