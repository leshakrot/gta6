using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESPedestrainCrossing : MonoBehaviour
{
    public bool TellHumansToStop = true;
    public bool ReturnCarCalls = false;
    [HideInInspector]
    public float Delay;

    private void Start()
    {
        // InvokeRepeating("Calls", 0.1f, (Delay * 2));
    }
    private void Calls()
    {
        ReturnCarCalls = !ReturnCarCalls;
    }
    //
}
