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

    [Header("Lightning properties")]
    public GameObject lightningPrefab;

    [Header("Tornado properties")]
    public GameObject tornadoPrefab;

    [Header("WindUp properties")]
    public GameObject windUpPrefab;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    private TransformData SpawnTransformCamera(float distance)
    {
        TransformData transformData;
        transformData.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        transformData.position.y = 0;

        //Get only y rotation
        transformData.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

        return transformData;
    }

    public void Fireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position, SpawnTransformCamera(0).rotation);
        fireball.GetComponent<Rigidbody>().AddForce(transform.forward * 15, ForceMode.Impulse);
    }

    public void Firewave()
    {
        TransformData transformData = SpawnTransformCamera(0);

        GameObject firewave = Instantiate(firewavePrefab, transformData.position, transformData.rotation);
    }

    //Spawn 5 rocks in front of the player on the ground
    public void Empalement()
    {
        TransformData transformData = SpawnTransformCamera(3);

        //Spawn the rocks and add an Angle to them
        for (int i = 0; i < numberRocks; i++)
        {
            GameObject empalement = Instantiate(empalementPrefab, transformData.position + empalementPrefab.transform.position, transformData.rotation);
            //Random Angle arround a cirlce pattern distance from center

            //empalement.transform.RotateAround(transformData.position, Vector3.up, 360 / numberRocks * i);
            
            
            empalement.GetComponent<Empalement>().angle = new Vector3(Random.Range(-25, 25), Random.Range(0, 360), Random.Range(-25, 25));
        }
    }


    public void Wall()
    {
        TransformData transformData = SpawnTransformCamera(3);

        GameObject wall = Instantiate(wallPrefab, transformData.position + wallPrefab.transform.position, transformData.rotation);

    }

    public void Lightning()
    {
        //Spawn lightning in front of the player
        TransformData transformData = SpawnTransformCamera(4);

        GameObject lightning = Instantiate(lightningPrefab, SpawnTransformCamera(1).position, lightningPrefab.transform.rotation);
    }

    public void Tornado()
    {
        //Spawn tornado in front of the player
        TransformData transformData = SpawnTransformCamera(1);

        GameObject tornado = Instantiate(tornadoPrefab, transformData.position, SpawnTransformCamera(0).rotation);
    }

    public void WindUp()
    {
        //Spawn windUp in front of the player
        TransformData transformData = SpawnTransformCamera(1);

        GameObject windUp = Instantiate(windUpPrefab, transformData.position, windUpPrefab.transform.rotation);
    }

    public void PlanetBursto()
    {
        //Get the player position in front of him
        Vector3 planetPosition = transform.position + transform.forward * 6;
        planetPosition.y += planetHigh + planetBurstoPrefab.transform.localScale.y / 3.5f;
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

        planet.GetComponent<PlanetBursto>().StartFade(time / 4);

        //Wait 1/4 to start the crash effect
        yield return new WaitForSeconds(time / 4);

        planet.GetComponent<PlanetBursto>().ExplosionForce();

        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 1f, 0.2f);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 1f, 0.2f);

    }

}
