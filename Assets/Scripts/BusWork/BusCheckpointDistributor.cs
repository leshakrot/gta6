using System.Collections.Generic;
using UnityEngine;

public class BusCheckpointDistributor : MonoBehaviour
{
    public static BusCheckpointDistributor instance;

    [SerializeField] private List<GameObject> checkpoints = new List<GameObject>();

    private int checkpointToShowIndex;
    private int checkpointToDirectionArrowIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ResetCheckpoints();
    }

    private void OnEnable()
    {
        BusWorker.onWorkStarted += ShowNextCheckpoint;
        BusWorker.onCheckpointPassed += ShowNextCheckpoint;
        BusWorker.onBusStopPassed += ShowNextCheckpoint;
        BusWorker.onWorkStop += ResetCheckpoints;
    }

    private void OnDisable()
    {
        BusWorker.onWorkStarted -= ShowNextCheckpoint;
        BusWorker.onCheckpointPassed -= ShowNextCheckpoint;
        BusWorker.onBusStopPassed -= ShowNextCheckpoint;
        BusWorker.onWorkStop -= ResetCheckpoints;
    }

    public void ShowNextCheckpoint()
    {
        checkpoints[checkpointToShowIndex].SetActive(true);
        checkpointToDirectionArrowIndex = checkpointToShowIndex;
        checkpointToShowIndex++;
    }

    public void ResetCheckpoints()
    {
        checkpointToShowIndex = 0;
        checkpointToDirectionArrowIndex = 0;
        foreach(var checkpoint in checkpoints)
        {
            checkpoint.SetActive(false);
        }
    }

    public Transform GetActiveCheckpointTransform()
    {
        return checkpoints[checkpointToDirectionArrowIndex].transform;
    }
}
