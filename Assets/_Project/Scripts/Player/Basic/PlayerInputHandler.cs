using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    // Interaction
    public bool InteractPressed { get; private set; }
    public bool DropPressed { get; private set; }

    // Dialogue
    public bool DialogueNextPressed { get; private set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractPressed = true;
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
            DropPressed = true;
    }

    // Dialogue
    public void OnDialogueNext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DialogueNextPressed = true;
        }
    }

    public void ConsumeInput()
    {
        InteractPressed = false;
        DropPressed = false;
        DialogueNextPressed = false;
    }
}