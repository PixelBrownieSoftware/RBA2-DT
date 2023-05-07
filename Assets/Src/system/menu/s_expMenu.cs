/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class s_expMenu : s_menucontroller
{
    public R_CharacterList playersReference;
    public R_CharacterList enemiesReference;
    Text[] names;
    TextMeshProUGUI[] expereniceNumbers;
    Slider[] characterEXP;
    public GameObject[] EXPThings;

    public override void OnOpen()
    {
        base.OnOpen();
        characterEXP = new Slider[EXPThings.Length];
        names = new Text[EXPThings.Length];
        expereniceNumbers = new TextMeshProUGUI[EXPThings.Length];
        for (int i = 0; i < EXPThings.Length; i++)
        {
            GameObject ob = EXPThings[i];
            ob.SetActive(false);
            names[i] = ob.transform.Find("Text").GetComponent<Text>();
            characterEXP[i] = ob.transform.Find("Bar").GetComponent<Slider>();
            expereniceNumbers[i] = ob.transform.Find("ExpText").GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        int i = 0;
        foreach (var bc in playersReference.characterListRef)
        {
            EXPThings[i].gameObject.SetActive(true);
            characterEXP[i].value = bc.exp;
            names[i].text = bc.cName + " lv: " + bc.level;
            expereniceNumbers[i].text = (bc.exp / 1f) + "%";
            i++;
        }
    }
}
*/