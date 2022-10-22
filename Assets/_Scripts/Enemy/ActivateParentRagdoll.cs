using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParentRagdoll : MonoBehaviour
{

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ActivateRagdoll()
    {
        if(rb.isKinematic)
        {
            rb.isKinematic = false;
            Robots robots = GetComponentInParent<Robots>();
            robots.ActivateRagdoll();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ActivateRagdoll();
    }
}
