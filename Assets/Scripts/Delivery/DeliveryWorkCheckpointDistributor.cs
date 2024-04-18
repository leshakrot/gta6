using System.Collections.Generic;
using UnityEngine;

public class DeliveryWorkCheckpointDistributor : MonoBehaviour
{
    public static DeliveryWorkCheckpointDistributor instance;

    [SerializeField] private List<DeliveryDestinationCheckpoint> deliveryCheckpoints = new List<DeliveryDestinationCheckpoint>();
    [SerializeField] private GameObject _stockCheckpoint;

    private int checkpointToShowIndex;

    private void OnEnable()
    {
        DeliveryMan.onWorkStarted += ShowStockCheckpoint;
        DeliveryMan.onDeliveryStarted += ShowRandomDeliveryCheckpoint;
        DeliveryMan.onDelivered += ShowStockCheckpoint;
        DeliveryMan.onWorkStop += HideCheckpoints;
    }

    private void OnDisable()
    {
        DeliveryMan.onWorkStarted -= ShowStockCheckpoint;
        DeliveryMan.onDeliveryStarted -= ShowRandomDeliveryCheckpoint;
        DeliveryMan.onDelivered -= ShowStockCheckpoint;
        DeliveryMan.onWorkStop -= HideCheckpoints;
    }

    private void Awake()
    {
        instance = this;
    }

    public void ShowRandomDeliveryCheckpoint()
    {
        checkpointToShowIndex = Random.Range(0, deliveryCheckpoints.Count);
        deliveryCheckpoints[checkpointToShowIndex].gameObject.SetActive(true);
    }

    public void HideCheckpoints()
    {
        foreach (var checkpoint in deliveryCheckpoints)
        {
            checkpoint.gameObject.SetActive(false);
            _stockCheckpoint.SetActive(false);
        }
    }

    public void ShowStockCheckpoint()
    {
        _stockCheckpoint.SetActive(true);
    }

    public Transform GetStockCheckpointTransform()
    {
        return _stockCheckpoint.transform;
    }

    public Transform GetDestCheckpointTransform()
    {
        return deliveryCheckpoints[checkpointToShowIndex].transform;
    }
}
