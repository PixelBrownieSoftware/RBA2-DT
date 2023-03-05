using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class M_BattleOptions : S_MenuSystem
{
    public R_Character currentCharacter;
    public B_Int[] buttons;
    public R_Items items;
    public R_Boolean isItem;
    public enum B_OPTIONS_TYPE { 
        MOVES,
        ITEMS
    }
    public B_OPTIONS_TYPE options;

    private void Awake()
    {
        foreach (var b in buttons) {
            b.gameObject.SetActive(false);
        }
    }

    public override void StartMenu()
    {
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        base.StartMenu();
        List<s_move> moves = null;

        switch (options) {
            case B_OPTIONS_TYPE.MOVES:
                isItem.boolean = false; 
                moves = currentCharacter.characterRef.characterData.AllSkills;
                if (moves.Count > 0)
                {
                    for (int i = 0; i < moves.Count; i++)
                    {
                        var button = buttons[i];
                        button.SetButonText(moves[i].name);
                        button.SetIntButton(i);
                        button.gameObject.SetActive(true);
                    }
                }
                break;

            case B_OPTIONS_TYPE.ITEMS:
                isItem.boolean = true;
                if (items.inventory.Count > 0)
                {
                    int ind = 0;
                    foreach (var item in items.inventory) {

                        var button = buttons[ind];
                        button.SetIntButton(ind);
                        button.SetButonText(item.Key.name + " x " + item.Value);
                        button.gameObject.SetActive(true);
                        ind++;
                    }
                }
                break;
        }
    }
}
