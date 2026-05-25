using UnityEngine;

public class TruckCargo : MonoBehaviour
{
    [Header("Cargo")]
    public ResourceType carriedResource = ResourceType.None;

    public int amount = 0;
    public int maxCapacity = 1;

    [Header("Cargo Visuals")]
    public GameObject steelCargo;
    public GameObject oilCargo;

    void Start()
    {
        UpdateVisual();
    }

    void Update()
    {
        // 🔥 TESTE TEMPORÁRIO
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load(ResourceType.Steel, 1);
        }
    }

    // =====================================================
    // VISUAL
    // =====================================================
    void UpdateVisual()
    {
        // desliga tudo
        if (steelCargo != null)
            steelCargo.SetActive(false);

        if (oilCargo != null)
            oilCargo.SetActive(false);

        // sem carga
        if (!HasCargo())
            return;

        // aço
        if (carriedResource == ResourceType.Steel)
        {
            if (steelCargo != null)
                steelCargo.SetActive(true);
        }

        // petróleo
        if (carriedResource == ResourceType.Oil)
        {
            if (oilCargo != null)
                oilCargo.SetActive(true);
        }
    }

    // =====================================================
    // COLETA NA MINA
    // =====================================================
    void OnTriggerEnter(Collider other)
    {
        ResourceNode node =
            other.GetComponent<ResourceNode>() ??
            other.GetComponentInParent<ResourceNode>() ??
            other.GetComponentInChildren<ResourceNode>();

        if (node == null)
            return;

        TeamMember tm = GetComponent<TeamMember>();

        if (tm == null)
            return;

        node.TryCollect(this, tm.team);
    }

    // =====================================================
    // TEM CARGA
    // =====================================================
    public bool HasCargo()
    {
        return carriedResource != ResourceType.None && amount > 0;
    }

    // =====================================================
    // CARREGAR
    // =====================================================
    public bool Load(ResourceType type, int value)
    {
        if (HasCargo())
        {
            Debug.Log("🚫 Truck já possui carga");
            return false;
        }

        carriedResource = type;

        amount = Mathf.Clamp(
            value,
            0,
            maxCapacity
        );

        Debug.Log(
            $"📦 Truck carregou {amount} de {type}"
        );

        UpdateVisual();

        return true;
    }

    // =====================================================
    // DESCARREGAR
    // =====================================================
    public bool Unload(BaseInventory inventory)
    {
        if (!HasCargo())
        {
            Debug.Log("🚫 Truck vazio");
            return false;
        }

        // adiciona recurso
        inventory.AddResource(
            carriedResource,
            amount
        );

        Debug.Log(
            $"🏭 Entregue {amount} de {carriedResource}"
        );

        // =================================================
        // DESBLOQUEIA TANK
        // =================================================
        TeamMember tm = GetComponent<TeamMember>();

        if (tm != null && GameManager.Instance != null)
        {
            if (tm.team == Team.TeamA)
            {
                GameManager.Instance.teamATankUnlocked = true;
            }

            if (tm.team == Team.TeamB)
            {
                GameManager.Instance.teamBTankUnlocked = true;
            }

            Debug.Log("🛡️ Tank desbloqueado!");
        }

        // limpa carga
        carriedResource = ResourceType.None;
        amount = 0;

        UpdateVisual();

        return true;
    }
}