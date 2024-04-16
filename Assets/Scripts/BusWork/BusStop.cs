using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField] private List<Passenger> _passengers = new List<Passenger>();
    [SerializeField] private Transform _spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out BusWorker worker))
        {
            SpawnPassengers();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BusWorker worker))
        {
            HidePassengers();
        }
    }

    private void SpawnPassengers()
    {
        foreach(var passenger in _passengers)
        {
            passenger.transform.position = _spawnPoint.transform.position;
            if (Random.Range(0, 2) > 0.5f) passenger.gameObject.SetActive(true); 
        }
    }

    private void HidePassengers()
    {
        foreach (var passenger in _passengers)
        {
            passenger.gameObject.SetActive(false);
        }
    }

}
