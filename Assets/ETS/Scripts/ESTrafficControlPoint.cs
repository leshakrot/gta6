using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESTrafficControlPoint : MonoBehaviour
{
    public TrafficManager[] managers;
    public ESPedestrainCrossing[] pedestrainCrossings;
    [Header("Zebra Crossing Settings")]
    public float ShiftTime = 30f;
    [Header("General Settings")]
    [HideInInspector]
    public bool FullStop = false;
    public float slowdowntime = 2;
    public float cooldowntime = 30.0f;
    [HideInInspector]
    public float cooldowntimer = 0.0f;
    [HideInInspector]
    public int selectedindex = 0;
    [HideInInspector]
    public float S_time;

    //
    private void Start()
    {
        //
        InvokeRepeating("CallStop", 0.01f, ShiftTime);
        cooldowntimer = 0;
        S_time = cooldowntime - slowdowntime;
        if (managers.Length > 0)
        {
            managers[selectedindex].play = true;
            managers[selectedindex].S_time = S_time;
        }
    }
    //
    void CallStop()
    {
        if (pedestrainCrossings.Length == 0) return;
        for (int i = 0; i < pedestrainCrossings.Length; ++i)
        {
            if (pedestrainCrossings[i].ReturnCarCalls)
            {
                FullStop = true;
                pedestrainCrossings[i].Delay = ShiftTime;
                pedestrainCrossings[i].TellHumansToStop = false;
                return;
            }
        }
        FullStop = !FullStop;
        for (int i = 0; i < pedestrainCrossings.Length; ++i)
        {
            if (pedestrainCrossings[i].ReturnCarCalls) return;
            pedestrainCrossings[i].TellHumansToStop = !FullStop;
        }
    }
    //
    private void Update()
    {
        if (FullStop == false)
        {
            if (managers.Length > 0)
            {
                for (int i = 0; i < managers.Length; ++i)
                {
                    managers[i].Stop = false;
                }
            }
            //
            if (cooldowntimer < (cooldowntime + 5))
            {
                cooldowntimer += Time.deltaTime;
            }
            if (cooldowntimer >= cooldowntime)
            {
                if (managers.Length > 0)
                {
                    if (selectedindex <= managers.Length)
                    {
                        if (selectedindex != 0)
                        {
                            if (managers[selectedindex - 1].Lastveh.Count > 0)
                            {
                                return;
                            }
                            managers[selectedindex - 1].play = false;
                        }
                        //
                        if (selectedindex == 0)
                        {
                            if (managers[0].Lastveh.Count > 0)
                            {
                                return;
                            }
                            managers[0].play = false;
                        }
                        //
                        if (selectedindex == managers.Length)
                        {
                            selectedindex = 0;
                        }
                        //
                        selectedindex++;
                        managers[selectedindex - 1].play = true;
                        managers[selectedindex - 1].S_time = S_time;
                    }
                }
                cooldowntimer = 0.0f;
            }
            //
        }
        else
        {
            if (managers.Length > 0)
            {
                for (int i = 0; i < managers.Length; ++i)
                {
                    managers[i].Stop = true;
                }
            }
        }
    }

    //

}
