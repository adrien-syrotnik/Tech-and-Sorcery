using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Wall : MonoBehaviour
{
    //Speed of the projectile
    public float speed = 1f;

    public float lifeTime = 10f;

    private VisualEffect rockEffect;

    private bool isSummoned = false;

    private float yElevated = 0;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
        rockEffect = GetComponentInChildren<VisualEffect>();
    }

    private void FixedUpdate()
    {
        isSummoned = true;
        while (yElevated < transform.localScale.y * 2)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime * 0.1f);
            //Negative for rockEffect
            rockEffect.gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime * 0.1f);
            isSummoned = false;
            yElevated += speed * Time.deltaTime * 0.1f;
        }
        if (isSummoned)
        {
            rockEffect.Stop();
        }
    }

}
