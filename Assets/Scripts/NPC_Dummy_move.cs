using UnityEngine;
using UnityEngine.InputSystem; // new Input System

public class SimpleMove : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        // WASD / arrows using new Input System
        float x = 0f, z = 0f;
        if (Keyboard.current != null)
        {
            x = (Keyboard.current.dKey.isPressed ? 1f : 0f)
              - (Keyboard.current.aKey.isPressed ? 1f : 0f);
            z = (Keyboard.current.wKey.isPressed ? 1f : 0f)
              - (Keyboard.current.sKey.isPressed ? 1f : 0f);

            // arrow keys too
            x += (Keyboard.current.rightArrowKey.isPressed ? 1f : 0f)
               - (Keyboard.current.leftArrowKey.isPressed ? 1f : 0f);
            z += (Keyboard.current.upArrowKey.isPressed ? 1f : 0f)
               - (Keyboard.current.downArrowKey.isPressed ? 1f : 0f);
        }

        var dir = new Vector3(x, 0f, z).normalized;
        transform.Translate(dir * speed * Time.deltaTime, Space.Self);
    }
}