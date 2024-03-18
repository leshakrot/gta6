using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESHumanNode : MonoBehaviour
{
    public Transform NextNode;
    public bool DisableSpawn;
    public Transform PreviousNode;
    [HideInInspector] public GameObject mesh;
    [HideInInspector] public Mesh gizmesh;
    public bool drawline = true;
    public bool showdirecton = true;
    //[HideInInspector]
    public bool first;
    [Header("DEBUG")]
    public bool CanSpawn = true;
    public GameObject RawMesh;
    public Material NodeMat;
    // Start is called before the first frame update
    //
    void OnTriggerEnter()
    {
        CanSpawn = false;
    }
    void OnTriggerExit()
    {
        CanSpawn = true;
    }
    //
    void Update()
    {
        if (CanSpawn)
        {
            if (this.GetComponent<MeshRenderer>().isVisible)
            {
                DisableSpawn = true;
            }
            else
            {
                DisableSpawn = false;
            }
        }
        else
        {
            DisableSpawn = true;
        }
    }
    //
    void OnDrawGizmos()
    {
        if (RawMesh == null)
        {
            RawMesh = Resources.Load("Node/Node") as GameObject;
            NodeMat = Resources.Load("Node/Trans") as Material;
            this.GetComponent<MeshRenderer>().sharedMaterial = NodeMat;
            this.GetComponent<MeshFilter>().sharedMesh = RawMesh.GetComponent<MeshFilter>().sharedMesh;
        }
    }
    //
    void OnDrawGizmosSelected()
    {
        if (mesh != null)
        {
            if (first)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.red;
            }


            if (drawline == true)
            {
                if (NextNode != null)
                {
                    if (showdirecton)
                    {
                        Vector3 dir = NextNode.transform.position - this.transform.position;
                        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5);
                        Gizmos.DrawMesh(mesh.GetComponent<MeshFilter>().sharedMesh, -1, this.transform.position, rot, new Vector3(2f, 2f, 2f));
                        this.transform.rotation = rot;
                    }
                    // Vector4 d = new Vector4(1, 1, 1, 0.035f);
                    Gizmos.color = Color.grey;
                    Debug.DrawLine(this.transform.position, NextNode.position);
                }
                else
                {
                    if (showdirecton)
                        Gizmos.DrawMesh(mesh.GetComponent<MeshFilter>().sharedMesh, -1, this.transform.position, transform.rotation, new Vector3(2f, 2f, 2f));
                }
            }

        }
    }
}
