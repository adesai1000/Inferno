using UnityEngine;

public class GunFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset = new Vector3(0.3f, -0.25f, 0.6f);
    public float followSpeed = 20f;

    void LateUpdate()
{
    if (!cameraTransform) return;

    // Position follows camera
    transform.position = Vector3.Lerp(
        transform.position,
        cameraTransform.position + cameraTransform.TransformDirection(offset),
        Time.deltaTime * followSpeed
    );

    // Rotation follows camera but keeps gun's local rotation (so it doesn't point up)
    Quaternion cameraRot = cameraTransform.rotation;
    Quaternion gunLocalRot = Quaternion.Euler(90, 0, 0); // your preferred gun angle
    transform.rotation = cameraRot * gunLocalRot;
}

}
