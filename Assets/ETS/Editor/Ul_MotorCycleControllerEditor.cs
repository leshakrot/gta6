
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(UL_MotorCycleController))]
public class Ul_MotorCycleControllerEditor : Editor
{
    public UL_MotorCycleController myscript;
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        myscript = target as UL_MotorCycleController;

        MotorCtrlCustomInspector(myscript);
        //base.OnInspectorGUI();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(myscript);
        }
    }
    //
    public void MotorCtrlCustomInspector(UL_MotorCycleController MTC)
    {
        EditorGUI.BeginChangeCheck();
        UL_MotorCycleController.ControllerType _controllertype = new UL_MotorCycleController.ControllerType();

        _controllertype = (UL_MotorCycleController.ControllerType)EditorGUILayout.EnumPopup("ControllerType", myscript.GetControllerType);

        if (EditorGUI.EndChangeCheck())
        {
            myscript.GetControllerType = _controllertype;
        }
        //
        myscript.tab = GUILayout.Toolbar(myscript.tab, new string[] { "Welcome", "AI Controller", "User Controller", "General Settings" });

        switch (myscript.tab)
        {

            case 0:
                {
                    GUILayout.Space(6);
                    GUILayout.Label("Thanks for Choosing UTC ,\n  now you can enjoy\n real Open World Traffic in your game ;)", EditorStyles.boldLabel);
                }
                break;
            case 1:
                {
                    EditorGUI.BeginChangeCheck();
                    #region Ai variable
                    float fowardsensor = new float();
                    float sidesensors = new float();
                    WheelCollider frontwheel = new WheelCollider();
                    WheelCollider rearwheel = new WheelCollider();
                    Transform TargetNode = null;
                    float NavigateSpeed = new float();
                    float LookAtSpeed = new float();
                    float DistanceApart = new float();
                    float Topspeed = new float();
                    #endregion
                    if (myscript.GetControllerType == UL_MotorCycleController.ControllerType.AI)
                    {
                        GUILayout.Space(6);
                        GUILayout.Label("Sensors/AI Navigate Settings", EditorStyles.boldLabel);
                        GUILayout.Space(1.5f);
                        fowardsensor = EditorGUILayout.FloatField("FowardSensor", myscript.fowardsensor);
                        sidesensors = EditorGUILayout.FloatField("SideSensor", myscript.sidesensor);
                        NavigateSpeed = EditorGUILayout.FloatField("NavigateSpeed", myscript.NavigateSpeed);
                        LookAtSpeed = EditorGUILayout.FloatField("LookAtSpeed", myscript.LookAtSpeed);
                        DistanceApart = EditorGUILayout.FloatField(" DistanceApart ", myscript.DistanceApart);
                        Topspeed = EditorGUILayout.FloatField("AI Topseed", myscript.topspeed);
                        GUILayout.Space(6);
                        GUILayout.Label("AI WheelSetup", EditorStyles.boldLabel);
                        frontwheel = EditorGUILayout.ObjectField("FrontWheel", myscript.FrontWheelCol, typeof(WheelCollider), true) as WheelCollider;
                        rearwheel = EditorGUILayout.ObjectField("RearWheel", myscript.RearWheelCol, typeof(WheelCollider), true) as WheelCollider;
                        TargetNode = EditorGUILayout.ObjectField("TargetNode", myscript.TargetNode, typeof(Transform), true) as Transform;
                        GUILayout.Space(1.5f);

                        if (EditorGUI.EndChangeCheck())
                        {
                            myscript.TargetNode = TargetNode;
                            myscript.FrontWheelCol = frontwheel;
                            myscript.RearWheelCol = rearwheel;
                            myscript.NavigateSpeed = NavigateSpeed;
                            myscript.topspeed = Topspeed;
                            myscript.LookAtSpeed = LookAtSpeed;
                            myscript.DistanceApart = DistanceApart;
                            myscript.sidesensor = sidesensors;
                            myscript.fowardsensor = fowardsensor;
                        }
                    }
                    else
                    {
                        GUILayout.Space(6);
                        GUILayout.Label("Please Change The Above Controller PopUp Menu to (Ai) \nBefore You Can Edit AI Settngs", EditorStyles.boldLabel);
                    }
                }
                break;
            case 2:
                {
                    EditorGUI.BeginChangeCheck();
                    #region Player variable
                    Ul_Suspension _frontwheel = null;
                    Ul_Suspension _rearwheel = null;
                    float OverallTorque = new float();
                    float RollTorque = new float();
                    float BrakeTorque = new float();
                    float ReverseTorque = new float();
                    float sidelerptorque = new float();
                    float balancingforce = new float();
                    float neckang = new float();
                    float driftspeed = new float();
                    float killdriftspeed = new float();
                    Transform bikehead = null;
                    Ul_SurfaceDetector surfaceDetector = null;


                    #endregion
                    if (myscript.GetControllerType == UL_MotorCycleController.ControllerType.PlayerCotrol)
                    {
                        GUILayout.Space(6);
                        GUILayout.Label("WheelSettings", EditorStyles.boldLabel);
                        _frontwheel = EditorGUILayout.ObjectField("FrontWheelSuspension", myscript.FrontWheel, typeof(Ul_Suspension), true) as Ul_Suspension;
                        _rearwheel = EditorGUILayout.ObjectField("RearWheelSuspension", myscript.RearWheel, typeof(Ul_Suspension), true) as Ul_Suspension;
                        GUILayout.Space(6);
                        GUILayout.Label("TorqueSettings", EditorStyles.boldLabel);
                        OverallTorque = EditorGUILayout.FloatField("OverallTorque", myscript.OverRallTorque);
                        sidelerptorque = EditorGUILayout.FloatField("SideLerpTorque", myscript.SideLerpTorque);
                        balancingforce = EditorGUILayout.FloatField("BalancingForce", myscript.Balancingforce);
                        RollTorque = EditorGUILayout.FloatField("RollTorque", myscript.RollTorque);
                        BrakeTorque = EditorGUILayout.FloatField("BrakeTorque", myscript.Braketorque);
                        ReverseTorque = EditorGUILayout.FloatField("ReverseTorque", myscript.ReverseTorque);
                        GUILayout.Space(6);
                        GUILayout.Label("Neck/Drift Setings", EditorStyles.boldLabel);
                        bikehead = EditorGUILayout.ObjectField("BikeHeadParent", myscript.BikeHead, typeof(Transform), true) as Transform;
                        neckang = EditorGUILayout.FloatField("NeckAngle", myscript.NeckAngle);
                        killdriftspeed = EditorGUILayout.FloatField("KillDriftSpeed", myscript.KillDriftSpeed);
                        driftspeed = EditorGUILayout.FloatField("DriftSpeed", myscript.DriftSpeed);
                        surfaceDetector = EditorGUILayout.ObjectField("SurfaceDetector", myscript.surfaceDetector, typeof(Ul_SurfaceDetector), true) as Ul_SurfaceDetector;

                    }
                    else
                    {
                        GUILayout.Space(6);
                        GUILayout.Label("Please Change The Above Controller PopUp Menu to (PlayerController) \n Before You Can Edit Player Settngs", EditorStyles.boldLabel);

                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        myscript.surfaceDetector = surfaceDetector;
                        myscript.BikeHead = bikehead;
                        myscript.OverRallTorque = OverallTorque;
                        myscript.RollTorque = RollTorque;
                        myscript.SideLerpTorque = sidelerptorque;
                        myscript.NeckAngle = neckang;
                        myscript.DriftSpeed = driftspeed;
                        myscript.KillDriftSpeed = killdriftspeed;
                        myscript.Balancingforce = balancingforce;
                        myscript.ReverseTorque = ReverseTorque;
                        myscript.Braketorque = BrakeTorque;
                        myscript.FrontWheel = _frontwheel;
                        myscript.RearWheel = _rearwheel;
                    }
                }
                break;
            case 3:
                {
                    EditorGUI.BeginChangeCheck();
                    Transform FrontWheelMesh = null;
                    Transform RearWheelMesh = null;
                    GUILayout.Label("Settings here will affect both AI and player control", EditorStyles.boldLabel);
                    FrontWheelMesh = EditorGUILayout.ObjectField("FrontWheelMesh", myscript.FrontWheelMesh, typeof(Transform), true) as Transform;
                    RearWheelMesh = EditorGUILayout.ObjectField("RearWheelMesh", myscript.RearWheelMesh, typeof(Transform), true) as Transform;
                    if (EditorGUI.EndChangeCheck())
                    {
                        myscript.FrontWheelMesh = FrontWheelMesh;
                        myscript.RearWheelMesh = RearWheelMesh;
                    }
                }
                break;
        }
    }
}
#endif

