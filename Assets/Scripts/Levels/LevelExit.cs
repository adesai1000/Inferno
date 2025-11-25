using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelExit : MonoBehaviour
{
    [Tooltip("How long to show 'Level Completed' before loading the next level.")]
    public float completeCardSeconds = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        LevelManager.I.CompleteLevelAndLoadNext(completeCardSeconds);
    }
}
