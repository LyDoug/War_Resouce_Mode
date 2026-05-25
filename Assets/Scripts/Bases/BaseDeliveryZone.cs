using UnityEngine;

public class BaseDeliveryZone : MonoBehaviour
{
    public BaseInventory inventory;

    void OnTriggerEnter(Collider other)
    {
        TruckCargo cargo =
            other.GetComponent<TruckCargo>() ??
            other.GetComponentInParent<TruckCargo>();

        if (cargo == null)
            return;

        if (inventory == null)
            return;

        cargo.Unload(inventory);

        Debug.Log("🏭 Recurso entregue!");
    }
}