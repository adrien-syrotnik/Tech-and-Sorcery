using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : GiveDamage
{
    public float duration = 0.5f;
    public float impactTime = 0.1f;

    public float explosionForce = 1000f;
    public float explosionRadius = 15f;


    // Start is called before the first frame update
    void Start()
    {
        damage = 5;
        StartCoroutine(ExplosionImpact());
    }

    private IEnumerator ExplosionImpact()
    {
        yield return new WaitForSeconds(impactTime);

        //Add explosion force
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider collider in colliders)
        {
            ActivateParentRagdoll activate = collider.GetComponent<ActivateParentRagdoll>();
            if (activate != null)
            {
                activate.GiveDamage(damage);
            }

            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 2f, ForceMode.Impulse);
            }

            Debug.Log(collider.name);
        }

        Destroy(gameObject,duration + 1f);
    }
}
