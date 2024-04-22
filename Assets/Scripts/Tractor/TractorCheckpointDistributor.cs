using System.Collections.Generic;
using UnityEngine;

public class TractorCheckpointDistributor : MonoBehaviour
{
    public static TractorCheckpointDistributor instance;

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
        TractorWorker.onWorkStarted += ShowNextCheckpoint;
        TractorWorker.onCheckpointPassed += ShowNextCheckpoint;
        TractorWorker.onWorkStop += ResetCheckpoints;
    }

    private void OnDisable()
    {
        TractorWorker.onWorkStarted -= ShowNextCheckpoint;
        TractorWorker.onCheckpointPassed -= ShowNextCheckpoint;
        TractorWorker.onWorkStop -= ResetCheckpoints;
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
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.SetActive(false);
        }
    }

    public Transform GetActiveCheckpointTransform()
    {
        return checkpoints[checkpointToDirectionArrowIndex].transform;
    }
}
