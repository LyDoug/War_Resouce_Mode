using UnityEngine;

public class VehicleStatsApplier : MonoBehaviour
{
    public VehicleBlueprint blueprint;

    void Start()
    {
        if (blueprint == null)
        {
            Debug.LogError("VehicleStatsApplier: Blueprint não atribuído.");
            return;
        }

        ApplyBlueprint();
    }

    public void ApplyBlueprint()
    {
        // Movimento
        TankMovement movement = GetComponent<TankMovement>();
        Rigidbody rb = GetComponent<Rigidbody>();

        if (movement != null)
        {
            movement.moveForce = blueprint.moveForce;
            movement.turnTorque = blueprint.turnTorque;
            movement.maxSpeed = blueprint.maxSpeed;
        }

        if (rb != null)
        {
            rb.mass = blueprint.mass;
        }

        // Arma
        TankGunBallistic gun = GetComponentInChildren<TankGunBallistic>();
if (gun != null)
{
    gun.fireRate = blueprint.fireRate;
    gun.shellSpeed = blueprint.shellSpeed;
    gun.currentShellDamage = blueprint.shellDamage;
    gun.recoilDistance = blueprint.recoilDistance;
}

        // Camera
VehicleOrbitCamera cam = GetComponentInChildren<VehicleOrbitCamera>();
if (cam != null)
{
    cam.SetDefaultDistance(blueprint.cameraDistance);
    cam.SetFOV(blueprint.cameraFOV);
}


        Debug.Log("Blueprint aplicado: " + blueprint.vehicleName);
    }
    
}
