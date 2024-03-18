using Invector.vCharacterController.AI;
using System.Collections.Generic;
using UnityEngine;

public class VehicleNPC : MonoBehaviour
{
    private RCC_CarControllerV3 _controller;
    private RCC_AICarController _aiController;
    private int _nearestWaypointIndex;
    private float _nearestWaypointDistance = 1000;

    private List<RCC_Waypoint> _waypoints;
    void Start()
    {
        _controller = GetComponent<RCC_CarControllerV3>();
        _aiController = GetComponent<RCC_AICarController>();
        _controller.canControl = true;

        _waypoints = _aiController.waypointsContainer.waypoints;      
    }

    private int FindNearestWaypointIndex()
    {
        for (int i = 0; i < _waypoints.Count; i++)
        {
            if (Vector3.Distance(_waypoints[i].transform.position, transform.position) < _nearestWaypointDistance)
            {    
                _nearestWaypointDistance = Vector3.Distance(_waypoints[i].transform.position, transform.position);
                _nearestWaypointIndex = i;
            }
        }
        return _nearestWaypointIndex;
    }

    private void OnEnable()
    {
        Debug.Log("RESPAWNED");
        _nearestWaypointDistance = 1000;
        _aiController.currentWaypointIndex = FindNearestWaypointIndex();
        transform.LookAt(_waypoints[_nearestWaypointIndex + 1].transform.position);
    }
}
