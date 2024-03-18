using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCrossPlatformInputManager : MonoBehaviour
{
    /* this class is something we are working on and would be fully functional as we progress
    */

    // Start is called before the first frame update
    public float GetAxis(string AxisName, bool IsRaw)
    {
        float axis = IsRaw == false ? Input.GetAxis(AxisName) : Input.GetAxisRaw(AxisName);
        return axis;
    }
    //
    public bool GetKey(KeyCode keycode)
    {
        bool keystatus = Input.GetKey(keycode);
        return keystatus;
    }
    //
    public bool GetKeyDown(KeyCode keycode)
    {
        bool keystatus = Input.GetKeyDown(keycode);
        return keystatus;
    }
}
