using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ESGateWayManager))]
public class ESGateWaySpawnEditor : Editor
{
    public ESGateWayManager scripts;

    public override void OnInspectorGUI()
    {
        scripts = target as ESGateWayManager;
        EditorGUI.BeginChangeCheck();
        //
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RegisterCompleteObjectUndo(scripts, "gatewayspawn");
        }
        //
        if (GUI.changed)
        EditorUtility.SetDirty(scripts);
        base.OnInspectorGUI();
    }
    //
    public void OnSceneGUI()
    {
        scripts = target as ESGateWayManager;
        //
        Event e = Event.current;
        scripts.SpawnPrefab = scripts.SpawnPrefab == null ? Resources.Load("Spawn/Spawn") as GameObject : scripts.SpawnPrefab;
        if (Event.current.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.C && !scripts.created)
            {
                CallGateWay(scripts);
                scripts.created = true;
            }
        }
        //
        if (Event.current.type == EventType.KeyUp)
        {
            scripts.created = false;
        }
        //
        if (GUI.changed)
            EditorUtility.SetDirty(scripts);
     
    }
    //
    private void CallGateWay(ESGateWayManager es)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            // Transform t = Instantiate(es.nodeprefab.transform, hit.point, Quaternion.identity);
            //creates a parent
            GameObject go = new GameObject("SpawnPoint");
            // Create a custom game object
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.AddComponent<BoxCollider>();
            go.AddComponent<ESGateWaySpawnSetup>();
            //
            go.GetComponent<MeshFilter>().sharedMesh = es.SpawnPrefab.GetComponent<MeshFilter>().sharedMesh;
            go.GetComponent<MeshRenderer>().sharedMaterial = es.SpawnPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            go.transform.position = hit.point;
           
            go.transform.parent = scripts.transform;
        }
    }
}
