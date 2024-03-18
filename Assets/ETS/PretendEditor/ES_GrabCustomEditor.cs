using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(ESGrabVehicle))]
public class ES_GrabCustomEditor : Editor
{
    public ESGrabVehicle scripts;
    public enum ModeType
    {
        IKs,
        Enteries
    }
    public static ModeType GetModeType = ModeType.Enteries;

    public override void OnInspectorGUI()
    {
        scripts = target as ESGrabVehicle;
        CustomGrabInspector();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(scripts);
        }
    }
    //
    private void CustomGrabInspector()
    {
        scripts.ModeIndex = GUILayout.Toolbar(scripts.ModeIndex, new string[] { "NormalMode", "EditMode" });
        switch (scripts.ModeIndex)
        {
            case 0:
                {
                    //
                    ESGrabVehicle.VehicleType vehicleType = new ESGrabVehicle.VehicleType();
                    ESGrabVehicle.StaticLocalEulerAngles staticLocal = new ESGrabVehicle.StaticLocalEulerAngles();
                    Transform thirdveiw = null;
                    Transform firstveiw = null;
                    string PickUpFromTopTrigger = "";
                    string PickUpFromBehindTrigger = "";
                    float liftspeed = new float();
                    Transform PickFrmBehindEntry = null, PickFrmTopEntry = null;
                    string EnterVehicle = "";
                    string EnterVehicleMirror = "";
                    string ExitVehile = "";
                    string AgressiveExit = "";
                    Transform LeftEntery = null;
                    Transform RightEntery = null;
                    bool UseSitpoint = new bool();
                    Transform SitPoint = null;
                    string MountText = "";
                    string pickUpText = "";
                    bool OwnedByAI = false;
                    bool SpawnAIWithVehicle = false;
                    Transform AiTransform = null;
                    bool ApplyMirrorToExit = new bool();
                    float SitBlendLevel = new float();
                    float GrabBlendLevel = new float();
                    float GrabSpeed = new float();
                    float StepHeight = new float();
                    KeyCode GrabKey = new KeyCode();
                    UL_MotorCycleController motorCycleController = null;
                    UL_MotorCycleControl motorCycleControl = null;
                    //
                    EditorGUI.BeginChangeCheck();
                    GUILayout.Space(2.5f);
                    GUILayout.Label("AI Settings;)", EditorStyles.boldLabel);
                    OwnedByAI = EditorGUILayout.Toggle("OwnedByAI", scripts.OwnedByAI);
                    if (OwnedByAI)
                    {
                        SpawnAIWithVehicle = EditorGUILayout.Toggle("SpawnWithAi", scripts.SpawnAIWithVehicle);
                        AiTransform = EditorGUILayout.ObjectField("AITransform", scripts.AiTransform, typeof(Transform), true) as Transform;
                    }
                    GUILayout.Space(6);
                    GUILayout.Label("General Settings;)", EditorStyles.boldLabel);
                    GUILayout.Label("Animation Settings", EditorStyles.miniBoldLabel);
                    EnterVehicle = EditorGUILayout.TextField("EnterVehicleLeft", scripts.EnterVehicle);
                    EnterVehicleMirror = EditorGUILayout.TextField("EnterVehicleRight", scripts.EnterVehicleMirror);
                    ExitVehile = EditorGUILayout.TextField("ExitVehicle", scripts.ExitVehile);
                    AgressiveExit = EditorGUILayout.TextField("AgressiveExit", scripts.AgressiveExit);

                    GUILayout.Label("EnteryPoints", EditorStyles.miniBoldLabel);
                    RightEntery = EditorGUILayout.ObjectField("RightEntery", scripts.RightEntery, typeof(Transform), true) as Transform;
                    LeftEntery = EditorGUILayout.ObjectField("LeftEntery", scripts.LeftEntery, typeof(Transform), true) as Transform;
                    UseSitpoint = EditorGUILayout.Toggle("UseSitPoint", scripts.UseSitpoint);
                    if (UseSitpoint)
                    {
                        SitPoint = EditorGUILayout.ObjectField("SitPoint", scripts.SitPoint, typeof(Transform), true) as Transform;
                    }
                    SitBlendLevel = EditorGUILayout.FloatField("SitBlendLevel", scripts.SitBlendLevel);
                    GrabBlendLevel = EditorGUILayout.FloatField("GrabBlendLevel", scripts.GrabBlendLevel);
                    GrabSpeed = EditorGUILayout.FloatField("GrabSpeed", scripts.GrabSpeed);
                    GrabKey = (KeyCode)EditorGUILayout.EnumPopup("GrabKey", scripts.GrabKey);
                    StepHeight = EditorGUILayout.FloatField("StepHeight", scripts.StepHeight);



                    GUILayout.Label("Other", EditorStyles.miniBoldLabel);
                    staticLocal = (ESGrabVehicle.StaticLocalEulerAngles)EditorGUILayout.EnumPopup("StaticLocalEuler", scripts.StaticLocalEuler);
                    if (scripts._vehicleType == ESGrabVehicle.VehicleType.Bike)
                    {
                        motorCycleControl = EditorGUILayout.ObjectField("MotorCycleControl", scripts.motorCycleControl, typeof(UL_MotorCycleControl), true) as UL_MotorCycleControl;
                        motorCycleController = EditorGUILayout.ObjectField("MotorCycleController", scripts.motorCycleController, typeof(UL_MotorCycleController), true) as UL_MotorCycleController;
                    }
                    GUILayout.Label("Camera Settings", EditorStyles.miniBoldLabel);
                    thirdveiw = EditorGUILayout.ObjectField("ThirdVeiw", scripts.ThirdView, typeof(Transform), true) as Transform;
                    firstveiw = EditorGUILayout.ObjectField("FirstView", scripts.FirstView, typeof(Transform), true) as Transform;
                    GUILayout.Label("UI Settings", EditorStyles.miniBoldLabel);
                    MountText = EditorGUILayout.TextField("MountText", scripts.MountText);
                    pickUpText = EditorGUILayout.TextField("PickUpText", scripts.pickUpText);

                    GUILayout.Space(6);
                    vehicleType = (ESGrabVehicle.VehicleType)EditorGUILayout.EnumPopup("VehicleType", scripts._vehicleType);
                    if (vehicleType == ESGrabVehicle.VehicleType.Bike)
                    {
                        GUILayout.Space(6);
                        GUILayout.Label("Based On Motorcycle Settings;)", EditorStyles.boldLabel);
                        PickUpFromTopTrigger = EditorGUILayout.TextField("PickUpFromTopTrigger", scripts.PickUpFromTopTrigger);
                        PickUpFromBehindTrigger = EditorGUILayout.TextField("PickUpFromBehindTrigger", scripts.PickUpFromBehindTrigger);
                        PickFrmBehindEntry = EditorGUILayout.ObjectField("PickFrmBehindEntry", scripts.PickFrmBehindEntry, typeof(Transform), true) as Transform;
                        PickFrmTopEntry = EditorGUILayout.ObjectField("PickFrmTopEntry", scripts.PickFrmTopEntry, typeof(Transform), true) as Transform;
                        liftspeed = EditorGUILayout.FloatField("LiftSpeed", scripts.LiftSpeed);

                    }
                    GUILayout.Label("Experimental", EditorStyles.boldLabel);
                    ApplyMirrorToExit = EditorGUILayout.Toggle("ApplyMirrorExit", scripts.ApplyMirrorToExit);
                    //FollowObject = EditorGUILayout.ObjectField("FollowObject", myscript.Target, typeof(GameObject), true) as GameObject;
                    if (EditorGUI.EndChangeCheck())
                    {
                        scripts._vehicleType = vehicleType;
                        scripts.StaticLocalEuler = staticLocal;
                        scripts.AgressiveExit = AgressiveExit;
                        scripts.EnterVehicle = EnterVehicle;
                        scripts.ExitVehile = ExitVehile;
                        scripts.EnterVehicleMirror = EnterVehicleMirror;
                        scripts.StepHeight = StepHeight;
                        scripts.SitBlendLevel = SitBlendLevel;
                        scripts.ThirdView = thirdveiw;
                        scripts.FirstView = firstveiw;
                        scripts.pickUpText = pickUpText;
                        scripts.PickFrmBehindEntry = PickFrmBehindEntry;
                        scripts.PickFrmTopEntry = PickFrmTopEntry;
                        scripts.PickUpFromTopTrigger = PickUpFromTopTrigger;
                        scripts.PickUpFromBehindTrigger = PickUpFromBehindTrigger;
                        scripts.UseSitpoint = UseSitpoint;
                        scripts.SitPoint = SitPoint;
                        scripts.GrabKey = GrabKey;
                        scripts.GrabSpeed = GrabSpeed;
                        scripts.GrabBlendLevel = GrabBlendLevel;
                        scripts.OwnedByAI = OwnedByAI;
                        scripts.AiTransform = AiTransform;
                        scripts.SpawnAIWithVehicle = SpawnAIWithVehicle;
                        scripts.LeftEntery = LeftEntery;
                        scripts.RightEntery = RightEntery;
                        scripts.LiftSpeed = liftspeed;
                        scripts.pickUpText = pickUpText;
                        scripts.MountText = MountText;
                        scripts.motorCycleController = motorCycleController;
                        scripts.motorCycleControl = motorCycleControl;
                        Undo.RegisterCompleteObjectUndo(scripts, "Grabthinhgs");
                    }
                }
                break;
            case 1:
                {
                    EditorGUI.BeginChangeCheck();
                    float deltaTime = new float();
                    Transform testmodel = null;
                    testmodel = EditorGUILayout.ObjectField("TestSubject", scripts.TestModelPrefab, typeof(Transform), true) as Transform;

                    scripts.TestIks = false;
                    deltaTime = EditorGUILayout.Slider("AnimDeltaTime", scripts.AnimdeltaTime, 0.00000001f, 2f);
                    if (EditorGUI.EndChangeCheck())
                    {
                        scripts.TestModelPrefab = testmodel;
                        scripts.AnimdeltaTime = deltaTime;
                    }
                    GUILayout.Label("Make sure your testsubject animator is a rootmotion", EditorStyles.boldLabel);
                    GUILayout.Space(6);
                    GetModeType = (ModeType)EditorGUILayout.EnumPopup("ModeType", GetModeType);
                    GUILayout.Space(6);
                    GUI.color = Color.white;
                    if (GetModeType == ModeType.Enteries)
                    {
                        if (GUILayout.Button("Test Left Entery", GUILayout.MinWidth(150), GUILayout.MinHeight(40)))
                        {
                            if (scripts.TestModelPrefab != null)
                            {
                                if (scripts.TestModelTrans == null)
                                {
                                    scripts.TestModelTrans = Instantiate(scripts.TestModelPrefab);
                                }
                            }
                            else
                            {
                                Debug.Log("Test Subject Missing Please Make a test model prefab with animator controller attached /n navigate to folder ETS/ ThirdPerson/Prefab/TestAvatar.prefab");
                            }
                            if (scripts.TestModelPrefab != null)
                            {
                                scripts.TestRightEntry = false;
                                scripts.TestLeftEntry = true;
                                scripts.TestSimplexit = false;
                            }
                        }
                        //
                        GUILayout.Space(6);
                        if (GUILayout.Button("Test Right Entery", GUILayout.MinWidth(150), GUILayout.MinHeight(40)))
                        {
                            if (scripts.TestModelPrefab != null)
                            {
                                if (scripts.TestModelTrans == null)
                                {
                                    scripts.TestModelTrans = Instantiate(scripts.TestModelPrefab);
                                }
                            }
                            else
                            {
                                Debug.Log("Test Subject Missing Please Make a test model prefab with animator controller attached navigate to folder ETS/ ThirdPerson/Prefab/TestAvatar.prefab");
                            }
                            if (scripts.TestModelPrefab != null)
                            {
                                scripts.TestRightEntry = true;
                                scripts.TestLeftEntry = false;
                                scripts.TestSimplexit = false;
                            }
                        }
                        //
                        GUILayout.Space(6);
                        if (scripts.TestModelTrans != null)
                        {
                            if (GUILayout.Button("Test Simple Exit", GUILayout.MinWidth(150), GUILayout.MinHeight(40)))
                            {
                                if (scripts.TestModelTrans == null)
                                {
                                    scripts.TestModelTrans = Instantiate(scripts.TestModelPrefab);
                                    Vector3 vec = scripts.SitPoint.localPosition;
                                    vec.y = 0f;
                                    scripts.TestModelTrans.localPosition = vec;
                                }
                                if (scripts.TestModelPrefab != null)
                                {
                                    scripts.TestRightEntry = false;
                                    scripts.TestLeftEntry = false;
                                    scripts.TestSimplexit = true;
                                }
                            }
                            //
                            GUILayout.Space(10f);
                            GUI.color = Color.red;
                            if (GUILayout.Button("DeleteTestSubject", GUILayout.MinWidth(150), GUILayout.MinHeight(40)))
                            {
                                DestroyImmediate(scripts.TestModelTrans.gameObject);
                            }
                        }
                    }
                    else
                    {
                        EditorGUI.BeginChangeCheck();
                        if (scripts.TestModelTrans == null)
                        {
                            scripts.TestModelTrans = Instantiate(scripts.TestModelPrefab);
                        }
                        //
                        //
                        Transform LeftIkTarget = null;
                        Transform RightIkTarget = null;
                        Transform LeftFootIkTarget = null;
                        Transform RightFootIkTarget = null;
                        Transform LookPos = null;
                        float IKWeight = new float();
                        float LookWeight = new float();
                        float BodyWeight = new float();
                        float HeadWeight = new float();
                        float Eyeweight = new float();
                        float ClampWeight = new float();

                        GUILayout.Label("Ik Setup", EditorStyles.miniBoldLabel);
                        if (scripts.TestModelTrans != null)
                        {
                            LeftIkTarget = EditorGUILayout.ObjectField("LeftHandIKTarget", scripts.LeftIkTarget, typeof(Transform), true) as Transform;
                            RightIkTarget = EditorGUILayout.ObjectField("RightHandIKTarget", scripts.RightIkTarget, typeof(Transform), true) as Transform;
                            LeftFootIkTarget = EditorGUILayout.ObjectField("LeftFootIKTarget", scripts.LeftFootIkTarget, typeof(Transform), true) as Transform;
                            RightFootIkTarget = EditorGUILayout.ObjectField("RightFootIKTarget", scripts.RightFootIkTarget, typeof(Transform), true) as Transform;
                            LookPos = EditorGUILayout.ObjectField("LookPosition", scripts.LookPos, typeof(Transform), true) as Transform;
                            IKWeight = EditorGUILayout.FloatField("IKWeight", scripts.IKWeight);
                            LookWeight = EditorGUILayout.FloatField("LookWeight", scripts.LookWeight);
                            BodyWeight = EditorGUILayout.FloatField("BodyWeight", scripts.BodyWeight);
                            ClampWeight = EditorGUILayout.FloatField("ClampWeight", scripts.ClampWeight);
                            Eyeweight = EditorGUILayout.FloatField("EyeWeight", scripts.Eyeweight);
                            HeadWeight = EditorGUILayout.FloatField("HeadWeight", scripts.HeadWeight);
                        }
                        else
                        {
                            GUILayout.Label("TestModelMissing", EditorStyles.boldLabel);
                        }
                        if (scripts.RightIkTarget != null && scripts.SitPoint != null && scripts.LeftIkTarget != null
                        && LookPos != null && scripts.LeftFootIkTarget != null && scripts.RightFootIkTarget)
                            scripts.TestIks = true;
                        else
                            scripts.TestIks = false;
                        //

                        //
                        if (EditorGUI.EndChangeCheck())
                        {
                            scripts.LeftIkTarget = LeftIkTarget;
                            scripts.RightIkTarget = RightIkTarget;
                            scripts.RightFootIkTarget = RightFootIkTarget;
                            scripts.LeftFootIkTarget = LeftFootIkTarget;
                            scripts.IKWeight = IKWeight;
                            scripts.BodyWeight = BodyWeight;
                            scripts.HeadWeight = HeadWeight;
                            scripts.LookWeight = LookWeight;
                            scripts.LookPos = LookPos;
                            scripts.Eyeweight = Eyeweight;
                            scripts.ClampWeight = ClampWeight;
                        }
                    }
                }
                break;

        }

    }
}
#endif
