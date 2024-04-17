using UnityEngine;

public class DeliveryDirectionArrow : MonoBehaviour
{
    private Transform _target;

    private void Start()
    {
        SetTarget();
    }
    private void OnEnable()
    {
        DeliveryMan.onDelivered += SetTarget;
        DeliveryMan.onDeliveryEnded += SetTarget;
    }

    private void OnDisable()
    {
        DeliveryMan.onDelivered -= SetTarget;
        DeliveryMan.onDeliveryEnded -= SetTarget;
    }

    void Update()
    {
        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
    }

    private void SetTarget()
    {
        _target = DeliveryWorkCheckpointDistributor.instance.GetActiveCheckpointTransform();
    }
}
