using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform; // Main Camera
    public Transform gunTransform;    // The gun object (your cylinder)

    [Header("Recoil Settings")]
    public float recoilKickback = 0.1f;      // how much the gun moves backward
    public float recoilRotation = 2f;        // how much the gun rotates upward
    public float recoverySpeed = 10f;        // how fast it returns to normal

    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;
    private Vector3 currentVelocity;

    void Start()
    {
        if (gunTransform == null)
            gunTransform = transform;
        originalLocalPos = gunTransform.localPosition;
        originalLocalRot = gunTransform.localRotation;
    }

    void LateUpdate()
    {
        // Smoothly reset back to default position & rotation
        gunTransform.localPosition = Vector3.SmoothDamp(
            gunTransform.localPosition, 
            originalLocalPos, 
            ref currentVelocity, 
            1f / recoverySpeed
        );

        gunTransform.localRotation = Quaternion.Lerp(
            gunTransform.localRotation,
            originalLocalRot,
            Time.deltaTime * recoverySpeed
        );
    }

    public void ApplyRecoil()
    {
        // Kick gun backward and slightly rotate upward
        gunTransform.localPosition -= Vector3.forward * recoilKickback;
        gunTransform.localRotation *= Quaternion.Euler(-recoilRotation, 0, 0);

        // Optional: tiny camera nudge
        if (cameraTransform)
            cameraTransform.localRotation *= Quaternion.Euler(-recoilRotation * 0.3f, 0, 0);
    }
}
