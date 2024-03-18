using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UL_MotorCycleControl : MonoBehaviour
{
    [SerializeField] private ESCrossPlatformInputManager crossPlatformInputManager;
    private UL_MotorCycleController motorCycleController;
    // Start is called before the first frame update
    void Start()
    {
        crossPlatformInputManager = GameObject.FindObjectOfType<ESCrossPlatformInputManager>();
        motorCycleController = this.GetComponent<UL_MotorCycleController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (motorCycleController == null) return;

        motorCycleController.MotorCycleControl(crossPlatformInputManager);
    }
}
