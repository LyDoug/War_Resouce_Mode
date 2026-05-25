using UnityEngine;

public class TankGunBallistic : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public ProjectileBallistic shellPrefab;

    [Header("Weapon Stats")]
    public float fireRate = 5f;          // tiros por segundo
    public float shellSpeed = 120f;
    public float recoilDistance = 0.3f;

    [HideInInspector]
    public float currentShellDamage = 25f;

    [Header("Recoil")]
    public float recoilReturnSpeed = 10f;

    [Header("VFX")]
    public GameObject muzzleFlashPrefab;
    public float muzzleFlashDuration = 1.5f;

    float nextFireTime;
    Vector3 cannonInitialLocalPos;
    bool isRecoiling;

    void Start()
    {
        if (!firePoint)
        {
            Debug.LogError("TankGunBallistic: FirePoint não atribuído.");
            enabled = false;
            return;
        }

        cannonInitialLocalPos = transform.localPosition;
    }

    void Update()
    {
        HandleFire();
        HandleRecoilReturn();
    }

    void HandleFire()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void Fire()
{
    // 🔥 MUZZLE FLASH (FIX REAL)
    if (muzzleFlashPrefab)
    {   
        GameObject flash = Instantiate(
            muzzleFlashPrefab,
            firePoint.position,
            firePoint.rotation,
            firePoint // 🔥 parentado corretamente
        );

        flash.transform.localPosition = Vector3.zero;
        flash.transform.localRotation = Quaternion.identity;
        flash.transform.localScale = Vector3.one;

        ParticleSystem[] particles = flash.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
        {
            ps.Play();
        }

        Destroy(flash, 2f);
    }

    // 💣 PROJÉTIL
    ProjectileBallistic shell = Instantiate(
        shellPrefab,
        firePoint.position,
        firePoint.rotation
    );

    shell.Init(shellSpeed, currentShellDamage);

    ApplyRecoil();
}


    void ApplyRecoil()
    {
        transform.localPosition = cannonInitialLocalPos - Vector3.forward * recoilDistance;
        isRecoiling = true;
    }

    void HandleRecoilReturn()
    {
        if (!isRecoiling) return;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            cannonInitialLocalPos,
            Time.deltaTime * recoilReturnSpeed
        );

        if (Vector3.Distance(transform.localPosition, cannonInitialLocalPos) < 0.001f)
        {
            transform.localPosition = cannonInitialLocalPos;
            isRecoiling = false;
        }
    }
}
