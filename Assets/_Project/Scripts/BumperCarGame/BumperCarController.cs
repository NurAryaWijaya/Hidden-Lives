using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BumperCarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Movement")]
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float reverseSpeed = 5f;
    [SerializeField] private float brakeForce = 8f;

    [Header("Steering")]
    [SerializeField] private float steeringPower = 120f;
    //[SerializeField] private float steeringSmoothness = 8f;

    [Header("Grip")]
    [SerializeField] private float lateralGrip = 5f;

    [Header("Hit Reaction")]
    [SerializeField] private float inputLockDuration = 1f;

    private bool inputLocked;
    private float inputLockTimer;

    private Vector2 externalMoveInput;
    private bool useExternalInput;

    private Rigidbody rb;
    private float currentSteering;

    public void LockInput()
    {
        inputLocked = true;
        inputLockTimer = inputLockDuration;
    }

    public void SetMoveInput(Vector2 input)
    {
        externalMoveInput = input;
        useExternalInput = true;
    }

    public void ClearExternalInput()
    {
        useExternalInput = false;
        externalMoveInput = Vector2.zero;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (inputHandler == null)
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }
    }

    private void FixedUpdate()
    {
        if (!BumperCarMatch.Instance.IsMatchStarted)
            return;

        if (inputLocked)
        {
            inputLockTimer -= Time.fixedDeltaTime;

            if (inputLockTimer <= 0f)
            {
                inputLocked = false;
            }

            ApplyLateralGrip();
            return;
        }

        Vector2 input;

        if (useExternalInput)
        {
            input = externalMoveInput;
        }
        else
        {
            if (inputHandler == null)
                return;

            input = inputHandler.MoveInput;
        }

        HandleMovement(input.y);
        HandleSteering(input.x);
        ApplyLateralGrip();
    }


    private void HandleMovement(float throttleInput)
    {
        Vector3 forward = transform.forward;

        float forwardSpeed =
            Vector3.Dot(rb.linearVelocity, forward);

        if (throttleInput > 0f)
        {
            if (forwardSpeed < maxSpeed)
            {
                rb.AddForce(
                    forward * throttleInput * acceleration,
                    ForceMode.Acceleration
                );
            }
        }
        else if (throttleInput < 0f)
        {
            if (forwardSpeed > -reverseSpeed)
            {
                rb.AddForce(
                    forward * throttleInput * acceleration,
                    ForceMode.Acceleration
                );
            }
        }
        else
        {
            Vector3 velocity = rb.linearVelocity;

            velocity = Vector3.Lerp(
                velocity,
                Vector3.zero,
                brakeForce * Time.fixedDeltaTime
            );

            rb.linearVelocity = velocity;
        }
    }
    private void HandleSteering(float steeringInput)
    {
        if (Mathf.Abs(steeringInput) < 0.01f)
            return;

        float forwardSpeed =
            Vector3.Dot(
                rb.linearVelocity,
                transform.forward
            );

        float rotationSpeed;

        // Berputar di tempat
        if (Mathf.Abs(forwardSpeed) < 0.1f)
        {
            rotationSpeed = steeringPower * 0.5f;
        }
        else
        {
            // Saat bergerak, steering mengikuti kecepatan.
            float speedFactor = Mathf.Clamp01(
                Mathf.Abs(forwardSpeed) / maxSpeed
            );

            rotationSpeed = steeringPower * speedFactor;

            // Saat mundur, arah steering dibalik.
            if (forwardSpeed < 0f)
            {
                rotationSpeed *= -1f;
            }
        }

        float rotationAmount =
            steeringInput *
            rotationSpeed *
            Time.fixedDeltaTime;

        rb.MoveRotation(
            rb.rotation *
            Quaternion.Euler(
                0f,
                rotationAmount,
                0f
            )
        );
    }

    private void ApplyLateralGrip()
    {
        Vector3 localVelocity =
            transform.InverseTransformDirection(
                rb.linearVelocity
            );

        localVelocity.x = Mathf.Lerp(
            localVelocity.x,
            0f,
            lateralGrip * Time.fixedDeltaTime
        );

        rb.linearVelocity =
            transform.TransformDirection(localVelocity);
    }
}