using UnityEngine;

public class RotationLimiter : MonoBehaviour
{
    [Header("X Axis - Up / Down")]
    [SerializeField] private float minX = -85f;
    [SerializeField] private float maxX = 85f;

    [Header("Y Axis - Left / Right")]
    [SerializeField] private float minY = -360f;
    [SerializeField] private float maxY = 360f;

    private void LateUpdate()
    {
        Vector3 rotation = transform.localEulerAngles;

        float x = NormalizeAngle(rotation.x);
        float y = NormalizeAngle(rotation.y);

        x = Mathf.Clamp(x, minX, maxX);
        y = Mathf.Clamp(y, minY, maxY);

        transform.localRotation = Quaternion.Euler(x, y, 0f);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }
}