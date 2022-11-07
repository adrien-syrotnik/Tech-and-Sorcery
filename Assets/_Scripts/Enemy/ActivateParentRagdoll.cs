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

    public void GiveDamage(float damage)
    {
        if(rb.isKinematic)
        {
            Robots robots = GetComponentInParent<Robots>();
            robots.TakeDamage(damage);
        }
        
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        GiveDamage giveDamage = collision.gameObject.GetComponent<GiveDamage>();
        if (giveDamage != null)
        {
            GiveDamage(giveDamage.damage);
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        GiveDamage giveDamage = other.gameObject.GetComponent<GiveDamage>();
        if (giveDamage != null)
        {
            GiveDamage(giveDamage.damage);
        }
    }
}
