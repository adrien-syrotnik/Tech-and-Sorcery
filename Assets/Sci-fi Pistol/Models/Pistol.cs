using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Playables;
//XRTool
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : XRGrabInteractable
{

    public PlayableDirector playableDirector;
    private XRBaseController controller;
    private bool isGrabbed = false;
    public bool canShoot = true;

    public float shootForce = 1000f;
    public float treshholdReload = 0.2f;
    public float treshholdShoot = 0.5f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        //Get value of the interractor from interactorObject
        var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        controller = controllerInteractor.xrController;

        isGrabbed = true;

        playableDirector.Play();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isGrabbed = false;

        playableDirector.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

    }

    // Update is called once per frame
    void Update()
    {
        if(isGrabbed)
        {
            //Set time with the activate value
            playableDirector.time = controller.activateInteractionState.value;
            //Debug.Log(playableDirector.time);
            if (controller.activateInteractionState.value > treshholdShoot && canShoot)
            {
                Shoot();
            }
            else if (controller.activateInteractionState.value < treshholdReload)
            {
                Reload();
            }
        }
    }

    
    private void Shoot()
    {
        canShoot = false;
        //Spawn the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        //Add force to the bullet
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * shootForce);
        //Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);

    }

    private void Reload()
    {
        canShoot = true;
    }



}
