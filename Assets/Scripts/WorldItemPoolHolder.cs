using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR;

public class WorldItemPoolHolder : MonoBehaviour
{
    public static WorldItemPoolHolder instance;

    public List<WorldItem> _pool = new List<WorldItem>();

    private void Awake()
    {
        instance = this;
    }

    public List<WorldItem> GetRandomItemsFromPool(int count)
    {
        List<WorldItem> items = new List<WorldItem>();
        for(int i = 0; i < count; i++)
        {
            var random = new System.Random();
            var randomItem = _pool[random.Next(_pool.Count)];
            items.Add(randomItem);
        } 
        return items;
    }

    public void GetItemFromPool()
    {

    }

    public void PutItemsAwayToPool(WorldItem item)
    {
        item.gameObject.SetActive(false);
    }
}
