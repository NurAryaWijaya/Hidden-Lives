using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private readonly int MoveXHash = Animator.StringToHash("MoveX");
    private readonly int MoveYHash = Animator.StringToHash("MoveY");
    //private readonly int SpeedHash = Animator.StringToHash("Speed");

    [SerializeField]
    private float dampTime = 0.1f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateMovement(Vector2 input)
    {
        animator.SetFloat(MoveXHash, input.x, dampTime, Time.deltaTime);
        animator.SetFloat(MoveYHash, input.y, dampTime, Time.deltaTime);
        //animator.SetFloat(SpeedHash, input.sqrMagnitude, dampTime, Time.deltaTime);
    }
}