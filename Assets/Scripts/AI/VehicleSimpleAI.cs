using UnityEngine;

public class VehicleSimpleAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    public float waypointReachDistance = 10f;

    VehicleMotor motor;

    // =====================================================
    // START
    // =====================================================
    void Start()
    {
        motor =
            GetComponent<VehicleMotor>();

        if (motor == null)
        {
            motor =
                GetComponentInChildren<VehicleMotor>();
        }
    }

    // =====================================================
    // UPDATE
    // =====================================================
    void Update()
    {
        if (target == null)
            return;

        if (motor == null)
            return;

        Vector3 dir =
            target.position -
            transform.position;

        dir.y = 0f;

        // =====================================
        // DISTÂNCIA
        // =====================================
        if (
            dir.magnitude <
            waypointReachDistance
        )
        {
            motor.SetAIInput(0f, 0f);
            return;
        }

        // =====================================
        // ÂNGULO
        // =====================================
        float angle =
            Vector3.SignedAngle(
                transform.forward,
                dir.normalized,
                Vector3.up
            );

        float turn =
            Mathf.Clamp(
                angle / 45f,
                -1f,
                1f
            );

        float move = 1f;

        // desacelera em curvas fortes
        if (Mathf.Abs(angle) > 70f)
        {
            move = 0.3f;
        }

        // =====================================
        // ENVIA INPUT
        // =====================================
        motor.SetAIInput(
            move,
            turn
        );
    }
}