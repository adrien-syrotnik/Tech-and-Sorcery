using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RocketLauncherGrab : XRGrabInteractable
{

    //private bool canShoot = false;

    protected override void Awake()
    {
        base.Awake();
        selectMode = InteractableSelectMode.Multiple;
    }

    [SerializeField] private Transform _secondAttachTransform;

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        //canShoot = false;
        if (interactorsSelecting.Count == 1)
        {
            base.ProcessInteractable(updatePhase);
        }
        else if (interactorsSelecting.Count == 2 && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            ProcessDoubleGrip();
        }

        if (interactorsSelecting.Count == 2)
        {
            //canShoot = true;
        }

    }

    private void ProcessDoubleGrip()
    {
        Transform firstAttach = GetAttachTransform(null);
        Transform secondAttach = _secondAttachTransform;
        Transform firstHand = interactorsSelecting[0].transform;
        Transform secondHand = interactorsSelecting[1].transform;

        transform.position = secondAttach.position;
        transform.rotation = Quaternion.LookRotation(firstHand.position - secondHand.position);
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        Debug.Log("OnActivated");
        base.OnActivated(args);
        Debug.Log("Shoot");
        Shoot();

    }

    private void Shoot()
    {
        GunfireController gun = GetComponent<GunfireController>();
        gun.FireWeapon();
    }

    protected override void Drop()
    {
        if (!isSelected)
        {
            base.Drop();
        }

    }

    protected override void Grab()
    {
        if (interactorsSelecting.Count == 1)
        {
            base.Grab();
        }
    }
}
