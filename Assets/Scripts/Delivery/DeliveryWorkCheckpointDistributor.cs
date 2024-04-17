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
        DeliveryMan.onWorkStarted += ShowRandomDeliveryCheckpoint;
        DeliveryMan.onDeliveryEnded += ShowStockCheckpoint;
        DeliveryMan.onDelivered += ShowRandomDeliveryCheckpoint;
        DeliveryMan.onWorkStop += HideCheckpoints;
    }

    private void OnDisable()
    {
        DeliveryMan.onWorkStarted -= ShowRandomDeliveryCheckpoint;
        DeliveryMan.onDeliveryEnded -= ShowStockCheckpoint;
        DeliveryMan.onDelivered -= ShowRandomDeliveryCheckpoint;
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

    public Transform GetActiveCheckpointTransform()
    {
        if(deliveryCheckpoints[checkpointToShowIndex].gameObject.activeSelf)
            return deliveryCheckpoints[checkpointToShowIndex].transform;
        else return _stockCheckpoint.transform;
    }
}
