using UnityEngine;

public class LastTractorCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TractorWorker player))
        {
            TractorWorker.onWorkStop?.Invoke();
        }
    }
}
