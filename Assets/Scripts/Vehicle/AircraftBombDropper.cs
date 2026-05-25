using UnityEngine;

public class AircraftBombDropper : MonoBehaviour
{
    [Header("Bomb Settings")]
    public BombProjectile bombPrefab;
    public Transform bombDropPoint;

    [Header("Bomb Capacity")]
    public int maxBombs = 6;
    public float dropCooldown = 0.35f;

    [Header("Input")]
    public KeyCode dropKey = KeyCode.Space;

    Rigidbody rb;
    float nextDropTime;
    int currentBombs;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentBombs = maxBombs;
    }

    void Update()
    {
        if (Input.GetKeyDown(dropKey))
        {
            TryDropBomb();
        }
    }

    // ================= DROP =================
    void TryDropBomb()
    {
        if (Time.time < nextDropTime) return;
        if (currentBombs <= 0) return;
        if (bombPrefab == null || bombDropPoint == null) return;

        DropBomb();
    }

    void DropBomb()
    {
        nextDropTime = Time.time + dropCooldown;
        currentBombs--;

        BombProjectile bomb = Instantiate(
            bombPrefab,
            bombDropPoint.position,
            bombDropPoint.rotation
        );

        // Herda velocidade do avião
        Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
        bombRb.linearVelocity = rb.linearVelocity;

        // Ignora colisão com o próprio avião
        Collider[] aircraftColliders = GetComponentsInChildren<Collider>();
        bomb.IgnoreOwnerCollisions(aircraftColliders);
    }

    // ================= DEBUG =================
    public void ReloadBombs()
    {
        currentBombs = maxBombs;
    }
}
