using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_H_traffcsystem : MonoBehaviour
{
    /*
    this class has been deprecated and will not be updated but we have left this behind incase of customers who are making use of this 
    THANKS FOR CHOOSING UTC.
    */
    public float stoplenght = 35f;
    public float walktime = 2f;
    private float counter;
    [HideInInspector] public bool wait = true, go = false;
    void FixedUpdate()
    {
        counter += Time.deltaTime;
        if (wait == true)
        {
            if (counter > stoplenght)
            {
                wait = false;
                go = true;
                counter = 0;

            }

        }
        if (go)
        {
            if (counter > walktime)
            {
                wait = true;
                go = false;
                counter = 0;

            }
        }
    }
}
