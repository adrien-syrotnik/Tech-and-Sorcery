using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDemoSpellList : MonoBehaviour
{
    public ComboHandler comboHandler;

    public GameObject panelSpellList;
    public GameObject panelSpellPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        Combo[] combos = comboHandler.combos;
        //Create a panelSpell from the prefab and add it to the panelSpellList
        foreach (Combo c in combos)
        {
            GameObject panelSpell = Instantiate(panelSpellPrefab, panelSpellList.transform);
            panelSpell.GetComponentInChildren<TextMeshProUGUI>().text = c.comboName;
            //Add Images of the combo spells from teh _Images folder
            foreach (string s in c.symbols)
            {
                //create the image
                GameObject image = new GameObject();
                image.AddComponent<Image>();
                image.GetComponent<Image>().sprite = Resources.Load<Sprite>("_Images/" + s);
                //set the image as a child of the panelSpell
                image.transform.SetParent(panelSpell.transform);
                //set the image size
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                //set the image position
                image.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
