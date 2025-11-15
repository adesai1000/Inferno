using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [Header("References")]
    public EnemyHealth enemy;
    public Slider slider;

    [Header("Position")]
    public Vector3 worldOffset = new Vector3(0f, 1f, 0f); 

    private Camera _cam;

    void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponentInParent<EnemyHealth>();
        }

        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }

        _cam = Camera.main;

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;      
            slider.value = 1f;
        }
    }

    void LateUpdate()
    {
        if (enemy == null || slider == null) return;

        
        slider.value = enemy.NormalizedHealth;

        
        if (enemy != null)
        {
            transform.position = enemy.transform.position + worldOffset;
        }

      
        if (_cam != null)
        {
            transform.rotation =
                Quaternion.LookRotation(transform.position - _cam.transform.position);
        }
    }
}
