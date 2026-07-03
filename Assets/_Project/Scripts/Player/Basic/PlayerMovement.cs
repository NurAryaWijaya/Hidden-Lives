using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity; //Gravity

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Move(Vector2 input)
    {
        if (!GameStateManager.Instance.IsState(GameState.Exploration))
            return;

        Vector3 moveDirection =
            transform.forward * input.y +
            transform.right * input.x;

        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        Vector3 horizontalMove = 
            moveDirection * moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        horizontalMove.y = verticalVelocity;

        controller.Move(
            horizontalMove * Time.deltaTime
        );
    }
}