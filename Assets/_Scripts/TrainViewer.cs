using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainViewer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] symbolsList;
    public Vector3 spawnPosition = new Vector3(0, -200, 400);

    void Start()
    {
        FindObjectOfType<SymbolHandler>().OnSymbol.AddListener(OnSymbol);
        FindObjectOfType<ComboHandler>().OnComboTimeout.AddListener(OnComboTimeout);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSymbol(string comboName)
    {
        currentSymbolDrawn = comboName;
    }

    private GameObject currentSymbolShowing;
    private string currentSymbolDrawn;
    private string currentSymbolShowingName;

    private Combo currentCombo;

    public void StartShowCombo(Combo combo)
    {
        StopAllCoroutines();
        currentCombo = combo;
        StartCoroutine(ShowCombo(combo));
    }
    
    public void ResetActualCombo()
    {
        if(currentCombo != null)
            StartShowCombo(currentCombo);
    }

    private void OnComboTimeout()
    {
        ResetActualCombo();
    }




    private IEnumerator ShowCombo(Combo combo)
    {
        Destroy(currentSymbolShowing);
        int index = 0;

        //Foreach symbols, show it, wait the user to draw it, and continue. If its the wrong symbol, restart
        while (index < combo.symbols.Length)
        {
            if (currentSymbolShowing == null)
                foreach (GameObject g in symbolsList)
                {
                    if (combo.symbols[index] == g.name)
                    {
                        currentSymbolShowing = Instantiate(g, transform);
                        currentSymbolShowing.transform.localPosition = spawnPosition;
                        currentSymbolShowingName = g.name;
                        break;
                    }
                }
            yield return new WaitUntil(() => currentSymbolDrawn == currentSymbolShowingName);
            Destroy(currentSymbolShowing);
            currentSymbolShowing = null;
            currentSymbolShowingName = "";
            index++;
        }

        currentCombo = null;

    }

}
