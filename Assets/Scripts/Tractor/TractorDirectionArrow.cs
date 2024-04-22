using UnityEngine;

public class TractorDirectionArrow : MonoBehaviour
{
    private Transform _target;

    private void Start()
    {
        SetTarget();
    }

    private void OnEnable()
    {
        TractorWorker.onCheckpointPassed += SetTarget;
    }

    private void OnDisable()
    {
        TractorWorker.onCheckpointPassed -= SetTarget;
    }

    void Update()
    {
        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
    }

    private void SetTarget()
    {
        _target = TractorCheckpointDistributor.instance.GetActiveCheckpointTransform();
    }
}
