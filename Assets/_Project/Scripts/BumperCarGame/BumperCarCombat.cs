using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BumperCarCombat : MonoBehaviour
{
    private BumperCarController controller;
    private BumperCarAI ai;

    public enum HitDirection
    {
        Front,
        Side,
        Rear
    }

    [Header("Knockback")]
    [SerializeField] private float frontForce = 5f;
    [SerializeField] private float sideForce = 7f;
    [SerializeField] private float rearForce = 10f;

    [Header("Collision")]
    [SerializeField] private float minimumImpactSpeed = 1f;
    [SerializeField] private float bumpCooldown = 0.75f;

    private Rigidbody rb;

    private float bumpCooldownTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        controller = GetComponent<BumperCarController>();
        ai = GetComponent<BumperCarAI>();
    }

    private void Update()
    {
        if (bumpCooldownTimer > 0f)
        {
            bumpCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BumperCarCombat other =
            collision.gameObject.GetComponentInParent<BumperCarCombat>();

        if (other == null)
            return;

        if (other == this)
            return;

        HandleBump(other, collision);
    }

    private void HandleBump(
        BumperCarCombat other,
        Collision collision)
    {
        if (bumpCooldownTimer > 0f)
            return;

        float impactSpeed =
            collision.relativeVelocity.magnitude;

        if (impactSpeed < minimumImpactSpeed)
            return;

        bumpCooldownTimer = bumpCooldown;

        HitDirection hitDirection =
            GetHitDirection(other.transform);

        float force =
            GetForce(hitDirection);

        Vector3 knockbackDirection =
            other.transform.position - transform.position;

        knockbackDirection.y = 0f;

        if (knockbackDirection.sqrMagnitude < 0.001f)
            return;

        knockbackDirection.Normalize();

        Debug.Log(
            $"[BUMPER] {gameObject.name} mengenai " +
            $"{other.gameObject.name} pada bagian: " +
            $"{hitDirection} | " +
            $"Impact Speed: {impactSpeed:F2} | " +
            $"Force: {force:F2}"
        );

        if (BumperCarMatch.Instance != null)
        {

            if (ai == null)
            {
                BumperCarMatch.Instance.PlayerHitNPC(
                    hitDirection
                );
            }

            else
            {
                BumperCarMatch.Instance.NPCHitPlayer(
                    hitDirection
                );
            }
        }

        other.ApplyKnockback(
            knockbackDirection,
            force
        );

        if (ai != null)
        {
            ai.ReactToBump();
        }
    }

    private HitDirection GetHitDirection(Transform other)
    {
        Vector3 directionToOther =
            transform.position - other.position;

        directionToOther.y = 0f;

        if (directionToOther.sqrMagnitude < 0.001f)
        {
            return HitDirection.Front;
        }

        directionToOther.Normalize();

        float dot =
            Vector3.Dot(
                other.forward,
                directionToOther
            );

        if (dot > 0.5f)
        {
            return HitDirection.Front;
        }

        if (dot < -0.5f)
        {
            return HitDirection.Rear;
        }


        return HitDirection.Side;
    }

    private float GetForce(HitDirection hitDirection)
    {
        switch (hitDirection)
        {
            case HitDirection.Front:
                return frontForce;

            case HitDirection.Side:
                return sideForce;

            case HitDirection.Rear:
                return rearForce;

            default:
                return frontForce;
        }
    }

    private void ApplyKnockback(
        Vector3 direction,
        float force)
    {
        rb.AddForce(
            direction * force,
            ForceMode.Impulse
        );

        if (controller != null)
        {
            controller.LockInput();
        }
    }
}