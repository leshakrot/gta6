using UnityEngine;

public class BusDirectionArrow : MonoBehaviour
{
    private Transform _target;

    private void Start()
    {
        SetTarget();
    }

    private void OnEnable()
    {
        BusWorker.onBusStopPassed += SetTarget;
        BusWorker.onCheckpointPassed += SetTarget;
    }

    private void OnDisable()
    {
        BusWorker.onBusStopPassed -= SetTarget;
        BusWorker.onCheckpointPassed -= SetTarget;
    }

    void Update()
    {
        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
    }

    private void SetTarget()
    {
        _target = BusCheckpointDistributor.instance.GetActiveCheckpointTransform();
    }
}
