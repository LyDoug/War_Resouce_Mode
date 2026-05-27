using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject vehiclePrefab;

    public Transform spawnPoint;

    [Header("Team")]
    public Team enemyTeam = Team.TeamB;

    [Header("Timer")]
    public float spawnInterval = 20f;

    public int maxVehicles = 5;

    int currentVehicles;

    // =====================================================
    // START
    // =====================================================
    void Start()
    {
        InvokeRepeating(
            nameof(SpawnEnemy),
            2f,
            spawnInterval
        );
    }

    // =====================================================
    // SPAWN
    // =====================================================
    void SpawnEnemy()
    {
        if (currentVehicles >= maxVehicles)
            return;

        if (vehiclePrefab == null)
        {
            Debug.LogWarning(
                "⚠️ Vehicle Prefab não atribuído"
            );

            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning(
                "⚠️ Spawn Point não atribuído"
            );

            return;
        }

        // =====================================
        // INSTANTIATE
        // =====================================
        GameObject obj =
            Instantiate(
                vehiclePrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

        // =====================================
        // TEAM
        // =====================================
        TeamMember tm =
            obj.GetComponentInChildren<TeamMember>();

        if (tm != null)
        {
            tm.SetTeam(enemyTeam);
        }

        // =====================================
        // AI
        // =====================================
        VehicleSimpleAI ai =
            obj.GetComponent<VehicleSimpleAI>();

        if (ai == null)
        {
            ai =
                obj.AddComponent<VehicleSimpleAI>();
        }

        // =====================================
        // TARGET SYSTEM
        // =====================================
        AIStructureTarget targetAI =
            obj.GetComponent<AIStructureTarget>();

        if (targetAI == null)
        {
            obj.AddComponent<AIStructureTarget>();
        }

        // =====================================
        // PLAYER INPUT OFF
        // =====================================
        VehicleMotor motor =
            obj.GetComponentInChildren<VehicleMotor>();

        if (motor != null)
        {
            motor.usePlayerInput = false;
        }

        currentVehicles++;

        Debug.Log(
            "🤖 Enemy spawned: " +
            obj.name
        );
    }
}