using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class Ul_MotorCycleGearShift : MonoBehaviour
{
    public int CurrentGear;
    [Range(0, 7)] public int MaxGear = 4;
    [HideInInspector] public float PreviousGearRatio;
    public float CurrentGearRatio;
    public float EngineRpm;
    [HideInInspector] public int InverseGear;

    public float[] gearRatios;
    [HideInInspector] public int CurrentMaxGear;
    public UL_MotorCycleController motorCycleController;
    public float MaxSpeed;
    [Tooltip("Do not touch unless needed to")]
    public float ShiftUpSpeed, ShiftDownSpeed;

    // Start is called before the first frame update
    void Start()
    {
        motorCycleController = this.GetComponent<UL_MotorCycleController>();
        motorCycleController.TopSpeed = MaxSpeed;
        CurrentGearRatio = gearRatios[0];
        PreviousGearRatio = 1;
        CurrentGear = 1;
        ShiftUpSpeed = MaxSpeed * CurrentGearRatio;
        ShiftDownSpeed = 0.0f;
        InverseGear = MaxGear;
        if (Application.isPlaying)
            motorCycleController.TopSpeed = ShiftUpSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateEngineRPM();
        CalculateGearRatio();
        ShifterUP();
        ShifterDown();
    }
    //
    void OnDisable()
    {
        motorCycleController.TopSpeed = MaxSpeed;
    }
    //
    void CalculateEngineRPM()
    {
        if (motorCycleController == null) return;
        // EngineRpm = EngineRpm <= MaxEngineRpm ? EngineRpm = vehiclecontroller.CurrentSpeed * inverseGearRatio : EngineRpm;
        EngineRpm = motorCycleController.CurrentSpeed * PreviousGearRatio;
    }
    //
    void CalculateGearRatio()
    {
        if (CurrentMaxGear != MaxGear)
        {
            System.Array.Resize(ref gearRatios, MaxGear);
            for (int i = 0; i < gearRatios.Length; i++)
            {
                gearRatios[i] = (float)(i + 1) / (float)MaxGear;
            }
            CurrentMaxGear = MaxGear;
        }
        //
    }
    //
    //
    void ShifterUP()
    {
        if (motorCycleController == null || motorCycleController.Accel < 0) return;
        if (motorCycleController.CurrentSpeed > (ShiftUpSpeed - 2))
        {
            ShiftDownSpeed = ShiftUpSpeed;
            if (CurrentGear < MaxGear)
            {
                InverseGear--;
                CurrentGear++;
            }
            CurrentGearRatio = gearRatios[CurrentGear - 1];
            PreviousGearRatio = gearRatios[InverseGear - 1];
            ShiftUpSpeed = MaxSpeed * CurrentGearRatio;
            if (Application.isPlaying)
                motorCycleController.TopSpeed = ShiftUpSpeed;
        }
    }
    void ShifterDown()
    {
        if (motorCycleController == null) return;
        if (motorCycleController.CurrentSpeed < (ShiftDownSpeed - 2))
        {
            ShiftUpSpeed = ShiftDownSpeed;
            if (CurrentGear > 1)
            {
                InverseGear++;
                CurrentGear--;
            }
            CurrentGearRatio = gearRatios[CurrentGear - 1];
            PreviousGearRatio = gearRatios[InverseGear - 1];
            ShiftDownSpeed = MaxSpeed * CurrentGearRatio;
            if (Application.isPlaying)
                motorCycleController.TopSpeed = ShiftUpSpeed;
        }
    }
}
