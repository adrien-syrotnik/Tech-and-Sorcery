using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Robots : Enemy
{

    Rigidbody[] rigidBodies;

    private AudioSource audioSource;
    public Vector2 pitchRange = new Vector2(0.8f, 1.2f);
    public float randomTimeBeforeSound = 15f;

    // Start is called before the first frame update
    protected void Start()
    {
        //Get all the rigidbodies except the one attached to the parent
        rigidBodies = GetComponentsInChildren<Rigidbody>().Where(rb => rb.gameObject != gameObject).ToArray();

        //Add ActivateRagdoll fonction on collider to rigidbodies components
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.gameObject.AddComponent<ActivateParentRagdoll>();
            rb.gameObject.AddComponent<XRGrabInteractable>();
        }

        DeactivateRagdoll();

        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSound());
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

        audioSource.pitch = 0.6f;
        audioSource.Play();

        //Remove the rigidbody component to avoid the enemy to move
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        //Remove box collider
        Destroy(GetComponent<BoxCollider>());
    }

    private IEnumerator PlayRandomSound()
    {
        yield return new WaitForSeconds(Random.Range(randomTimeBeforeSound - 3, randomTimeBeforeSound));
        if(isDead)
            yield break;
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.Play();
        StartCoroutine(PlayRandomSound());
    }

}
