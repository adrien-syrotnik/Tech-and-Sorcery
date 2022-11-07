using BigRookGames.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RocketLauncherGrab : XRGrabInteractable
{

    private bool canShoot = false;

    protected override void Awake()
    {
        base.Awake();
        selectMode = InteractableSelectMode.Multiple;
    }

    [SerializeField] private Transform _secondAttachTransform;

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        canShoot = false;
        if (interactorsSelecting.Count == 1)
        {
            base.ProcessInteractable(updatePhase);
        }
        else if (interactorsSelecting.Count == 2 && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            ProcessDoubleGrip();
        }

    }

    private void ProcessDoubleGrip()
    {
        Transform firstAttach = GetAttachTransform(null);
        Transform secondAttach = _secondAttachTransform;
        Transform firstHand = interactorsSelecting[0].transform;
        Transform secondHand = interactorsSelecting[1].transform;

        //Tp the first hand to the first attach point
        firstHand.position = firstAttach.position;
        firstHand.rotation = firstAttach.rotation;

        //Tp the second hand to the second attach point
        secondHand.position = secondAttach.position;
        secondHand.rotation = secondAttach.rotation;

        canShoot = true;
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        if (canShoot)
            Shoot();
    }

    private void Shoot()
    {
        GunfireController gun = GetComponent<GunfireController>();
        gun.FireWeapon();
    }
}
