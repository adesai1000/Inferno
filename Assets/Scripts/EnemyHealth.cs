using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    float current;

    void Awake() => current = maxHealth;

    public void TakeDamage(float dmg)
    {
        current -= dmg;
        Debug.Log($"{name} took {dmg} (HP={current})");
        if (current <= 0f) gameObject.SetActive(false); // simple “die”
    }
}
