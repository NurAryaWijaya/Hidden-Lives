using Game.Dialogue;
using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float sensitivity = 0.1f;

    private PlayerInputHandler inputHandler;
    private float pitch;
    private bool isInDialogue;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueModeChanged += SetDialogueMode;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueModeChanged -= SetDialogueMode;
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsState(GameState.Dialogue))
        {
            LookAtDialogueTarget();
            return;
        }

        if (!GameStateManager.Instance.IsState(GameState.Exploration))
            return;

        Vector2 look = inputHandler.LookInput;

        float mouseX = look.x * sensitivity;
        float mouseY = look.y * sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -85f, 85f);

        cameraPivot.localRotation =
            Quaternion.Euler(pitch, 0f, 0f);
    }
    private void LookAtDialogueTarget()
    {
        Transform target = DialogueManager.Instance?.CurrentFocusPoint;
        if (target == null)
            return;

        Vector3 direction = target.position - cameraPivot.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float targetYaw = targetRotation.eulerAngles.y;
        float targetPitch = NormalizeAngle(targetRotation.eulerAngles.x);

        float currentYaw = transform.eulerAngles.y;
        float currentPitch = cameraPivot.localEulerAngles.x;

        currentPitch = NormalizeAngle(currentPitch);

        float yaw = Mathf.LerpAngle(
            currentYaw,
            targetYaw,
            Time.deltaTime * 6f
        );

        float pitch = Mathf.LerpAngle(
            currentPitch,
            targetPitch,
            Time.deltaTime * 6f
        );

        transform.rotation = Quaternion.Euler(
            0f,
            yaw,
            0f
        );

        cameraPivot.localRotation = Quaternion.Euler(
            pitch,
            0f,
            0f
        );
    }

    public IEnumerator LookAtTarget(Transform target)
    {
        GameStateManager.Instance.SetState(GameState.OnComputer);
        if (target == null)
            yield break;

        Vector3 direction = target.position - cameraPivot.position;

        if (direction.sqrMagnitude < 0.001f)
            yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float targetYaw = targetRotation.eulerAngles.y;
        float targetPitch = NormalizeAngle(targetRotation.eulerAngles.x);

        while (true)
        {
            float currentYaw = transform.eulerAngles.y;
            float currentPitch = NormalizeAngle(cameraPivot.localEulerAngles.x);

            float yaw = Mathf.LerpAngle(
                currentYaw,
                targetYaw,
                Time.deltaTime * 6f
            );

            float pitch = Mathf.LerpAngle(
                currentPitch,
                targetPitch,
                Time.deltaTime * 6f
            );

            transform.rotation = Quaternion.Euler(
                0f,
                yaw,
                0f
            );

            cameraPivot.localRotation = Quaternion.Euler(
                pitch,
                0f,
                0f
            );

            if (Mathf.Abs(Mathf.DeltaAngle(currentYaw, targetYaw)) < 0.5f &&
                Mathf.Abs(Mathf.DeltaAngle(currentPitch, targetPitch)) < 0.5f)
            {
                break;
            }

            yield return null;
        }
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }

    private void SetDialogueMode(bool value)
    {
        isInDialogue = value;

        if (value)
        {
            pitch = NormalizeAngle(cameraPivot.localEulerAngles.x);
        }
        else
        {
            pitch = NormalizeAngle(cameraPivot.localEulerAngles.x);
        }
    }
}