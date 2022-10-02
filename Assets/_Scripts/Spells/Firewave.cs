using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewave : MonoBehaviour
{
    public float timeOut = 10f;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeOut);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime * 0.1f);
    }
}
