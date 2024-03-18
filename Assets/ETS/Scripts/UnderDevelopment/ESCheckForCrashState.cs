using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCheckForCrashState : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    [Header("OPtimization")]
    public bool OptimizeForMobile = false;
    public float VeiwDistance = 300f;
    public float UpdateSpeed = 5f;
    public float RidOfCrashedVehicleDelay = 10f;
    public float RidOfCrashedVehicleDistance = 20f;
    //[SerializeField]
    private Transform player;
    //[SerializeField] 
    [HideInInspector] public MeshRenderer mesh;
    float c;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (mesh == null)
            mesh = transform.GetComponentInChildren<MeshRenderer>();

        //
        if (OptimizeForMobile)
            InvokeRepeating("UpdateVehicleState", 1f, UpdateSpeed);
    }
    void Update()
    {
        CheckForFlip();
        OPtimization();
    }
    void CheckForFlip()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            this.gameObject.SetActive(false);
        }
        //check for side ways
        else if (Mathf.Abs(Vector3.Dot(transform.up, Vector3.down)) < 0.125f)
        {
            this.gameObject.SetActive(false);
        }
        else if (Mathf.Abs(Vector3.Dot(transform.right, Vector3.down)) > 0.825f)
        {
            this.gameObject.SetActive(false);
        }
        //
    }
    //
    void OPtimization()
    {
        //gets rid of AI that gets stucked
        if (mesh == null) return;
        if (player == null) return;
        if (this.GetComponent<ESVehicleAI>() == null) return;
        if (this.GetComponent<ESVehicleAI>().currentspeed > 0.17f) { c = 0.0f; return; }
        if (UCTMath.CalculateVector3Distance(player.position, this.transform.position) > RidOfCrashedVehicleDistance * RidOfCrashedVehicleDistance)
        {
            if (!mesh.isVisible)
            {
                if (c < RidOfCrashedVehicleDelay)
                {
                    c += Time.deltaTime;
                }
                else
                {
                    c = 0.0f;
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                c = 0.0f;
            }
        }
        else
        {
            c = 0.0f;
        }
    }
    //
    void UpdateVehicleState()
    {
        if (mesh == null) return;
        if (player == null) return;
        if (UCTMath.CalculateVector3Distance(player.position, this.transform.position) > VeiwDistance * VeiwDistance)
        {
            if (!mesh.isVisible)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
