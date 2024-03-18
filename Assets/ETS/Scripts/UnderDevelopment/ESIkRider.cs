using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESIkRider : MonoBehaviour
{
    [HideInInspector] public float IKWeight;
    [HideInInspector] public float LookWeight = .5f;
    [HideInInspector] public float BodyWeight;
    [HideInInspector] public float HeadWeight;
    [HideInInspector] public float Eyeweight;
    [HideInInspector] public float ClampWeight;
    [HideInInspector] public Transform LookPos;
    [HideInInspector] public Transform LeftIkTarget, RightIKTarget, RightFootIKTarget, LeftFootIKTarget;
    [HideInInspector] public UL_MotorCycleController MotorRigid;
    [HideInInspector] public ESGrabVehicle _Exit_Vehicle;
    [Header("DEBUG")]
    public Animator animator;
    [SerializeField] private Transform RightFoot, LeftFoot;
    [Header("Base On MotorCycle Preset")]
    [SerializeField] private float LegIkSpeed = 1.5f;
    private int LegToGroundIndex;
    [SerializeField] private Vector3 LegOffset;
    [SerializeField] private Vector3 DriftLegOffset;
    private Vector3 BackPositionRightIkTarget, BackPositionLeftIKTarget;
    [SerializeField] private float legdist = 5;
    private Vector3 Lpos, Rpos;
    private Vector3 LeftIkAnchor, RightIkAnchor;
    [HideInInspector] public bool OreintLeg, OreinlegsInreverse, firstleg;
    // Start is called before the first frame update
    void Start()
    {

        animator = this.GetComponent<Animator>();
        RightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        LeftFoot = animator.GetBoneTransform(HumanBodyBones.LeftToes);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit Lhit = new RaycastHit();
        if (LeftFootIKTarget != null && MotorRigid != null)
        {

            Lpos = LeftFootIKTarget.TransformPoint(Vector3.zero);
            Vector3 offsetvec = !MotorRigid.IsDrifting ? LegOffset : DriftLegOffset;
            if (Physics.Raycast(Lpos, -LeftFootIKTarget.up, out Lhit, legdist))
            {

                if (LeftFootIKTarget != null)
                {
                    Lpos = Lhit.point + (Vector3.up * offsetvec.y) +
                    (LeftFootIKTarget.right * -offsetvec.x) + (LeftFootIKTarget.forward * offsetvec.z);
                    //Lpos.y -= offsetvec.y;
                }
            }
        }
        //
        RaycastHit Rhit = new RaycastHit();
        if (RightFootIKTarget != null && MotorRigid != null)
        {

            Rpos = RightFootIKTarget.TransformPoint(Vector3.zero);
            Vector3 offsetvec = !MotorRigid.IsDrifting ? LegOffset : DriftLegOffset;
            if (Physics.Raycast(Rpos, -RightFootIKTarget.up, out Rhit, legdist))
            {
                if (RightFootIKTarget != null)
                {
                    Rpos = Rhit.point + (Vector3.up * offsetvec.y) +
                    (RightFootIKTarget.right * offsetvec.x) + (RightFootIKTarget.forward * offsetvec.z);
                    //Lpos.y -= offsetvec.y;
                }
            }
        }
        //if (Application.isPlaying == false)
        //     Debug.DrawRay(Lpos, dir, Color.black);
    }
    private void OnAnimatorIK()
    {
        if (MotorRigid == null) return;
        #region IkRrider
        SetRiderGrip();
        SetRiderFootBaseOnMotorCycle();
        #endregion
    }
    //
    private void KeepLegAtSitpos()
    {
        Vector3 LeftBackIK = new Vector3();
        if (LeftIkAnchor != null)
        {
            LeftBackIK = LeftFootIKTarget.TransformPoint(Vector3.zero);
        }
        LeftIkAnchor = Vector3.Lerp(LeftIkAnchor,
         LeftBackIK, LegIkSpeed * Time.deltaTime);


        Vector3 RightbackIk = new Vector3();
        if (RightIKTarget != null)
        {
            RightbackIk = RightFootIKTarget.TransformPoint(Vector3.zero);
        }
        RightIkAnchor = Vector3.Lerp(RightIkAnchor,
         RightbackIk, LegIkSpeed * Time.deltaTime);

        LegToGroundIndex = Random.Range(0, 2);
        //
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
        //
        animator.SetIKPosition(AvatarIKGoal.RightFoot, RightFootIKTarget.position);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftFootIKTarget.position);
    }
    //

    //
    private void SetRiderFootBaseOnMotorCycle()
    {
        if (_Exit_Vehicle == null) return;
        if (MotorRigid != null)
        {
            if (_Exit_Vehicle.BikeBackardZ < 0)
            {
                LeftFootIKTarget = _Exit_Vehicle.LeftFootIkTarget;
                RightFootIKTarget = _Exit_Vehicle.RightFootIkTarget;
                RightIkAnchor = RightFootIKTarget.TransformPoint(Vector3.zero);
                LeftIkAnchor = LeftFootIKTarget.TransformPoint(Vector3.zero);
                OreinlegsInreverse = false;
                return;
            }
            if (firstleg)
            {
                RightIkAnchor = RightFootIKTarget.TransformPoint(Vector3.zero);
                LeftIkAnchor = LeftFootIKTarget.TransformPoint(Vector3.zero);
                KeepLegAtSitpos();
                firstleg = false;
            }
        }

        if (_Exit_Vehicle.OnSit == true)
        {
            OreinlegsInreverse = true;
            MotorRigid = _Exit_Vehicle.motorCycleController;
            LeftFootIKTarget = _Exit_Vehicle.LeftFootIkTarget;
            RightFootIKTarget = _Exit_Vehicle.RightFootIkTarget;
            if (BackPositionRightIkTarget == Vector3.zero)
            {
                BackPositionRightIkTarget = RightFootIKTarget.localPosition;
                RightIkAnchor = RightFootIKTarget.TransformPoint(Vector3.zero);
                LeftIkAnchor = LeftFootIKTarget.TransformPoint(Vector3.zero);
            }
            //
            if (BackPositionLeftIKTarget == Vector3.zero)
            {
                BackPositionLeftIKTarget = LeftFootIKTarget.localPosition;
            }
            if (MotorRigid.MotorRb.velocity.sqrMagnitude > 0.2f && MotorRigid.IsDrifting == false)
            {

                if (!OreintLeg)
                {
                    RightIkAnchor = RightFootIKTarget.TransformPoint(Vector3.zero);
                    LeftIkAnchor = LeftFootIKTarget.TransformPoint(Vector3.zero);
                    OreintLeg = true;
                }
                KeepLegAtSitpos();
            }
            else
            {
                if (OreintLeg)
                {
                    RightIkAnchor = RightFootIKTarget.TransformPoint(Vector3.zero);
                    LeftIkAnchor = LeftFootIKTarget.TransformPoint(Vector3.zero);
                    OreintLeg = false;
                }
                if (MotorRigid.eulerz < 0.1f)
                {
                    if (true)
                    {
                        if (RightIkAnchor != null)
                        {
                            //LeftIkAnchor = LeftFootIKTarget.TransformPoint(Vector3.zero);
                            if (!MotorRigid.IsDrifting)
                            {
                                RightIkAnchor = Vector3.Lerp(RightIkAnchor,
                                 Rpos, LegIkSpeed * Time.deltaTime);
                            }
                            else
                            {
                                RightIkAnchor = Rpos;
                            }
                            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);
                            //
                            animator.SetIKPosition(AvatarIKGoal.RightFoot, RightIkAnchor);
                        }
                    }
                    //
                    Vector3 BackIK = new Vector3();
                    if (LeftIkAnchor != null)
                    {
                        BackIK = LeftFootIKTarget.TransformPoint(Vector3.zero);
                    }
                    if (!MotorRigid.IsDrifting)
                    {
                        LeftIkAnchor = Vector3.Lerp(LeftIkAnchor,
                         BackIK, LegIkSpeed * Time.deltaTime);
                    }
                    else
                    {
                        LeftIkAnchor = BackIK;
                    }
                    //
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
                    //
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftIkAnchor);
                }
                else if (MotorRigid.eulerz > 0.1f)
                {
                    if (LeftIkAnchor != null)
                    {
                        if (!MotorRigid.IsDrifting)
                        {
                            LeftIkAnchor = Vector3.Lerp(LeftIkAnchor,
                                                    Lpos, LegIkSpeed * Time.deltaTime);
                        }
                        else
                        {
                            LeftIkAnchor = Lpos;
                        }
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
                        //
                        animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftIkAnchor);
                    }
                    //
                    Vector3 backIk = new Vector3();
                    if (RightIKTarget != null)
                    {
                        backIk = RightFootIKTarget.TransformPoint(Vector3.zero);
                    }
                    if (!MotorRigid.IsDrifting)
                    {
                        RightIkAnchor = Vector3.Lerp(RightIkAnchor,
                         backIk, LegIkSpeed * Time.deltaTime);
                    }
                    else
                    {
                        RightIkAnchor = backIk;
                    }
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);

                    //
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, RightIkAnchor);
                }
            }
        }
    }
    //
    private void SetRiderGrip()
    {
        if (_Exit_Vehicle == null) return;
        if (_Exit_Vehicle.OnSit == true)
        {
            LeftIkTarget = _Exit_Vehicle.LeftIkTarget;
            RightIKTarget = _Exit_Vehicle.RightIkTarget;
            LookPos = _Exit_Vehicle.LookPos;
            //set ik weightposition
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftIkTarget.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, RightIKTarget.position);
            //
            //set ik weightRotation
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, IKWeight);
            //
            animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftIkTarget.rotation);
            animator.SetIKRotation(AvatarIKGoal.RightHand, RightIKTarget.rotation);
            //

            animator.SetLookAtWeight(LookWeight, BodyWeight, HeadWeight, Eyeweight, ClampWeight);
            animator.SetLookAtPosition(LookPos.position);
        }
        //
    }
}
