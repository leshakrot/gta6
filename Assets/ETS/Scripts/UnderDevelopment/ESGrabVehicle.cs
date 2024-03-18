using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteAlways]
public class ESGrabVehicle : MonoBehaviour
{
    #region  EditModevariable
    public Transform TestModelPrefab, TestModelTrans;
    public int ModeIndex = 0;
    public float AnimdeltaTime;
    public bool TestLeftEntry, TestRightEntry, TestSimplexit, TestIks;
    #endregion

    #region Normal Variables

    public enum VehicleType
    {
        Bike,
        Car
    }
    //
    [Header("Car Grab System Coming Soon")]
    public VehicleType _vehicleType = VehicleType.Bike;
    public enum StaticLocalEulerAngles
    {
        X_AxisOnly,
        Z_AxisOnly,
        BothAxis,
        None
    }
    public StaticLocalEulerAngles StaticLocalEuler = StaticLocalEulerAngles.BothAxis;
    public Transform FirstView, ThirdView;
    public bool ISFirst = true;

    [Header("Based On Bike Settings")]
    public string PickUpFromTopTrigger = "UpBike";
    public string PickUpFromBehindTrigger = "BehindBike";
    [Range(0.01f, 50.0f)] public float LiftSpeed = 1.5f;
    public Transform PickFrmBehindEntry, PickFrmTopEntry;
    [Header("Based General Settings")]
    [Header("AnimationName")]
    public string EnterVehicle = "EnterBike";
    public string EnterVehicleMirror = "EnterBikeMirroe";
    public string ExitVehile = "ExitVehicle";
    public string AgressiveExit = "JumpBike";
    [Header("Enteries")]
    public Transform LeftEntery;
    public Transform RightEntery;
    public bool UseSitpoint;
    public Transform SitPoint;
    [HideInInspector] public Transform Player;
    [Header("Grab Text")]
    public string MountText = "Press V to Grab";
    public string pickUpText = "Press V to PickUp";
    [Header("Based On AI Settings")]
    public bool OwnedByAI = false;
    public bool SpawnAIWithVehicle = false;
    public Transform AiTransform;
    [Header("End")]
    [Header("Grab Settings")]
    public string VehicleState = "Empty";
    [Tooltip("May Be Unstable")]
    public bool ApplyMirrorToExit;
    [Range(0.00001f, 1f)] public float SitBlendLevel = 0.05f;
    [Range(0.00001f, 1f)] public float GrabBlendLevel = 0.05f;
    [Range(0, 5f)] public float GrabSpeed = 0.7f;
    [Range(0, 1f)] public float StepHeight = 0.1f;
    public KeyCode GrabKey = KeyCode.V;
    [HideInInspector] public float BikeBackardZ;
    [HideInInspector]
    public bool OnSit;
    [HideInInspector] public bool reset;
    public UL_MotorCycleController motorCycleController;
    public UL_MotorCycleControl motorCycleControl;
    public Transform LeftIkTarget;
    public Transform RightIkTarget;
    public Transform LeftFootIkTarget;
    public Transform RightFootIkTarget;
    public Transform LookPos;
    public float IKWeight = 1;
    public float LookWeight = .5f;
    public float BodyWeight = 1;
    public float HeadWeight = 1;
    public float Eyeweight = 0;
    public float ClampWeight = 1;
    [HideInInspector] public bool lerptositpoint, attemptpick;
    [HideInInspector] public float normalizedTime;
    //private 
    private int InverseCount;
    //[SerializeField] 
    private float sidedot;
    private Transform PlayerTransform;
    //[SerializeField]
    private float Updot, entrysidedist, idlenormalizedTime, hipsidedist, exitnormalizedTime, fwdentrysidedist;
    // [SerializeField]
    private bool GrabbedLeft, GrabbedRight, movetopos, callragdollz, returnreverseanim, AIGrab,
    Stop, SingleClick, Umount, pickfrmright, pickfrmleft, returnUpdate, InRange;
    #endregion
    private void Awake()
    {
        StopCameraRender();
        if (OwnedByAI)
        {
            if (AiTransform != null)
            {
                //ParentToVehicleOnStart();
                if (SpawnAIWithVehicle)
                    returnUpdate = true;
            }
        }
    }
    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (SpawnAIWithVehicle)
        {
            Player = AiTransform;
        }
        else
        {
            Player = PlayerTransform;
        }
        if (_vehicleType == VehicleType.Bike)
        {
            if (motorCycleControl != null)
                motorCycleControl.enabled = false;
        }
    }
    //
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (LeftIkTarget != null)
        {
            Gizmos.DrawWireSphere(LeftIkTarget.position, .075f);
        }
        if (RightIkTarget != null)
        {
            Gizmos.DrawWireSphere(RightIkTarget.position, .075f);
        }
        if (LeftFootIkTarget != null)
        {
            Gizmos.DrawWireSphere(LeftFootIkTarget.position, .075f);
        }
        if (RightFootIkTarget != null)
        {
            Gizmos.DrawWireSphere(RightFootIkTarget.position, .075f);
        }
        if (SitPoint != null)
        {
            Gizmos.DrawWireSphere(SitPoint.position, .075f);
        }
        //
        MakeEditMode();
    }
    private void StopCameraRender()
    {
        if (ThirdView == null) return; if (FirstView == null) return;
        FirstView.GetComponent<Camera>().enabled = false;
        FirstView.GetComponent<AudioListener>().enabled = false;
        ThirdView.GetComponent<AudioListener>().enabled = false;
        ThirdView.GetComponent<Camera>().enabled = false;
    }
    //
    private void StartCameraRender()
    {
        if (ThirdView == null) return; if (FirstView == null) return;

        if (ISFirst)
        {
            FirstView.GetComponent<Camera>().enabled = true;
            FirstView.GetComponent<AudioListener>().enabled = true;
            ThirdView.GetComponent<AudioListener>().enabled = false;
            ThirdView.GetComponent<Camera>().enabled = false;
        }
        else
        {
            ThirdView.GetComponent<Camera>().enabled = true;
            ThirdView.GetComponent<AudioListener>().enabled = true;
            FirstView.GetComponent<Camera>().enabled = false;
            FirstView.GetComponent<AudioListener>().enabled = false;
        }
    }
    #region Edit
    //
    private void MakeEditMode()
    {
        if (Application.isPlaying == false)
        {
            if (TestModelTrans != null)
            {
                if (TestLeftEntry)
                {
                    if (LeftEntery != null)
                    {
                        TestModelTrans.parent = this.transform;
                        TestModelTrans.localRotation = LeftEntery.localRotation;
                        TestModelTrans.localPosition = LeftEntery.localPosition;
                        if (TestLeftEntry)
                        {
                            TestModelTrans.GetComponent<Animator>().Play(EnterVehicle, 0, 0);
                            TestLeftEntry = false;
                        }
                    }
                }
                //
                if (TestRightEntry)
                {
                    if (RightEntery != null)
                    {
                        TestModelTrans.parent = this.transform;
                        TestModelTrans.localRotation = RightEntery.localRotation;
                        TestModelTrans.localPosition = RightEntery.localPosition;
                        if (TestRightEntry)
                        {
                            TestModelTrans.GetComponent<Animator>().Play(EnterVehicleMirror, 0, 0);
                            TestRightEntry = false;
                        }
                    }
                }
                //
                if (TestSimplexit)
                {
                    if (SitPoint != null)
                    {
                        TestModelTrans.parent = this.transform;
                        if (TestSimplexit)
                        {
                            TestModelTrans.GetComponent<Animator>().Play(ExitVehile, 0, 0);
                            TestSimplexit = false;
                        }
                    }
                }

                if (TestIks)
                {
                    if (SitPoint != null)
                    {
                        if (TestModelTrans != null)
                        {
                            TestModelTrans.GetComponent<ESInEditmodeIK>().sitpoint = SitPoint;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().animator = TestModelTrans.GetComponent<Animator>();
                            TestModelTrans.GetComponent<ESInEditmodeIK>().RightIkTarget = RightIkTarget;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().LeftIkTarget = LeftIkTarget;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().RightFootIkTarget = RightFootIkTarget;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().LeftFootIkTarget = LeftFootIkTarget;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().BodyWeight = BodyWeight;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().IKWeight = IKWeight;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().LookPos = LookPos;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().LookWeight = LookWeight;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().HeadWeight = HeadWeight;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().ClampWeight = ClampWeight;
                            TestModelTrans.GetComponent<ESInEditmodeIK>().Eyeweight = Eyeweight;
                            TestModelTrans.GetComponent<Animator>().Play(EnterVehicle, 0, 1);
                            TestModelTrans.GetComponent<ESInEditmodeIK>().GoIk = true;
                        }
                    }
                }
                //
                if (TestIks == false)
                {
                    TestModelTrans.GetComponent<ESInEditmodeIK>().GoIk = false;
                    TestModelTrans.GetComponent<Animator>().Update(AnimdeltaTime);
                }
                else
                    TestModelTrans.GetComponent<Animator>().Play(EnterVehicle, 0, 1);
            }
        }
        else
        {
            if (TestModelTrans != null)
                TestModelTrans = null;
        }
    }
    #endregion
    #region Normal
    //
    private void Update()
    {
        if (Application.isPlaying == false) return;
        if (OwnedByAI)
        {
            if (AiTransform != null)
            {
                ParentToVehicleOnStart();
            }
        }
        if (returnUpdate) return;
        //
        if (Player == null) return;
        VehicleGrabSystem();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!AIGrab)
            {
                if (OnSit)
                {
                    if (Player.GetComponent<ES_UniversalCharacterController>().GetCharacterType == ES_UniversalCharacterController.CharacterType.ThirdPerson)
                    {
                        ISFirst = !ISFirst;
                    }
                    else
                    {
                        ISFirst = true;
                    }
                    StartCameraRender();
                }
                //

            }
        }
        if (OnSit)
        {
            SetRiderIK();
            if (AIGrab)
            {
                if (Player != null)
                {
                    if (!Player.GetComponent<Animator>().GetNextAnimatorStateInfo(0).IsName(EnterVehicle))
                    {
                        Player.GetComponent<Animator>().Play(EnterVehicle, 0, 1);
                    }
                }
            }
        }
        else
        {
            RemoveIk();
        }
        DeleteOwnerWithRespectToDist();
        if (AIGrab == false && !OnSit && !callragdollz)
        {
            CheckForWithDraw(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Jump"));
            if (Stop && Player != null)
            {
                GrabbedRight = false;
                GrabbedLeft = false;
                pickfrmleft = false;
                pickfrmright = false;
                lerptositpoint = false;
                OnSit = false;
                Player.GetComponent<ES_UniversalCharacterController>().enabled = true;
                Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = false;
                MountText = "V To Grab Vehicle";
                pickUpText = "V To Pick Vehicle";
                movetopos = false;
                Player.GetComponent<Animator>().applyRootMotion = false;
                Stop = false;
            }
        }
    }
    //
    private void ReturnAct(Transform trans)
    {
        pickfrmright = false;
        trans.GetComponent<Animator>().applyRootMotion = false;
        //trans.GetComponent<Animator>().Play("Grounded", 0, 0);
        lerptositpoint = false;
        OnSit = false;
        attemptpick = false;
        SingleClick = false;
        Stop = false;
        GrabbedRight = false;
        movetopos = false;
        GrabbedLeft = false;
        pickfrmleft = false;
    }
    //
    private void DeleteOwnerWithRespectToDist()
    {
        if (AiTransform == null) return;
        if (Vector3.Distance(AiTransform.position, this.transform.position) > 15)
        {
            Destroy(AiTransform.gameObject);
        }
    }
    //
    private void ParentToVehicleOnStart()
    {
        if (SpawnAIWithVehicle)
        {
            if (returnUpdate)
            {
                AiTransform.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                AiTransform.GetComponent<ES_UniversalCharacterController>().enabled = false;
                AiTransform.GetComponent<CharacterController>().enabled = false;
                AiTransform.GetComponent<ES_HumanEnter_Exit_Vehicle>().grabVehicle = this;
                Vector3 Localplayer = LeftEntery.localPosition;
                Localplayer.y = Player.localPosition.y;
                AiTransform.position = transform.position;
                AiTransform.transform.localRotation = SitPoint.localRotation;
                AiTransform.GetComponent<Animator>().applyRootMotion = true;
                AiTransform.GetComponent<Animator>().Play(EnterVehicle, 0, 1);
                Player = AiTransform;
                //
                OnSit = true;
                VehicleState = "OwnedByAI";
                GrabbedRight = false;
                GrabbedLeft = false;
                if (_vehicleType == VehicleType.Bike)
                {
                    if (VehicleState == "OwnedByAI")
                    {
                        motorCycleController.GetControllerType = UL_MotorCycleController.ControllerType.AI;
                        motorCycleControl.enabled = false;
                    }
                }
                else
                {
                    //code for car heree
                }
                returnUpdate = false;
            }
        }
    }
    //
    private void PlayerToAIManager()
    {
        if (OwnedByAI && AiTransform != null)
        {
            float dot = new float();
            float Lsidedot = new float();
            float LUpdot = new float();
            //sides
            if (!OnSit)
            {
                Vector3 Sides = transform.TransformDirection(Vector3.right);
                Vector3 playerpositon = AiTransform.position - transform.position;
                Lsidedot = Vector3.Dot(Sides, playerpositon);
                //Updot
                Vector3 Upside = transform.TransformDirection(Vector3.up);
                Vector3 playerUppositon = AiTransform.position - transform.position;
                LUpdot = Vector3.Dot(Upside, playerUppositon);
            }

            if (_vehicleType == VehicleType.Bike)
            {
                dot = motorCycleController.LooseControl ? Lsidedot : LUpdot;
            }
            else
            {
                dot = Lsidedot;
            }
            if (!AiTransform.GetComponent<ESRagDollHelper>().FreezeControl && Mathf.Abs(dot) < StepHeight)
            {
                AIGrab = true;
            }
            else
            {
                if (AIGrab)
                {
                    ReturnAct(AiTransform);
                    ReturnAct(PlayerTransform);
                    AIGrab = false;
                }
            }
            if (AIGrab)
            {
                Player = AiTransform;
            }
            else
            {
                Player = PlayerTransform;
            }
        }
        else
        {
            if (Player == null)
            {
                /*
                ReturnAct(PlayerTransform);
                AIGrab = false;
                Player = PlayerTransform;
                */
            }
        }
    }
    //
    private void FixedUpdate()
    {
        //if (motorCycleController == null) return;
        // if (Player == null) return;
        if (Application.isPlaying == false) return;
        if (OwnedByAI)
        {
            if (AiTransform != null)
            {
                ParentToVehicleOnStart();
            }
        }
        if (returnUpdate) return;
        PlayerToAIManager();
        if (Player == null) return;
        Animator animator = Player.GetComponent<Animator>();
        if (_vehicleType == VehicleType.Bike)
        {
            if (OnSit)
            {
                if (motorCycleController.Accel < 0 && motorCycleController.pre_accel_mul < 0)
                    BikeBackardZ = -1;
                else
                    BikeBackardZ = 0;
                //
                if (BikeBackardZ < 0)
                {
                    animator.applyRootMotion = false;
                    animator.SetFloat("BikeBackardZ", BikeBackardZ);
                    returnreverseanim = true;
                }
                else if (BikeBackardZ == 0)
                {
                    if (returnreverseanim)
                    {
                        animator.applyRootMotion = true;
                        animator.SetFloat("BikeBackardZ", 0);
                        if (GrabbedLeft)
                        {
                            animator.Play(EnterVehicle, 0, 1);
                        }
                        if (GrabbedRight)
                        {
                            animator.Play(EnterVehicleMirror, 0, 1);
                        }
                        animator.applyRootMotion = true;
                        returnreverseanim = false;
                    }
                }
            }
        }
        FixLocalEuler();
        //
        MovePlayerToGrabPosition();
        CheckForBodyBalancing();
        PrepareRiderOffVehicle();
        MovePlayerToPickPosition();
        SetRiderForRagDollz();
        PerformPickOperationOnBike();
        //
        //LerpPlayerBodyPosToSitPoint();
        if (Player != null)
        {
            if (Player.GetComponent<ESRagDollHelper>().Ragdollz)
            {
                if (Player.GetComponent<ESRagDollHelper>().BoneState == "Calm")
                {
                    if (callragdollz)
                    {
                        Player.GetComponent<ESRagDollHelper>().Ragdollz = false;
                        InRange = false;
                        callragdollz = false;
                    }
                }
            }
        }
    }
    //
    private void FixLocalEuler()
    {
        #region  PickUpEntries
        if (PickFrmTopEntry != null)
        {
            Vector3 PickFrmTopEntryeuler = PickFrmTopEntry.localEulerAngles;
            if (StaticLocalEuler == StaticLocalEulerAngles.BothAxis)
            {
                PickFrmTopEntryeuler.x = -1 * motorCycleController.transform.eulerAngles.x;
                PickFrmTopEntryeuler.z = -1 * motorCycleController.transform.eulerAngles.z;
            }
            else if (StaticLocalEuler == StaticLocalEulerAngles.X_AxisOnly)
            {
                PickFrmTopEntryeuler.x = -1 * motorCycleController.transform.eulerAngles.x;
            }
            else if (StaticLocalEuler == StaticLocalEulerAngles.Z_AxisOnly)
            {
                PickFrmTopEntryeuler.z = -1 * motorCycleController.transform.eulerAngles.z;
            }
            PickFrmTopEntry.localEulerAngles = PickFrmTopEntryeuler;
        }
        //
        if (PickFrmBehindEntry != null)
        {
            Vector3 PickFrmBehindEntryeuler = PickFrmBehindEntry.localEulerAngles;
            if (StaticLocalEuler == StaticLocalEulerAngles.BothAxis)
            {
                PickFrmBehindEntryeuler.x = -1 * motorCycleController.transform.eulerAngles.x;
                PickFrmBehindEntryeuler.z = -1 * motorCycleController.transform.eulerAngles.z;
            }
            else if (StaticLocalEuler == StaticLocalEulerAngles.X_AxisOnly)
            {
                PickFrmBehindEntryeuler.x = -1 * motorCycleController.transform.eulerAngles.x;
            }
            else if (StaticLocalEuler == StaticLocalEulerAngles.Z_AxisOnly)
            {
                PickFrmBehindEntryeuler.z = -1 * motorCycleController.transform.eulerAngles.z;
            }
            PickFrmBehindEntry.localEulerAngles = PickFrmBehindEntryeuler;
        }
        //
        if (RightEntery != null)
        {
            Transform vehicletransform = _vehicleType == VehicleType.Bike ? motorCycleController.transform : null;
            Vector3 rightentryeuler = RightEntery.localEulerAngles;
            if (StaticLocalEuler == StaticLocalEulerAngles.BothAxis)
            {
                rightentryeuler.x = -1 * vehicletransform.transform.eulerAngles.x;
                rightentryeuler.z = -1 * vehicletransform.transform.eulerAngles.z;
            }
            else if (StaticLocalEuler == StaticLocalEulerAngles.X_AxisOnly)
            {
                rightentryeuler.x = -1 * vehicletransform.transform.eulerAngles.x;
            }
            else if (StaticLocalEuler == StaticLocalEulerAngles.X_AxisOnly)
            {
                rightentryeuler.z = -1 * vehicletransform.transform.eulerAngles.z;
            }
            RightEntery.localEulerAngles = rightentryeuler;
        }
        if (LeftEntery != null)
        {
            Transform vehicletransform = _vehicleType == VehicleType.Bike ? motorCycleController.transform : null;
            Vector3 Leftentryeuler = LeftEntery.localEulerAngles;
            if (StaticLocalEuler == StaticLocalEulerAngles.BothAxis)
            {
                Leftentryeuler.x = -1 * vehicletransform.transform.eulerAngles.x;
                Leftentryeuler.z = -1 * vehicletransform.transform.eulerAngles.z;
            }
            else if (StaticLocalEuler == StaticLocalEulerAngles.X_AxisOnly)
            {
                Leftentryeuler.x = -1 * vehicletransform.transform.eulerAngles.x;
            }
            else if (StaticLocalEuler == StaticLocalEulerAngles.X_AxisOnly)
            {
                Leftentryeuler.z = -1 * vehicletransform.transform.eulerAngles.z;
            }
            LeftEntery.localEulerAngles = Leftentryeuler;
        }
        #endregion
    }
    //
    private void SetRiderIK()
    {
        if (Player == null) return;
        ESIkRider ikRider = Player.GetComponent<ESIkRider>();
        ikRider.LookPos = LookPos;
        ikRider.LeftFootIKTarget = LeftFootIkTarget;
        ikRider.RightFootIKTarget = RightFootIkTarget;
        ikRider.RightIKTarget = RightIkTarget;
        ikRider.LeftIkTarget = LeftIkTarget;
        ikRider.IKWeight = IKWeight;
        ikRider.BodyWeight = BodyWeight;
        ikRider.LookWeight = LookWeight;
        ikRider.Eyeweight = Eyeweight;
        ikRider.HeadWeight = HeadWeight;
        ikRider.ClampWeight = ClampWeight;
        ikRider._Exit_Vehicle = this;
        ikRider.MotorRigid = motorCycleController;
    }
    //
    private void RemoveIk()
    {
        if (Player == null) return;
        ESIkRider ikRider = Player.GetComponent<ESIkRider>();
        ikRider._Exit_Vehicle = null;
        ikRider.MotorRigid = null;
    }
    //
    private void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            if (!AIGrab)
            {
                // print("Player");
                InRange = true;
                Player = PlayerTransform;
            }
        }
    }
    private void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            if (callragdollz == false)
                InRange = false;
        }
    }
    private void VehicleGrabSystem()
    {
        if (Player == null) return;
        if (RightEntery == null && LeftEntery == null) return;

        if (InRange)
        {
            Vector3 Sides = transform.TransformDirection(Vector3.right);
            Vector3 playerpositon = Player.position - transform.position;
            sidedot = Vector3.Dot(Sides, playerpositon);
            //Updot
            Vector3 Upside = transform.TransformDirection(Vector3.up);
            Vector3 playerUppositon = Player.position - transform.position;
            Updot = Vector3.Dot(Upside, playerUppositon);
            Text GrabText = null;
            GrabText = Player.GetComponent<ES_HumanEnter_Exit_Vehicle>().GrabText;
            if (!SingleClick)
            {
                if (_vehicleType == VehicleType.Car)
                {
                    if (Mathf.Abs(Updot) < StepHeight)
                        if (GrabText != null)
                            GrabText.text = Player.CompareTag("Player") ? MountText : "";
                        else
                         if (GrabText != null)
                            GrabText.text = "";
                }
                else
                {
                    if (motorCycleController.LooseControl)
                    {
                        if (Mathf.Abs(sidedot) < StepHeight)
                            if (GrabText != null)
                                GrabText.text = Player.CompareTag("Player") ? pickUpText : "";
                            else
                             if (GrabText != null)
                                GrabText.text = "";
                    }
                    else
                    {
                        if (Mathf.Abs(Updot) < StepHeight)
                            if (GrabText != null)
                                GrabText.text = Player.CompareTag("Player") ? MountText : "";
                            else
                             if (GrabText != null)
                                GrabText.text = "";
                    }
                }
            }
            if (!SingleClick)
            {
                if (_vehicleType == VehicleType.Bike)
                {
                    if (motorCycleController.LooseControl == false)
                    {
                        if (Mathf.Abs(Updot) < StepHeight)
                        {
                            if (sidedot > 0)
                            {
                                GrabFromRight();
                            }
                            else if (sidedot < 0)
                            {
                                GrabFromLeft();
                            }
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(sidedot) < StepHeight)
                        {
                            if (Updot > 0)
                            {
                                PickUpBikeRight();
                            }
                            else if (Updot < 0)
                            {
                                PickUpBikeLeft();
                            }
                        }
                    }
                }
                else
                {
                    if (Mathf.Abs(Updot) < StepHeight)
                    {
                        if (sidedot > 0)
                        {
                            GrabFromRight();
                        }
                        else if (sidedot < 0)
                        {
                            GrabFromLeft();
                        }
                    }
                }
            }
            else
            {
                if (OnSit)
                {
                    if (GrabText != null)
                        GrabText.text = "";
                    UnmoutVehicle();
                }
            }
        }
        else
        {
            if (Player != null)
            {
                Text GrabText = null;
                GrabText = Player.GetComponent<ES_HumanEnter_Exit_Vehicle>().GrabText;
                SingleClick = false;
                if (GrabText != null)
                    GrabText.text = "";
            }
            if (Player.CompareTag("Player"))
            {
                if (!callragdollz && !pickfrmright && !pickfrmleft)
                {
                    Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = false;
                    Player = null;
                }
            }
        }
    }
    //
    private void OreintBodyPosForGrabing(Transform EntryTrans)
    {
        Player.transform.LookAt(EntryTrans.position);
        Vector3 PlayerEuler = Player.eulerAngles;
        PlayerEuler.x = 0; PlayerEuler.z = 0; Player.eulerAngles = PlayerEuler;
    }
    //
    private void PerformPickOperationOnBike()
    {
        if (_vehicleType == VehicleType.Car) return;
        if (attemptpick)
        {
            if (motorCycleController.LooseControl)
            {
                motorCycleController.MotorRb.isKinematic = true;
                Animator animator = Player.GetComponent<Animator>();
                animator.SetFloat("PickBikeEuler", Mathf.Abs(motorCycleController.eulerz));
                if (Mathf.Abs(motorCycleController.eulerz) > 3)
                {
                    if (motorCycleController.eulerz > 0)
                    {
                        Vector3 bikeeuler = transform.eulerAngles;
                        bikeeuler.z -= LiftSpeed * Time.deltaTime;
                        transform.eulerAngles = bikeeuler;
                    }
                    if (motorCycleController.eulerz < 0)
                    {
                        Vector3 bikeeuler = transform.eulerAngles;
                        bikeeuler.z += LiftSpeed * Time.deltaTime;
                        transform.eulerAngles = bikeeuler;
                    }
                }
                else
                {
                    animator.SetTrigger("Return");
                    animator.applyRootMotion = false;
                    motorCycleController.MotorRb.isKinematic = false;
                    pickfrmleft = false;
                    pickfrmright = false;
                    GrabbedRight = false;
                    GrabbedLeft = false;
                    Stop = false;
                    motorCycleController.LooseControl = false;
                    attemptpick = false;
                }
            }
        }
    }
    //
    private void MovePlayerToPickPosition()
    {
        if (Player == null) return;
        if (pickfrmleft || pickfrmright)
        {
            Transform EntryTrans = pickfrmleft ? PickFrmTopEntry : PickFrmBehindEntry;
            //CharacterController controller = Player.GetComponent<CharacterController>();
            //  animator.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
            Vector3 Sides = EntryTrans.TransformDirection(Vector3.right);
            Vector3 playerpositon = Player.position - EntryTrans.position;
            float mysidedist = Vector3.Dot(Sides, playerpositon);
            //            print(mysidedist);
            //
            Vector3 fwd = EntryTrans.TransformDirection(Vector3.forward);
            Vector3 playerfwdpositon = Player.position - EntryTrans.position;
            float myfwdentrysidedist = Vector3.Dot(fwd, playerpositon);
            if (attemptpick == false)
            {
                if (Mathf.Abs(mysidedist) < GrabBlendLevel && Mathf.Abs(myfwdentrysidedist) < GrabBlendLevel)
                {
                    //
                    if (!attemptpick)
                    {
                        OreintBodyPosForGrabing(EntryTrans);
                        Animator animator = Player.GetComponent<Animator>();
                        animator.applyRootMotion = true;

                        Player.transform.parent = this.transform;

                        Vector3 Localplayer = pickfrmleft ? PickFrmTopEntry.localPosition : PickFrmBehindEntry.localPosition;
                        Localplayer.y = Player.localPosition.y;
                        Player.localPosition = Localplayer;
                        Player.transform.localRotation = pickfrmleft ? PickFrmTopEntry.localRotation : PickFrmBehindEntry.localRotation;

                        Player.parent = null;
                        //
                        string TriggerName = pickfrmleft && motorCycleController.eulerz > 0 ? PickUpFromTopTrigger :
                         pickfrmright && motorCycleController.eulerz > 0 ? PickUpFromBehindTrigger :
                         pickfrmleft && motorCycleController.eulerz < 0 ? PickUpFromTopTrigger : pickfrmright
                         && motorCycleController.eulerz < 0 ? PickUpFromBehindTrigger : "Pussy";
                        //
                        // print(TriggerName);
                        animator.SetTrigger(TriggerName);
                        attemptpick = true;
                    }
                }
                else
                {
                    OreintBodyPosForGrabing(EntryTrans);
                    Animator animator = Player.GetComponent<Animator>();
                    animator.applyRootMotion = true;
                    animator.SetFloat("Forward", GrabSpeed, 0.1f, Time.deltaTime);
                }
            }
        }
    }
    //
    //
    private void MovePlayerToGrabPosition()
    {
        if (Player == null || LeftEntery == null || RightEntery == null) return;
        if (pickfrmleft || pickfrmright) return;
        if (Umount) return;
        Transform EntryTrans = GrabbedLeft ? LeftEntery : RightEntery;
        Vector3 Sides = EntryTrans.TransformDirection(Vector3.right);
        Vector3 playerpositon = Player.position - EntryTrans.position;
        entrysidedist = Vector3.Dot(Sides, playerpositon);
        //
        Vector3 fwd = EntryTrans.TransformDirection(Vector3.forward);
        Vector3 playerfwdpositon = Player.position - EntryTrans.position;
        fwdentrysidedist = Vector3.Dot(fwd, playerpositon);
        if (lerptositpoint) return; if (!movetopos) return;
        //CheckForWithDraw(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxis("Jump"));
        if (Stop)
        {
            GrabbedRight = false;
            GrabbedLeft = false;
            MountText = "V To Grab Vehicle";
            pickUpText = "V To Pick Vehicle";
            movetopos = false;
            Player.GetComponent<Animator>().applyRootMotion = false;
            Stop = false;
            return;
        }

        if (Mathf.Abs(entrysidedist) < GrabBlendLevel && Mathf.Abs(fwdentrysidedist) < GrabBlendLevel)
        {
            lerptositpoint = true;
            Player.GetComponent<ES_HumanEnter_Exit_Vehicle>().grabVehicle = this;
            if (movetopos)
            {
                OreintBodyPosForGrabing(EntryTrans);
                Animator animator = Player.GetComponent<Animator>();
                animator.applyRootMotion = false;
                Player.GetComponent<CharacterController>().enabled = false;
                Player.GetComponent<ES_UniversalCharacterController>().enabled = false;
                Player.transform.parent = this.transform;
                Vector3 Localplayer = GrabbedLeft ? LeftEntery.localPosition : RightEntery.localPosition;
                Localplayer.y = Player.localPosition.y;
                Player.localPosition = Localplayer;
                Player.transform.localRotation = GrabbedLeft ? LeftEntery.localRotation : RightEntery.localRotation;
                animator.applyRootMotion = true;
                if (GrabbedLeft)
                {
                    animator.Play(EnterVehicle, 0, 0);
                }
                if (GrabbedRight)
                {
                    animator.Play(EnterVehicleMirror, 0, 0);
                }
                Player.GetComponent<ESIkRider>().firstleg = true;
                //animator.applyRootMotion = true;
                if (!AIGrab)
                {
                    StartCameraRender();
                    Player.GetComponent<ES_UniversalCharacterController>().StopCameraRender();
                }
                movetopos = false;
            }
        }
        else
        {
            Animator animator = Player.GetComponent<Animator>();
            animator.applyRootMotion = true;
            //CharacterController controller = Player.GetComponent<CharacterController>();
            animator.SetFloat("Forward", GrabSpeed, 0.1f, Time.deltaTime);
            OreintBodyPosForGrabing(EntryTrans);
        }
        //
    }
    //
    private void CheckForWithDraw(float Hmove, float Vmove, float Jump)
    {
        if (lerptositpoint) return;
        if (attemptpick) return;
        if (Mathf.Abs(Hmove) > 0 || Mathf.Abs(Vmove) > 0 || Jump > 0)
        {
            Stop = true;
            SingleClick = false;
        }
    }
    //

    public void CheckForBodyBalancing()
    {
        //print("work");
        if (Player == null) return;
        Animator animator = Player.GetComponent<Animator>();
        Transform HipTranform = animator.GetBoneTransform(HumanBodyBones.Hips);
        Vector3 Sides = transform.TransformDirection(Vector3.right);
        Vector3 bodypositon = HipTranform.position - SitPoint.position;
        //hipsidedist = Mathf.Abs(Vector3.Dot(SitPoint.position, bodypositon));
        if (lerptositpoint)
        {
            string Myname = GrabbedLeft ? EnterVehicle : EnterVehicleMirror;
            //string Myname = "EnterBike 0";
            //print(Myname);
            hipsidedist = animator.GetCurrentAnimatorStateInfo(0).IsName(Myname) ? animator.GetCurrentAnimatorStateInfo(0).normalizedTime : 0.0f;
            if (hipsidedist > SitBlendLevel)
            {

                OnSit = true;
                if (Player.tag == "Player")
                {
                    VehicleState = "OwnedByPlayer";
                }
                else
                {
                    VehicleState = "OwnedByAI";
                }
                //GrabbedRight = false;
                //GrabbedLeft = false;
                if (_vehicleType == VehicleType.Bike)
                {
                    if (VehicleState == "OwnedByAI")
                    {
                        motorCycleController.GetControllerType = UL_MotorCycleController.ControllerType.AI;
                        motorCycleControl.enabled = false;
                    }
                    else if (VehicleState == "OwnedByPlayer")
                    {
                        motorCycleControl.enabled = true;
                        motorCycleController.GetControllerType = UL_MotorCycleController.ControllerType.PlayerCotrol;
                    }
                }
                else
                {
                    //remeber to code for car here

                }
            }
        }
    }
    //
    private void PickUpBikeLeft()
    {
        if (lerptositpoint) return;
        if (pickfrmleft) return;
        if (Player.GetComponent<ESRagDollHelper>().FreezeControl) return;
        if (PickFrmTopEntry == false) return;
        if (!AIGrab)
        {
            if (Input.GetKeyDown(GrabKey))
            {
                Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                pickfrmleft = true;
                SingleClick = false;
            }
        }
        else
        {
            //
            if (OwnedByAI)
            {
                if (!pickfrmleft && !OnSit)
                {
                    if (AiTransform != null)
                    {
                        Player = AiTransform;
                        if (Player.tag == "AIRider")
                        {
                            Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                            pickfrmleft = true;
                            SingleClick = false;
                        }
                    }
                }
            }
        }
    }
    //
    private void PickUpBikeRight()
    {
        if (lerptositpoint) return;
        if (pickfrmright) return;
        if (Player.GetComponent<ESRagDollHelper>().FreezeControl) return;
        if (PickFrmBehindEntry == null) return;
        if (!AIGrab)
        {
            if (Input.GetKeyDown(GrabKey))
            {
                Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                pickfrmright = true;
                SingleClick = false;
            }
        }
        else
        {
            //AI Way
            if (OwnedByAI)
            {
                if (!pickfrmright && !OnSit)
                {
                    if (AiTransform != null)
                    {
                        Player = AiTransform;
                        if (Player.tag == "AIRider")
                        {
                            Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                            pickfrmright = true;
                            SingleClick = false;
                        }
                    }
                }
            }
        }
    }
    //
    //
    private void UnmoutVehicle()
    {
        if (BikeBackardZ < 0) return;
        if (AIGrab) return;
        if (Input.GetKeyDown(GrabKey))
        {
            if (!AIGrab)
            {
                StopCameraRender();
                Player.GetComponent<ES_UniversalCharacterController>().StartCameraRender();
            }
            lerptositpoint = false;
            InverseCount = Random.Range(0, 2);

            Animator animator = Player.GetComponent<Animator>();
            animator.applyRootMotion = true;
            string Exit = motorCycleController.MotorRb.velocity.sqrMagnitude > 0.1f ? AgressiveExit : ExitVehile;
            if (Exit == AgressiveExit)
            {
                callragdollz = true;
                motorCycleController.LooseControl = true;
            }
            if (_vehicleType == VehicleType.Bike)
            {
                motorCycleControl.enabled = false;
            }
            else
            {

            }
            if (ApplyMirrorToExit)
            {
                if (Exit == ExitVehile)
                {
                    if (InverseCount == 0)
                    {
                        animator.SetBool("InverseExit", true);
                    }
                    else
                    {
                        animator.SetBool("InverseExit", false);
                    }
                }
            }
            animator.Play(Exit, 0, 0);
            Player.parent = null;
            Vector3 euler = Player.eulerAngles;
            euler.x = this.transform.eulerAngles.x;
            euler.z = this.transform.eulerAngles.z;
            Player.eulerAngles = euler;
            Umount = true;
            OnSit = false;
        }
    }
    //
    private void JackVehicleFromOwned()
    {
        /*
       will be implemented in next update 
        if (!OnSit) return;
        */
    }
    //
    void CorrectCharacterControllerCoordinate(Vector3 JumpPos, Transform BodyHipBone)
    {
        BodyHipBone.position -= JumpPos;
        Player.position += JumpPos;
    }
    private void SetRiderForRagDollz()
    {
        if (Player == null) return;
        if (!callragdollz) return;
        if (Umount)
        {
            ESRagDollHelper ragDollHelper = Player.GetComponent<ESRagDollHelper>();
            Animator animator = Player.GetComponent<Animator>();
            string Myname = AgressiveExit;
            exitnormalizedTime = animator.GetCurrentAnimatorStateInfo(0).IsName(Myname) ? animator.GetCurrentAnimatorStateInfo(0).normalizedTime : 0.0f;
            if (exitnormalizedTime > 0.8f)
            {
                InRange = false;
                ragDollHelper.Ragdollz = true;
                //startragdollcountdown = true;
                if (_vehicleType == VehicleType.Bike)
                {
                    animator.applyRootMotion = false;
                    motorCycleController.Accel = 0;
                    motorCycleController.shoebrake = 0;
                    motorCycleController.Lerp = 0;
                }
                ESIkRider ikRider = Player.GetComponent<ESIkRider>();
                ikRider.MotorRigid = null;
                ikRider._Exit_Vehicle = null;
                GrabbedLeft = false;
                GrabbedRight = false;
                Stop = false;
                movetopos = false;
                Vector3 euler = Player.eulerAngles;
                euler.x = 0;
                euler.z = 0;
                Player.eulerAngles = euler;
                animator.applyRootMotion = false;
                SingleClick = false;
                Player.GetComponent<ES_UniversalCharacterController>().enabled = true;
                //Player.GetComponent<CharacterController>().enabled = true;
                Umount = false;
                //callragdollz = false;
            }
        }
    }
    //
    private void PrepareRiderOffVehicle()
    {
        if (Player == null) return;
        if (!Umount) return;
        if (callragdollz) return;
        Animator animator = Player.GetComponent<Animator>();
        string Myname = ExitVehile;

        normalizedTime = animator.GetCurrentAnimatorStateInfo(0).IsName(Myname) ? animator.GetCurrentAnimatorStateInfo(0).normalizedTime : 0.0f;

        if (normalizedTime > 1f)
        {
            Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = false;
            animator.applyRootMotion = false;
            if (_vehicleType == VehicleType.Bike)
            {
                motorCycleControl.enabled = false;
                motorCycleController.Accel = 0;
                motorCycleController.shoebrake = 0;
                motorCycleController.Lerp = 0;
            }
            else
            {
                //code  me some vehicle script
            }
            ESIkRider ikRider = Player.GetComponent<ESIkRider>();
            ikRider.MotorRigid = null;
            ikRider._Exit_Vehicle = null;
            GrabbedLeft = false;
            GrabbedRight = false;
            Stop = false;
            InRange = false;
            movetopos = false;
            animator.applyRootMotion = false;
            Vector3 euler = Player.eulerAngles;
            euler.x = 0;
            euler.z = 0;
            Player.eulerAngles = euler;
            reset = true;
            SingleClick = false;
            Player.GetComponent<ES_UniversalCharacterController>().enabled = true;
            Player.GetComponent<CharacterController>().enabled = true;
            Umount = false;
        }
        //
    }
    //
    private void GrabFromLeft()
    {
        if (movetopos) return;
        if (GrabbedLeft) return;
        if (lerptositpoint) return;
        if (Player.GetComponent<ESRagDollHelper>().FreezeControl) return;
        if (LeftEntery == null) return;
        if (Stop) return;
        if (!AIGrab)
        {

            if (Input.GetKeyDown(GrabKey))
            {
                //print("en");
                Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                GrabbedLeft = true;
                GrabbedRight = false;
                movetopos = true;
                Vector3 euler = transform.eulerAngles;
                euler.x = 0;
                euler.z = 0;
                transform.eulerAngles = euler;
                SingleClick = true;
            }
        }
        else
        {
            //AI Way
            if (OwnedByAI)
            {
                if (!GrabbedLeft && !OnSit)
                {
                    if (AiTransform != null)
                    {
                        Player = AiTransform;
                        if (Player.tag == "AIRider")
                        {
                            Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                            GrabbedLeft = true;
                            GrabbedRight = false;
                            movetopos = true;
                            Vector3 euler = transform.eulerAngles;
                            euler.x = 0;
                            euler.z = 0;
                            transform.eulerAngles = euler;
                            SingleClick = true;
                        }
                    }
                }
            }
        }
    }
    //
    private void GrabFromRight()
    {
        if (movetopos) return;
        if (GrabbedRight) return;
        if (lerptositpoint) return;
        if (Player.GetComponent<ESRagDollHelper>().FreezeControl) return;
        if (RightEntery == null) return;
        if (Stop) return;
        //controls for user
        if (!AIGrab)
        {
            if (Input.GetKeyDown(GrabKey))
            {
                //print("en");
                Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                GrabbedRight = true;
                GrabbedLeft = false;
                movetopos = true;
                Vector3 euler = transform.eulerAngles;
                euler.x = 0;
                euler.z = 0;
                transform.eulerAngles = euler;
                SingleClick = true;
            }
        }
        else
        {
            //AI way
            if (OwnedByAI)
            {
                if (!GrabbedRight && !OnSit)
                {
                    if (AiTransform != null)
                    {
                        Player = AiTransform;
                        if (Player.tag == "AIRider")
                        {
                            Player.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
                            GrabbedRight = true;
                            GrabbedLeft = false;
                            movetopos = true;
                            Vector3 euler = transform.eulerAngles;
                            euler.x = 0;
                            euler.z = 0;
                            transform.eulerAngles = euler;
                            SingleClick = true;
                        }
                    }
                }
            }
        }
    }
    #endregion
}
