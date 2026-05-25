using UnityEngine;

public class StructureSpawnZone : MonoBehaviour
{
    public VehicleSpawnerFinal spawner;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Unit"))
            return;

        Debug.Log("🏭 Entrou na zona");

        // 🔥 desativa somente os conhecidos
        VehicleSpawnerFinal[] allSpawners =
            FindObjectsOfType<VehicleSpawnerFinal>();

        foreach (VehicleSpawnerFinal s in allSpawners)
        {
            s.isPlayerSpawner = false;
        }

        if (spawner != null)
        {
            spawner.isPlayerSpawner = true;

            Debug.Log(
                "✅ Spawner ativado: " +
                spawner.name
            );
        }
    }
}