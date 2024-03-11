using UnityEngine;

public class VehicleSpawnTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out RCC_AICarController aICarController))
        {
            Debug.Log("EXIT");
            aICarController.gameObject.SetActive(false);
        }
    }
}
