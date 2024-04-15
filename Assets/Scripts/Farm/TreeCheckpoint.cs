using UnityEngine;

public class TreeCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Farmer player))
        {
            Farmer.onHarvestStarted?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
