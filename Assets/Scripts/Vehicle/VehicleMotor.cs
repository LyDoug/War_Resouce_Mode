using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleMotor : MonoBehaviour
{
    [Header("Blueprint")]
    public VehicleBlueprint blueprint;

    [Header("Center Of Mass")]
    public Transform centerOfMass;

    [Header("Input")]
    public bool usePlayerInput = true;

    Rigidbody rb;

    float moveInput;
    float turnInput;

    // =====================================================
    // AWAKE
    // =====================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // =========================
        // RIGIDBODY SETTINGS
        // =========================
        rb.mass = 1200f;

        if (blueprint != null)
        {
            rb.mass = blueprint.mass;
        }

        rb.linearDamping = 0.3f;

        rb.angularDamping = 4f;

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;

        rb.collisionDetectionMode =
            CollisionDetectionMode.Continuous;

        // =========================
        // CONSTRAINTS
        // =========================
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        // =========================
        // CENTER OF MASS
        // =========================
        if (centerOfMass != null)
        {
            rb.centerOfMass =
                transform.InverseTransformPoint(
                    centerOfMass.position
                );
        }
    }

    // =====================================================
    // UPDATE
    // =====================================================
    void Update()
    {
        // =====================================
        // PLAYER INPUT
        // =====================================
        if (usePlayerInput)
        {
            moveInput =
                Input.GetAxis("Vertical");

            turnInput =
                Input.GetAxis("Horizontal");
        }
    }

    // =====================================================
    // AI INPUT
    // =====================================================
    public void SetAIInput(
        float move,
        float turn
    )
    {
        moveInput =
            Mathf.Clamp(move, -1f, 1f);

        turnInput =
            Mathf.Clamp(turn, -1f, 1f);
    }

    // =====================================================
    // FIXED UPDATE
    // =====================================================
    void FixedUpdate()
    {
        if (blueprint == null)
            return;

        MoveVehicle();

        TurnVehicle();

        ApplySideGrip();

        ApplyDownforce();

        LimitSpeed();
    }

    // =====================================================
    // MOVE
    // =====================================================
    void MoveVehicle()
    {
        rb.AddForce(
            transform.forward *
            moveInput *
            blueprint.moveForce,
            ForceMode.Acceleration
        );
    }

    // =====================================================
    // TURN
    // =====================================================
    void TurnVehicle()
    {
        // só vira se estiver andando
        if (Mathf.Abs(moveInput) < 0.05f)
            return;

        float turnAmount =
            turnInput *
            blueprint.turnTorque *
            Time.fixedDeltaTime;

        Quaternion turnRotation =
            Quaternion.Euler(
                0f,
                turnAmount,
                0f
            );

        rb.MoveRotation(
            rb.rotation * turnRotation
        );
    }

    // =====================================================
    // SIDE GRIP
    // =====================================================
    void ApplySideGrip()
    {
        Vector3 localVelocity =
            transform.InverseTransformDirection(
                rb.linearVelocity
            );

        // reduz drift lateral
        localVelocity.x *= 0.85f;

        rb.linearVelocity =
            transform.TransformDirection(
                localVelocity
            );
    }

    // =====================================================
    // DOWNFORCE
    // =====================================================
    void ApplyDownforce()
    {
        rb.AddForce(
            Vector3.down * 50f,
            ForceMode.Acceleration
        );
    }

    // =====================================================
    // LIMIT SPEED
    // =====================================================
    void LimitSpeed()
    {
        Vector3 flatVelocity =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        if (
            flatVelocity.magnitude >
            blueprint.maxSpeed
        )
        {
            Vector3 limitedVelocity =
                flatVelocity.normalized *
                blueprint.maxSpeed;

            rb.linearVelocity =
                new Vector3(
                    limitedVelocity.x,
                    rb.linearVelocity.y,
                    limitedVelocity.z
                );
        }
    }
}