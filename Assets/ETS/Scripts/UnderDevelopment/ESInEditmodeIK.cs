using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class ESInEditmodeIK : MonoBehaviour
{
    [HideInInspector] public Transform sitpoint;
    [HideInInspector] public Transform LeftIkTarget;
    [HideInInspector] public Transform RightIkTarget;
    public Transform LeftFootIkTarget;
    public Transform RightFootIkTarget;
    [HideInInspector] public Transform LookPos;
    [HideInInspector] public float IKWeight = 1;
    [HideInInspector] public float LookWeight = .5f;
    [HideInInspector] public float BodyWeight = 1;
    [HideInInspector] public float HeadWeight = 1;
    [HideInInspector] public float Eyeweight = 0;
    [HideInInspector] public float ClampWeight = 1;
    //[HideInInspector]
    public bool GoIk, pinpoint;
    public Animator animator;

    private void OnAnimatorIK()
    {
        //if (pinpoint)
        //  PinPointSit();
        /*
        if (GoIk == false) return;
        animator.bodyPosition = sitpoint.position;
        animator.bodyRotation = sitpoint.rotation;
        */
        if (GoIk == false) return;
        //set ik weightposition
        animator.bodyPosition = sitpoint.position;
        animator.bodyRotation = sitpoint.rotation;
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);
        //
        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftIkTarget.position);
        animator.SetIKPosition(AvatarIKGoal.RightHand, RightIkTarget.position);
        //
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);
        //
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftFootIkTarget.position);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, RightFootIkTarget.position);
        //
        //set ik weightRotation
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, IKWeight);
        //
        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftIkTarget.rotation);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RightIkTarget.rotation);
        //

        animator.SetLookAtWeight(LookWeight, BodyWeight, HeadWeight, Eyeweight, ClampWeight);
        animator.SetLookAtPosition(LookPos.position);
    }
    //
    private void PinPointSit()
    {
        animator.bodyPosition = sitpoint.position;
        animator.bodyRotation = sitpoint.rotation;
        pinpoint = false;
    }
    //
    private void Update()
    {
        if (animator != null)
            if (GoIk)
                animator.Update(Time.deltaTime);
    }
}
