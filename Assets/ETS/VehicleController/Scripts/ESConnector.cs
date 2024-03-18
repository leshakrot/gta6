using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class ESConnector : MonoBehaviour
{
    public enum BodyType
    {
        Container,
        Head
    }
    //Container Setup
    [Tooltip("apply settings in edit mode")]
    public Vector3 ContainerStartPos;
    //head
    [Header("Head Settings")]
    [Tooltip("this settings affects only the ESconnector attached the Truck's head")]
    public HingeJoint ContainerHinge;
    [Header("Container Settings")]
    [Tooltip("this settings affects only the ESconnector attached the Truck's head")]
    public bool UseLimits = true;
      [Tooltip("this settings affects only the ESconnector attached the Truck's head")]
    public float Range = 90.0f;
    [HideInInspector]
    [Tooltip("this settings affects only the ESconnector attached the Truck's head")]
    public Rigidbody HeadRigidBody;
    //general
    [Header("General Settings")]
    [Tooltip("this settings affects only the ESconnector attached to both")]
    public SphereCollider Connector;
    [Tooltip("this settings affects only the ESconnector attached to both")]
    public float raduis = 0.2f;
     [Tooltip("this settings affects only the ESconnector attached to both")]
    public BodyType _bodytype = BodyType.Head;
     [Tooltip("this settings affects only the ESconnector attached to both")]
    public Vector3 Center = Vector3.zero;
      [Tooltip("this settings affects only the ESconnector attached to both")]
    public KeyCode DetachKey = KeyCode.F;

    // Start is called before the first frame update
    void Start()
    {
        //if(_bodytype == BodyType.Container)
        //ContainerStartPos = this.transform.localPosition;
      
    }
    float clampang(float angle, float from, float to)
    {

        if (angle < 0f)
            angle = 360 + angle;
        if (angle > 180)
            return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
    // Update is called once per frame
    void Update()
    {
        MakeConnectorAvailable();
        Disconnect();
    }
    //
    void MakeConnectorAvailable()
    {
        if (Connector == null)
        {
            Connector = this.gameObject.AddComponent<SphereCollider>();
        }
        //
        if(Connector == null)return;
        Connector.radius = raduis;
        Connector.center = Center;
        Connector.isTrigger = false ? true : true;  
        //
        if (_bodytype == BodyType.Container)
        {
            if (this.GetComponent<HingeJoint>().connectedBody != null)
            {
                Vector3 localeuler = this.transform.localEulerAngles;

                localeuler.y = clampang(localeuler.y, -Range, Range);

                this.transform.localEulerAngles = localeuler;
            }
        }
    }
    //
    void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.GetComponent<ESConnector>()._bodytype == BodyType.Head)
        {
            if (other.gameObject.GetComponent<ESConnector>() != null)
            {
                if(other.gameObject.GetComponent<HingeJoint>() != null)
                {
                    ContainerHinge = other.gameObject.GetComponent<HingeJoint>();
                    ContainerHinge.transform.localPosition = ContainerHinge.GetComponent<ESConnector>().ContainerStartPos;
                    ContainerHinge.connectedBody = HeadRigidBody;
                    
                }
            }
        }
    }
    //
    void Disconnect()
    {
        if (_bodytype == BodyType.Head)
        {
            if (Input.GetKey(KeyCode.M))
            {

                if (ContainerHinge != null)
                {
                    ContainerHinge.connectedBody = null;
                }
            }
        }
    }
    //
}
