using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantForce : MonoBehaviour
{
    public Vector3 force;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        rb.AddForce(force);

    }
}
