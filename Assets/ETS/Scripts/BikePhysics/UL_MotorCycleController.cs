using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(UL_MotorCycleControl))]
public class UL_MotorCycleController : MonoBehaviour
{

    public enum ControllerType
    {
        AI,
        PlayerCotrol
    }
    public ControllerType GetControllerType = ControllerType.PlayerCotrol;
    #region  PlayerVariable
    [Header("Bike Setup")]
    //
    [Header(" ")]

    [Header("a rigidbody of mass 1000 or higher is recomended")]
    public Ul_Suspension FrontWheel;
    public bool LooseControl;
    public Ul_Suspension RearWheel;
    public Transform FrontWheelMesh, RearWheelMesh;
    public float OverRallTorque = 5000f, Braketorque = 5000f;
    public float ReverseTorque = 500f;
    [Tooltip("above 10000 will do just fine")]
    public float RollTorque = 10000;
    public float SideLerpTorque = 5000f;
    public Rigidbody MotorRb;
    public float Balancingforce = 7000;
    public float CurrentFrictionForce = 0.1f;
    public float KillDriftSpeed = 40f, DriftSpeed = 35f;
    public Ul_SurfaceDetector surfaceDetector;
    public Transform BikeHead;
    public float NeckAngle = 25f;
    public bool IsDrifting;
    //
    //private
    public float TopSpeed = 110f;
    [HideInInspector] public Vector3 pre_pos, newpos, movement, fwd;
    [HideInInspector] public float SideGrip;
    public float CurrentSpeed, CurrentRotateSpeed;
    [HideInInspector]
    public float Accel;
    [HideInInspector]
    public bool ReturnInputs;
    public float eulerx, eulery, eulerz;

    [HideInInspector] public float Lerp;
    [HideInInspector]
    public float pre_accel_mul;
    [HideInInspector]
    public float shoebrake;
    public int tab;
    private float waituntilrotstop;
    private float RotationDifference;
    private Quaternion oldrot;
    private float DriftCounter;
    private bool SeizeRot;
    private float backbodydrag, backZ;
    #endregion

