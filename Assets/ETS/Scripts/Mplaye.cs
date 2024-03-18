using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mplaye : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * 30;
    }
}
