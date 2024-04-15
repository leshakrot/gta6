using UnityEngine;

public class BarnCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Farmer player))
        {
            Farmer.onHarvestDelivered?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
