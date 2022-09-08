using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robots : MonoBehaviour
{

    Rigidbody[] rigidBodies;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        //Add ActivateRagdoll fonction on collider to rigidbodies components
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.gameObject.AddComponent<ActivateParentRagdoll>();
        }


        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        //animator.enabled = true;
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = true;
        }
    }

    public void ActivateRagdoll(Collision collision)
    {
        animator.enabled = false;
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = false;
        }
    }
}
