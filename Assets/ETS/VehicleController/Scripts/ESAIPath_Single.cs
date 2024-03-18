using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ESAIPath_Single : MonoBehaviour
{
    [HideInInspector]
    public GameObject spawnnode;
    [HideInInspector]
    public bool done;
    public Color linecolor;
    public bool Merg;
    public float Raduis = 30f;
    public bool DebugMode;

    private List<Transform> nodes = new List<Transform>();

    private void Update()
    {
        spawnnode = Resources.Load("Path/node") as GameObject;
        //
        MeshRenderer[] pathmeshrenderer = GetComponentsInChildren<MeshRenderer>();
        if (DebugMode == false)
        {
            for (int i = 0; i < pathmeshrenderer.Length; i++)
            {
                pathmeshrenderer[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < pathmeshrenderer.Length; i++)
            {
                pathmeshrenderer[i].enabled = true;
            }
        }
        for (int i = 0; i < pathmeshrenderer.Length; i++)
        {
            if (!pathmeshrenderer[i].GetComponent<ESShowForwardDirection>().ResizeCollider)
                pathmeshrenderer[i].GetComponent<SphereCollider>().radius = Raduis;
        }

    }


    private void OnDrawGizmosSelected()
    {

        Gizmos.color = linecolor;

        Transform[] pathtrans = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        for (int i = 0; i < pathtrans.Length; i++)
        {
            if (pathtrans[i] != transform)
            {
                nodes.Add(pathtrans[i]);
            }
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 curnode = nodes[i].position;
            Vector3 prevnode = Vector3.zero;
            if (i > 0)
            {
                prevnode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1 && Merg)
            {
                prevnode = nodes[nodes.Count - 1].position;
            }
            else if (i == 0)
            {
                prevnode = nodes[0].position;
            }
            Gizmos.DrawLine(prevnode, curnode);
            Gizmos.DrawWireSphere(curnode, 0.5f);


        }
    }
}
