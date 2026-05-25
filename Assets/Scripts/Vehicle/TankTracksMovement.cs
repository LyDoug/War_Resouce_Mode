using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankTracksMovement : MonoBehaviour
{
    public Transform leftTrack;
    public Transform rightTrack;

    public float trackForce = 150000f;
    public float turnForceMultiplier = 1.5f;
    public float maxSpeed = 10f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float forward = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        float left = Mathf.Clamp(forward - turn, -1f, 1f);
        float right = Mathf.Clamp(forward + turn, -1f, 1f);

        Vector3 forceLeft = transform.forward * left * trackForce * Time.fixedDeltaTime;
        Vector3 forceRight = transform.forward * right * trackForce * Time.fixedDeltaTime;

        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForceAtPosition(forceLeft, leftTrack.position);
            rb.AddForceAtPosition(forceRight, rightTrack.position);
        }

        // AJUDA EXTRA DE GIRO (realista)
        rb.AddTorque(Vector3.up * turn * trackForce * turnForceMultiplier * Time.fixedDeltaTime);
    }
}
