using UnityEngine;

public class DeliveryDirectionArrow : MonoBehaviour
{
    private Transform _target;

    private void Start()
    {
        SetTargetStock();
    }
    private void OnEnable()
    {
        DeliveryMan.onDeliveryStarted += SetTargetDest;
        DeliveryMan.onDelivered += SetTargetStock;
    }

    private void OnDisable()
    {
        DeliveryMan.onDeliveryStarted -= SetTargetDest;
        DeliveryMan.onDelivered -= SetTargetStock;
    }

    void Update()
    {
        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
    }

    private void SetTargetStock()
    {
        _target = DeliveryWorkCheckpointDistributor.instance.GetStockCheckpointTransform();
    }

    private void SetTargetDest()
    {
        _target = DeliveryWorkCheckpointDistributor.instance.GetDestCheckpointTransform();
    }
}
