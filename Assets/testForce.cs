using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testForce : MonoBehaviour
{
    public bool explode = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(explode)
        {
            ExplosionForce();
            Destroy(gameObject);
        }
    }

    public void ExplosionForce()
    {
        
        
        //Get all rigidbodies arround the explosion
        Collider[] colliders = Physics.OverlapSphere(transform.position, 35f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(1000f, transform.position, 35f, 3.0F, ForceMode.Impulse);
        }
    }
}
