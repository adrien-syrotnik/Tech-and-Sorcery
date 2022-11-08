using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Empalement : MonoBehaviour
{
    //Angle to rotate the projectile
    public Vector3 angle = new Vector3(0, 0, 0);
    //Speed of the projectile
    public float speed = 5f;

    public float lifeTime = 10f;

    private VisualEffect rockEffect;

    private bool isSummoned = false;

    private float yElevated = 0;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
        rockEffect = GetComponentInChildren<VisualEffect>();
        //add angle
        transform.Rotate(angle);
    }

    private void FixedUpdate()
    {
        isSummoned = true;
        if (yElevated < transform.localScale.y*0.5)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            //Negative for rockEffect
            rockEffect.gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);
            isSummoned = false;
            yElevated += speed * Time.deltaTime;
        }
        if(isSummoned)
        {
            rockEffect.Stop();
            GetComponent<AudioSource>().Stop();
        }
    }

}
