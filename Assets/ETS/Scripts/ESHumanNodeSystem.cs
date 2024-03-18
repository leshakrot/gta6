using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ESHumanNodeSystem : MonoBehaviour
{
    [HideInInspector] public bool done;
    [HideInInspector] public GameObject nodeprefab;
    [HideInInspector] public Transform LastcreatedNode, Pnode, PreviousNode;
    [HideInInspector] public List<Transform> nodelist;
    [HideInInspector] public Vector3 lastnodepos;
    [HideInInspector] public int max;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (nodelist.Count > 0)
        {
            for (int i = 0; i < nodelist.Count; ++i)
            {
                //nodelist[i].name = "H_Node" + i.ToString();
                //if(nodelist[i].GetComponent<MeshRenderer>() != null)
                //nodelist[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
        */
    }
    //
    private void OnDrawGizmos()
    {

        Transform[] nodes = GetComponentsInChildren<Transform>();

        nodelist = new List<Transform>();

        for (int i = 0; i < nodes.Length; ++i)
        {
            if (nodes[i] != this.transform)
            {
                nodelist.Add(nodes[i]);
            }
        }
        if (nodelist.Count > 0)
        {
            nodelist[0].GetComponent<ESHumanNode>().first = true;
        }

        if (max != nodes.Length)
        {

            //
            if (nodelist.Count > 0)
            {
                for (int i = 0; i < nodelist.Count; ++i)
                {
                    nodelist[i].name = "Node" + i.ToString();

                }
            }
            if (nodelist.Count > 0)
            {
                lastnodepos = nodelist[nodelist.Count - 1].position;
            }
            if (nodelist.Count == 0)
            {
                lastnodepos = Vector3.zero;
            }
            //
            max = nodes.Length;
        }

    }
}
