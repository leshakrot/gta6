using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testbones : MonoBehaviour
{
    public List<Transform> TransJoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody[] bodys = this.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody r in bodys)
        {
            if (r.transform != transform)
                continue;
            TransJoints.Add(r.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
