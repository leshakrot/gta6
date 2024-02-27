using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVehiclesHolder : MonoBehaviour
{
    [Header("AI vehicles Pool")]
    public List<GameObject> pool;
    public GameObject objectToPool;
    public int amountToPool;

    private void Start()
    {
        AIVehiclesObjectsPoolInit();
    }

    private void AIVehiclesObjectsPoolInit()
    {
        pool = new List<GameObject>();
        GameObject tmpPoolObject;
        for (int i = 0; i < amountToPool; i++)
        {
            tmpPoolObject = Instantiate(objectToPool);
            tmpPoolObject.gameObject.SetActive(false);
            pool.Add(tmpPoolObject);
        }
    }

    public GameObject GetPooledDamageTextObject()
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
