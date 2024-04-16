using UnityEngine;

public class BusStopCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BusWorker player))
        {
            BusWorker.onStopAtBusStop?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
