using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
[RequireComponent(typeof(ESCheckForCrashState))]
[BurstCompile]
public class ESVehicleAI : MonoBehaviour
{
    public enum TargetUpdateMethod
    {
        ByLateUpdate,
        ByUpdateOnly,
        ByFixedOnly
    }
    //

    [Header("Version 2.0.0F")]
    public TargetUpdateMethod GetTargetUpdateMethod = TargetUpdateMethod.ByLateUpdate;

    public Transform TargetNode;
    [Header("WheelSettings")]
    public WheelCollider[] frontwheel;
    public WheelCollider[] rearwheel;
    public Transform[] frontwheelmeshes;
    public Transform[] rearwheelmeshes;
    //
    [Header("EngineSettings")]
    public float EngineTorque;
    public float BrakeDrag = 10f;
    //
    //
    [Header("AI Settings")]
    public Vector3 CenterOfMass = Vector3.zero;
    public float SteerBalanceFactor = 0.5f;
    public float SharpAngleSpeed = 11;
    public float topspeed = 50.0f;
    public float SharpBendSpeed = 30f;
    public float SpawnDistance = 400f;
    [HideInInspector] public Transform player;
    //
    [Header("AISensorSettings")]
    [Tooltip("How far Ai Veiw Incoming Vehicles while avoiding")]
    public float DangerZone = 50f;
    public float sidesensor = 10f;
    public float fowardsensor = 5.5f;
    [Tooltip("wont make attempt to avoid")]
    public bool IgnoreOverTake = true;
    [Tooltip("EnableAvoidance")]
    public bool UseSmartAvoidMethod = true;
    public float OvertakeMultiplier = 200f, Rspeed = 1.5f;
    public bool DisableDetection = true;
    [Range(2, 10)]
    public float Rdistance = 7f;
    [Range(0.15f, 2f)]
    public float Ai_VehicleWidth = .25f, Ai_VehicleOverTakeWidth = 0.71f;
    [Range(10f, 30)]
    public float Ai_OvertakeSteerSensitivity = 15f, AvoidDistance = 15f;

    public float overtakedistance = 15f;
    [Range(0, 15)] public float RealseBrakeDistance = 15f;
    //public bool Box2Isavoiding = false;
    //
    [Header("PathSettings")]
    public float DistanceApart = 10f;
    //
    [Header("Debug")]
    public Rigidbody CarRb;
    [Tooltip("ReadOnly")] public float currentspeed;
    [Tooltip("ReadOnly")] public float Brakemul;
    //[Tooltip("ReadOnly")] public bool IsBraking;
    [Tooltip("ReadOnly")] public float backuptopspeed;


    [HideInInspector]
    public bool returnmotor;
    [HideInInspector]

