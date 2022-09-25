using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParentRagdoll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Get the far parent robots possible
        Robots robots = GetComponentInParent<Robots>();
        robots.ActivateRagdoll(collision);
    }
}
