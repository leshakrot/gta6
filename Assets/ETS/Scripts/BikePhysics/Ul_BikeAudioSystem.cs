using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ul_BikeAudioSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audiosource;
    public float PitchModifier = 0.5f, PitchMultiplier = 100;
    public UL_MotorCycleController motorCycleController;
    public Ul_MotorCycleGearShift gearShift;
    void Start()
    {
        audiosource = this.GetComponent<AudioSource>();
        motorCycleController = this.GetComponent<UL_MotorCycleController>();
        gearShift = this.GetComponent<Ul_MotorCycleGearShift>();
        audiosource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (audiosource == null) return;
        if (gearShift == null) return;

        audiosource.pitch = (gearShift.EngineRpm / PitchMultiplier) + PitchModifier;
    }
}