    public float RoadWidth = 4f;
    [HideInInspector]
    public bool Stop;
    [HideInInspector]
    public bool DisableSmartAI = false;
    public bool callsensor = false;
    [HideInInspector]
    public ESTrafficLghtCtrl trafficlightctrl;
    [HideInInspector]
    public Transform TriggerObject, prevouisOvertakedObject;
    [HideInInspector]
    public float TriggerDistance;
    [HideInInspector]
    public float backupdistapart;
    [HideInInspector]
    public bool AngleBraking = false, CopySpeed;
    [HideInInspector]
    public bool Trafficlightbraking = false;
    [HideInInspector]
    public bool returntriggerstay = false, triggerobjectbraking = false, optimizerandom = true, Reverse, Hasreversed, dontalterspeed;
    [HideInInspector]
    public float checkfirstdist;
    [HideInInspector] public Vector3 relativevec;
    [HideInInspector] public float newsteer;
    [HideInInspector]
    public float fwddot, side, playerdist, steersign, steerang;
    [HideInInspector]
    public bool EnableAvoid = true, alignwithroad, manuever;
    [HideInInspector] public bool isovertaking, IndividualTracker = false;
    private float OldRot;
    [HideInInspector]
    public Vector3 Tar, RelativePoint;
    //
    public List<Collider> _collider = new List<Collider>();
    public float Speedmodifier = 100, backdrag, backengine;
    [HideInInspector] public float turnspeed;
    float Vsign, updatedistback;
    GameObject TargetHolder;
    bool Tooclose;
    //
    private void Awake()
    {
        // runs before game starts
        EnableAvoid = false;
        backuptopspeed = topspeed;
        Speedmodifier = 100f;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }
    //
    private void Start()
    {
        //runs as soon as game starts
        updatedistback = DistanceApart;
        steersign = 0;
        _collider = new List<Collider>();
        backuptopspeed = topspeed;
        CarRb = this.GetComponent<Rigidbody>();
        backdrag = CarRb.angularDrag;
        CarRb.centerOfMass += CenterOfMass;
        backengine = EngineTorque;
        //IsBraking = false;
        backupdistapart = DistanceApart;
        CarRb.solverIterations = 10;
    }
    // Update is called once per frame
    //
    private void VehicleUpdate()
    {
        /*
            this contains all functions that moves the AI vehicle
        */
        Motor();
        SteerBalance();
        ApplySteer();
        Behaviour();
        WheelAlignment();
        RegulateAI();
    }
    //
    private void Update()
    {
        //
        if (GetTargetUpdateMethod == TargetUpdateMethod.ByLateUpdate) return;
        CheckDist();
        UpdateIndividually();
        VehicleUpdate();
    }
    //
    private void LateUpdate()
    {
        //
        if (GetTargetUpdateMethod == TargetUpdateMethod.ByUpdateOnly) return;
        CheckDist();
        UpdateIndividually();
        VehicleUpdate();
    }
    //
    private void FixedUpdate()
    {
        if (GetTargetUpdateMethod == TargetUpdateMethod.ByLateUpdate || GetTargetUpdateMethod == TargetUpdateMethod.ByUpdateOnly) return;
        CheckDist();
        UpdateIndividually();
        VehicleUpdate();
    }
    //
    private void Motor()
    {
        //if (IsBraking) return;
        if (returnmotor)
        {
            //if true the stop every force applied to wheel
            for (int i = 0; i < frontwheel.Length; ++i)
            {
                frontwheel[i].motorTorque = 0;
                frontwheel[i].brakeTorque = float.MaxValue;
                CarRb.drag = BrakeDrag;
                //

            }
            for (int i = 0; i < rearwheel.Length; ++i)
            {
                rearwheel[i].motorTorque = 0;
                rearwheel[i].brakeTorque = float.MaxValue;
                //
                CarRb.drag = BrakeDrag;
            }

        }
        else
        {
            //if returned false then add force to all wheeels
            for (int i = 0; i < frontwheel.Length; ++i)
            {
                frontwheel[i].motorTorque = !Reverse ? EngineTorque : -1000;
                frontwheel[i].brakeTorque = 0;
                //
                CarRb.drag = 0.5f;
                WheelHit wh = new WheelHit();
                frontwheel[i].GetGroundHit(out wh);
                if (Mathf.Abs(wh.forwardSlip) >= 1.89f)
                {
                    //if foward slip is greater than 1.89f then reduce torque to prevent traction as possible  
                    if (EngineTorque > 100)
                        EngineTorque -= 10 * 0.5f;
                }
                else
                {
                    if (EngineTorque < backengine)
                    {
                        EngineTorque += 10 * .5f;
                        //  vehiclecontroller.forceAppliedToWheels = currentengineforce;
                    }
                }

            }
            for (int i = 0; i < rearwheel.Length; ++i)
            {
                rearwheel[i].motorTorque = !Reverse ? EngineTorque : -1000;
                //
                rearwheel[i].brakeTorque = 0;
                CarRb.drag = 0.5f;
                WheelHit wh = new WheelHit();
                rearwheel[i].GetGroundHit(out wh);
                if (Mathf.Abs(wh.forwardSlip) >= .89f)
                {
                    if (EngineTorque > 100)
                        EngineTorque -= 10 * 0.5f;
                }
                else
                {
                    if (EngineTorque < backengine)
                    {
                        EngineTorque += 10 * .5f;
                        //  vehiclecontroller.forceAppliedToWheels = currentengineforce;
                    }
                }

            }
        }

    }
    //
    public void SteerBalance()
    {
        //controls the steering of the AI car
        if (Mathf.Abs(newsteer) < 15) return;
        for (int i = 0; i < frontwheel.Length; i++)
        {
            WheelHit wheelhit;
            frontwheel[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return;
        }
        for (int i = 0; i < rearwheel.Length; i++)
        {
            WheelHit wheelhit;
            rearwheel[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return;
        }
        if (Mathf.Abs(OldRot - transform.eulerAngles.y) < 10f)
        {
            var alignturn = (transform.eulerAngles.y - OldRot) * SteerBalanceFactor;
            Quaternion angvelocity = Quaternion.AngleAxis(alignturn, Vector3.up);
            CarRb.velocity = angvelocity * CarRb.velocity;
        }
        OldRot = transform.eulerAngles.y;
    }
    //
    private void ApplySteer()
    {
        // here the steering functions are applied
        LerpToSteerAngle();
        relativevec = transform.InverseTransformPoint(Tar);
    }
    //

    private void UpdateIndividually()
    {
        //call when you choose not to update position burst compiler
        // if (ActivatePoliceTrack) return;
        if (!IndividualTracker) return;
        if (TargetNode != null)
        {
            checkfirstdist = Vector3.Distance(this.transform.position, TargetNode.position);
            if (checkfirstdist < DistanceApart)
            {
                if (TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count == 0)
                {
                    if (TargetNode.GetComponent<ESNodeManager>().NextNode != null)
                    {
                        TargetNode = TargetNode.GetComponent<ESNodeManager>().NextNode;
                    }
                    else
                    {
                        CancelInvoke();
                        this.gameObject.SetActive(false);
                    }
                    optimizerandom = true;
                }
                else
                {
                    if (TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count > 0)
                    {
                        int pathindex = 0;
                        pathindex = Random.Range(0, TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count);
                        optimizerandom = false;
                        TargetNode = TargetNode.GetComponent<ESNodeManager>().ConnectedNode[pathindex];
                    }
                }
            }
        }
    }
    //
    private void CheckDist()
    {
        //if (EnableAvoid) return;
        //if (ActivatePoliceTrack) return;
        //updates the nodes of the AI car
        if (IndividualTracker) return;
        if (TargetNode != null)
        {
            if (checkfirstdist < DistanceApart)
            {
                if (TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count == 0)
                {
                    if (TargetNode.GetComponent<ESNodeManager>().NextNode != null)
                    {
                        TargetNode = TargetNode.GetComponent<ESNodeManager>().NextNode;
                    }
                    else
                    {
                        CancelInvoke();
                        this.gameObject.SetActive(false);
                    }
                    optimizerandom = true;
                }
                else
                {
                    if (TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count > 0)
                    {
                        int pathindex = 0;
                        pathindex = Random.Range(0, TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count);
                        optimizerandom = false;
                        TargetNode = TargetNode.GetComponent<ESNodeManager>().ConnectedNode[pathindex];
                    }
                }
            }
        }
    }
    //
    //
    private void LerpToSteerAngle()
    {
        Avoid();
        for (int i = 0; i < frontwheel.Length; i++)
        {
            //steerang = alignwithroad == false ? newsteer : 0;
            frontwheel[i].steerAngle = steerang;
        }
    }
    //
    private void RegulateAI()
    {
        if (player == null) return;

        if (playerdist > SpawnDistance)
        {
            CancelInvoke();
            this.gameObject.SetActive(false);
        }
    }
    //
    IEnumerator ReturnAlign()
    {
        yield return new WaitForSeconds(5.9f);
        //alignwithroad = false;
    }
    //
    void overtake()
    {
        //TriggerObject = null;
        //Speedmodifier = 1;
        /*
        controls the overtake behavoiur of the AI car disable  this and you prevent overtake 
        */
        if (IgnoreOverTake) return;
        Speedmodifier = OvertakeMultiplier;
        Tar = TargetNode.position;
        RelativePoint = transform.InverseTransformPoint(TargetHolder.transform.position);
        float distobst = Vector3.Distance(TargetHolder.transform.position, transform.position);
        //
        //check if too close too close to an obstacle
        if (distobst < AvoidDistance && Tooclose == false)
        {
            CarRb.detectCollisions = !DisableDetection;
            DistanceApart = 10f;
            if (Mathf.Abs(relativevec.x) < Ai_VehicleOverTakeWidth)
            {
                if (Mathf.Abs(relativevec.x) < .12f)
                    steerang = Vsign * Ai_OvertakeSteerSensitivity;
                else
                    steerang = Vsign * .15f;
                //steerang = newsteer;
                // Debug.Log("Manuer");
            }
            else
            {
                Tar = TargetNode.position;
                steerang = newsteer;
            }
        }
        else
        {
            //  if too close withdraaw overtake
            isovertaking = false;
            TargetHolder = null;
            DistanceApart = updatedistback;
            CarRb.detectCollisions = true;
        }
    }
    //
    void Manuever()
    {
        //helps AI car detect player object and try to avoid it
        TriggerObject = null;
        Speedmodifier = 1;
        Tar = TargetNode.position;
        RelativePoint = transform.InverseTransformPoint(TargetHolder.transform.position);
        float distobst = Vector3.Distance(TargetHolder.transform.position, transform.position);
        //check if reversed too place 
        if (!Hasreversed)
            if (distobst < Rdistance)
            {
                Reverse = true;
            }
            else
            {
                Reverse = false;
                if (currentspeed < Rspeed)
                {
                    Hasreversed = true;
                }
            }
        //
        if (Hasreversed)
        {
            //runs if already reversed
            Vector3 swd = transform.TransformDirection(Vector3.right);
            Vector3 playersidepositon = TargetHolder.transform.position - transform.position;
            side = Vector3.Dot(swd, playersidepositon);
            if (steersign == 0)
            {
                if (side < 0)
                {
                    steersign = 1;
                }
                else
                {
                    steersign = -1;
                }
            }
        }
        if (Hasreversed)
            if (distobst < AvoidDistance)
            {
                CopySpeed = false;
                CarRb.detectCollisions = !DisableDetection;
                if (Mathf.Abs(relativevec.x) < Ai_VehicleWidth)
                {
                    if (Mathf.Abs(relativevec.x) < .1f)
                        steerang = steersign * 15f;
                    else
                        steerang = steersign * .67f;
                    //steerang = newsteer;
                    // Debug.Log("Manuer");
                }
                else
                {
                    Speedmodifier = 1;
                    Tar = TargetNode.position;
                    steerang = newsteer;
                }
            }
            else
            {
                steersign = 0;
                manuever = false;
                CopySpeed = false;
                CarRb.detectCollisions = true;
                TargetHolder = null;
                Hasreversed = false;
            }
        else
        {
            if (distobst > AvoidDistance)
            {
                Speedmodifier = 1;
                CopySpeed = false;
                Tar = TargetNode.position;
                steerang = newsteer;
                manuever = false;
                CarRb.detectCollisions = true;
                TargetHolder = null;
            }
        }
    }
    private void Avoid()
    {
        //enables AI Car avoid 
        if (_collider.Count > 0) return;
        if (UseSmartAvoidMethod == false) return;
        //start
        if (!manuever && !isovertaking)
        {
            Speedmodifier = 1;
            Tar = TargetNode.position;
            steerang = newsteer;
        }
        //
        if (TriggerObject != null)
            TargetHolder = TriggerObject.gameObject;

        if (TargetHolder != null)
        {
            if (TargetHolder.CompareTag("Player"))
            {
                manuever = true;
            }
            //
            if (TargetHolder.transform.root.name == "AI" && !IgnoreOverTake)
            {
                if (TargetNode.GetComponent<ESNodeManager>() != null)
                    if (TargetNode.GetComponent<ESNodeManager>().OverTakeType == "None")
                        isovertaking = false;
                    else
                        isovertaking = true;
            }
        }
        //
        if (manuever)
        {
            Manuever();
        }
        //
        if (isovertaking)
        {
            overtake();
        }
    }
    //
    private void Behaviour()
    {
        if (DisableSmartAI) return;
        if (trafficlightctrl == null)
        {
            if (triggerobjectbraking)
            {
                returnmotor = true;
            }
            else
            {
                returnmotor = false;
            }
            //

            //
            if (TriggerObject != null)
            {
                if (!TriggerObject.gameObject.activeSelf)
                {
                    CopySpeed = false;
                    EnableAvoid = false;
                    triggerobjectbraking = false;
                    TriggerObject = null;
                }
            }
            else
            {
                //Hasreversed = false;
                triggerobjectbraking = false;
                dontalterspeed = false;
            }
        }
        else if (trafficlightctrl != null)
        {
            if (trafficlightctrl.red)
            {
                returnmotor = true;
            }
            else
            {
                if (triggerobjectbraking)
                {

                    returnmotor = true;
                }
                else
                {
                    returnmotor = false;
                }

            }
        }
        //
        if (TriggerObject != null)
        {
            float BrakeDist = TriggerObject.GetComponent<Rigidbody>() != null ? RealseBrakeDistance : 5f;
            float brakedistance = EnableAvoid == false ? RealseBrakeDistance : (overtakedistance + 5f);
            if (TriggerDistance > brakedistance)
            {
                triggerobjectbraking = false;
                returntriggerstay = false;
                CopySpeed = false;
                EnableAvoid = false;
                TriggerObject = null;
            }
            if (TriggerObject != null)
            {
                if (!TriggerObject.gameObject.activeSelf)
                {
                    triggerobjectbraking = false;
                    returntriggerstay = false;
                    CopySpeed = false;
                    EnableAvoid = false;
                    TriggerObject = null;
                }
            }
        }
        //
        if (!CopySpeed)
        {
            if (alignwithroad == false)
            {
                if (Mathf.Abs(newsteer) > SharpAngleSpeed)
                {
                    if (dontalterspeed == false)
                        topspeed = SharpBendSpeed;
                }
                else
                {
                    topspeed = backuptopspeed;
                }
            }
        }
        else
        {
            if (TriggerObject != null)
            {
                if (TriggerObject.GetComponent<Rigidbody>() != null)
                {
                    float Pi = Mathf.PI * 1.15f;
                    float CurrentSpeed = TriggerObject.GetComponent<Rigidbody>().velocity.magnitude * Pi;
                    if (currentspeed > 10f)
                    {
                        //float f = new float();
                        //returntriggerstay = true;
                        topspeed = !EnableAvoid ? CurrentSpeed * 0.5f : Speedmodifier;
                        if (TargetNode.GetComponent<ESNodeManager>() != null)
                            if (TargetNode.GetComponent<ESNodeManager>().OverTakeType == "Right")
                            {
                                bool DontOverTake = new bool();
                                if (TriggerObject.GetComponent<ESVehicleAI>() != null)
                                {
                                    DontOverTake = TriggerObject.GetComponent<ESVehicleAI>().IgnoreOverTake;
                                }
                                if (checkfirstdist > 10 && TargetNode.GetComponent<ESNodeManager>().NextNode != null
                                && !DontOverTake)
                                {
                                    if (_collider.Count == 0)
                                    {
                                        EnableAvoid = true;

                                        Vsign = RoadWidth;
                                    }
                                }

                            }
                            else if (TargetNode.GetComponent<ESNodeManager>().OverTakeType == "Left")
                            {
                                bool DontOverTake = new bool();
                                if (TriggerObject.GetComponent<ESVehicleAI>() != null)
                                {
                                    DontOverTake = TriggerObject.GetComponent<ESVehicleAI>().IgnoreOverTake;
                                }
                                if (checkfirstdist > 10 && TargetNode.GetComponent<ESNodeManager>().NextNode != null
                                 && !DontOverTake)
                                {
                                    if (_collider.Count == 0)
                                    {
                                        EnableAvoid = true;

                                        Vsign = -RoadWidth;
                                    }
                                }
                            }
                            else
                            {
                                EnableAvoid = false;
                            }

                        //topspeed = f;
                    }
                    else
                    {
                        triggerobjectbraking = true;
                    }
                }
            }
        }

    }
    //
    public void WheelAlignment()
    {
        // make tyre meshes follow wheels;
        if (currentspeed < 0.09f) return;
        // align front wheel meshes
        Vector3 frontwheelposition;
        Quaternion frontwheelrotation;
        if (frontwheelmeshes.Length > 0)
        {
            for (int i = 0; i < frontwheel.Length; i++)
            {
                if (frontwheelmeshes[i] == null)
                {
                    return;
                }
                frontwheel[i].GetWorldPose(out frontwheelposition, out frontwheelrotation);
                frontwheelmeshes[i].transform.position = frontwheelposition;
                frontwheelmeshes[i].transform.rotation = frontwheelrotation;
            }
        }
        // align rear wheel meshes
        Vector3 rearwheelposition;
        Quaternion rearwheelrotation;
        if (rearwheelmeshes.Length > 0)
        {
            for (int i = 0; i < rearwheel.Length; i++)
            {
                if (rearwheelmeshes[i] == null)
                {
                    return;
                }
                rearwheel[i].GetWorldPose(out rearwheelposition, out rearwheelrotation);
                rearwheelmeshes[i].transform.position = rearwheelposition;
                rearwheelmeshes[i].transform.rotation = rearwheelrotation;
                // Rpm = m_wheelsettings.frontwheels.frontwheelcols[i].rpm;
            }
        }
    }
    void CheckForbjectIncoming()
    {
        _collider = new List<Collider>();
        Collider[] hits = Physics.OverlapSphere(this.transform.position, DangerZone);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; ++i)
            {
                if (hits[i].attachedRigidbody != null)
                {
                    if (hits[i].attachedRigidbody.transform != this.transform)
                    {
                        if (hits[i].attachedRigidbody.transform.root.name == "AI")
                            if (TriggerObject != null)
                            {
                                if (hits[i].attachedRigidbody.transform != TriggerObject.transform)
                                {
                                    Vector3 fwd = transform.forward;
                                    Vector3 Aipositon = hits[i].transform.position - transform.position;
                                    float hitfwd = Vector3.Dot(fwd, Aipositon);
                                    if (hitfwd > 0)
                                        _collider.Add(hits[i]);
                                }
                            }
                    }
                }
            }
        }
    }
    // 
    //
    void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("TrafficLight"))
        {
            //trafficlight ahead
            if (Other.transform.GetComponent<ESTrafficLghtCtrl>() != null)
                trafficlightctrl = Other.transform.GetComponent<ESTrafficLghtCtrl>();
            if (Other.transform.root.GetComponent<ESTrafficLghtCtrl>() != null)
                trafficlightctrl = Other.transform.root.GetComponent<ESTrafficLghtCtrl>();
            if (Other.transform.parent.GetComponent<ESTrafficLghtCtrl>() != null)
                trafficlightctrl = Other.transform.parent.GetComponent<ESTrafficLghtCtrl>();

            if (trafficlightctrl != null)
            {
                trafficlightctrl.LastVeh = this.transform;
            }
        }
        //
        if (Other.transform.root.name == "AI" || Other.transform.CompareTag("Player") || Other.CompareTag("CommonTrigger"))
        {
            if (TriggerObject == null)
            {
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                Vector3 playerpositon = Other.transform.position - transform.position;
                fwddot = Vector3.Dot(fwd, playerpositon);
                //
                Vector3 swd = transform.TransformDirection(Vector3.right);
                Vector3 playersidepositon = Other.transform.position - transform.position;
                side = Vector3.Dot(swd, playersidepositon);
                if (DisableSmartAI == false)
                {
                    if (fwddot > fowardsensor && Mathf.Abs(side) < sidesensor)
                    {
                        TriggerObject = Other.attachedRigidbody == null ? Other.transform : Other.attachedRigidbody.transform;
                        if (TriggerObject.GetComponent<Rigidbody>() != null)
                        {
                            float distobst = Vector3.Distance(TriggerObject.transform.position, transform.position);

                            //
                            if (distobst < 5f)
                            {
                                Tooclose = true;
                            }
                            else
                            {
                                Tooclose = false;
                            }
                            float Pi = Mathf.PI * 1.15f;
                            float CurrentSpeed = TriggerObject.GetComponent<Rigidbody>().velocity.magnitude * Pi;
                            if (CurrentSpeed > 10f)
                            {
                                CopySpeed = true;
                                topspeed = CurrentSpeed * 0.5f;
                            }
                            else
                            {
                                if (!CopySpeed)
                                {
                                    triggerobjectbraking = true;
                                    returnmotor = true;
                                    CarRb.drag = 50f;
                                }
                            }
                        }
                        else
                        {
                            if (!CopySpeed)
                            {
                                triggerobjectbraking = true;
                                returnmotor = true;
                                CarRb.drag = 50f;
                            }
                        }
                    }
                    /*
                    if (UseReverse)
                    {
                        if (fwddot < ReverseOnCloseImpact && !Hasreversed)
                        {
                            if (trafficlightctrl == null)
                                StartCoroutine(ReverseVehicle(2f, ReverseTime));
                        }
                    }
                    */
                }
            }
        }
        //
        CheckForbjectIncoming();
        //UpdateByCollider(Other.transform);
        // this.GetComponent<SphereCollider>().enabled = false;
        callsensor = true;
    }
    //
    IEnumerator ReverseVehicle(float time, float StopTime)
    {
        yield return new WaitForSeconds(time);
        if (!Hasreversed)
        {
            Reverse = true;
            triggerobjectbraking = false;
            returnmotor = false;
            StartCoroutine("StopReverse", StopTime);
        }
    }
    //
    IEnumerator StopReverse(float time)
    {
        yield return new WaitForSeconds(time);
        print("stop");
        Hasreversed = true;
        triggerobjectbraking = true;
        Reverse = false;
    }
    //
    void InvokeRemovePlayer()
    {
        //print("Removed");
        if (TriggerObject == null)
        {
            CancelInvoke();
            return;
        }
        if (Vector3.Distance(TriggerObject.position, this.transform.position) > 4f)
        {
            triggerobjectbraking = false;
            CopySpeed = false;
            EnableAvoid = false;
            TriggerObject = null;
        }
    }
    //
    //
    void OnTriggerExit(Collider Other)
    {
        //
        if (TriggerObject != null)
        {
            if (Other.attachedRigidbody == null)
            {

                if (TriggerObject == Other.transform)
                {
                    CancelInvoke();
                    InvokeRepeating("InvokeRemovePlayer", 0.1f, 5f);
                }
            }
        }
        //TriggerObject = null;
        //returntriggerstay = false;
        trafficlightctrl = null;
        callsensor = false;
        Stop = false;
    }
}
