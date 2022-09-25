using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainViewer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] symbolsList;

    void Start()
    {
        FindObjectOfType<SymbolHandler>().OnSymbol.AddListener(OnSymbol);
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
    
    public IEnumerator ShowCombo(Combo combo)
    {
        Destroy(currentSymbolShowing);
        int index = 0;

        //Foreach symbols, show it, wait the user to draw it, and continue. If its the wrong symbol, restart
        while (index < combo.symbols.Length)
        {
            if(currentSymbolShowing == null)
                foreach (GameObject g in symbolsList)
                { 
                    if (combo.symbols[index] == g.name)
                    {
                        currentSymbolShowing = Instantiate(g, transform);
                        break;
                    }
                }
            yield return new WaitUntil(() => currentSymbolDrawn == currentSymbolShowing.name);
            Destroy(currentSymbolShowing);
            index++;
        }
        
        
        {
            
            
        }
    }

}
