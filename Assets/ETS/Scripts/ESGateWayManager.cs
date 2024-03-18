using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class ESGateWayManager : MonoBehaviour
{
    public bool created;
    public bool ShowSpawnPoint = true;
    public GameObject SpawnPrefab;
    // Start is called before the first frame update
    void Start()
    {
        ShowSpawnPoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.isPlaying == false)
        SpawnPrefab = SpawnPrefab == null ? Resources.Load("Spawn/Spawn") as GameObject : SpawnPrefab;
    }
}