    #region  AI Variable
    [Header("AI")]
    public float fowardsensor = 2f, sidesensor = 2f;
    public WheelCollider FrontWheelCol, RearWheelCol;
    public Transform TargetNode;
    public float NavigateSpeed = 500f;
    public float LookAtSpeed = 10f;
    public float DistanceApart = 5;
    public float topspeed = 50f;
    public Transform TriggerObject;
    public bool TriggerBraking = false;
    public float DistanceCheck;
    public ESTrafficLghtCtrl trafficlightctrl;
    private float currentspeed, TotalDownWardForce;
    private bool optimizerandom;
    public float distanceapartplayer;
    private Transform player;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        MotorRb = this.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MotorRb.drag = 0.17f;
        backbodydrag = MotorRb.drag;
        MotorRb.angularDrag = 5.00f;
    }
    //
    float ClampingAngleAt(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);

        return Mathf.Min(angle, to);
    }
    //
    public float wrapangle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
    // Update is called once per frame
    void Update()
    {
        if (GetControllerType == ControllerType.PlayerCotrol)
        {
            newpos = transform.position;
            movement = (newpos - pre_pos);
            //checkdot = (Vector3.Dot(fwd, movement));
            //print(Vector3.Dot(fwd, movement));
            if (Vector3.Dot(fwd, movement) < 0f)
            {
                pre_accel_mul = -1;
            }
            else if ((Vector3.Dot(fwd, movement) > 0f))
            {
                pre_accel_mul = 1;
            }
            if (pre_accel_mul < 0 && Accel < 0)
                backZ = -1;
            else
                backZ = 0;
            //
            FrontWheel.GetWorldPos(FrontWheelMesh, eulerz, pre_accel_mul);
            RearWheel.GetWorldPos(RearWheelMesh, eulerz, pre_accel_mul);
        }
    }
    //
    void LateUpdate()
    {
        if (GetControllerType == ControllerType.PlayerCotrol)
        {
            pre_pos = transform.position;
            fwd = transform.forward;
        }
        else
        {
            UpdateTarget();
        }
    }
    public void MotorCycleControl(ESCrossPlatformInputManager crossPlatformInputManager)
    {
        Accel = crossPlatformInputManager.GetAxis("Vertical", false);
        Lerp = crossPlatformInputManager.GetAxis("Horizontal", false);
        shoebrake = crossPlatformInputManager.GetAxis("Jump", false);
    }
    void FixedUpdate()
    {
        if (GetControllerType == ControllerType.PlayerCotrol)
        {
            Vector3 dir = transform.position - pre_pos;
            if (LooseControl == false)
            {
                SurfaceManager();
                ApplyMotorTorque();
                SteerControl();
                Drifiting(shoebrake);
                if (CurrentSpeed > 2)
                {
                    ApplyBrake(shoebrake);
                }

                BodyRotation(shoebrake);
            }
            else
            {
                CalculateRandomForce();
                eulerx = wrapangle(transform.eulerAngles.x);
                eulery = wrapangle(transform.eulerAngles.y);
                eulerz = wrapangle(transform.eulerAngles.z);
                MotorRb.angularDrag = 0.001f;
                //
                Vector3 locangvel = transform.InverseTransformDirection(MotorRb.angularVelocity);
                locangvel.z *= 1f;
                transform.TransformDirection(locangvel);
                //
                Vector3 locvel = transform.InverseTransformDirection(MotorRb.velocity);
                locvel.x *= 0.000000000001f;
                transform.TransformDirection(locvel);
            }
            //
            Vector3 rot = transform.eulerAngles;
            if (!LooseControl)
            {
                rot.z = ClampingAngleAt(rot.z, -20f, 20f);
            }
            transform.eulerAngles = rot;
            //

            //
            OreintBodyRot();
        }
        else
        {
            AIBehaviour();
            WheelAlignment();
            VehicleSpeed();
        }
    }
    //

    #region  AIStuff
    //
    public void WheelAlignment()
    {
        // make tyre meshes follow wheels;
        if (currentspeed < 0.09f) return;
        // align front wheel meshes
        Vector3 frontwheelposition;
        Quaternion frontwheelrotation;
        FrontWheelCol.GetWorldPose(out frontwheelposition, out frontwheelrotation);
        FrontWheelMesh.transform.position = frontwheelposition;

        // align rear wheel meshes
        Vector3 rearwheelposition;
        Quaternion rearwheelrotation;
        RearWheelCol.GetWorldPose(out rearwheelposition, out rearwheelrotation);
        RearWheelMesh.transform.position = rearwheelposition;

        //

        TotalDownWardForce += MotorRb.velocity.sqrMagnitude;
        //        print(MotorRb.velocity.sqrMagnitude);
        TotalDownWardForce += 5000 / .5f / 10;
        FrontWheelMesh.localEulerAngles = new Vector3(TotalDownWardForce, 0, 0);
        RearWheelMesh.localEulerAngles = new Vector3(TotalDownWardForce, 0, 0);
        //rearwheelmeshes[i].transform.rotation = rearwheelrotation;
    }
    void AIBehaviour()
    {
        if (TargetNode == null) return;

        Vector3 locvel = transform.TransformDirection(MotorRb.velocity);
        locvel.x *= 0.000000000001f;
        transform.InverseTransformDirection(locvel);
        //motorCycleController.CurrentSpeed = 5f;
        Vector3 Targetdirection = TargetNode.position - this.transform.position;
        float singlestep = LookAtSpeed * Time.deltaTime;

        Vector3 newdir = Vector3.RotateTowards(transform.forward, Targetdirection, singlestep, 0.0f);
        Vector3 baceuler = transform.eulerAngles;
        transform.rotation = Quaternion.LookRotation(newdir);
        Vector3 euler = transform.eulerAngles;
        euler.x = baceuler.x;
        transform.eulerAngles = euler;

        float Distancefromplayer = Vector3.Distance(player.position, this.transform.position);
        if (Distancefromplayer > distanceapartplayer)
        {
            this.gameObject.SetActive(false);
        }
        //
        if (TriggerObject != null)
        {
            if (TriggerObject.gameObject.activeSelf == false)
            {
                MotorRb.drag = 0.17f;
                if (FrontWheelCol.GetComponent<WheelCollider>().isGrounded)
                {
                    Vector3 move = transform.forward * NavigateSpeed * Time.deltaTime;
                    MotorRb.velocity = move;
                }
                TriggerBraking = false;
                TriggerObject = null;
            }
        }
        if (trafficlightctrl == null)
        {
            if (!TriggerBraking)
            {
                MotorRb.drag = 0.17f;
                if (FrontWheelCol.GetComponent<WheelCollider>().isGrounded)
                {
                    Vector3 move = transform.forward * NavigateSpeed * Time.deltaTime;
                    MotorRb.velocity = move;
                }
            }
            else
            {
                if (TriggerObject != null)
                {
                    if (TriggerObject.GetComponent<Rigidbody>() != null)
                    {
                        if (TriggerObject.GetComponent<Rigidbody>().velocity.magnitude > 10f)
                        {
                            Vector3 move = transform.forward * TriggerObject.GetComponent<Rigidbody>().velocity.magnitude * 0.125f * Time.deltaTime;
                            MotorRb.velocity = move;
                        }
                        else
                        {
                            MotorRb.drag = (RearWheelCol.isGrounded) ? 500f : 0.17f;
                        }
                    }
                    else
                    {
                        MotorRb.drag = (RearWheelCol.isGrounded) ? 500f : 0.17f;
                    }
                }
            }
        }
        else
        {
            if (trafficlightctrl.red)
            {
                MotorRb.drag = 500f;
            }
            else
            {
                MotorRb.drag = 0.17f;
                if (FrontWheelCol.GetComponent<WheelCollider>().isGrounded)
                {
                    Vector3 move = transform.forward * NavigateSpeed * Time.deltaTime;
                    MotorRb.velocity = move;
                }
            }
        }
        //
    }
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
        }

        if (Other.transform.CompareTag("Player") || Other.CompareTag("AIBike") || Other.CompareTag("CommonTrigger"))
        {
            if (TriggerObject == null)
            {
                checkfortriggerobject(Other);
            }
        }
        else
        {
            if (Other.transform.root.name == "AI")
            {
                checkfortriggerobject(Other);
            }
        }
        //
        if (TriggerObject != null)
        {
            if (TriggerObject.CompareTag("IgnoreTrigger"))
            {
                TriggerObject = null;
            }
        }
    }
    //
    void checkfortriggerobject(Collider Other)
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 playerpositon = Other.transform.position - transform.position;
        float fwddot = Vector3.Dot(fwd, playerpositon);
        //
        Vector3 swd = transform.TransformDirection(Vector3.right);
        Vector3 playersidepositon = Other.transform.position - transform.position;
        float side = Vector3.Dot(swd, playersidepositon);
        if (fwddot > fowardsensor && Mathf.Abs(side) < sidesensor)
        {
            TriggerBraking = true;
            TriggerObject = Other.attachedRigidbody == null ? Other.transform : Other.attachedRigidbody.transform;
        }
    }
    //
    void OnTriggerExit(Collider Other)
    {
        if (TriggerObject != null)
        {
            if (Other.attachedRigidbody != null)
            {
                if (Other.attachedRigidbody.transform == TriggerObject)
                {
                    TriggerBraking = false;
                    TriggerObject = null;
                }
            }
            else
            {
                if (Other.transform == TriggerObject)
                {
                    TriggerBraking = false;
                    TriggerObject = null;
                }
            }
        }
        //
        if (trafficlightctrl != null)
        {
            trafficlightctrl.LastVeh = this.transform;
        }
        trafficlightctrl = null;
    }
    //
    void UpdateTarget()
    {
        if (TargetNode != null)
        {
            DistanceCheck = Vector3.Distance(this.transform.position, TargetNode.position);
            if (DistanceCheck < DistanceApart)
            {
                if (TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count == 0)
                {
                    if (TargetNode.GetComponent<ESNodeManager>().NextNode != null)
                    {
                        TargetNode = TargetNode.GetComponent<ESNodeManager>().NextNode;
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }
                    optimizerandom = true;
                }
                else
                {
                    if (TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count > 0)
                    {
                        int pathindex = 0;

                        if (optimizerandom)
                        {
                            pathindex = Random.Range(0, TargetNode.GetComponent<ESNodeManager>().ConnectedNode.Count);
                            optimizerandom = false;
                        }

                        TargetNode = TargetNode.GetComponent<ESNodeManager>().ConnectedNode[pathindex];
                    }
                }
            }
        }
    }
    //
    public void VehicleSpeed()
    {
        //km/h
        float Pi = Mathf.PI * 1.15f;
        currentspeed = MotorRb.velocity.magnitude * Pi;
        if (currentspeed > topspeed)
            MotorRb.velocity = (topspeed / Pi) * MotorRb.velocity.normalized;
    }
    #endregion
    //
    //boundries 
    //
    #region  PlayerStuff

    private void OreintBodyRot()
    {
        if (CurrentSpeed < 0.1f)
        {
            if (Mathf.Abs(Accel) > 0)
            {
                MotorRb.drag = backbodydrag;
                return;
            }
            if (!LooseControl)
            {
                if (RearWheel.m_isgrounded || FrontWheel.m_isgrounded)
                {
                    Vector3 bike_euler = transform.eulerAngles;
                    bike_euler.z = 0;
                    transform.eulerAngles = bike_euler;
                }

                if (RearWheel.m_isgrounded && FrontWheel.m_isgrounded)
                {
                    MotorRb.drag = 50;
                }
                else
                {
                    MotorRb.drag = backbodydrag;
                }
            }
            else
            {
                MotorRb.drag = backbodydrag;
                return;
            }
        }
    }
    //

    private void CalculateRandomForce()
    {
        if (LooseControl == false) return;
        if (Mathf.Abs(eulerz) < 35f)
        {
            if (eulerz < 0)
            {
                MotorRb.AddTorque(transform.forward * 1500f * Mathf.Sign(eulerz));
            }
            else if (eulerz > 0)
            {
                MotorRb.AddTorque(transform.forward * -1500f * Mathf.Sign(eulerz));
            }
        }
    }
    //
    void Drifiting(float brakeval)
    {
        //if (!LooseControl) return;
        RotationDifference = Quaternion.Angle(oldrot, transform.rotation);
        RotationDifference = Mathf.Abs(Lerp) > 0 ? surfaceDetector.RotationDifferenceLimit : RotationDifference;
        if (CurrentSpeed > DriftSpeed)
        {
            if (Mathf.Abs(Accel) > 0 && brakeval > 0)
            {
                IsDrifting = true;
            }
            if (IsDrifting)
            {
                if (CurrentSpeed < KillDriftSpeed || RotationDifference < surfaceDetector.RotationDifferenceLimit || Accel < 0)
                {
                    IsDrifting = false;
                }
            }
        }
        else
        {
            IsDrifting = false;
        }
        if (IsDrifting)
        {
            if (brakeval > 0)
            {
                DriftCounter += Time.deltaTime;
            }
            else
            {
                DriftCounter = 0.0f;
            }

            if (RearWheel != null)
            {
                //MotorRb.AddForceAtPosition(FrontWheel.transform.right * -Lerp * surfaceDetector.MaxSideRotationalForce, FrontWheel.transform.position);
                MotorRb.AddForceAtPosition(RearWheel.transform.right * -Lerp * surfaceDetector.MaxSideRotationalForce,
                 RearWheel.transform.position);
                MotorRb.AddRelativeForce(RearWheel.transform.right * Lerp * surfaceDetector.MaxSideSlipForce);
                MotorRb.AddRelativeForce(transform.forward * 1 * (surfaceDetector.MaxFowardSlipForce));
            }
        }
        if (!IsDrifting)
        {
            DriftCounter = 0.0f;
        }
        oldrot = transform.rotation;
    }
    //
    void BodyRotation(float shoebrake)
    {
        eulerx = wrapangle(transform.eulerAngles.x);
        eulery = wrapangle(transform.eulerAngles.y);
        eulerz = wrapangle(transform.eulerAngles.z);
        //
        //transform.rotation = Quaternion.AngleAxis(25f * -Lerp, Vector3.forward);
        //pre_accel_mul = Accel > 0 ? 1 : Accel < 0 ? -1 : pre_accel_mul;
        //
        //BikeRigidBody.AddTorque(transform.forward * Lerp * Balancingforce);
        //continue from sign_mag you wanted checker great and less
        if (!LooseControl)
        {
            if (Mathf.Abs(Lerp) > 0)
            {
                if (CurrentSpeed > 5)
                {
                    MotorRb.AddRelativeTorque(Vector3.forward * SideLerpTorque * -Lerp);
                }
                else
                {
                    if (eulerz > 3f)
                    {
                        MotorRb.AddRelativeTorque(Vector3.forward * -Balancingforce);
                    }
                    if (eulerz < -3f)
                    {
                        MotorRb.AddRelativeTorque(Vector3.forward * Balancingforce);
                    }
                }
            }
            else
            {
                if (Mathf.Abs(eulerz) > 3f)
                {
                    if (eulerz > 3f)
                    {
                        MotorRb.AddRelativeTorque(Vector3.forward * -Balancingforce);
                    }
                    if (eulerz < -3f)
                    {
                        MotorRb.AddRelativeTorque(Vector3.forward * Balancingforce);
                    }
                }
                else
                {
                    if (eulerz > 0)
                    {
                        MotorRb.AddRelativeTorque(Vector3.forward * -50);
                    }
                    if (eulerz < 0)
                    {
                        MotorRb.AddRelativeTorque(Vector3.forward * 50);
                    }
                }
            }
        }
        BodyDrag(shoebrake);
        TweakSpeed();
        #region ManualBodyDrag
        Vector3 locvel = transform.InverseTransformDirection(MotorRb.angularVelocity);
        locvel.z *= !LooseControl ? 0.0000000001f : 1f;
        if (CurrentSpeed > 0.5f)
        {
            waituntilrotstop = 0.0f;
            SeizeRot = true;
            MotorRb.angularDrag = 5.00f;
            locvel.y *= Mathf.Abs(Lerp) > 0 ? 0.999f : CurrentFrictionForce;
        }
        else
        {
            locvel.y *= 0.000001f;
            if (!LooseControl)
            {
                if (SeizeRot == true)
                {
                    //MotorRb.angularDrag = 1000f;
                    MotorRb.angularDrag = !RearWheel.m_isgrounded || !FrontWheel.m_isgrounded ? 0.01f : 1000f;
                    SeizeRot = false;
                }
                else
                {
                    if (CurrentRotateSpeed < 0.1f)
                    {
                        if (waituntilrotstop < 100f)
                        {
                            waituntilrotstop += Time.deltaTime;
                        }
                        if (waituntilrotstop > 2f)
                            MotorRb.angularDrag = 5.00f;
                    }
                    else
                    {
                        if (Mathf.Abs(eulerz) < 3f)
                        {
                            waituntilrotstop = 0.0f;
                        }
                        MotorRb.angularDrag = !RearWheel.m_isgrounded || !FrontWheel.m_isgrounded ? 0.01f : 1000f;
                        //MotorRb.angularDrag = 1000f;
                    }
                }
            }
            else
            {
                MotorRb.angularDrag = 0.001f;
            }
        }
        //locvel.x *= 0.0001f;
        MotorRb.angularVelocity = transform.TransformDirection(locvel);
        #endregion
    }
    //
    public void ApplyBrake(float brakeforce)
    {
        FrontWheel.ApplyBrake(Braketorque, pre_accel_mul * brakeforce);
        RearWheel.ApplyBrake(Braketorque, pre_accel_mul * brakeforce);
    }
    void ApplyMotorTorque()
    {
        if (RearWheel != null)
        {
            if (backZ < 0)
                RearWheel.MotorTorque = Accel * ReverseTorque;
            else
                RearWheel.MotorTorque = Accel * OverRallTorque;
        }
    }
    //
    void SteerControl()
    {
        if (FrontWheel == null) return;
        if (BikeHead != null)
        {
            Vector3 neckEuler = BikeHead.localEulerAngles;
            neckEuler.y = NeckAngle * Lerp;
            BikeHead.localEulerAngles = neckEuler;
        }
        if (CurrentSpeed > .5f && FrontWheel.m_isgrounded)
            FrontWheel.RollTorque = CurrentSpeed < 10f && pre_accel_mul < 0 ? RollTorque * -Lerp : RollTorque * Lerp;
    }
    //
    void BodyDrag(float shoebrake)
    {
        Vector3 localvel = transform.InverseTransformDirection(MotorRb.velocity);
        if (!IsDrifting)
        {
            localvel.x *= SideGrip;
        }
        else
        {
            localvel.x *= surfaceDetector.SideGrip;
        }
        if (Mathf.Abs(Accel) == 0)
        {
            localvel.z *= MotorRb.velocity.magnitude < 2f ? 0.000001f : 1f;
        }
        else
        {
            localvel.z *= 1f;
        }
        //
        if (CurrentSpeed < 2)
        {
            if (!IsDrifting)
            {
                localvel.x *= 0.0001f;
            }
        }
        //print(localvel.x);
        MotorRb.velocity = transform.TransformDirection(localvel);
        /*
        if (localvel.x > 0)
        {
            // totalforce.x *= 0f;
        }
        */
        TweakSpeed();
    }
    //
    void SurfaceManager()
    {
        if (RearWheel == null || surfaceDetector == null) return;
        //
        if (RearWheel.wheelhit.collider != null)
        {
            if (surfaceDetector.SurfacePresets.Count < 1) return;
            for (int i = 0; i < surfaceDetector.SurfacePresets.Count; ++i)
            {
                if (RearWheel.wheelhit.collider.tag == surfaceDetector.SurfacePresets[i].SurfaceTagName)
                {
                    //

                    // print(RearWheel.wheelhit.collider.tag);
                    if (IsDrifting == false)
                    {
                        MotorRb.AddForceAtPosition(RearWheel.transform.right * -Lerp * surfaceDetector.SurfacePresets[i].MaxSideRotationalForce,
               RearWheel.transform.position);
                        MotorRb.AddRelativeForce(RearWheel.transform.right * Lerp * surfaceDetector.SurfacePresets[i].MaxSideSlipForce);
                        MotorRb.AddRelativeForce(transform.forward * 1 * (surfaceDetector.SurfacePresets[i].MaxFowardSlipForce));
                        if (SideGrip > surfaceDetector.SurfacePresets[i].SideGrip)
                        {
                            SideGrip -= 0.01f;
                        }
                        else
                        {
                            SideGrip = surfaceDetector.SurfacePresets[i].SideGrip;
                        }
                        // SideGrip = surfaceDetector.SurfacePresets[i].SideGrip;
                        CurrentFrictionForce = surfaceDetector.SurfacePresets[i].Dynamicfriction;
                    }
                    else
                    {
                        SideGrip = 1f;
                        CurrentFrictionForce = surfaceDetector.Driftfriction;
                    }
                }
            }
        }
    }
    public void TweakSpeed()
    {
        //km/h
        float Pi = Mathf.PI * 1.15f;
        float maxrotspeed;
        Vector3 rigidspeed = new Vector3(MotorRb.velocity.x, 0.0f, MotorRb.velocity.z);
        CurrentSpeed = rigidspeed.magnitude * Pi;
        CurrentRotateSpeed = MotorRb.angularVelocity.magnitude * Pi;
        if (!IsDrifting)
        {
            maxrotspeed = CurrentSpeed > 70f ? 2f : CurrentSpeed > 50f && CurrentSpeed < 70f ? 5f : 15f;
        }
        else
        {
            maxrotspeed = 5f;
        }
        if (MotorRb.velocity.magnitude > 10)
        {
            if (CurrentSpeed > TopSpeed)
                MotorRb.velocity = (TopSpeed / Pi) * MotorRb.velocity.normalized;
        }
        else
        {
            if (pre_accel_mul < 0)
            {
                maxrotspeed = .9f;
                if (CurrentSpeed > 10f)
                    MotorRb.velocity = (10f / Pi) * MotorRb.velocity.normalized;
            }
        }
        //
        if (CurrentRotateSpeed > maxrotspeed)
        {
            MotorRb.angularVelocity = (maxrotspeed / Pi) * MotorRb.angularVelocity.normalized;
        }
    }
    #endregion
}

