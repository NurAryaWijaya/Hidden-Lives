using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComputerMouseLook : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference lookAction;

    [Header("Settings")]
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float smoothSpeed = 8f;

    [Header("Horizontal Limit")]
    [SerializeField] private float minY = -90f;
    [SerializeField] private float maxY = 90f;

    private float yaw;
    private bool canLook = false;

    private void OnEnable()
    {
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }

    private void Start()
    {
        yaw = NormalizeAngle(transform.localEulerAngles.y);
    }

    private void Update()
    {
        if (!canLook)
            return;

        Vector2 look = lookAction.action.ReadValue<Vector2>();

        // Hanya gunakan gerakan mouse kiri-kanan
        yaw += look.x * sensitivity;

        // Batasi rotasi kiri-kanan
        yaw = Mathf.Clamp(yaw, minY, maxY);

        Quaternion targetRotation =
            Quaternion.Euler(0f, yaw, 0f);

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }

    public IEnumerator StartControlDelay()
    {
        yield return new WaitForSeconds(2f);
        canLook = true;
    }
}