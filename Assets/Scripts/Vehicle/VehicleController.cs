using UnityEngine;

public class VehicleController : MonoBehaviour
{
    MonoBehaviour[] scripts;

    void Awake()
    {
        scripts = GetComponentsInChildren<MonoBehaviour>();
    }

    public void SetControlled(bool value)
    {
        foreach (MonoBehaviour s in scripts)
        {
            // não desliga este script
            if (s == this)
                continue;

            s.enabled = value;
        }

        Rigidbody[] rbs =
            GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = !value;
        }

        Debug.Log(
            value
            ? "🎮 Veículo ATIVO"
            : "💤 Veículo DESATIVADO"
        );
    }
}