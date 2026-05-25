using UnityEngine;

public class VehicleRegisterer : MonoBehaviour
{
    public VehicleSpawnerFinal spawner;
    public GameObject startingVehicle;         // Truck inicial
    public VehicleBlueprint startingBlueprint; // Blueprint da Truck
    

    void Start()
    {
        if (spawner != null && startingVehicle != null && startingBlueprint != null)
        {
           // spawner.RegisterCurrentVehicle(startingVehicle, startingBlueprint);

            // Ajusta a câmera para seguir o veículo inicial
            VehicleOrbitCamera cam = Camera.main?.GetComponent<VehicleOrbitCamera>();
            if (cam != null)
            {
                cam.target = startingVehicle.transform;
                cam.SetDefaultDistance(startingBlueprint.cameraDistance);
                cam.SetFOV(startingBlueprint.cameraFOV);
            }
        }
        
    }
}
