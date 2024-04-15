
using System.Collections.Generic;
using UnityEngine;

public class FarmerCheckpointDistributor : MonoBehaviour
{
    [SerializeField] private List<TreeCheckpoint> treeCheckpoints = new List<TreeCheckpoint>();
    [SerializeField] private GameObject _barnCheckpoint;

    private int checkpointToShowIndex;

    private void Start()
    {
        ShowRandomTreeCheckpoint();
    }

    private void OnEnable()
    {
        Farmer.onHarvestEnded += ShowBarnCheckpoint;
        Farmer.onHarvestDelivered += ShowRandomTreeCheckpoint;
    }

    private void OnDisable()
    {
        Farmer.onHarvestEnded -= ShowBarnCheckpoint;
        Farmer.onHarvestDelivered -= ShowRandomTreeCheckpoint;
    }

    public void ShowRandomTreeCheckpoint()
    {
        checkpointToShowIndex = Random.Range(0, treeCheckpoints.Count);
        treeCheckpoints[checkpointToShowIndex].gameObject.SetActive(true);
    }

    public void ShowBarnCheckpoint()
    {
        _barnCheckpoint.SetActive(true);
    }
}
