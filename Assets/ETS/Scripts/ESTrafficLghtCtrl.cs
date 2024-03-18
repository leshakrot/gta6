using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESTrafficLghtCtrl : MonoBehaviour
{
    public GameObject RedLight;
    public GameObject Greenlight;
    public GameObject YellowLight;
    [HideInInspector]
    public bool simulate = false;
    [HideInInspector]
    public float slowdowntime;
    [HideInInspector]
    public Transform LastVeh;
    public float DistApartLastVeh = 50f;
    [HideInInspector]

    public bool red, yellow, green;
    private float Timer;
    [HideInInspector]
    public bool addtoarray = false, Halt;
    public bool UseLights = true;
    // Start is called before the first frame update
    private void Start()
    {
        Timer = 0.0f;
    }
    //
    private void LateUpdate()
    {
        CheckForLastVehicle();
    }
    //
    private void FixedUpdate()
    {
        //
        if (!Halt)
        {
            if (simulate)
            {
                ESController();
            }
            else
            {
                Timer = 0.0f;
                red = true;
                yellow = false;
                green = false;
                //
                if (Greenlight != null)
                    if (UseLights)
                    {
                        Greenlight.SetActive(green);
                        YellowLight.SetActive(yellow);
                        RedLight.SetActive(red);
                    }
                    else
                    {
                        if (Greenlight != null)
                            Greenlight.SetActive(false);
                        if (YellowLight != null)
                            YellowLight.SetActive(false);
                        if (RedLight != null)
                            RedLight.SetActive(false);
                    }
            }
        }
        else
        {

            if (UseLights)
            {
                if (Greenlight != null)
                {
                    Greenlight.SetActive(false);
                    YellowLight.SetActive(false);
                    RedLight.SetActive(true);
                }
                //
                red = true;
                yellow = false;
                green = false;
            }
        }
    }
    //
    private void ESController()
    {
        if (Halt == true) return;
        Timer += Time.deltaTime;
        if (slowdowntime > Timer)
        {
            green = true;
            yellow = false;
            red = false;
        }
        else
        {
            green = false;
            yellow = true;
            red = false;
        }
        //
        if (Greenlight != null)
            if (UseLights)
            {
                Greenlight.SetActive(green);
                YellowLight.SetActive(yellow);
                RedLight.SetActive(red);
            }
            else
            {
                Greenlight.SetActive(false);
                YellowLight.SetActive(false);
                RedLight.SetActive(false);
            }
    }
    private float CalculateDistance(Vector3 currentposition, Vector3 targetposition)
    {
        Vector3 offset = targetposition - currentposition;
        float sqrlen = offset.sqrMagnitude;
        return sqrlen;
    }
    //
    private void CheckForLastVehicle()
    {
        if (LastVeh == null) return;
        if (CalculateDistance(this.transform.position, LastVeh.transform.position) > DistApartLastVeh * DistApartLastVeh)
        {
            addtoarray = false;
            LastVeh = null;
        }
    }
}
