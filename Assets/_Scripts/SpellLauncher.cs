using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpellLauncher : MonoBehaviour
{
    [Header("Fireball properties")]
    public GameObject fireballPrefab;

    [Header("Firewave properties")]
    public GameObject firewavePrefab;

    [Header("PlanetBursto properties")]
    public GameObject planetBurstoPrefab;
    public float planetSpeed = 10f;
    public float planetHigh = 80f;
    public float planetFar = 10f;
    public AudioClip planetExplosion;

    [Header("Empalement properties")]
    public GameObject empalementPrefab;
    public int numberRocks = 5;

    [Header("Wall properties")]
    public GameObject wallPrefab;

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

    public void Firewave()
    {
        GameObject firewave = Instantiate(firewavePrefab, transform.position, transform.rotation);
    }

    //Spawn 5 rocks in front of the player on the ground
    public void Empalement()
    {
        //Get the player transform (camera)
        Transform cameraTransform = Camera.main.transform;

        //Get the vector3 position in front of the camera
        Vector3 position = cameraTransform.position + cameraTransform.forward * 1;

        //Get the vector3 position project on the ground
        Vector3 positionGround = new Vector3(position.x, 0, position.z);

        //Spawn the rocks and add an Angle to them
        for (int i = 0; i < numberRocks; i++)
        {
            GameObject empalement = Instantiate(empalementPrefab, positionGround, transform.rotation);
            empalement.GetComponent<Empalement>().angle = new Vector3(0, 0, 360/numberRocks * i);
        }
    }


    public void Wall()
    {
        //Spawn wall in front of the player under the ground
        GameObject wall = Instantiate(wallPrefab, transform.position + transform.forward * 1, transform.rotation);
        
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
