using UnityEngine;

public class BaseDestructibleWithHealthVisual : MonoBehaviour
{
    [Header("Spawner Reference")]
    public VehicleSpawnerFinal spawner;

    [Header("Health Settings")]
    public float maxHealth = 200f;

    [SerializeField]
    private float currentHealth;

    bool destroyed = false;

    [Header("Respawn Settings")]
    public Transform[] respawnPoints;

    public float respawnDelay = 5f;

    [Header("Visuals")]
    public Renderer baseRenderer;

    public Color fullHealthColor = Color.green;

    public Color lowHealthColor = Color.red;

    CaptureTriggerAdvanced capture;

    Rigidbody[] rigidbodies;

    // =========================================================
    // START
    // =========================================================
    void Start()
    {
        currentHealth = maxHealth;

        // CAPTURE
        capture =
            GetComponentInChildren<CaptureTriggerAdvanced>(true);

        // RIGIDBODIES
        rigidbodies =
            GetComponentsInChildren<Rigidbody>(true);

        // AUTO RENDERER
        if (baseRenderer == null)
        {
            baseRenderer =
                GetComponentInChildren<Renderer>();

            if (baseRenderer == null)
            {
                Debug.LogWarning(
                    "⚠️ Renderer não encontrado na base!"
                );
            }
        }

        UpdateColor();
    }

    // =========================================================
    // DAMAGE
    // =========================================================
    public void TakeDamage(float damage)
    {
        if (destroyed)
            return;

        currentHealth -= damage;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0f,
                maxHealth
            );

        UpdateColor();

        Debug.Log(
            "💥 Base tomou dano: " +
            currentHealth +
            "/" +
            maxHealth
        );

        if (currentHealth <= 0f)
        {
            DestroyBase();
        }
    }

    // =========================================================
    // VISUAL
    // =========================================================
    void UpdateColor()
    {
        if (baseRenderer == null)
            return;

        float t =
            1f - (currentHealth / maxHealth);

        Color finalColor =
            Color.Lerp(
                fullHealthColor,
                lowHealthColor,
                t
            );

        // URP/HDRP
        if (
            baseRenderer.material.HasProperty(
                "_BaseColor"
            )
        )
        {
            baseRenderer.material.SetColor(
                "_BaseColor",
                finalColor
            );
        }
        else
        {
            baseRenderer.material.color =
                finalColor;
        }
    }

    // =========================================================
    // DESTROY
    // =========================================================
    void DestroyBase()
    {
        if (destroyed)
            return;

        destroyed = true;

        Debug.Log("💥 Base destruída!");

        // =====================================
        // DESLIGA RENDERERS
        // =====================================
        Renderer[] rends =
            GetComponentsInChildren<Renderer>(true);

        foreach (Renderer r in rends)
        {
            r.enabled = false;
        }

        // =====================================
        // DESLIGA COLLIDERS
        // =====================================
        Collider[] cols =
            GetComponentsInChildren<Collider>(true);

        foreach (Collider c in cols)
        {
            c.enabled = false;
        }

        // =====================================
        // DESLIGA CAPTURE
        // =====================================
        if (capture != null)
        {
            capture.enabled = false;
        }

        // =====================================
        // DESLIGA RIGIDBODIES
        // =====================================
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
        }

        // =====================================
        // RESPAWN
        // =====================================
        Invoke(
            nameof(RespawnBase),
            respawnDelay
        );
    }

    // =========================================================
    // RESPAWN
    // =========================================================
    void RespawnBase()
    {
        Debug.Log("🔄 Respawnando base...");

        // =====================================
        // POSIÇÃO NOVA
        // =====================================
        if (
            respawnPoints != null &&
            respawnPoints.Length > 0
        )
        {
            Transform newPoint =
                respawnPoints[
                    Random.Range(
                        0,
                        respawnPoints.Length
                    )
                ];

            transform.position =
                newPoint.position;

            transform.rotation =
                newPoint.rotation;
        }
        else
        {
            Debug.LogWarning(
                "⚠️ Nenhum ponto de respawn definido!"
            );
        }

        // =====================================
        // RESET HEALTH
        // =====================================
        currentHealth = maxHealth;

        destroyed = false;

        // =====================================
        // REATIVA RENDERERS
        // =====================================
        Renderer[] rends =
            GetComponentsInChildren<Renderer>(true);

        foreach (Renderer r in rends)
        {
            r.enabled = true;
        }

        // =====================================
        // REATIVA COLLIDERS
        // =====================================
        Collider[] cols =
            GetComponentsInChildren<Collider>(true);

        foreach (Collider c in cols)
        {
            c.enabled = true;
        }

        // =====================================
        // REATIVA RIGIDBODIES
        // =====================================
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // =====================================
        // RESET CAPTURE
        // =====================================
        if (capture != null)
        {
            capture.enabled = true;

            capture.ResetCapture();

            Debug.Log(
                "🏳️ Capture resetado"
            );
        }
        else
        {
            Debug.LogWarning(
                "⚠️ CaptureTrigger não encontrado!"
            );
        }

        // =====================================
        // RESET VISUAL
        // =====================================
        UpdateColor();

        Debug.Log("🏗️ Base reapareceu");
    }

    // =========================================================
    // MANUAL RESPAWN
    // =========================================================
    public void Respawn()
    {
        RespawnBase();
    }

    // =========================================================
    // CAPTURE
    // =========================================================
    public void OnCaptured()
    {
        Debug.Log("🏭 Base capturada");
    }

    // =========================================================
    // GETTERS
    // =========================================================
    public float GetHealth()
    {
        return currentHealth;
    }

    public bool IsDestroyed()
    {
        return destroyed;
    }
}

