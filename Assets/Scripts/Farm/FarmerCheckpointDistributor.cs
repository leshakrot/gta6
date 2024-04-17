using System.Collections.Generic;
using UnityEngine;

public class FarmerCheckpointDistributor : MonoBehaviour
{
    [SerializeField] private List<TreeCheckpoint> treeCheckpoints = new List<TreeCheckpoint>();
    [SerializeField] private GameObject _barnCheckpoint;

    private int checkpointToShowIndex;

    private void OnEnable()
    {
        Farmer.onWorkStarted += ShowRandomTreeCheckpoint;
        Farmer.onHarvestEnded += ShowBarnCheckpoint;
        Farmer.onHarvestDelivered += ShowRandomTreeCheckpoint;
        Farmer.onWorkStop += HideCheckpoints;
    }

    private void OnDisable()
    {
        Farmer.onWorkStarted -= ShowRandomTreeCheckpoint;
        Farmer.onHarvestEnded -= ShowBarnCheckpoint;
        Farmer.onHarvestDelivered -= ShowRandomTreeCheckpoint;
        Farmer.onWorkStop -= HideCheckpoints;
    }

    public void ShowRandomTreeCheckpoint()
    {
        checkpointToShowIndex = Random.Range(0, treeCheckpoints.Count);
        treeCheckpoints[checkpointToShowIndex].gameObject.SetActive(true);
    }

    public void HideCheckpoints()
    {
        foreach(var checkpoint in treeCheckpoints)
        {
            checkpoint.gameObject.SetActive(false);
            _barnCheckpoint.SetActive(false);
        }
    }

    public void ShowBarnCheckpoint()
    {
        _barnCheckpoint.SetActive(true);
    }
}
