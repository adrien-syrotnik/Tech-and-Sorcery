using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpellLauncher : MonoBehaviour
{

    public GameObject fireballPrefab;
    public Transform fireballTransform;

    public GameObject planetBurstoPrefab;
    public float planetSpeed = 10f;
    public float planetHigh = 80f;
    public float planetFar = 10f;
    public AudioClip planetExplosion;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position, transform.rotation);

        //Add force to laucnh the fireball
        fireball.GetComponent<Rigidbody>().AddForce(transform.forward * 15, ForceMode.Impulse);
    }


    public void PlanetBursto()
    {
        //Get the player position in front of him
        Vector3 planetPosition = transform.position + transform.forward * 6;
        planetPosition.y += planetHigh + planetBurstoPrefab.transform.localScale.y/3.5f;
        planetPosition.z += (transform.position + transform.forward * planetFar).z;
        planetPosition.x += (transform.position + transform.forward * planetFar).x;

        //Spawn the planet
        GameObject planet = Instantiate(planetBurstoPrefab, planetPosition, transform.rotation);

        //Add the speed and targetPos
        planet.GetComponent<PlanetBursto>().planetSpeed = planetSpeed;
        planet.GetComponent<PlanetBursto>().planetTarget = transform.position + transform.forward * 6 - Vector3.up * planetBurstoPrefab.transform.localScale.y / 4;
        
        StartCoroutine(WaitPlanetCrash(planetHigh / planetSpeed, planet));
    }

    IEnumerator WaitPlanetCrash(float time, GameObject planet)
    {
        //////// FIRST PART ////////

        //Play audio
        audioSource.PlayOneShot(planetExplosion);

        //Add haptic feedback
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 0.05f, time / 4);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 0.05f, time / 4);

        //Camera Shake
        Camera.main.GetComponent<ScreenShakeVR>().Shake(1f, time);

        yield return new WaitForSeconds(time / 4);

        //////// SECOND PART ////////
        
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 0.1f, time / 4);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 0.1f, time / 4);

        yield return new WaitForSeconds(time / 4);

        //////// THIRD PART ////////
        ///
        Camera.main.GetComponent<ScreenShakeVR>().Shake(2f, time / 3);

        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 0.2f, time / 4);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 0.2f, time / 4);

        yield return new WaitForSeconds(time / 4);

        //////// FOURTH PART ////////

        Camera.main.GetComponent<ScreenShakeVR>().Shake(5f, time / 4);

        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 0.5f, time / 4);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 0.5f, time / 4);

        planet.GetComponent<PlanetBursto>().StartFade(time/4);

        //Wait 1/4 to start the crash effect
        yield return new WaitForSeconds(time / 4);
        
        planet.GetComponent<PlanetBursto>().ExplosionForce();

        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 1f, 0.2f);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 1f, 0.2f);

    }

}
