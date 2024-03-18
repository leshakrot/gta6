using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESIKSystem : MonoBehaviour
{
    /*
    this script is still under testing but we have uploaded this along with so you can update on it.
    */
    Animator animator;
    private Vector3 RightFootPosition, LeftFootPosition, RightFootIKPosition, LeftFootIKPosition;
    private Quaternion LeftFootIKRotation, RightFootIKRotation;

    private float LastRightFootPos, LastLeftFootPos, LastPelvisPosY;
    [Range(0, 2)] [SerializeField] private float HeightFromGroundRaycast = 1.14f;
    [Range(0, 2)] [SerializeField] private float RaycastDownDistance = 1.5f;
    [SerializeField] private LayerMask EnvironmentMask;
    [SerializeField] private float PelvisOffset = 0f;
    [Range(0, 1)] [SerializeField] private float PelvisUpDownSPeed = 0.28f;
    [Range(0, 1)] private float FeetIKPositionSPeed = 0.5f;
    public string LeftAnimVariableName = "LeftFootCurve";
    public string RightAnimVariableName = "RightFootCurve";
    public bool EnableProIK = false;

    public bool ShowDebug = true;
    public bool EnableFootIK = true;
    public CharacterController controller;

    // Start is called before the first frame update
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!EnableFootIK) return;
        if (animator == null) return;
        //if (controller.isGrounded) return;
        //
        AdjustFeetTarget(ref RightFootPosition, HumanBodyBones.RightFoot);
        AdjustFeetTarget(ref LeftFootPosition, HumanBodyBones.LeftFoot);
        //find and raycast to ground to get loc
        FeetPositionSolver(RightFootPosition, ref RightFootIKPosition, ref RightFootIKRotation);
        FeetPositionSolver(LeftFootPosition, ref LeftFootIKPosition, ref LeftFootIKRotation);
        //
    }
    private void OnAnimatorIK()
    {
        if (!EnableFootIK) return;
        if (animator == null) return;
        //if (controller.isGrounded) return;
        MovePelvisHeight();
        //
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        if (EnableProIK)
        {
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat(RightAnimVariableName));

        }
        MoveFeetToIkPoint(AvatarIKGoal.RightFoot, RightFootIKPosition, RightFootIKRotation, ref LastRightFootPos);
        //
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        if (EnableProIK)
        {
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(LeftAnimVariableName));

        }
        MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, LeftFootIKPosition, LeftFootIKRotation, ref LastLeftFootPos);
        #region junks
        /*
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);

        animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftIkTarget.position);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, RightIKTarget.position);
        //


        animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, IKWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, IKWeight);

        animator.SetIKHintPosition(AvatarIKHint.LeftKnee, hintLeft.position);
        animator.SetIKHintPosition(AvatarIKHint.RightKnee, hintRight.position);

        //

        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, IKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, IKWeight);

        animator.SetIKRotation(AvatarIKGoal.LeftFoot, LeftIkTarget.rotation);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, RightIKTarget.rotation);
        */
        #endregion
    }
    //
    private void MoveFeetToIkPoint(AvatarIKGoal Foot, Vector3 PositionIKHolder, Quaternion RotationIKHolder, ref float LastFootPositionY)
    {
        Vector3 TargetIKPosition = animator.GetIKPosition(Foot);
        if (PositionIKHolder != Vector3.zero)
        {
            TargetIKPosition = transform.InverseTransformPoint(TargetIKPosition);
            PositionIKHolder = transform.InverseTransformPoint(PositionIKHolder);

            float Yvar = Mathf.Lerp(LastFootPositionY, PositionIKHolder.y, FeetIKPositionSPeed);
            TargetIKPosition.y += Yvar;
            LastFootPositionY = Yvar;

            TargetIKPosition = transform.TransformPoint(TargetIKPosition);

            animator.SetIKRotation(Foot, RotationIKHolder);
        }
        //
        animator.SetIKPosition(Foot, TargetIKPosition);
    }
    //
    private void MovePelvisHeight()
    {
        if (!controller.isGrounded) return;
        if (RightFootIKPosition == Vector3.zero || LeftFootIKPosition == Vector3.zero || LastPelvisPosY == 0)
        {
            LastPelvisPosY = animator.bodyPosition.y;
            return;
        }

        float LeftOffsetpos = LeftFootIKPosition.y - transform.position.y;
        float RightOffsetpos = RightFootIKPosition.y - transform.position.y;

        float TotalOffset = (LeftOffsetpos < RightOffsetpos) ? LeftOffsetpos : RightOffsetpos;

        Vector3 newpelvispos = animator.bodyPosition + Vector3.up * TotalOffset;

        newpelvispos.y = Mathf.Lerp(LastPelvisPosY, newpelvispos.y, PelvisUpDownSPeed);
        animator.bodyPosition = newpelvispos;
        LastPelvisPosY = animator.bodyPosition.y;
    }
    //
    private void FeetPositionSolver(Vector3 SkyPosition, ref Vector3 FeetIkPositions, ref Quaternion FeetIKRotations)
    {
        RaycastHit FeetOutHit;
        if (ShowDebug)
        {
            Debug.DrawLine(SkyPosition, SkyPosition + Vector3.down * (RaycastDownDistance + HeightFromGroundRaycast), Color.yellow);
        }
        if (Physics.Raycast(SkyPosition, Vector3.down, out FeetOutHit, RaycastDownDistance + HeightFromGroundRaycast, EnvironmentMask))
        {
            FeetIkPositions = SkyPosition;
            FeetIkPositions.y = FeetOutHit.point.y + PelvisOffset;
            FeetIKRotations = Quaternion.FromToRotation(Vector3.up, FeetOutHit.normal) * transform.rotation;

            return;
        }

        //FeetIkPositions = Vector3.zero;
    }
    //
    private void AdjustFeetTarget(ref Vector3 FeetPositions, HumanBodyBones Foot)
    {
        FeetPositions = animator.GetBoneTransform(Foot).position;
        FeetPositions.y = transform.position.y + HeightFromGroundRaycast;

    }
}
