using UnityEngine;

public class DeliveryDestinationCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DeliveryMan player))
        {
            DeliveryMan.onDelivered?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
