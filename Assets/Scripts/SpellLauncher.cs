using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLauncher : MonoBehaviour
{

    public GameObject fireballPrefab;
    public Transform fireballTransform;


    public void Fireball()
    {
        Instantiate(fireballPrefab, fireballTransform.position, fireballTransform.rotation);
        //Add force to laucnh the fireball
        fireballPrefab.GetComponent<Rigidbody>().AddForce(fireballTransform.forward * 1000);
    }
    
}
