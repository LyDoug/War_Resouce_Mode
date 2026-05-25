using UnityEngine;

[CreateAssetMenu(menuName = "Vehicles/Vehicle Blueprint")]
public class VehicleBlueprint : ScriptableObject
{
    [Header("Info")]
    public string vehicleName;
    public GameObject prefab;
    public float cost = 100f;

    [Header("Role")]
    public VehicleRole role;    

    [Header("Movement")]
    public float moveForce = 120000f;
    public float turnTorque = 35000f;
    public float maxSpeed = 12f;
    public float mass = 30000f;

    [Header("Weapon")]
    public float fireRate = 1.5f;
    public float shellSpeed = 120f;
    public float shellDamage = 100f;
    public float recoilDistance = 0.3f;

    [Header("Camera")]
    public float cameraDistance = 6f;
    public float cameraFOV = 60f;


    // ================= RESOURCE REQUIREMENTS =================
    [System.Serializable]
    public class ResourceCost
    {
    public ResourceType resource;
    public int amount;
    }

    public ResourceCost[] costs;
}