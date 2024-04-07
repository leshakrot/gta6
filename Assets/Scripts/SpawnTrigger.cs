using Invector.vCharacterController.AI;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
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
