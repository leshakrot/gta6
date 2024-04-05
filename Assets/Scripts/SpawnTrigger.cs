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

        if (other.TryGetComponent(out vSimpleMeleeAI_Controller npcController))
        {
            Debug.Log("EXIT");
            npcController.gameObject.SetActive(false);
        }
    }
}
