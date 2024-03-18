using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Burst;
[BurstCompile]
public class ESGateWaySpawnSetup : MonoBehaviour
{
    public bool CanSpawn = true;
    //[HideInInspector]
    //public float distanceapart = 20f, m_DistApartFromPlayer = 50f, m_SpawnAngle = 50f, LineOfSight = 200f;
    public bool findplayer = true;
    public Transform playertransform;
    //[HideInInspector]
    public Transform obj;
    [HideInInspector] public bool add, remove;
    [HideInInspector]
    public bool done;
    [HideInInspector]
    public Transform temptrans;
    [HideInInspector]
    public bool ignorebikes;
    //
    public Transform TargetNode, parent;
    //
    [HideInInspector]
    public Vector3 size = new Vector3(1, 1, 1), center = Vector3.zero;
    [HideInInspector]
    public bool IsTrigger = true, showpoint = true;
    [HideInInspector]
    public float groundoffset = 0.1f;
    [Header("Uncheck this if you dont like to use the quick tool")]
    public bool UseQuickTool = true;
    public ESTrafficLghtCtrl[] Trafficlight;
    //[HideInInspector]
    public bool neverspawn, returnGateUpdate;
    [HideInInspector] public bool ConsiderVisibleMeshes;
    [HideInInspector] public bool ClosedTrafficLight;
    [HideInInspector] public float Sensitivity = 50f;

    private void Awake()
    {
        Trafficlight = GameObject.FindObjectsOfType<ESTrafficLghtCtrl>();
        Sensitivity = this.GetComponent<ESNodeManager>().Sensitivity;
        for (int j = 0; j < Trafficlight.Length; ++j)
        {
            if (UCTMath.CalculateVector3Distance(this.transform.position, Trafficlight[j].transform.position) < Sensitivity * Sensitivity)
            {
                neverspawn = true;
                ClosedTrafficLight = true;
                CanSpawn = false;
            }
        }

        if (Application.isPlaying == true)
        {
            if (neverspawn)
            {
                CanSpawn = false;
            }
            else
            {
                CanSpawn = true;
            }

            if (findplayer)
                playertransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    void Start()
    {
        InvokeRepeating("CheckTriggerObjectDistance", 0.1f, 3f);
    }
    //
    private void OnDrawGizmos()
    {
        AddTarget();
    }
    //
    private void OnDrawGizmosSelected()
    {
        if (TargetNode != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(TargetNode.position, 6f);
        }
    }
    //
    void UpdateGateWay()
    {
        if (this.GetComponent<ESNodeManager>().NextNode.GetComponent<ESNodeManager>().NextNode == null)
        {
            neverspawn = true;
        }
        if (obj == null)
        {
            float dist = (playertransform.position - this.transform.position).sqrMagnitude;
            if (dist < 200 * 200)
            {
                if (!ClosedTrafficLight && ConsiderVisibleMeshes && obj == null)
                {
                    if (this.GetComponent<MeshRenderer>().isVisible)
                    {
                        neverspawn = true;
                    }
                    else
                    {
                        CanSpawn = true;
                        neverspawn = false;
                    }
                }
            }
            else
            {
                if (!ClosedTrafficLight && ConsiderVisibleMeshes && obj == null)
                {
                    if (neverspawn)
                    {
                        CanSpawn = true;
                        neverspawn = false;
                    }
                }
            }
        }
        //you can free this code but it draws a lot from the , it will accurately check for object inside the spawn cage :{} so your choice
        #region  junks 
        /*
        if (obj != null)
        {
            //float dist = Vector3.Distance(obj.transform.position, this.transform.position);
            float dist = (obj.transform.position - this.transform.position).sqrMagnitude;
            if (dist > 30 * 30)
            {
                obj = null;
            }
        }

        
        else
        {
            //float dist = Vector3.Distance(playertransform.position, this.transform.position);
            float dist = (playertransform.position - this.transform.position).sqrMagnitude;
            if (dist < 100 * 100)
            {
                if (!ClosedTrafficLight && ConsiderVisibleMeshes && obj == null)
                {
                    if (this.GetComponent<MeshRenderer>().isVisible)
                    {
                        neverspawn = true;
                    }
                    else
                    {
                        CanSpawn = true;
                        neverspawn = false;
                    }
                }
            }
            else
            {
                if (!ClosedTrafficLight && ConsiderVisibleMeshes && obj == null)
                {
                    if (neverspawn)
                    {
                        CanSpawn = true;
                        neverspawn = false;
                    }
                }
            }

        }
        */
        #endregion
        if (neverspawn)
        {
            CanSpawn = false;
            return;
        }
    }
    //
    private void LateUpdate()
    {
        if (returnGateUpdate) return;
        UpdateGateWay();
    }
    public IEnumerator Cool(float time)
    {
        yield return new WaitForSeconds(time);
        returnGateUpdate = false;
    }
    //
    private void Update()
    {
        if (returnGateUpdate) return;
        if (neverspawn) return;

        if (obj != null)
        {
            if (this.GetComponent<ESNodeManager>().OverTakeType == "None")
            {
                if (obj.gameObject.activeSelf == false)
                {
                    CanSpawn = true;
                    obj = null;
                }
            }
        }
    }
    //
    void OnTriggerEnter(Collider other)
    {
        CanSpawn = false;
        if (other.attachedRigidbody != null)
        {
            obj = other.attachedRigidbody.transform;
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                obj = other.transform;
            }
        }
    }
    //
    /*
        void OnTriggerStay(Collider other)
        {
            CanSpawn = false;
        }
    */
    // 
    void CheckTriggerObjectDistance()
    {
        //this is cpu instensive so we have choosen not use this method , but you can explore it thats why we have left this inactive.    
        if (obj != null)
        {
            if (UCTMath.CalculateVector3Distance(this.transform.position, obj.transform.position) > 30 * 30)
            {
                CanSpawn = true;
                obj = null;
            }
        }
    }
    //
    /*
    void OnTriggerExit(Collider other)
    {
        //if (greater || neverspawn) return;

        if (obj != null)
        {
            if (other.gameObject.transform == obj.transform)
            {
                CanSpawn = true;
                obj = null;
            }
        }
    }
    */
    //
    void AddTarget()
    {
        if (add)
        {
#if UNITY_EDITOR

            temptrans = Selection.activeTransform;
            if (temptrans != null)
            {
                if (temptrans != this)
                    TargetNode = temptrans;
            }
            add = false;
            //
#endif
        }
        else if (remove)
        {
            if (TargetNode != null)
            {
                TargetNode = null;
            }
            remove = false;
        }
    }
}
