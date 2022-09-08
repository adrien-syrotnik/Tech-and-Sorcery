using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBursto : MonoBehaviour
{
    public float planetSpeed = 10f;
    public Vector3 planetTarget;
    Vector3 planetDir;
    Rigidbody rb;

    private Canvas canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        planetDir = planetTarget - transform.position;
        rb = GetComponent<Rigidbody>();
        canvas = GetComponentInChildren<Canvas>();
        //Set the canvas camera
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.1f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Tp the planet on the target and prevent it to collision
        transform.position = Vector3.MoveTowards(transform.position, planetTarget, planetSpeed * Time.fixedDeltaTime);

        //Rotate the planet
        transform.Rotate(0.01f, -0.01f, 0.01f);
        
        //Force tp if collide
        
    }

    public void ExplosionForce()
    {
        //Disable collider
        GetComponent<SphereCollider>().enabled = false;

        //Get all rigidbodies arround the explosion
        Collider[] colliders = Physics.OverlapSphere(planetTarget, 35f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(1000f, planetTarget, 35f, 3.0F, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }

    public void StartFade(float duration)
    {
        StartCoroutine(FadeIn(duration));
    }

    IEnumerator FadeIn(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            canvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvas.GetComponent<CanvasGroup>().alpha = 1;

    }
}
