using UnityEngine;

public class Passenger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out BusWorker worker))
        {
            worker.AddPassenger();
            gameObject.SetActive(false);
        }
    }
}
