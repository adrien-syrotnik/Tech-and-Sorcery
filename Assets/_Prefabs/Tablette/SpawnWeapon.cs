using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct WeaponTemplate
{
    public string name;
    public Sprite imageToDisplay;
    public GameObject prefab;
}

public class SpawnWeapon : MonoBehaviour
{
    public WeaponTemplate[] weapons;
    public int index = 0;
    public Transform spawnPoint;

    public GameObject viewportUI;
    public GameObject templateUI;

    public TextMeshProUGUI nameText;

    private void Start()
    {
        //Get the viewport and create the weapon's UI
        foreach (WeaponTemplate weapon in weapons)
        {
            GameObject UI = Instantiate(templateUI, viewportUI.transform);
            UI.GetComponentInChildren<Image>().sprite = weapon.imageToDisplay;
            UI.GetComponentInChildren<Text>().text = weapon.name;
            EventTrigger eventT = UI.AddComponent<EventTrigger>();

            //Create the event for the UI
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { ChangeWeapon(weapon.name); });
            eventT.triggers.Add(entry);
        }

        //Set the first weapon
        ChangeWeapon(weapons[0].name);

    }

    public void Spawn()
    {
        if (weapons[index].prefab != null)
        {
            GameObject weapon = Instantiate(weapons[index].prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void ChangeWeapon(string name)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].name == name)
            {
                index = i;
                nameText.text = weapons[i].name;
                break;
            }
        }
    }

    public void ChangeWeapon(int index)
    {
        this.index = index;
    }
}
