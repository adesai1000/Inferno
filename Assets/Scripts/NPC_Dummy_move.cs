using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMove : MonoBehaviour
{
    public float speed = 3f;
    public float sprintSpeed = 6f;

    public Camera playerCamera;
    public float mouseSensitivity = 0.1f;
    public float pitchMin = -85f;
    public float pitchMax = 85f;

    float yaw;
    float pitch;
    CharacterController controller;
    float verticalVelocity;
    float gravity = -9.81f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        Vector2 look = Vector2.zero;

        if (Mouse.current != null)
            look += Mouse.current.delta.ReadValue();

        if (Gamepad.current != null)
            look += Gamepad.current.rightStick.ReadValue() * 10f;

        yaw += look.x * mouseSensitivity;
        pitch -= look.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        float x = 0f;
        float z = 0f;

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

        float currentSpeed = speed;
        if (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed)
            currentSpeed = sprintSpeed;

        Vector3 move = (transform.right * x + transform.forward * z).normalized * currentSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
        }
    }
}
