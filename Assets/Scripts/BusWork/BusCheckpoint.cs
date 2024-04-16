using UnityEngine;

public class BusCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BusWorker player))
        {
            BusWorker.onCheckpointPassed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
