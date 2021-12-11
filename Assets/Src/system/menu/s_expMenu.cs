using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_expMenu : s_menucontroller
{
    Text[] names;
    Slider[] characterEXP;
    public GameObject[] EXPThings;

    public override void OnOpen()
    {
        base.OnOpen();
        characterEXP = new Slider[EXPThings.Length];
        names = new Text[EXPThings.Length];
        for (int i = 0; i < EXPThings.Length; i++)
        {
            GameObject ob = EXPThings[i];
            ob.SetActive(false);
            names[i] = ob.transform.Find("Text").GetComponent<Text>();
            characterEXP[i] = ob.transform.Find("Bar").GetComponent<Slider>();
        }
    }

    private void Update()
    {
        int i = 0;
        foreach (o_battleCharacter bc in s_battleEngine.GetInstance().playerCharacters)
        {
            EXPThings[i].gameObject.SetActive(true);
            i++;
        }
    }
}
