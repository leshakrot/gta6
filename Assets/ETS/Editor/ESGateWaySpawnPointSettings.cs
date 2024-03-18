using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(ESGateWaySpawnSetup))]
public class ESGateWaySpawnPointSettings : Editor
{
    public ESGateWaySpawnSetup es;
    //
    public override void OnInspectorGUI()
    {
        es = target as ESGateWaySpawnSetup;
        base.OnInspectorGUI();
    }

    //
    public void OnSceneGUI()
    {
        es = target as ESGateWaySpawnSetup;
        if (es.UseQuickTool == false) return;
        Event e = Event.current;

        if (Event.current.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.M)
            {

                CallGenericMenu(e.mousePosition);
            }
            //
            if (e.keyCode == KeyCode.O)
            {

                CallGenericMenuDetach(e.mousePosition);
            }
        }


        if (Event.current.type == EventType.KeyUp)
        {
            es.done = false;
        }
        //
        if (es.UseQuickTool)
            CreatePathSettings(es);
    }

    private void CallGenericMenu(Vector2 mousepos)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("MakeTarget"), false, () => Performadd());

        genericMenu.ShowAsContext();
    }
    //
    private void CallGenericMenuDetach(Vector2 mousepos)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("RemoveTarget"), false, () => Performremove());

        genericMenu.ShowAsContext();
    }
    //
    private void Performadd()
    {
        //  Debug.Log("wow");
        es.add = true;
    }
    //
    //
    private void Performremove()
    {
        // Debug.Log("wow");
        es.remove = true;
    }
    //
    private void CreatePathSettings(ESGateWaySpawnSetup scripts)
    {
        Handles.BeginGUI();
        EditorGUI.BeginChangeCheck();

        Rect boxrect = new Rect(0, 0, 350, 200);
        GUIStyle boxstyle = new GUIStyle();
        boxstyle.border = GUI.skin.box.border;
        boxstyle.alignment = TextAnchor.UpperCenter;
        boxstyle.normal.textColor = Color.white;
        boxstyle.normal.background = Resources.Load("stuff/settingspan") as Texture2D;

        GUI.Box(boxrect, "SpawnPointSettings", boxstyle);
        //
        GUIStyle L_style = new GUIStyle();
        L_style.normal.textColor = Color.white;
        EditorGUI.LabelField(new Rect(5, 20, 20, 20), "BoxColliderSettings:", L_style);
        GUI.Box(new Rect(5, 35.5f, 310, 2.5f), "");
        //ESNodeSystem.AlignAxis alignAxis = new ESNodeSystem.AlignAxis();
        //alignAxis = (ESNodeSystem.AlignAxis)EditorGUI.EnumPopup(new Rect(53, 20, 50, 20), scripts.GetAlign);
        EditorGUI.LabelField(new Rect(5, 46, 50, 20), "Position:>", L_style);
        EditorGUI.LabelField(new Rect(85, 46, 50, 20), "X", L_style);
        EditorGUI.LabelField(new Rect(175, 46, 50, 20), "Y", L_style);
        EditorGUI.LabelField(new Rect(270, 46, 50, 20), "Z", L_style);
        //
        float x = 0, y = 0, z = 0;
        x = EditorGUI.FloatField(new Rect(100, 46, 50, 18), scripts.center.x);
        y = EditorGUI.FloatField(new Rect(190, 46, 50, 18), scripts.center.y);
        z = EditorGUI.FloatField(new Rect(285, 46, 50, 18), scripts.center.z);
        //
        EditorGUI.LabelField(new Rect(5, 70, 50, 20), "Size:>", L_style);
        EditorGUI.LabelField(new Rect(85, 70, 50, 20), "X", L_style);
        EditorGUI.LabelField(new Rect(175, 70, 50, 20), "Y", L_style);
        EditorGUI.LabelField(new Rect(270, 70, 50, 20), "Z", L_style);
        //
        float s_x = 1, s_y = 1, s_z = 1;
        s_x = EditorGUI.FloatField(new Rect(100, 70, 50, 18), scripts.size.x);
        s_y = EditorGUI.FloatField(new Rect(190, 70, 50, 18), scripts.size.y);
        s_z = EditorGUI.FloatField(new Rect(285, 70, 50, 18), scripts.size.z);

        bool istrigger = true;
        EditorGUI.LabelField(new Rect(5, 100, 50, 18), "IsTrigger:>", L_style);
        istrigger = EditorGUI.Toggle(new Rect(80, 100, 50, 18), scripts.IsTrigger);
        //
        bool show = true;
        GUI.Box(new Rect(102, 100f, 2.5f, 18f), "");
        EditorGUI.LabelField(new Rect(110, 100, 50, 18), "RenderMesh:>", L_style);
        show = EditorGUI.Toggle(new Rect(198, 100, 50, 18), scripts.showpoint);
        //
        float groundoffset = 0.1f;
        GUI.Box(new Rect(219, 100f, 2.5f, 18f), "");
        EditorGUI.LabelField(new Rect(225, 100, 50, 18), "Offset :>", L_style);
        groundoffset = EditorGUI.FloatField(new Rect(285, 100, 50, 18), scripts.groundoffset);
        //
        //debug
        EditorGUI.LabelField(new Rect(5, 135, 50, 18), "Debug", L_style);
        GUI.Box(new Rect(5, 155.5f, 310, 2.5f), "");
        EditorGUI.LabelField(new Rect(5, 170, 50, 18), "TargetNodeConnected:>", L_style);
        if (scripts.TargetNode == null)
            EditorGUI.LabelField(new Rect(170, 170, 50, 18), "False", L_style);
        else
            EditorGUI.LabelField(new Rect(170, 170, 50, 18), "True", L_style);
        /*
        if (GUI.Button(new Rect(5, 67, 50, 15), "-"))
        {
            Undo.RegisterFullObjectHierarchyUndo(scripts.gameObject, "Undo last");
            if (scripts.nodelist.Count > 0)
            {
                DestroyImmediate(scripts.nodelist[scripts.nodelist.Count - 1].gameObject);
            }

        }
        EditorGUI.LabelField(new Rect(70, 47, 50, 20), "clear", L_style);
        if (GUI.Button(new Rect(58, 67, 80, 15), "ClearNode"))
        {
            Undo.RegisterFullObjectHierarchyUndo(scripts.gameObject, "Undo full delete");
            if (scripts.nodelist.Count > 0)
            {
                for (int i = 0; i < scripts.nodelist.Count; ++i)
                {
                    DestroyImmediate(scripts.nodelist[i].gameObject);
                }
            }

            scripts.nodelist.Clear();
        }
         */
        Handles.EndGUI();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(scripts, "Changes to scripts for spawn");
            scripts.IsTrigger = istrigger;
            scripts.center.x = x;
            scripts.center.y = y;
            scripts.center.z = z;
            //
            scripts.size.x = s_x;
            scripts.size.y = s_y;
            scripts.size.z = s_z;
            //
            scripts.groundoffset = groundoffset;
            //
            scripts.showpoint = show;
            //byeeee\\\\\\\\
            //scripts.GetAlign = alignAxis;
        }
        //
        scripts.transform.position = new Vector3(scripts.transform.position.x, scripts.groundoffset, scripts.transform.position.z);
        //
        if (scripts.GetComponent<BoxCollider>() == null)
        {
            scripts.gameObject.AddComponent<BoxCollider>();
        }
        //
        if (scripts.GetComponent<BoxCollider>() != null)
        {

            scripts.GetComponent<BoxCollider>().isTrigger = scripts.IsTrigger;
        }
        //
        if (scripts.GetComponent<BoxCollider>() != null)
            scripts.GetComponent<BoxCollider>().center = scripts.center;
        //
        if (scripts.GetComponent<BoxCollider>() != null)
            scripts.GetComponent<BoxCollider>().size = scripts.size;
        //

        if (scripts.GetComponent<MeshRenderer>() != null)
            scripts.GetComponent<MeshRenderer>().enabled = scripts.showpoint;
    }
}
