/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_itemMenu : s_menucontroller
{
    public override void OnOpen()
    {
        base.OnOpen();
        List<s_move> rpgItems = s_rpgGlobals.rpgGlSingleton.GetItems();
        ResetButton();
        for (int i = 0; i < rpgItems.Count; i++)
        {
            s_buttonItem bI = GetButton<s_buttonItem>(i);
            bI.gameObject.SetActive(true);
            bI.moveButton = rpgItems[i];
            bI.txt.text = rpgItems[i].name;
        }
    }
}
*/
