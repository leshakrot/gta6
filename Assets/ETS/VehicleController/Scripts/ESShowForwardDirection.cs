using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESShowForwardDirection : MonoBehaviour
{
    [SerializeField]
    private float raylength = 6;
    [SerializeField]
    private Color LineColor = new Vector4(1, 1, 1, 1);
    public bool ResizeCollider;
    public GameObject mesh;
    private void OnDrawGizmos()
    {
        Gizmos.color = LineColor;
        Vector3 dir = transform.TransformDirection(Vector3.forward) * raylength;
        Gizmos.DrawRay(transform.position, dir);
        if(mesh != null)
        Gizmos.DrawMesh(mesh.GetComponent<MeshFilter>().sharedMesh, -1, (this.transform.position), this.transform.rotation, new Vector3(6f, 0.125f, 6f));
    }
}