using UnityEngine;

public class BumperCarAI : MonoBehaviour
{
    private enum AIState
    {
        RotateToRandomPoint,
        MoveToRandomPoint,
        RotateToPlayer,
        ChasePlayer
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private BoxCollider arenaBounds;

    [Header("Random Target")]
    [SerializeField] private float arenaMargin = 2f;
    [SerializeField] private float targetReachDistance = 1.5f;

    [Header("Rotation")]
    [SerializeField] private float rotationAngleThreshold = 8f;
    [SerializeField] private float rotationSensitivity = 45f;

    [Header("Player Chase")]
    [SerializeField] private float chaseDistance = 15f;
    [SerializeField] private float playerReachDistance = 2f;

    [Header("Stuck Detection")]
    [SerializeField] private float stuckCheckInterval = 0.3f;
    [SerializeField] private float minimumMovement = 0.15f;

    [Header("After Bump")]
    [SerializeField] private float bumpTargetDelay = 0.1f;

    private BumperCarController controller;

    private AIState currentState;

    private Vector3 targetPosition;

    private float stuckTimer;
    private Vector3 lastCheckedPosition;

    private float bumpDelayTimer;

    private void Awake()
    {
        controller = GetComponent<BumperCarController>();
    }

    private void Start()
    {
        ChooseRandomTarget();
    }

    private void Update()
    {
        if (!BumperCarMatch.Instance.IsMatchStarted)
            return;

        if (player == null || controller == null)
            return;

        if (bumpDelayTimer > 0f)
        {
            bumpDelayTimer -= Time.deltaTime;

            controller.SetMoveInput(Vector2.zero);

            return;
        }

        switch (currentState)
        {
            case AIState.RotateToRandomPoint:
                HandleRotateToRandomPoint();
                break;

            case AIState.MoveToRandomPoint:
                HandleMoveToRandomPoint();
                break;

            case AIState.RotateToPlayer:
                HandleRotateToPlayer();
                break;

            case AIState.ChasePlayer:
                HandleChasePlayer();
                break;
        }

        CheckIfStuck();
    }

    private void ChooseRandomTarget()
    {
        if (arenaBounds == null)
        {
            Debug.LogWarning(
                $"{name}: Arena Bounds belum di-assign pada BumperCarAI."
            );

            return;
        }

        Bounds bounds = arenaBounds.bounds;

        float minX = bounds.min.x + arenaMargin;
        float maxX = bounds.max.x - arenaMargin;

        float minZ = bounds.min.z + arenaMargin;
        float maxZ = bounds.max.z - arenaMargin;

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        targetPosition = new Vector3(
            randomX,
            transform.position.y,
            randomZ
        );

        currentState = AIState.RotateToRandomPoint;

        ResetStuckDetection();
    }

    private void HandleRotateToRandomPoint()
    {
        Vector3 direction =
            targetPosition - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            currentState = AIState.MoveToRandomPoint;
            return;
        }

        direction.Normalize();

        float angle =
            Vector3.SignedAngle(
                transform.forward,
                direction,
                Vector3.up
            );


        if (Mathf.Abs(angle) <= rotationAngleThreshold)
        {
            controller.SetMoveInput(Vector2.zero);

            currentState = AIState.MoveToRandomPoint;

            ResetStuckDetection();

            return;
        }

        RotateTowardDirection(direction);
    }

    private void HandleMoveToRandomPoint()
    {
        Vector3 direction =
            targetPosition - transform.position;

        direction.y = 0f;

        float distance = direction.magnitude;


        if (distance <= targetReachDistance)
        {
            controller.SetMoveInput(Vector2.zero);

            currentState = AIState.RotateToPlayer;

            ResetStuckDetection();

            return;
        }


        controller.SetMoveInput(
            new Vector2(
                0f,
                1f
            )
        );
    }

    private void HandleRotateToPlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        direction.Normalize();

        float angle =
            Vector3.SignedAngle(
                transform.forward,
                direction,
                Vector3.up
            );

        if (Mathf.Abs(angle) <= rotationAngleThreshold)
        {
            controller.SetMoveInput(Vector2.zero);

            currentState = AIState.ChasePlayer;

            ResetStuckDetection();

            return;
        }


        RotateTowardDirection(direction);
    }

    private void HandleChasePlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        float distance = direction.magnitude;


        if (distance > chaseDistance)
        {
            ChooseRandomTarget();
            return;
        }

        if (distance <= playerReachDistance)
        {
            controller.SetMoveInput(Vector2.zero);

            return;
        }


        controller.SetMoveInput(
            new Vector2(
                0f,
                1f
            )
        );
    }


    private void RotateTowardDirection(Vector3 direction)
    {
        float angle =
            Vector3.SignedAngle(
                transform.forward,
                direction,
                Vector3.up
            );

        float steering =
            Mathf.Clamp(
                angle / rotationSensitivity,
                -1f,
                1f
            );


        controller.SetMoveInput(
            new Vector2(
                steering,
                0f
            )
        );
    }


    private void CheckIfStuck()
    {


        if (currentState == AIState.RotateToRandomPoint ||
            currentState == AIState.RotateToPlayer)
        {
            ResetStuckDetection();
            return;
        }

        stuckTimer -= Time.deltaTime;

        if (stuckTimer > 0f)
            return;

        float movement =
            Vector3.Distance(
                transform.position,
                lastCheckedPosition
            );


        if (movement < minimumMovement)
        {

            ChooseRandomTarget();

            return;
        }

        lastCheckedPosition = transform.position;

        stuckTimer = stuckCheckInterval;
    }

    public void ReactToBump()
    {

        ChooseRandomTarget();

        bumpDelayTimer = bumpTargetDelay;

        ResetStuckDetection();
    }

    private void ResetStuckDetection()
    {
        stuckTimer = stuckCheckInterval;

        lastCheckedPosition = transform.position;
    }
}