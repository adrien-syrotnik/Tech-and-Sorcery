using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindUp : MonoBehaviour
{
    public float lifeTime = 5f;
    public float damage = 0.5f;
    public float radius = 1f;
    public float upForce = 30f;
    public float height = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Add force to all colliders in radius in a column
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(radius, height, radius));

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
                ActivateParentRagdoll activateParentRagdoll = rb.GetComponent<ActivateParentRagdoll>();
                if (activateParentRagdoll != null)
                {
                    activateParentRagdoll.GiveDamage(damage);
                }
            }
        }

    }
}
