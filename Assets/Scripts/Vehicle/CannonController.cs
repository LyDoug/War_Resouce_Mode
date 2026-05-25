using UnityEngine;

public class TankCannonAim : MonoBehaviour
{
    public float aimSpeed = 30f;
    public float minAngle = -5f;
    public float maxAngle = 20f;
    public float deadZone = 0.02f;

    float currentAngle = 0f;

    void Update()
    {
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // DEADZONE — evita tremedeira infinita
        if (Mathf.Abs(mouseY) < deadZone)
            return;

        currentAngle -= mouseY * aimSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
    }
}
