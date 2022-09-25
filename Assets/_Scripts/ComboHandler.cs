using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class ComboHandler : MonoBehaviour
{

    private List<string> actualCombo = new List<string>();

    public float timeOut = 3f;

    public TextMeshProUGUI debugText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI comboTimeoutText;

    [NonReorderable]
    public Combo[] combos;

    public void CheckCombo(string spell)
    {
        debugText.text = spell;
        //Vibrate right xr controller
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, .1f, 0.1f);

        //Cancel coroutine clear combo
        StopCoroutine("ClearCombo");
        
        actualCombo.Add(spell);

        actualCombo = Combo.RemoveUnusedSymbols(actualCombo, combos);
        comboText.text = "";

        if (actualCombo.Count > 0)
        {
            //Check for each combo if teh spell are in the right order
            foreach (Combo c in combos)
            {
                string comboAdvancement = c.GetComboAdvancement(actualCombo);
                if (comboAdvancement != "")
                {
                    comboText.text = comboAdvancement;
                }
                
                if (c.CheckCombo(actualCombo))
                {
                    debugText.text = c.comboName;
                    //If the combo is correct, clear the combo list and break the loop
                    ClearCombo();
                    break;
                }
            }
        }
        StartCoroutine("ClearCombo");
    }

    IEnumerator ClearCombo()
    {
        int time = 0;
        while (time < timeOut)
        {
            time++;
            comboTimeoutText.text = (timeOut - time).ToString();
            yield return new WaitForSeconds(1);
        }
        actualCombo.Clear();
        debugText.text = "";
        comboTimeoutText.text = "";
    }
}

[System.Serializable]
public class Combo
{
    public string[] symbols;
    public string comboName;
    public UnityEngine.Events.UnityEvent OnCombo = new UnityEngine.Events.UnityEvent();

    //Remove symbols if they are not in a combo
    public static List<string> RemoveUnusedSymbols(List<string> symbols, Combo[] combos)
    {
        int newStartIndex = 0;
        //Loop until find a combo advancement
        for (int i = 0; i < symbols.Count; i++)
        {
            bool found = false;
            foreach (Combo c in combos)
            {
                if (c.GetComboAdvancement(symbols.GetRange(i, symbols.Count - i)) != "")
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                newStartIndex = i;
                break;
            }
        }
        //Remove symbols before the combo advancement
        symbols.RemoveRange(0, newStartIndex);
        return symbols;
    }


    public bool CheckCombo(List<string> actualCombo)
    {
        //Check if the combo is the same length of the actual combo
        if (symbols.Length != actualCombo.Count)
            return false;

        //Check if the combo is the same as the actual combo
        for (int i = 0; i < symbols.Length; i++)
        {
            if (symbols[i] != actualCombo[i])
                return false;
        }

        Debug.Log("Combo " + comboName + " found!");
        //If the combo is correct, invoke the event and return true
        OnCombo.Invoke();
        return true;
    }
    public string GetComboAdvancement(List<string> actualCombo)
    {
        string comboMessage = "";

        //Check if the combo is the same length of the actual combo
        if (symbols.Length < actualCombo.Count)
            return "";

        //Check if the combo is the same as the actual combo
        for (int i = 0; i < symbols.Length; i++)
        {
            if (i < actualCombo.Count && symbols[i] != actualCombo[i])
                return "";

            if (i >= actualCombo.Count || symbols[i] != actualCombo[i])
            {
                comboMessage += symbols[i] + " -> ";
            }
            else
            {
                comboMessage += "<color=green>" + symbols[i] + "</color> -> ";
            }
        }

        return comboMessage;
    }
}