using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESRagDollHelper : MonoBehaviour
{
    [Header("AnimationName")]
    [SerializeField] private string FromBelly = "GetUpFromBelly";
    [SerializeField] private string FromBehind = "GetUpFromBehind";
    [Tooltip("Based On The Get UP Animation Length")]
    [SerializeField] private float FreezeControlTime = 2.9f;
    public string BodyState, BoneState = "Null";
    public bool Ragdollz;
    public Transform RagdollTransformCollection;
    private List<Rigidbody> BoneRigids;
    private List<Collider> BoneColliders;
    private Animator animator;
    private CharacterController controller;
    private ES_UniversalCharacterController characterController;
    private CapsuleCollider capsule;
    private Transform BodyHipBone;
    private bool BackPose;
    private List<Vector3> StoredPosition, PrivPosition;
    private List<Quaternion> StoredRotation, PrivRotation;
    private List<Transform> BonesTransformComp;
    private bool BlendRagdollz, callragdollz, startragdollcountdown;
    private string statename;
    float _ragdollingEndTime, StataCountDownTime, ragcount, RagdollDelay = 0.7f;
    const float RagdollToMecanimBlendTime = 0.5f;
    //
    public bool FreezeControl;
    void Start()
    {
        characterController = this.GetComponent<ES_UniversalCharacterController>();
        animator = this.GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        capsule = GetComponent<CapsuleCollider>();
        BodyHipBone = animator.GetBoneTransform(HumanBodyBones.Hips);
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
        Collider[] cols = GetComponentsInChildren<Collider>();
        BoneRigids = new List<Rigidbody>();
        BoneColliders = new List<Collider>();
        BonesTransformComp = new List<Transform>();
        Transform[] Bones = RagdollTransformCollection != null ?
         RagdollTransformCollection.GetComponentsInChildren<Transform>() : GetComponentsInChildren<Transform>();
        foreach (Transform B in Bones)
        {
            if (B != transform)
            {
                BonesTransformComp.Add(B);
            }
        }
        foreach (Rigidbody r in rigids)
        {
            if (r.transform != transform)
                BoneRigids.Add(r);
        }
        //
        foreach (Collider c in cols)
        {
            if (c.transform != transform)
                BoneColliders.Add(c);
        }
    }
    void FixedUpdate()
    {
        //
        RagdollPhysics(Ragdollz);
        checkForStaticBone();
        if (Ragdollz)
        {
            FreezeControl = true;
        }
        if (controller != null)
        {
            if (FreezeControl)
            {
                if (!Ragdollz)
                {
                    if (StataCountDownTime < FreezeControlTime)
                    {
                        StataCountDownTime += Time.deltaTime;
                    }
                    else
                    {
                        if (this.GetComponent<ES_UniversalCharacterController>() != null)
                            this.GetComponent<ES_UniversalCharacterController>().ReturnControls = false;
                        StataCountDownTime = 0.0f;
                        FreezeControl = false;
                    }
                }
            }
            //
            if (Ragdollz == false)
            {
                if (startragdollcountdown)
                {
                    //ESRagDollHelper ragDollHelper = Player.GetComponent<ESRagDollHelper>();
                    ragcount += Time.deltaTime;
                    if (ragcount > RagdollDelay && BoneState == "Calm")
                    {
                        //Animator animator = Player.GetComponent<Animator>();
                        animator.applyRootMotion = false;

                        //ragDollHelper.Ragdollz = false;
                        startragdollcountdown = false;
                    }
                }
                else
                {
                    ragcount = 0;
                }
            }
        }

    }
    void LateUpdate()
    {
        //
        BlendRagdollzToMecanim();
    }
    void BlendRagdollzToMecanim()
    {
        if (BlendRagdollz == false) return;
        float ragdollBlendAmount = 1f - Mathf.InverseLerp(
             _ragdollingEndTime,
             _ragdollingEndTime + RagdollToMecanimBlendTime,
             Time.time);

        //        print(ragdollBlendAmount);
        if (BackPose)
        {

            for (int i = 0; i < BonesTransformComp.Count; ++i)
            {
                if (PrivRotation[i] != BonesTransformComp[i].localRotation)
                {
                    PrivRotation[i] = Quaternion.Slerp(BonesTransformComp[i].localRotation, StoredRotation[i], ragdollBlendAmount);
                    BonesTransformComp[i].localRotation = PrivRotation[i];
                }
                if (PrivPosition[i] != BonesTransformComp[i].localPosition)
                {
                    PrivPosition[i] = Vector3.Slerp(BonesTransformComp[i].localPosition, StoredPosition[i], ragdollBlendAmount);
                    BonesTransformComp[i].localPosition = PrivPosition[i];
                }
            }
        }
        if (Mathf.Abs(ragdollBlendAmount) < Mathf.Epsilon)
        {
            BackPose = false;
            BlendRagdollz = false;
        }
    }

    private bool CheckIfLieOnBack()
    {
        var left = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position;
        var right = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).position;
        var hipsPos = BodyHipBone.position;

        left -= hipsPos;
        left.y = 0f;
        right -= hipsPos;
        right.y = 0f;

        var q = Quaternion.FromToRotation(left, Vector3.right);
        var t = q * right;

        return t.z < 0f;
    }
    void GetRagdollPosition()
    {
        StoredPosition = new List<Vector3>();
        StoredRotation = new List<Quaternion>();
        PrivPosition = new List<Vector3>();
        PrivRotation = new List<Quaternion>();
        foreach (Transform T in BonesTransformComp)
        {
            StoredPosition.Add(T.localPosition);
            PrivPosition.Add(T.localPosition);
            StoredRotation.Add(T.localRotation);
            PrivRotation.Add(T.localRotation);
        }
    }

    void GetUp()
    {
        if (BoneState == "Calm" && !Ragdollz)
        {
            _ragdollingEndTime = Time.time;
            BackPose = true;
            BlendRagdollz = true;
            animator.enabled = true;
            Vector3 shiftPos = BodyHipBone.position - transform.position;
            CorrectCharacterControllerCoordinate(shiftPos);
            GetRagdollPosition();
            if (characterController.GetCharacterType == ES_UniversalCharacterController.CharacterType.FirstMan)
            {
                if (characterController.FirstPersonBodyPartsMeshRender.Count > 0)
                {
                    for (int i = 0; i < characterController.FirstPersonBodyPartsMeshRender.Count; ++i)
                    {
                        characterController.FirstPersonBodyPartsMeshRender[i].enabled = true;
                    }
                }
            }
            statename = CheckIfLieOnBack() ? FromBehind : FromBelly;
            animator.Play(statename, 0, 0);
            StataCountDownTime = 0.0f;
            if (controller != null)
                controller.enabled = true;
            if (capsule != null)
                capsule.enabled = true;
            if (this.GetComponent<ES_UniversalCharacterController>() != null)
            {
                this.GetComponent<ES_UniversalCharacterController>().enabled = true;
                this.GetComponent<ES_UniversalCharacterController>().ReturnControls = true;
            }
            BoneState = "Null";
        }
        if (BoneState != "Null")
            return;
    }
    //

    void checkForStaticBone()
    {
        GetUp();
        if (!Ragdollz) return;
        Rigidbody hiprigid = BodyHipBone.GetComponent<Rigidbody>();
        if (hiprigid.velocity.sqrMagnitude < 0.01f)
            BoneState = "Calm";
        else
            BoneState = "Jerky";
    }
    bool ShootAray()
    {
        RaycastHit hit;
        bool CheckifFacedDown = Physics.Raycast(BodyHipBone.transform.position, BodyHipBone.transform.forward, out hit, 10);
        Debug.DrawLine(BodyHipBone.transform.position, hit.point, Color.red);
        return CheckifFacedDown;
    }
    //
    void RagdollPhysics(bool Status)
    {
        if (BoneRigids.Count == 0 || BoneColliders.Count == 0) return;
        foreach (Rigidbody r in BoneRigids)
        {
            r.isKinematic = !Status;
        }
        //
        foreach (Collider c in BoneColliders)
        {
            c.isTrigger = !Status;
            c.enabled = Status;
        }
        if (Ragdollz)
        {
            if (characterController.GetCharacterType == ES_UniversalCharacterController.CharacterType.FirstMan)
            {
                if (characterController.FirstPersonBodyPartsMeshRender.Count > 0)
                {
                    for (int i = 0; i < characterController.FirstPersonBodyPartsMeshRender.Count; ++i)
                    {
                        characterController.FirstPersonBodyPartsMeshRender[i].enabled = false;
                    }
                }
            }
            startragdollcountdown = true;
            animator.enabled = false;
            if (controller != null)
                controller.enabled = false;
            if (capsule != null)
                capsule.enabled = false;
            if (this.GetComponent<ES_UniversalCharacterController>() != null)
                this.GetComponent<ES_UniversalCharacterController>().enabled = false;
        }
    }
    //
    //
    private Vector3 HipBoneDirection()
    {
        Vector3 ragdolledFeetPosition = (
            animator.GetBoneTransform(HumanBodyBones.Hips).position);
        Vector3 ragdolledHeadPosition = animator.GetBoneTransform(HumanBodyBones.Head).position;
        Vector3 ragdollDirection = ragdolledFeetPosition - ragdolledHeadPosition;
        ragdollDirection.y = 0;
        ragdollDirection = ragdollDirection.normalized;
        //
        if (CheckIfLieOnBack())
            return ragdollDirection;
        else
            return -ragdollDirection;
    }
    //

    void CorrectCharacterControllerCoordinate(Vector3 JumpPos)
    {
        Vector3 ragdollDirection = HipBoneDirection();
        BodyHipBone.position -= JumpPos;
        transform.position += JumpPos;


        Vector3 forward = transform.forward;
        transform.rotation = Quaternion.FromToRotation(forward, ragdollDirection) * transform.rotation;
        BodyHipBone.rotation = Quaternion.FromToRotation(ragdollDirection, forward) * BodyHipBone.rotation;
    }
}



