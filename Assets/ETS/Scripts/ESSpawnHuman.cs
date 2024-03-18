using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public class ESSpawnHuman : MonoBehaviour
{
    public float startime = 0.5f, delaytime = 2;
    public Transform[] humanprefab;

    [Header("Warning may lead performance issue when value is too high due to the in built animator controller not optimized")]

    [Range(1, 50)] public int spawncount = 50;
    public Transform player;
    public float DistApartPlayer = 600.0f;
    public float Yoffset = 0.5f;
    public float CageSize = 5f;
    public List<Transform> SpawnPoints = new List<Transform>();
    [HideInInspector] public List<Transform> RawSpawnPoints = new List<Transform>();

    [HideInInspector] public List<Transform> SpawnedHumans = new List<Transform>();
    //
    public int c_n;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnPoints = new List<Transform>();
        ESHumanNode[] EHS = GameObject.FindObjectsOfType<ESHumanNode>();
        if (EHS.Length > 0)
        {
            for (int i = 0; i < EHS.Length; i++)
            {
                EHS[i].gameObject.AddComponent<SphereCollider>();
                EHS[i].GetComponent<SphereCollider>().isTrigger = true;
                EHS[i].gameObject.layer = 2;
                EHS[i].GetComponent<SphereCollider>().radius = CageSize;
                RawSpawnPoints.Add(EHS[i].transform);
            }
        }
        SpawnedHumans = new List<Transform>();
        CreateHuman();
        InvokeRepeating("spawnhuman", startime, delaytime);
    }
    //

    void CreateHuman()
    {
        for (int i = spawncount; i > 0; i--)
        {
            int HumanIndex = Random.Range(0, humanprefab.Length);
            GameObject g = Instantiate(humanprefab[HumanIndex].gameObject, this.transform.position, this.transform.rotation);
            SpawnedHumans.Add(g.transform);
            g.SetActive(false);
        }
    }
    //
    //
    void spawnhuman()
    {
        if (RawSpawnPoints.Count == 0) return;
        SpawnPoints = new List<Transform>();
        for (int i = 0; i < RawSpawnPoints.Count; i++)
        {
            if (Vector3.Distance(RawSpawnPoints[i].position, player.position) < DistApartPlayer)
            {
                SpawnPoints.Add(RawSpawnPoints[i].transform);
            }
        }
        if (SpawnPoints.Count == 0) return;
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (Vector3.Distance(SpawnPoints[i].position, player.position) > DistApartPlayer)
            {
                SpawnPoints.RemoveAt(i);
            }
        }
        if (SpawnPoints.Count == 0) return;
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (SpawnPoints[i].GetComponent<ESHumanNode>().DisableSpawn)
            {
                SpawnPoints.RemoveAt(i);
            }
        }
        if (SpawnPoints.Count == 0) return;
        if (SpawnedHumans.Count == 0) return;
        foreach (Transform Hspawn in SpawnedHumans)
        {
            if (Hspawn.gameObject.activeSelf == false)
            {
                int spawnindex = Random.Range(0, SpawnPoints.Count);
                //  Vector3 temppos = SpawnPoints[spawnindex].position;
                // temppos.y +=
                if (SpawnPoints[spawnindex].GetComponent<ESHumanNode>().DisableSpawn) return;
                Hspawn.position = SpawnPoints[spawnindex].position + new Vector3(0, Yoffset, 0);
                Hspawn.GetComponent<ESPedestrainCtrl>().DistApartFromPlayer = DistApartPlayer;
                Hspawn.GetComponent<ESPedestrainCtrl>().target = SpawnPoints[spawnindex];

                Hspawn.gameObject.SetActive(true);

                break;
            }
        }
    }
}
