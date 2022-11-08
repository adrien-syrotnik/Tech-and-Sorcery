using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{

    public float speed = 1f;
    public float rotationSpeed = 1f;
    public float lifeTime = 5f;
    public float damage = 0.1f;
    public float radius = 1f;
    public float force = 1f;
    public float upForce = 1f;


    private void Start()
    {
        Destroy(gameObject, lifeTime);
        //Rise up
        transform.position += Vector3.up;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius, upForce, ForceMode.Impulse);
            }
            ICanTakeDamage canTakeDamageObject = nearbyObject.GetComponent<ICanTakeDamage>();
            if (canTakeDamageObject != null)
            {
                canTakeDamageObject.TakeDamage(damage);
            }
        }

    }
}
