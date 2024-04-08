using Invector.vCharacterController;
using Invector.vCharacterController.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _npcPool;
    [SerializeField] private List<GameObject> _npctoPool;
    [SerializeField] private int _amountToPool;
    [SerializeField] private vWaypointArea _pathArea;

    private Transform[] _waypoints;

    private void Start()
    {
        InitializePool();
    }

    public void InitializePool()
    {
        _waypoints = _pathArea.gameObject.GetComponentsInChildren<Transform>();
        _npcPool = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < _amountToPool; i++)
        {
            tmp = Instantiate(_npctoPool[Random.Range(0, _npctoPool.Count)]);
            _npcPool.Add(tmp);
            InitNPC(tmp);
        }    
    }

    public GameObject GetPooledNPC()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!_npcPool[i].gameObject.activeInHierarchy)
            {
                _npcPool[i].transform.parent = gameObject.transform;
                return _npcPool[i];
            }
        }
        return null;
    }

    public void SpawnNPC(GameObject npc)
    {
        npc = GetPooledNPC();
        InitNPC(npc);
    }

    private void InitNPC(GameObject npc)
    {
        npc.GetComponent<vRagdoll>().RestoreRagdoll();
        npc.SetActive(true);
        npc.transform.position = _waypoints[Random.Range(0, _waypoints.Length)].position;
        npc.GetComponent<vSimpleMeleeAI_Controller>().isDead = false;
        npc.GetComponent<vSimpleMeleeAI_Controller>().ResetHealth();
        
        npc.GetComponent<vSimpleMeleeAI_Controller>().ragdolled = true;
        npc.GetComponent<vSimpleMeleeAI_Controller>().onDead.AddListener(RespawnNPC);
        npc.GetComponent<vSimpleMeleeAI_Controller>().ResetTargetSearch();
        npc.GetComponent<vSimpleMeleeAI_Controller>().pathArea = _pathArea;
    }

    public void RespawnNPC(GameObject arg0)
    {
        StartCoroutine(Respawn(arg0));
    }

    private IEnumerator Respawn(GameObject obj)
    {
        yield return new WaitForSeconds(10f);
        obj.SetActive(false);
        SpawnNPC(obj);
    }
}
