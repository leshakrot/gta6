using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDisableAngent : MonoBehaviour
{
    // Start is called before the first frame update
    public bool DisableSpawn = false;
    public ESSpawnManager spawnManager;
    void Start()
    {
        spawnManager = this.GetComponent<ESSpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager == null)
        {
            print("Manager Empty");
            return;
        }
        //
        spawnManager.enabled = DisableSpawn;
    }
}
