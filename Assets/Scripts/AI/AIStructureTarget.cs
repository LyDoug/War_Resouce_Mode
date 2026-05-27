using UnityEngine;

public class AIStructureTarget : MonoBehaviour
{
    [Header("Search")]
    public float refreshRate = 5f;

    VehicleSimpleAI ai;

    TeamMember teamMember;

    // =====================================================
    // START
    // =====================================================
    void Start()
    {
        ai =
            GetComponent<VehicleSimpleAI>();

        teamMember =
            GetComponent<TeamMember>();

        InvokeRepeating(
            nameof(FindTarget),
            1f,
            refreshRate
        );
    }

    // =====================================================
    // FIND TARGET
    // =====================================================
    void FindTarget()
    {
        if (ai == null)
            return;

        if (teamMember == null)
            return;

        CaptureTriggerAdvanced[] allBases =
            FindObjectsOfType<CaptureTriggerAdvanced>();

        CaptureTriggerAdvanced bestTarget = null;

        float closestDistance =
            Mathf.Infinity;

        foreach (var baseTarget in allBases)
        {
            if (baseTarget == null)
                continue;

            // ignora bases do próprio time
            if (
                baseTarget.currentOwner ==
                teamMember.team
            )
            {
                continue;
            }

            float dist =
                Vector3.Distance(
                    transform.position,
                    baseTarget.transform.position
                );

            if (dist < closestDistance)
            {
                closestDistance = dist;

                bestTarget = baseTarget;
            }
        }

        // aplica target
        if (bestTarget != null)
        {
            ai.target =
                bestTarget.transform;

            Debug.Log(
                "🎯 Novo alvo AI: " +
                bestTarget.name
            );
        }
    }
}