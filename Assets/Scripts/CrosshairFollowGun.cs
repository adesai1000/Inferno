using UnityEngine;
using UnityEngine.UI;

public class CrosshairFollowGun : MonoBehaviour
{
    [Header("References")]
    public Transform muzzle;          // your bulletSpawnPoint
    public Camera cam;                // Main Camera
    public Image crosshair;           // the UI Image (red dot)

    [Header("Raycast")]
    public LayerMask hitMask = ~0;    // what the crosshair can land on (exclude Player/IgnoreRaycast)
    public float maxDistance = 500f;

    [Header("Feel")]
    public float followLerp = 20f;    // smoothing for UI

    RectTransform _rt;
    Vector3 _targetScreenPos;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (crosshair) _rt = crosshair.rectTransform;
    }

    void LateUpdate()
    {
        if (!muzzle || !cam || !_rt) return;

        // Ray from gun’s muzzle along the same direction your bullets use
        Ray ray = new Ray(muzzle.position, muzzle.forward);
        Vector3 worldPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, hitMask, QueryTriggerInteraction.Ignore))
        {
            worldPoint = hit.point;

            // (Optional) change color if aiming at an enemy
            bool aimingEnemy = hit.collider.GetComponentInParent<EnemyHealth>() != null;
            if (aimingEnemy) crosshair.color = Color.green; else crosshair.color = Color.red;
        }
        else
        {
            worldPoint = muzzle.position + muzzle.forward * maxDistance;
            crosshair.color = Color.red;
        }

        // Project to screen
        Vector3 screenPos = cam.WorldToScreenPoint(worldPoint);

        // Hide if point is behind the camera
        if (screenPos.z <= 0f)
        {
            crosshair.enabled = false;
            return;
        }
        crosshair.enabled = true;

        // Smoothly move the UI dot
        _targetScreenPos = screenPos;
        _rt.position = Vector3.Lerp(_rt.position, _targetScreenPos, Time.deltaTime * followLerp);
    }
}
