using UnityEngine;

public class StockCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DeliveryMan player))
        {
            DeliveryMan.onDeliveryStarted?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
