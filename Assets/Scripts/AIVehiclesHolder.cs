using System.Collections.Generic;
using UnityEngine;

public class AIVehiclesHolder : MonoBehaviour
{
    [Header("AI vehicles Pool")]
    [SerializeField] private List<GameObject> _pool;
    //public GameObject objectToPool;
    [SerializeField] private int _amountToPool;

    [SerializeField] private Transform _player;

    [SerializeField] private SphereCollider _spawnTrigger;

    [SerializeField] private List<RCC_Waypoint> _spawnPoints;

    private Vector3 _currentSpawnPoint;
    private Vector3 _spawnPointOffset = new Vector3(0, 2, 0);
    private void Start()
    {
        AIVehiclesObjectsPoolInit(); 
    }

    private void AIVehiclesObjectsPoolInit()
    {
        foreach(var vehicle in _pool)
        {
            vehicle.gameObject.SetActive(false);
        }
    }

    public GameObject GetPooledVehicle()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!_pool[i].gameObject.activeInHierarchy)
            {
                _pool[i].transform.parent = gameObject.transform;
                return _pool[i];
            }
        }
        return null;
    }

    public void SpawnVehicle()
    {
        GameObject vehicle = GetPooledVehicle();
        foreach(var point in _spawnPoints)
        {
            if(Vector3.Distance(point.transform.position, _player.position) < _spawnTrigger.radius &&
               !point.isOccupied)
            {
                _currentSpawnPoint = point.transform.position;
                vehicle.transform.position = _currentSpawnPoint + _spawnPointOffset;
                vehicle.GetComponent<VehicleNPC>().enabled = true;
                vehicle.SetActive(true);
            }
        }       
    }

    private void Update()
    {
        SpawnVehicle();
    }
}
