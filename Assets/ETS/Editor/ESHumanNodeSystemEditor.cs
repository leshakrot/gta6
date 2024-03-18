using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//[CanEditMultipleObjects]
[CustomEditor(typeof(ESHumanNodeSystem))]
public class ESHumanNodeSystemEditor : Editor
{
    public ESHumanNodeSystem scripts;

    public void OnSceneGUI()
    {
        scripts = target as ESHumanNodeSystem;

        Event e = Event.current;
        // scripts.nodeprefab = scripts.nodeprefab == null ? Resources.Load("Node/Node") as GameObject : scripts.nodeprefab;
        if (Event.current.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.A && !scripts.done)
            {
                CallNodes(scripts);
                scripts.done = true;
            }
        }


        if (Event.current.type == EventType.KeyUp)
        {
            scripts.done = false;
        }
        //



        if (Event.current.type == EventType.KeyUp)
        {
            scripts.done = false;
        }
    }

    private void CallNodes(ESHumanNodeSystem es)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            // Transform t = Instantiate(es.nodeprefab.transform, hit.point, Quaternion.identity);
            //creates a parent
            GameObject go = new GameObject("H_Node");
            // Create a custom game object
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.AddComponent<ESHumanNode>();
            go.GetComponent<ESHumanNode>().mesh = Resources.Load("Node/h_node") as GameObject;
            Transform[] nodes = scripts.GetComponentsInChildren<Transform>();
            List<Transform> nodelist = new List<Transform>();
            for (int i = 0; i < nodes.Length; ++i)
            {
                if (nodes[i] != scripts.transform)
                {
                    nodelist.Add(nodes[i]);
                }
            }
            //
            if (nodelist.Count > 0)
            {
                scripts.LastcreatedNode = nodelist[nodelist.Count - 1];
            }

            if (scripts.LastcreatedNode != null)
            {
                scripts.nodelist[scripts.nodelist.Count - 1].GetComponent<ESHumanNode>().NextNode = go.transform;
                go.GetComponent<ESHumanNode>().PreviousNode = scripts.nodelist[scripts.nodelist.Count - 1];
            }
            // go.GetComponent<MeshFilter>().sharedMesh = es.nodeprefab.GetComponent<MeshFilter>().sharedMesh;
            // go.GetComponent<MeshRenderer>().sharedMaterial = es.nodeprefab.GetComponent<MeshRenderer>().sharedMaterial;
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            go.transform.position = hit.point;
            go.transform.parent = scripts.transform;
        }
    }
}
