using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity; //Gravity

    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void Move(Vector2 input)
    {
        if (!GameStateManager.Instance.IsState(GameState.Exploration))
            return;

        playerAnimation.UpdateMovement(input);

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

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        controller.enabled = false;

        transform.SetPositionAndRotation(position, rotation);

        verticalVelocity = 0f;

        controller.enabled = true;
    }
}