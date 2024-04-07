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
    private vSimpleMeleeAI_Controller _currentNPC;
    
    private void Start()
    {
        _waypoints = _pathArea.gameObject.GetComponentsInChildren<Transform>();

        _npcPool = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < _amountToPool; i++)
        {
            tmp = Instantiate(_npctoPool[Random.Range(0, _npctoPool.Count)]);
            tmp.SetActive(false);
            _npcPool.Add(tmp);
        }
        for (int i = 0; i < _amountToPool; i++)
        {
            SpawnNPC();
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

    public void SpawnNPC()
    {
        GameObject npc = GetPooledNPC();
        npc.transform.position = _waypoints[Random.Range(0, _waypoints.Length)].position;     
        npc.GetComponent<vSimpleMeleeAI_Controller>().isDead = false;
        npc.GetComponent<vSimpleMeleeAI_Controller>().ResetHealth();
        npc.GetComponent<vRagdoll>().RestoreRagdoll();
        
        npc.GetComponent<vSimpleMeleeAI_Controller>().onDead.AddListener(RespawnNPC);
        npc.SetActive(true);
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
        SpawnNPC();
    }
}
