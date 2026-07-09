using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private PlayerMovement movement;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (!GameStateManager.Instance.IsState(GameState.Exploration))
            return;

        movement.Move(
            inputHandler.MoveInput
        );
    }
}