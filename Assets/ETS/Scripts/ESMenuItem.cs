using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class ESMenuItem : MonoBehaviour
{
    [MenuItem("GameObject/Easy Traffic System/Vol2/AddPathParent", false, 10)]
    static void CreatePathParentGameObject(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject goparent = new GameObject("NodeParent");
        // Create a custom game object
        GameObject go = new GameObject("Path");
        go.AddComponent<ESNodeSystem>();
 
      
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        GameObjectUtility.SetParentAndAlign(goparent, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(goparent, "Create " + goparent.name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.transform.parent = goparent.transform;

    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/AddTrafficLight", false, 10)]
    static void CreateTrafficParentGameObject(MenuCommand menuCommand)
    {

        GameObject go = Resources.Load("stuff/TrafficParent") as GameObject;
        // Create a custom game object


        GameObject Tempgo = null;
        // Create a custom game object
        if (go != null)
        {
          Tempgo = Instantiate(go);
         
        }
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(Tempgo, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(Tempgo, "Create TrafficAI_i" + go.name);
        Selection.activeObject = Tempgo;

    }
    [MenuItem("GameObject/Easy Traffic System/Vol2/AddHumanPathParent", false, 10)]
    static void CreateHumanPathParentGameObject(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject goparent = new GameObject("HumanNodeParent");
        // Create a custom game object
        GameObject go = new GameObject("Path");
        go.AddComponent<ESHumanNodeSystem>();


        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        GameObjectUtility.SetParentAndAlign(goparent, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(goparent, "Create " + goparent.name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.transform.parent = goparent.transform;

    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/TrafficManager", false, 10)]
    static void CreateManager(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = new GameObject("Manager");
        // Create a custom game object
       
        go.AddComponent<TrafficManager>();


        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create manager" + go.name);
        Selection.activeObject = go;
     

    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/TrafficControlPoint", false, 10)]
    static void CreateCP(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = new GameObject("ControlPoint");
        // Create a custom game object

        go.AddComponent<ESTrafficControlPoint>();


        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create CP_i" + go.name);
        Selection.activeObject = go;


    }
    //
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/Spawn/SpawnManager", false, 10)]
    static void CreateSM(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = new GameObject("SpawnManager");
        // Create a custom game object

        go.AddComponent<ESSpawnManager>();


        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create SM_i" + go.name);
        Selection.activeObject = go;


    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/AIVehicles/TruckWithContainerAI", false, 10)]
    static void CreateTruck(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = Resources.Load("stuff/HTruck(prefab)(AI) ") as GameObject;
        // Create a custom game object


        GameObject Tempgo = null;
        // Create a custom game object
        if (go != null)
        {
            Tempgo = Instantiate(go);
        }
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(Tempgo, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(Tempgo, "Create TruckAI_i" + go.name);
        Selection.activeObject = Tempgo;


    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/AIVehicles/CarAI", false, 10)]
    static void CreateCarAI(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = Resources.Load("stuff/CarAI") as GameObject;
        // Create a custom game object


        GameObject Tempgo = null;
        // Create a custom game object
        if (go != null)
        {
            Tempgo = Instantiate(go);
        }
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(Tempgo, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(Tempgo, "Create CArAI_i" + go.name);
        Selection.activeObject = Tempgo;


    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/Spawn/HumanSpawnManager", false, 10)]
    static void CreateHSM(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = new GameObject("HumanSpawnManager");
        // Create a custom game object

        go.AddComponent<ESSpawnHuman>();


        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create HSM_i" + go.name);
        Selection.activeObject = go;


    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/ChatGroup/2", false, 10)]
    static void CreateChatGroupTwo(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = Resources.Load("stuff/ChatGroup(2people)  Variant") as GameObject;
        GameObject Tempgo = null;
        // Create a custom game object
        if (go != null)
        {
            Tempgo =  Instantiate(go);
        }
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(Tempgo, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(Tempgo, "Create ChatGrouptwo_i" + go.name);
        Selection.activeObject = Tempgo;
    }
    //
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/ChatGroup/5", false, 10)]
    static void CreateChatGroupfive(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = Resources.Load("stuff/ChatGroup(5people) Variant") as GameObject;
        GameObject Tempgo = null;
        // Create a custom game object
        if (go != null)
        {
            Tempgo = Instantiate(go);
        }
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(Tempgo, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(Tempgo, "Create ChatGroupFive_i" + go.name);
        Selection.activeObject = Tempgo;
    }
    //
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/ChatGroup/4", false, 10)]
    static void CreateChatGroupfour(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = Resources.Load("stuff/ChatGroup(4people)") as GameObject;
        GameObject Tempgo = null;
        // Create a custom game object
        if (go != null)
        {
            Tempgo =  Instantiate(go);
        }
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(Tempgo, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(Tempgo, "Create ChatGroupFour_i" + go.name);
        Selection.activeObject = Tempgo;
    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/MultiThread", false, 10)]
    static void CreateMul(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = new GameObject("Multi_Thread");
        // Create a custom game object
        go.AddComponent<ES_AI_Multi_thread>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create Mul_i" + go.name);
        Selection.activeObject = go;
    }
    //
    [MenuItem("GameObject/Easy Traffic System/Vol2/AddGateWay", false, 10)]
    static void CreateGateWay(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject go = new GameObject("SpawnParent");
        // Create a custom game object
        go.AddComponent<ESGateWayManager>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create GateWay" + go.name);
        Selection.activeObject = go;
    }
    //
    /*
    [MenuItem("GameObject/Easy Traffic System/AddTrafficLightManager", false, 10)]
    static void CreateTrafficLightManagerGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("TrafficLightManager");
        go.AddComponent<ES_TrafficLight_Manager>();

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
    */
}
#endif
