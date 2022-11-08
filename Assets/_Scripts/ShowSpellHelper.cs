using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpellHelper : MonoBehaviour
{

    public GameObject UIToShow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Show UI if the camera is looking at the object
        if (UIToShow != null)
        {
            if (Vector3.Dot((transform.position - Camera.main.transform.position).normalized, Camera.main.transform.forward) > 0.9f)
            {
                UIToShow.SetActive(true);
            }
            else
            {
                UIToShow.SetActive(false);
            }
        }

    }
}
