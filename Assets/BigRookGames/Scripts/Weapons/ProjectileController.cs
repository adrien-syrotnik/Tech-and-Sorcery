using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileController : MonoBehaviour
{
    // --- Config ---
    public float speed = 100;
    public LayerMask collisionLayerMask;

    // --- Explosion VFX ---
    public GameObject rocketExplosion;

    // --- Projectile Mesh ---
    public MeshRenderer projectileMesh;

    // --- Script Variables ---
    private bool targetHit;

    // --- Audio ---
    public AudioSource inFlightAudioSource;

    // --- VFX ---
    public ParticleSystem disableOnHit;


    public float radius = 5f;
    public float force = 700f;
    public float damage = 5f;

    private void Start()
    {
        //Add force tp launch the projectile
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        // --- Check to see if the target has been hit. We don't want to update the position if the target was hit ---
        if (targetHit) return;

        
    }


    /// <summary>
    /// Explodes on contact.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

        //If not in layer return
        if (!((collisionLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)) return;

        // --- return if not enabled because OnCollision is still called if compoenent is disabled ---
        if (!enabled) return;

        // --- Explode when hitting an object and disable the projectile mesh ---
        Explode();
        projectileMesh.enabled = false;
        targetHit = true;
        inFlightAudioSource.Stop();
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }
        disableOnHit.Stop();


        // --- Destroy this object after 2 seconds. Using a delay because the particle system needs to finish ---
        Destroy(gameObject, 5f);
    }


    /// <summary>
    /// Instantiates an explode object.
    /// </summary>
    private void Explode()
    {
        // --- Instantiate new explosion option. I would recommend using an object pool ---
        GameObject newExplosion = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);

        //Deal damage
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius, force / 2);
            }
            ICanTakeDamage canTakeDamageObject = nearbyObject.GetComponent<ICanTakeDamage>();
            if (canTakeDamageObject != null)
            {
                canTakeDamageObject.TakeDamage(damage);
            }


        }


    }
}
