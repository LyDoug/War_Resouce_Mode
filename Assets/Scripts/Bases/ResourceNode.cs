using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [Header("Resource")]
    public ResourceType resourceType = ResourceType.Steel;

    [Header("Owner")]
    public Team owner = Team.Neutral;

    // ================= DONO =================
    public void SetOwner(Team newOwner)
    {
        owner = newOwner;

        Debug.Log("⛏️ Mina agora pertence a: " + owner);
    }

    // ================= COLETA =================
    public bool TryCollect(TruckCargo truck, Team collectorTeam)
    {
        // sem dono
        if (owner == Team.Neutral)
        {
            Debug.Log("⚪ Mina neutra");
            return false;
        }

        // time errado
        if (collectorTeam != owner)
        {
            Debug.Log("⛔ Mina inimiga");
            return false;
        }

        // truck inválido
        if (truck == null)
        {
            Debug.Log("❌ Truck NULL");
            return false;
        }

        // 🔥 USA LOAD CORRETAMENTE
        bool loaded = truck.Load(resourceType, 1);

        if (loaded)
        {
            Debug.Log($"📦 Coletado 1 de {resourceType}");
        }

        return loaded;
    }
}