using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsHolder : MonoBehaviour
{
    public RCC_AIWaypointsContainer[] waypoints;

    private void Start()
    {
        waypoints = FindObjectsByType<RCC_AIWaypointsContainer>(FindObjectsSortMode.None);
    }
}
