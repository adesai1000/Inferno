using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class SimpleMove : MonoBehaviour
{
    [Header("Move")]
    public float speed = 3f;

    [Header("Look")]
    public Camera playerCamera;          // assign your Main Camera (child of this object)
    public float mouseSensitivity = 0.1f; // tweak to taste
    public float pitchMin = -85f;
    public float pitchMax = 85f;

    private float yaw;    // left/right
    private float pitch;  // up/down

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize yaw from current rotation so there’s no snap
        yaw = transform.eulerAngles.y;
        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // --- LOOK (mouse + optional gamepad) ---
        Vector2 look = Vector2.zero;

        if (Mouse.current != null)
            look += Mouse.current.delta.ReadValue();           // pixels this frame

        if (Gamepad.current != null)
            look += Gamepad.current.rightStick.ReadValue() * 10f; // optional stick look

        yaw   += look.x * mouseSensitivity;
        pitch -= look.y * mouseSensitivity;
        pitch  = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Apply rotations: yaw on body, pitch on camera
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // --- MOVE (WASD + arrows with New Input System) ---
        float x = 0f, z = 0f;
        if (Keyboard.current != null)
        {
            x = (Keyboard.current.dKey.isPressed ? 1f : 0f)
              - (Keyboard.current.aKey.isPressed ? 1f : 0f);
            z = (Keyboard.current.wKey.isPressed ? 1f : 0f)
              - (Keyboard.current.sKey.isPressed ? 1f : 0f);

            x += (Keyboard.current.rightArrowKey.isPressed ? 1f : 0f)
               - (Keyboard.current.leftArrowKey.isPressed ? 1f : 0f);
            z += (Keyboard.current.upArrowKey.isPressed ? 1f : 0f)
               - (Keyboard.current.downArrowKey.isPressed ? 1f : 0f);
        }

        Vector3 dir = (transform.right * x + transform.forward * z).normalized;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        // Quick unlock (optional): press Escape to free cursor
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked)
                ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
        }
    }
}