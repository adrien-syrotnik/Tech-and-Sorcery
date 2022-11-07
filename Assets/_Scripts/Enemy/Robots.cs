using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Robots : Enemy
{

    Rigidbody[] rigidBodies;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rigidBodies = GetComponentsInChildren<Rigidbody>();

        //Add ActivateRagdoll fonction on collider to rigidbodies components
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.gameObject.AddComponent<ActivateParentRagdoll>();
            rb.gameObject.AddComponent<XRGrabInteractable>();
        }

        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        //animator.enabled = true;
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = true;
            /*if(rb.GetComponent<XRGrabInteractable>())
                rb.GetComponent<XRGrabInteractable>().enabled = false;*/
        }
    }

    public void ActivateRagdoll()
    {
        animator.enabled = false;
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = false;
/*            if(rb.GetComponent<XRGrabInteractable>())
                rb.GetComponent<XRGrabInteractable>().enabled = true;*/
        }
    }

    protected override void Die()
    {
        base.Die();
        ActivateRagdoll();
    }
}
