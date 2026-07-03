using Game.Dialogue;
using UnityEngine;

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

        if (GameStateManager.Instance.IsState(GameState.Dialogue))
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
        if (target == null) return;

        Vector3 dir = target.position - cameraPivot.position;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion lookRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(0f, lookRot.eulerAngles.y, 0f),
            Time.deltaTime * 6f
        );

        cameraPivot.rotation = Quaternion.Slerp(
            cameraPivot.rotation,
            lookRot,
            Time.deltaTime * 6f
        );
    }

    private void SetDialogueMode(bool value)
    {
        isInDialogue = value;

        if (value)
        {
            pitch = cameraPivot.localEulerAngles.x;
        }
    }
}