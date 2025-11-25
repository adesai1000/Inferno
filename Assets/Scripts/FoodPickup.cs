using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    public enum FoodKind { Good, Bad }

    public FoodKind kind = FoodKind.Good;
    public float amount = 20f;
    public bool countsForGoal = false;
    public Level3FoodGoalManager goalManager;
    public AudioSource pickupSound;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var health = other.GetComponent<PlayerHealth>();
        if (health == null) return;

        if (kind == FoodKind.Good)
        {
            health.Heal(amount);
            if (countsForGoal && goalManager != null)
                goalManager.RegisterGoodPickup();
        }
        else
        {
            health.TakeDamage(amount);
        }

        if (pickupSound != null)
        {
            pickupSound.transform.parent = null;
            pickupSound.Play();
            Destroy(pickupSound.gameObject, pickupSound.clip.length);
        }

        Destroy(gameObject);
    }
}
