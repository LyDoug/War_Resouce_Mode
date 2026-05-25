using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AircraftController : MonoBehaviour
{
    [Header("Throttle")]
    public float maxThrust = 18000f;
    public float throttleStep = 0.12f;
    [Range(0, 1)]
    public float throttle = 0.3f;

    [Header("Aerodynamics")]
    public float liftCoefficient = 0.8f;
    public float maxLift = 25000f;
    public float drag = 0.015f;
    public float maxSpeed = 180f;

    [Header("Rotation Forces")]
    public float pitchForce = 2200f; // W / S
    public float rollForce  = 3500f; // A / D
    public float yawForce   = 1800f; // Q / E

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 1.2f;
    }

    void Update()
    {
        HandleThrottle();
    }

    void FixedUpdate()
    {
        ApplyThrust();
        ApplyLift();
        ApplyRotation();
        ApplyDrag();
        ClampSpeed();
    }

    // ================= THROTTLE =================
    void HandleThrottle()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            throttle += scroll * throttleStep;
            throttle = Mathf.Clamp01(throttle);
        }
    }

    // ================= THRUST =================
    void ApplyThrust()
    {
        rb.AddForce(transform.forward * throttle * maxThrust);
    }

    // ================= LIFT =================
    void ApplyLift()
    {
        // velocidade apenas para frente
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        if (forwardSpeed < 10f) return;

        float lift = forwardSpeed * forwardSpeed * liftCoefficient;
        lift = Mathf.Clamp(lift, 0f, maxLift);

        rb.AddForce(transform.up * lift);
    }

    // ================= ROTATION =================
    void ApplyRotation()
    {
        float pitch = Input.GetAxis("Vertical");   // W / S
        float roll  = Input.GetAxis("Horizontal"); // A / D

        float yaw = 0f;
        if (Input.GetKey(KeyCode.Q)) yaw = -1f;
        if (Input.GetKey(KeyCode.E)) yaw = 1f;

        rb.AddTorque(transform.right   * pitch * pitchForce);
        rb.AddTorque(transform.forward * -roll  * rollForce);
        rb.AddTorque(transform.up      * yaw   * yawForce);
    }

    // ================= DRAG =================
    void ApplyDrag()
    {
        Vector3 dragForce = -rb.linearVelocity.normalized
                            * rb.linearVelocity.sqrMagnitude
                            * drag;

        rb.AddForce(dragForce);
    }

    // ================= LIMIT SPEED =================
    void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
