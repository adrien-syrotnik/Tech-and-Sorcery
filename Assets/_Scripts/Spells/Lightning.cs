using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float duration = 0.5f;
    public float impactTime = 0.1f;

    public float explosionForce = 1000f;
    public float explosionRadius = 15f;

    public float damage = 10;

    public Vector2 pitchAudio = new Vector2(0.8f, 1.2f);


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplosionImpact());

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(pitchAudio.x, pitchAudio.y);
        audioSource.Play();
    }

    private IEnumerator ExplosionImpact()
    {
        yield return new WaitForSeconds(impactTime);

        //Add explosion force
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider collider in colliders)
        {
            ICanTakeDamage canTakeDamageObject = collider.GetComponent<ICanTakeDamage>();
            if (canTakeDamageObject != null)
            {
                canTakeDamageObject.TakeDamage(damage);
            }

            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 2f, ForceMode.Impulse);
            }
        }

        Destroy(gameObject,duration + 2f);
    }
}
