using UnityEngine;

public class VehicleSpawnDistributor : MonoBehaviour
{
    public static VehicleSpawnDistributor Instance;
    private WaypointsHolder _waypointsHolder;
    private RCC_AIWaypointsContainer[] _waypoints;

    private AIVehiclesHolder _vehiclesHolder;

    private void Start()
    {
        _waypointsHolder = GetComponent<WaypointsHolder>();
        _waypoints = _waypointsHolder.waypoints;

        _vehiclesHolder = GetComponent<AIVehiclesHolder>();

        SpawnAllVehiclesInPool();
    }

    private void SpawnAllVehiclesInPool()
    {
        foreach(var vehicle in _vehiclesHolder.pool)
        {
            var spawnPosition = _waypoints[Random.Range(0, _waypoints.Length)].transform.GetChild(Random.Range(0, _waypoints.Length)).position +
                new Vector3(0,1f,0);

            vehicle.transform.position = spawnPosition;
            vehicle.SetActive(true);
        }
    }

    public void SpawnRandomVehicle()
    {
        GameObject vehicle = AIVehiclesHolder.instance.GetPooledVehicle();

        var spawnPosition = _waypoints[Random.Range(0, _waypoints.Length)].transform.GetChild(Random.Range(0, _waypoints.Length)).position +
    new Vector3(0, 1f, 0);

        vehicle.transform.position = spawnPosition;
        vehicle.SetActive(true);
    }

}
