using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float sensitivity = 0.1f;

    private PlayerInputHandler inputHandler;
    private float pitch;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 look = inputHandler.LookInput;

        float mouseX = look.x * sensitivity;
        float mouseY = look.y * sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -85f, 85f);

        cameraPivot.localRotation =
            Quaternion.Euler(pitch, 0f, 0f);
    }
}