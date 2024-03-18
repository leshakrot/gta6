using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ESScenemanager : MonoBehaviour
{
    [HideInInspector] public List<ESNodeManager> nodeManagers;
    [HideInInspector] public Transform Pnode;
    [HideInInspector] public Transform[] t;
    [HideInInspector] public List<Transform> connectednode;
    [HideInInspector] public bool connect;
    [HideInInspector] public bool disconnect;
    [HideInInspector] public bool done, created;
    public bool shownodes = true;
    public bool UseGizmosSelected = true;
    public bool ShowDirection = true;
    public float TrafficLightSensitivity = 100f;
    public ESHumanNode[] EHN;
    [HideInInspector] private GameObject[] nm;
    //
    public void GenerateTrafficLights()
    {
        // this trys to auto create trafficlghts
        if (nodeManagers != null && !created)
        {
            if (nodeManagers.Count > 1)
            {
                GameObject TrafficHolder = new GameObject("TrafficHolder");
                TrafficHolder.transform.position = nodeManagers[0].transform.position;
                GameObject Controlpoint = new GameObject("ControlPoint");
                Controlpoint.transform.position = nodeManagers[0].transform.position;
                Controlpoint.transform.parent = TrafficHolder.transform;

                Controlpoint.AddComponent<ESTrafficControlPoint>();
                ESTrafficControlPoint controlPoint = Controlpoint.GetComponent<ESTrafficControlPoint>();
                System.Array.Resize(ref controlPoint.managers, nodeManagers.Count);
                controlPoint.slowdowntime = 2;
                controlPoint.cooldowntime = 10;
                controlPoint.ShiftTime = 10;

                for (int i = 0; i < nodeManagers.Count; ++i)
                {
                    //Create Managers 
                    GameObject Managers = new GameObject("TrafficManager");
                    Managers.transform.position = nodeManagers[i].transform.position;
                    Managers.transform.parent = controlPoint.transform;
                    Managers.AddComponent<TrafficManager>();
                    TrafficManager trafficManager = Managers.GetComponent<TrafficManager>();
                    //Create TrafficLights
                    GameObject Lights = new GameObject("Light");
                    Lights.AddComponent<ESTrafficLghtCtrl>();
                    Lights.AddComponent<BoxCollider>();
                    Lights.tag = "TrafficLight";
                    BoxCollider box = Lights.GetComponent<BoxCollider>();
                    box.isTrigger = true;
                    Vector3 scale = Lights.transform.localScale;
                    scale = new Vector3(3, 3, 3);
                    Lights.transform.localScale = scale;
                    Lights.transform.position = nodeManagers[i].transform.position;
                    Lights.transform.parent = Managers.transform;
                    trafficManager.trafficlights = new List<ESTrafficLghtCtrl>();
                    trafficManager.trafficlights.Add(Lights.GetComponent<ESTrafficLghtCtrl>());
                    //
                    controlPoint.managers[i] = trafficManager;

                }
            }
            created = true;
        }
    }
    //
    private void OnDrawGizmos()
    {
        nm = GameObject.FindGameObjectsWithTag("Nodes");
        EHN = GameObject.FindObjectsOfType<ESHumanNode>();
        if (nm.Length > 0)
        {
            for (int i = 0; i < nm.Length; i++)
            {
                nm[i].GetComponent<MeshRenderer>().enabled = shownodes;
                nm[i].GetComponent<ESNodeManager>().drawline = shownodes;
                nm[i].GetComponent<ESNodeManager>().UseGizmosSelected = UseGizmosSelected;
                nm[i].GetComponent<ESNodeManager>().Sensitivity = TrafficLightSensitivity;
            }
        }
        //
        if (EHN.Length > 0)
        {
            for (int i = 0; i < EHN.Length; i++)
            {

                EHN[i].showdirecton = ShowDirection;
            }
        }
#if UNITY_EDITOR

        t = Selection.transforms;
        if (Selection.transforms.Length == 1)
        {
            if (Selection.transforms[0] != this.transform && Selection.transforms[0].GetComponent<ESNodeManager>() != null)
                Pnode = Selection.transforms[0];

        }
        //
        if (Selection.transforms.Length > 1)
        {
            nodeManagers = new List<ESNodeManager>();
            foreach (Transform trans in Selection.transforms)
            {
                if (trans.GetComponent<ESNodeManager>() != null)
                {
                    nodeManagers.Add(trans.GetComponent<ESNodeManager>());
                }
            }
        }
#endif
        if (t.Length > 1)
        {
            connectednode = new List<Transform>();
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] != Pnode && t[i].GetComponent<ESNodeManager>() != null)
                {
                    connectednode.Add(t[i]);
                }
            }
        }
        //
        if (connect)
        {
            if (connectednode.Count > 0)
            {
                if (Pnode != null)
                {
                    if (Pnode.GetComponent<ESNodeManager>().ConnectedNode == null || Pnode.GetComponent<ESNodeManager>().ConnectedNode.Count == 0)
                    {
                        Pnode.GetComponent<ESNodeManager>().ConnectedNode = new List<Transform>();
                    }
                    for (int i = 0; i < connectednode.Count; ++i)
                    {
                        if (Pnode.GetComponent<ESNodeManager>().ConnectedNode.Count == 0)
                            Pnode.GetComponent<ESNodeManager>().ConnectedNode.Add(connectednode[i]);
                        for (int j = 0; j < Pnode.GetComponent<ESNodeManager>().ConnectedNode.Count; ++j)
                        {
                            if (connectednode[i] != Pnode.GetComponent<ESNodeManager>().ConnectedNode[j])
                                Pnode.GetComponent<ESNodeManager>().ConnectedNode.Add(connectednode[i]);
                        }


                    }

                }
            }
            Pnode = null;
            connect = false;
        }
        //
        if (disconnect == true)
        {
            if (Pnode != null)
            {
                Pnode.GetComponent<ESNodeManager>().ConnectedNode.Clear();
                connectednode.Clear();
            }
            disconnect = false;
        }
        //

    }
    //

}
