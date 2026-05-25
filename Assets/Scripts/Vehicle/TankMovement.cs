using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankMovement : MonoBehaviour
{
    public float moveForce = 120000f;
    public float turnTorque = 35000f;
    public float maxSpeed = 12f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Limite de velocidade
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveInput * moveForce * Time.fixedDeltaTime);
        }

        rb.AddTorque(Vector3.up * turnInput * turnTorque * Time.fixedDeltaTime);
    }
}
