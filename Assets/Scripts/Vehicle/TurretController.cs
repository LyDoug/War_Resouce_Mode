using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float rotationSpeed = 60f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0f, mouseX * rotationSpeed * Time.deltaTime, 0f);
    }
}
