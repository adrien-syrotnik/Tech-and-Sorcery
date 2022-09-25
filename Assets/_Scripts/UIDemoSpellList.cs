using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

public class UIDemoSpellList : MonoBehaviour
{
    public ComboHandler comboHandler;

    public GameObject panelSpellList;
    public GameObject panelSpellPrefab;

    private RectTransform rectTransform;

    public TrainViewer trainViewer;


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        trainViewer ??= FindObjectOfType<TrainViewer>();

        Combo[] combos = comboHandler.combos;
        int indexCombo = 0;
        //Create a panelSpell from the prefab and add it to the panelSpellList
        foreach (Combo c in combos)
        {
            indexCombo++;
            GameObject panelSpell = Instantiate(panelSpellPrefab, panelSpellList.transform);
            panelSpell.GetComponentInChildren<TextMeshProUGUI>().text = c.comboName;

            panelSpell.GetComponent<RectTransform>().localPosition = new Vector3(0, -indexCombo * 50 + 200, 0);

            //Get button component and add trainViewer function
            Button button = panelSpell.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => StartCoroutine(trainViewer.ShowCombo(c)));

            int indexSymbol = 0;

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
                image.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                //set the image position
                image.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

                //set the image position
                image.GetComponent<RectTransform>().localPosition = new Vector3(indexSymbol * 50, 0, 0);
                indexSymbol++;
            }
        }
    }

    public float maxY = 0;
    public float minY = -200;
    public float speed = 1f;
    

    // Update is called once per frame
    void Update()
    {

        //Show only if the user look is arm

        //Get XRController right joystick
        Vector2 joystick = Vector2.zero;
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out joystick);

        Vector3 newPos = rectTransform.localPosition;

        //Move the panelSpellList
        if (joystick.y < -0.5f)
        {
            newPos.y = Mathf.Clamp(panelSpellList.GetComponent<RectTransform>().localPosition.y + speed, minY, maxY);
        }
        else if (joystick.y > 0.5f)
        {
            newPos.y = Mathf.Clamp(panelSpellList.GetComponent<RectTransform>().localPosition.y - speed, minY, maxY);
        }

        rectTransform.localPosition = newPos;


    }
}
