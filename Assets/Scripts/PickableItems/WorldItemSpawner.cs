using Invector.vCharacterController;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
    private int _currentSpawnPointIndex = 0;

    private List<WorldItem> _currentItems = new List<WorldItem>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            SpawnRandomItem(4);
        }
    }

    private void SpawnRandomItem(int count)
    {
        _currentItems = WorldItemPoolHolder.instance.GetRandomItemsFromPool(count);
        foreach(var item in _currentItems)
        {
            item.gameObject.transform.position = _spawnPoints[_currentSpawnPointIndex].transform.position;
            item.gameObject.SetActive(true);
            _currentSpawnPointIndex++;
        }

        _currentSpawnPointIndex = 0;
    }
}
