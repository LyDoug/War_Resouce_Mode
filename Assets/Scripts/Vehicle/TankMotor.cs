using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankMotor : MonoBehaviour
{
    [Header("Blueprint")]
    public VehicleBlueprint blueprint;

    [Header("Tracks")]
    public Transform leftTrack;
    public Transform rightTrack;

    [Header("Physics")]
    public Transform centerOfMass;

    [Header("Tank Settings")]
    public float turnWhileStoppedMultiplier = 0.7f;

    [Range(0.7f, 1f)]
    public float sideGrip = 0.92f;

    public float downforce = 30f;

    Rigidbody rb;

    float moveForce;
    float turnForce;
    float maxSpeed;

    // =====================================================
    // AWAKE
    // =====================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        SetupPhysics();

        ApplyBlueprint();
    }

    // =====================================================
    // SETUP PHYSICS
    // =====================================================
    void SetupPhysics()
    {
        rb.mass = 1000f;

        rb.linearDamping = 1f;

        rb.angularDamping = 5f;

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;

        rb.collisionDetectionMode =
            CollisionDetectionMode.Continuous;

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        // IMPORTANTE
        rb.maxAngularVelocity = 4f;

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
    // APPLY BLUEPRINT
    // =====================================================
    void ApplyBlueprint()
    {
        if (blueprint == null)
        {
            Debug.LogWarning(
                "⚠️ VehicleBlueprint não atribuído!"
            );

            // FALLBACK
            moveForce = 20f;
            turnForce = 6f;
            maxSpeed = 8f;

            return;
        }

        moveForce =
            blueprint.moveForce;

        turnForce =
            blueprint.turnTorque;

        maxSpeed =
            blueprint.maxSpeed;

        rb.mass =
            blueprint.mass;
    }

    // =====================================================
    // FIXED UPDATE
    // =====================================================
    void FixedUpdate()
    {
        MoveTank();

        TurnTank();

        ApplySideGrip();

        ApplyDownforce();

        LimitSpeed();
    }

    // =====================================================
    // MOVE
    // =====================================================
    void MoveTank()
    {
        float moveInput =
            Input.GetAxis("Vertical");

        if (Mathf.Abs(moveInput) < 0.01f)
            return;

        Vector3 moveForceVector =
            transform.forward *
            moveInput *
            moveForce;

        // LEFT TRACK
        if (leftTrack != null)
        {
            rb.AddForceAtPosition(
                moveForceVector,
                leftTrack.position,
                ForceMode.Acceleration
            );
        }

        // RIGHT TRACK
        if (rightTrack != null)
        {
            rb.AddForceAtPosition(
                moveForceVector,
                rightTrack.position,
                ForceMode.Acceleration
            );
        }
    }

    // =====================================================
    // TURN
    // =====================================================
    void TurnTank()
    {
        float moveInput =
            Input.GetAxis("Vertical");

        float turnInput =
            Input.GetAxis("Horizontal");

        if (Mathf.Abs(turnInput) < 0.01f)
            return;

        float multiplier = 1f;

        // GIRO PARADO
        if (Mathf.Abs(moveInput) < 0.05f)
        {
            multiplier =
                turnWhileStoppedMultiplier;
        }

        rb.AddTorque(
            Vector3.up *
            turnInput *
            turnForce *
            multiplier,
            ForceMode.Acceleration
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

        // REMOVE DRIFT LATERAL
        localVelocity.x *= sideGrip;

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
            Vector3.down * downforce,
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
            maxSpeed
        )
        {
            Vector3 limited =
                flatVelocity.normalized *
                maxSpeed;

            rb.linearVelocity =
                new Vector3(
                    limited.x,
                    rb.linearVelocity.y,
                    limited.z
                );
        }
    }
}