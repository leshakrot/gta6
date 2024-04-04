using UnityEngine;

public class VehicleSpawnTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out ESVehicleAI aICarController))
        {
            Debug.Log("EXIT");
            aICarController.gameObject.SetActive(false);
        }
    }
}
