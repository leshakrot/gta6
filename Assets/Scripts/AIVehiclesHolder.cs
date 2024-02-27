using System.Collections.Generic;
using UnityEngine;

public class AIVehiclesHolder : MonoBehaviour
{
    public static AIVehiclesHolder instance;

    [Header("AI vehicles Pool")]
    public List<GameObject> pool;
    public int amountToPool;

    private void Start()
    {
        AIVehiclesObjectsPoolInit();
    }

    private void AIVehiclesObjectsPoolInit()
    {
        pool = new List<GameObject>();
        
        for (int i = 0; i < amountToPool; i++)
        {
            pool[i].SetActive(false);
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
}
