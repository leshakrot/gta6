using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ul_Suspension : MonoBehaviour
{
    public float WheelRaduis = 0.25f;
    public float RestLenght = 0.3f;
    public float SuspensionDistance = 0.02f;
    public float SpringConstant = 20000f;
    public float DamperConstant = 2000f;
    public bool m_isgrounded = false;
    public float MotorTorque;
    public float RollTorque;
    public float mul;
    [HideInInspector] public RaycastHit wheelhit;
    public Rigidbody MotorRb;
    public float forwardslip;
    //public float TractionMultiplier = 100f;
    public float TotalDownWardForce;
    //public float Currentdownforce;
    public float wheelheight = 1.13f;
    private float PreLenght;

    private float CurrentLenght;
    private float SpringVelocity;
    private float SpringForce;
    private float Damperforce;
    private Vector3 totalforce;
    Vector3 bacpos;
    Transform WheelMesh;
    private Vector3 m_previouspos;
    // Start is called before the first frame update
    private void Start()
    {
        if (this.transform.parent.GetComponent<Rigidbody>() != null)
            MotorRb = this.transform.parent.GetComponent<Rigidbody>();
        else if (this.transform.parent.parent.GetComponent<Rigidbody>() != null)
            MotorRb = this.transform.parent.parent.GetComponent<Rigidbody>();
        else
            MotorRb = this.transform.root.GetComponent<Rigidbody>();

        bacpos = transform.position;
        wheelheight = WheelRaduis + RestLenght;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Transform RayTrans = transform;
        Gizmos.DrawLine(RayTrans.position, transform.position + (-transform.up) * (RestLenght + WheelRaduis));
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        //Lpos = LeftFootIKTarget.TransformPoint(Vector3.zero);
        Vector3 RayTrans = transform.TransformPoint(Vector3.zero);
        m_isgrounded = Physics.Raycast(RayTrans, -transform.up, out wheelhit, (wheelheight), Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (m_isgrounded)
        {
            if (WheelMesh != null)
                WheelMesh.position = wheelhit.point + (transform.up * wheelheight);
        }
        else
        {
            if (WheelMesh != null)
            {
                Vector3 wheelpoint = transform.position;
                WheelMesh.position = transform.position - (transform.up * SuspensionDistance);
            }
        }

        if (m_isgrounded)
        {
            SuspensionForce(wheelhit);
            ApplyTorque();
        }
        ApplySteer();
    }
    //
    private void SuspensionForce(RaycastHit wheelhit)
    {
        PreLenght = CurrentLenght;
        CurrentLenght = RestLenght - (wheelhit.distance - WheelRaduis);
        SpringVelocity = (CurrentLenght - PreLenght) / Time.fixedDeltaTime;
        SpringForce = SpringConstant * CurrentLenght;
        Damperforce = DamperConstant * SpringVelocity;
        //
        //Debug.DrawRay(transform.position, -transform.up * (RestLenght + WheelRaduis), Color.red);
        totalforce = transform.up * (SpringForce + Damperforce);

        MotorRb.AddForceAtPosition(totalforce, transform.position);
    }
    //
    private void ApplyTorque()
    {
        //MotorRb.AddForceAtPosition(transform.forward * MotorTorque, transform.position);
        MotorRb.AddRelativeForce(Vector3.forward * MotorTorque);
        //
    }
    //
    public void ApplyBrake(float BrakeTorque, float Dir)
    {
        MotorRb.AddRelativeForce(Vector3.forward * BrakeTorque * -Dir);
    }
    //
    private void ApplySteer()
    {
        //
        MotorRb.AddRelativeTorque(Vector3.up * RollTorque);
        //Quaternion.AngleAxis(SteerAngle, Vector3.up);
        //MotorRb.AddForceAtPosition(transform.right * RollTorque, transform.position);
    }
    //
    public void GetWorldPos(Transform _WheelMesh, float angle, float Acceleration)
    {

        //
        WheelMesh = _WheelMesh;
        Vector3 velocity = (transform.position - m_previouspos) / Time.deltaTime;
        m_previouspos = transform.position;
        //
        Vector3 foward = transform.forward;
        Vector3 sideways = -transform.right;
        //
        Vector3 fwdvelocity = Vector3.Dot(velocity, foward) * foward;
        Vector3 sidewayvelocity = Vector3.Dot(velocity, sideways) * sideways;
        //
        forwardslip = -Mathf.Sign(Vector3.Dot(foward, fwdvelocity) * fwdvelocity.magnitude + (TotalDownWardForce * Mathf.PI / 180.0f * 10f));

        //print(forwardslip);
        if (MotorTorque == 0)
        {
            if (Acceleration > 0)
                TotalDownWardForce += MotorRb.velocity.sqrMagnitude;
            else if (Acceleration < 0)
                TotalDownWardForce -= MotorRb.velocity.sqrMagnitude;

        }
        //        print(MotorRb.velocity.sqrMagnitude);
        TotalDownWardForce += MotorTorque / WheelRaduis / 10;
        WheelMesh.localEulerAngles = new Vector3(TotalDownWardForce, 0, 0);

        //WheelMesh.Rotate(TotalDownWardForce, 0, 0);
    }
}
