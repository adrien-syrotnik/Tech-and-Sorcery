using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewave : MonoBehaviour
{
    public float timeOut = 10f;
    public float speed = 1f;
    public float damage = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeOut);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime * 0.1f);

        //Detect all rigidbody in an half circle in front of the firewave
        RaycastHit[] rayCastHits = Physics.SphereCastAll(transform.position, 2f, transform.forward, 1f);

        

        foreach (RaycastHit raycastHit in rayCastHits)
        {
            Debug.Log(Vector3.Distance(transform.position, raycastHit.transform.position));
            if (Vector3.Distance(transform.position, raycastHit.transform.position) < 2.2f)
                continue;

            Vector3 vectorToCollider = (raycastHit.transform.position - transform.position).normalized;
            // 180 degree arc, change 0 to 0.5 for a 90 degree "pie"
            if (Vector3.Dot(vectorToCollider, transform.forward) >= 0)
            {
                ICanTakeDamage canTakeDamage = raycastHit.transform.GetComponent<ICanTakeDamage>();
                if (canTakeDamage != null)
                {
                    canTakeDamage.TakeDamage(damage);
                    Debug.DrawLine(transform.position, raycastHit.transform.position, Color.red, 1f);
                }
            }
        }


    }
}

