using UnityEngine;

public class TractorCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TractorWorker player))
        {
            TractorWorker.onCheckpointPassed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
