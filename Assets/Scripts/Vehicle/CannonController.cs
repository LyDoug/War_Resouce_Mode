using UnityEngine;

public class TankCannonAim : MonoBehaviour
{
    [Header("Aim")]
    public float aimSpeed = 30f;

    [Header("Vertical Limits")]
    public float minAngle = -5f;

    public float maxAngle = 45f;

    [Header("Smoothing")]
    public float smoothSpeed = 8f;

    [Header("DeadZone")]
    public float deadZone = 0.02f;

    float targetAngle;
    float currentAngle;

    // =====================================================
    // START
    // =====================================================
    void Start()
    {
        currentAngle =
            NormalizeAngle(
                transform.localEulerAngles.x
            );

        targetAngle = currentAngle;
    }

    // =====================================================
    // UPDATE
    // =====================================================
    void Update()
    {
        HandleInput();

        ApplyRotation();
    }

    // =====================================================
    // INPUT
    // =====================================================
    void HandleInput()
    {
        float mouseY =
            Input.GetAxisRaw("Mouse Y");

        // evita tremedeira
        if (Mathf.Abs(mouseY) < deadZone)
            return;

        targetAngle -=
            mouseY *
            aimSpeed *
            Time.deltaTime;

        targetAngle =
            Mathf.Clamp(
                targetAngle,
                minAngle,
                maxAngle
            );
    }

    // =====================================================
    // ROTATION
    // =====================================================
    void ApplyRotation()
    {
        currentAngle =
            Mathf.Lerp(
                currentAngle,
                targetAngle,
                smoothSpeed *
                Time.deltaTime
            );

        transform.localRotation =
            Quaternion.Euler(
                currentAngle,
                0f,
                0f
            );
    }

    // =====================================================
    // NORMALIZE
    // =====================================================
    float NormalizeAngle(float angle)
    {
        while (angle > 180f)
            angle -= 360f;

        return angle;
    }

    // =====================================================
    // GET CURRENT ANGLE
    // =====================================================
    public float GetCurrentAngle()
    {
        return currentAngle;
    }
}