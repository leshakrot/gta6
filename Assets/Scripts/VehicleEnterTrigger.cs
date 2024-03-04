using UnityEngine;

public class VehicleEnterTrigger : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out VehicleDriver driver))
        {
            driver.canEnterVehicle = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out VehicleDriver driver))
        {
            driver.canEnterVehicle = false;
        }
    }
}
