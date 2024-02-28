using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVehiclesHolder : MonoBehaviour
{
    [Header("AI vehicles Pool")]
    public List<GameObject> pool;
    //public GameObject objectToPool;
    public int amountToPool;

    public Transform player;

    private void Start()
    {
        AIVehiclesObjectsPoolInit();
    }

    private void AIVehiclesObjectsPoolInit()
    {
        foreach(var vehicle in pool)
        {
            vehicle.gameObject.SetActive(false);
        }
    }

    public GameObject GetPooledVehicle()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                pool[i].transform.parent = gameObject.transform;
                return pool[i];
            }
        }
        return null;
    }

    public void SpawnVehicle(Vector3 spawnPoint)
    {
        GameObject vehicle = GetPooledVehicle();
        vehicle.transform.position = spawnPoint;
        vehicle.SetActive(true);
        

        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SpawnVehicle(player.transform.position + new Vector3(2, 3, 0));
        }
    }
}
