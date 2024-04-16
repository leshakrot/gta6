using UnityEngine;

public class LastBusCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BusWorker player))
        {
            BusWorker.onCheckpointPassed?.Invoke();
            BusWorker.onWorkStop?.Invoke();
        }
    }
}
