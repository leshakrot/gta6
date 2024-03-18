using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ESChatManager : MonoBehaviour
{
    public Transform Player;
    public float SpawnDistanceCheck = 100.6f;
    public Transform FocusNode;
    public Transform[] SpawnPoints;
    public GameObject[] HumanPrefab;
    public List<Transform> ListSpawnedHumans = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Player == null)
        {
            print("Player is missing");
        }
        //
        ListSpawnedHumans = new List<Transform>();
        if (this.GetComponent<MeshRenderer>() != null)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
        MeshRenderer[] rendermeoff = this.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < rendermeoff.Length; ++i)
        {
            rendermeoff[i].enabled = false;
        }
        //
        SpawnHumans();
    }

    // Update is called once per frame

    //
    private void SpawnHumans()
    {
        //lets get to biz...
        if (SpawnPoints.Length > 0)
        {
            for (int i = 0; i < SpawnPoints.Length; ++i)
            {
                if (HumanPrefab.Length > 0)
                {
                    if (SpawnPoints[i].GetComponent<ESChatSpawnPoint>() != null)
                    {
                        if (SpawnPoints[i].GetComponent<ESChatSpawnPoint>().SpawnedHuman == null)
                        {
                            int Humanindex = Random.Range(0, HumanPrefab.Length);
                            GameObject H = Instantiate(HumanPrefab[Humanindex], SpawnPoints[i].position, Quaternion.identity, this.transform);
                            //rotate humans to one another;
                            H.GetComponent<ESPedestrainCtrl>().target = FocusNode;
                            H.GetComponent<ESPedestrainCtrl>().StopUpdateTarget = true;
                            H.transform.LookAt(FocusNode);
                            SpawnPoints[i].GetComponent<ESChatSpawnPoint>().SpawnedHuman = H.transform;
                            ListSpawnedHumans.Add(H.transform);
                        }
                    }
                }
            }
        }
    }
}
