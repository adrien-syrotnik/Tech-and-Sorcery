using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public Transform body;
    private Vector3 footSpacing;

    // Start is called before the first frame update
    void Start()
    {
        //Get the footSpacing from the body
        footSpacing = body.InverseTransformPoint(transform.position);
        footSpacing.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Ray cast and find the ground to teleport
        RaycastHit hit;
        if (Physics.Raycast(body.position + (footSpacing), Vector3.down, out hit, 100))
        {
            transform.position = hit.point;
        }
        
    }
}
