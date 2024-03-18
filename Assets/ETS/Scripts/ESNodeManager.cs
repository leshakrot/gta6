using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ESNodeManager : MonoBehaviour
{
    public Transform NextNode;
    [HideInInspector] public Transform OverTakeNode;
    public List<Transform> ConnectedNode;
    public GameObject mesh;
    public float Sensitivity;
    public string OverTakeType = "None";
    [HideInInspector]
    public bool CanSpawn;
    [HideInInspector] public bool drawline;
    public bool UseGizmosSelected = true;
    public GameObject scenemanager;
    public float arrowsize = 2;
    [SerializeField] private bool done = true;
    void Awake()
    {
        CanSpawn = true;

    }
    void Start()
    {
        if (NextNode == null)
        {
            if (ConnectedNode != null)
            {
                if (ConnectedNode.Count > 0)
                    NextNode = ConnectedNode[0];
            }
        }
        else
        {
            if (ConnectedNode != null)
            {
                ConnectedNode.Add(NextNode);
            }
        }

        if (this.transform.parent.GetComponent<ESNodeSystem>().GetOverTakeMethod == ESNodeSystem.OverTakeMethod.Left)
        {
            OverTakeType = "Left";
            if (OverTakeNode == null)
            {
                GameObject g = new GameObject("OvertakeNode");

                g.transform.position = transform.position;
                g.transform.position = g.transform.right * -4;
                g.transform.parent = this.transform;
                OverTakeNode = g.transform;
            }
        }
        else if (this.transform.parent.GetComponent<ESNodeSystem>().GetOverTakeMethod == ESNodeSystem.OverTakeMethod.None)
        {
            OverTakeType = "None";
        }
        else if (this.transform.parent.GetComponent<ESNodeSystem>().GetOverTakeMethod == ESNodeSystem.OverTakeMethod.Right)
        {
            OverTakeType = "Right";
            if (OverTakeNode == null)
            {
                GameObject g = new GameObject("OvertakeNode");
                g.transform.position = transform.position;
                g.transform.position = g.transform.right * 4;
                g.transform.parent = this.transform;
                OverTakeNode = g.transform;
            }
        }
    }
    void LateUpdate()
    {
        if (OverTakeNode != null)
        {
            if (this.transform.parent.GetComponent<ESNodeSystem>().GetOverTakeMethod == ESNodeSystem.OverTakeMethod.Right)
            {
                OverTakeNode.localPosition = Vector3.zero;
                OverTakeNode.localPosition = OverTakeNode.right * 20;
            }
            else if (this.transform.parent.GetComponent<ESNodeSystem>().GetOverTakeMethod == ESNodeSystem.OverTakeMethod.Left)
            {
                OverTakeNode.localPosition = Vector3.zero;
                OverTakeNode.localPosition = OverTakeNode.right * -20;
            }
            //done = false;
        }

    }
    //
    private void OnDrawGizmosSelected()
    {
        if (scenemanager == null)
        {
            scenemanager = GameObject.Find("SceneManager");
        }
        if (UseGizmosSelected)
        {
            Gizmos_Me();
        }
    }
    //
    private void OnDrawGizmos()
    {
        if (scenemanager == null)
        {
            scenemanager = GameObject.Find("SceneManager");
        }
        if (!UseGizmosSelected)
        {
            Gizmos_Me();
        }
        //
    }
    //
    private void Gizmos_Me()
    {
        if (scenemanager == null)
        {
            scenemanager = GameObject.Find("SceneManager");
            if (scenemanager == null)
            {
                GameObject sm = new GameObject("SceneManager");
                sm.AddComponent<ESScenemanager>();
                scenemanager = sm;
            }
        }
        //scenemanager = GameObject.Find("SceneManager");
        if (gameObject.tag != "Nodes")
        {
            this.gameObject.tag = "Nodes";
        }
        //Gizmos.DrawMesh(mesh, 1,this.transform.position,this.transform.rotation  )
        if (NextNode != null)
        {
            transform.LookAt(NextNode);
        }
        //
        //m.a = 0.5f;
        if (mesh == null)
        {
            mesh = Resources.Load("stuff/Arrow") as GameObject;

        }
        Vector4 c = new Vector4(1, 0, 0, 0.8f);
        Gizmos.color = c;

        if (NextNode != null && drawline == true)
        {
            Gizmos.DrawMesh(mesh.GetComponent<MeshFilter>().sharedMesh, -1, (this.transform.position + NextNode.position) * 0.5f, this.transform.rotation, new Vector3(1, 1, 1f) * arrowsize);

            // Vector4 d = new Vector4(1, 1, 1, 0.035f);
            Gizmos.color = Color.grey;
            Debug.DrawLine(this.transform.position, NextNode.position);
        }
        //
        if (ConnectedNode != null)
        {
            if (ConnectedNode.Count > 0)
            {
                CanSpawn = false;
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(this.transform.position, 3f);
                if (drawline)
                {
                    for (int i = 0; i < ConnectedNode.Count; ++i)
                    {
                        if (ConnectedNode[i] == null)
                        {
                            ConnectedNode.RemoveAt(i);
                        }
                        Gizmos.color = Color.red;
                        if (ConnectedNode[i].position != null)
                        {
                            Gizmos.DrawLine(this.transform.position, ConnectedNode[i].position);

                            Vector4 c1 = new Vector4(1, 0, 5, 0.8f);
                            Gizmos.color = c1;
                            Vector3 dir = ConnectedNode[i].transform.position - this.transform.position;
                            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5);
                            Gizmos.DrawMesh(mesh.GetComponent<MeshFilter>().sharedMesh, -1, (this.transform.position + ConnectedNode[i].position) * 0.5f, rot, new Vector3(1, 1, 1f) * arrowsize);
                        }
                    }
                }

            }
        }

    }
    //
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Untagged")
        {
            CanSpawn = false;
        }
    }
    //
    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Untagged")
        {
            CanSpawn = true;
        }
    }
}
