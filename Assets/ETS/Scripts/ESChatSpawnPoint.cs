using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESChatSpawnPoint : MonoBehaviour
{
    public Transform SpawnedHuman;
    public float BackToPositionSpeed = 10f;
    
    // Update is called once per frame
    void Update()
    {
        ESChatManager chatmanager = this.transform.parent.GetComponent<ESChatManager>();
        if (UCTMath.CalculateVector3Distance(this.transform.position, chatmanager.Player.position) < chatmanager.SpawnDistanceCheck) return;
        if (SpawnedHuman.gameObject.activeSelf == false)
        {
            SpawnedHuman.transform.position = this.transform.position;
            SpawnedHuman.LookAt(this.transform.parent);
            SpawnedHuman.GetComponent<ESPedestrainCtrl>().StopUpdateTarget = true;
            SpawnedHuman.gameObject.SetActive(true);
        }
        //
        //UpdateTarget();
    }
    //
    void UpdateTarget()
    {
        /*
         * will be fully functional in the next update
        if (SpawnedHuman.GetComponent<ESPedestrainCtrl>().AvoidIncomingVehicle == false && SpawnedHuman.transform.position != this.transform.position)
        {
            SpawnedHuman.GetComponent<ESPedestrainCtrl>().agent.Stop();
            //SpawnedHuman.GetComponent<ESPedestrainCtrl>
            SpawnedHuman.transform.position = Vector3.Lerp(SpawnedHuman.transform.position, this.transform.position, BackToPositionSpeed * Time.fixedDeltaTime);
        }
         */
    }
    
}
