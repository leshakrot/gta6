using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Burst;

[BurstCompile]
public class ESPedestrainCtrl : MonoBehaviour
{
    public enum NavigateType
    {
        ByRigidBodyVelocity,
        ByRootMotion
    }
    public NavigateType GetNavigateType = NavigateType.ByRootMotion;

    [Header("SensorSettings")]
    public float distance = 2f;
    [Header("End ;-])")]
    public float speed;
    //
    public bool IgnoreCollsion = false;
    public Animator anim;
    public Transform RagdollSkeleton;
    public Transform MainSkeleton;
    public Transform target;
    public float WalkSpeed = .5f;
    public float DistApartFromPlayer = 600f;
    public Transform Player;
    //hide me
    [HideInInspector]
    public bool Terminated = false;
    [HideInInspector]
    public bool StopUpdateTarget, flip;
    public float UpdateDistance, PlayerDist;
    public Rigidbody _rigidbody;
    //end
    private float f;
    private ES_H_traffcsystem HTF;
    private GameObject Parent_AI;
    private List<Rigidbody> BoneRigids = new List<Rigidbody>();
    private List<Collider> BoneColliders = new List<Collider>();
    public bool StopBody = false;
    //
    private void Start()
    {
        anim = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
        MainSkeleton.gameObject.SetActive(true);
        RagdollSkeleton.gameObject.SetActive(false);
        _rigidbody.detectCollisions = !IgnoreCollsion;
        if (GetNavigateType == NavigateType.ByRootMotion)
        {
            if (anim != null)
                anim.applyRootMotion = true;
        }
        else
        {
            if (anim != null)
                anim.applyRootMotion = false;
        }
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        f = speed;
        Parent_AI = GameObject.Find("H_AI");
        if (Parent_AI == null)
        {
            GameObject Human = new GameObject("H_AI");
            Parent_AI = Human;
        }
        //
        this.transform.parent = Parent_AI.transform;
        //
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
        Collider[] cols = GetComponentsInChildren<Collider>();
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
        //
        DisableRagdsaollz();
    }
    //
    private void Update()
    {
        if (!Terminated)
        {
            HumanAIbehavoir();
            // ParentCharacter();
        }
    }
    //
    //
    IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);
        DisableRagdsaollz();
        StopBody = false;
        this.gameObject.SetActive(false);
    }
    //
    private void HumanAIbehavoir()
    {
        //
        UpdateTarget();
        if (PlayerDist > DistApartFromPlayer)
        {
            this.gameObject.SetActive(false);
        }
    }
    //
    private void EnableRagdollz()
    {
        if (anim != null)
            anim.enabled = false;
        //
        MainSkeleton.gameObject.SetActive(false);
        RagdollSkeleton.gameObject.SetActive(true);
        Terminated = true;
        _rigidbody.isKinematic = true;

        foreach (Rigidbody r in BoneRigids)
        {
            r.isKinematic = false;
        }
        //
        foreach (Collider c in BoneColliders)
        {
            c.isTrigger = false;
            c.enabled = true;
        }
        this.GetComponent<CapsuleCollider>().enabled = false;
    }
    //
    private void DisableRagdsaollz()
    {
        if (anim != null)
            anim.enabled = true;

        MainSkeleton.gameObject.SetActive(true);
        RagdollSkeleton.gameObject.SetActive(false);
        _rigidbody.isKinematic = false;
        this.GetComponent<CapsuleCollider>().enabled = true;
        foreach (Rigidbody r in BoneRigids)
        {
            r.isKinematic = true;
        }
        //
        foreach (Collider c in BoneColliders)
        {
            c.isTrigger = true;

            c.enabled = false;
        }
        Terminated = false;
    }
    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("CommonTrigger"))
        {
            //print("Player");
            Rigidbody myrigid = other.GetComponent<Rigidbody>();
            if (myrigid != null)
            {
                if (myrigid.velocity.magnitude > 2f)
                {
                    EnableRagdollz();
                    StartCoroutine(Die());
                }
            }
        }
        else if (other.transform.root.name == "AI")
        {
            if (other.attachedRigidbody != null)
            {
                Rigidbody myrigid = other.attachedRigidbody;
                if (myrigid.velocity.magnitude > 2f)
                {
                    EnableRagdollz();
                    StartCoroutine(Die());
                }
            }
        }
    }
    //
    private void OnTriggerStay(Collider other)
    {
        //might get expensive.
        if (other.GetComponent<ESPedestrainCrossing>() != null)
        {
            if (other.GetComponent<ESPedestrainCrossing>().TellHumansToStop)
                StopBody = true;
            else
                StopBody = false;
        }
        if (other.GetComponent<ESPedestrainCtrl>() != null)
        {
            other.GetComponent<ESPedestrainCtrl>().StopBody = StopBody;
        }
    }
    //
    //
    private void UpdateTarget()
    {
        if (Terminated) return;
        if (UpdateDistance < distance)
        {
            if (target.GetComponent<ESHumanNode>() == null) return;
            if (target.GetComponent<ESHumanNode>().NextNode == null)
            {
                flip = true;
            }
            else if (target.GetComponent<ESHumanNode>().PreviousNode == null)
            {
                flip = false;
            }
            target = flip == false ? target.GetComponent<ESHumanNode>().NextNode : target.GetComponent<ESHumanNode>().PreviousNode;
        }
        //
        //
        float speed = new float();
        speed = StopBody ? 0.0f : WalkSpeed;
        if (GetNavigateType == NavigateType.ByRootMotion)
            anim.SetFloat("Forward", speed, 0.1f, Time.deltaTime);
        else
        {
            Vector3 movevelocity = transform.forward * speed * Time.deltaTime;
            movevelocity.y = _rigidbody.velocity.y;
            if (movevelocity.magnitude > 1f) movevelocity.Normalize();
            _rigidbody.velocity = movevelocity;
            anim.SetFloat("Forward", movevelocity.magnitude * 0.5f, 0.1f, Time.deltaTime);
            //if (_rigidbody.velocity.magnitude > 1f) _rigidbody.velocity.Normalize();
            //if (anim != null)
            // anim.SetFloat("Forward", movevelocity.magnitude * 0.5f, 0.1f, Time.deltaTime);
        }
        //_rigidbody.AddForce(transform.forward * WalkSpeed * Time.deltaTime);
    }
    //
}
