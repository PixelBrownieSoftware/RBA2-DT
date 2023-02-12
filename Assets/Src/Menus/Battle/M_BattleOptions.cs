using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class M_BattleOptions : S_MenuSystem
{
    /*
    public R_Character currentCharacter;
    public B_BattleMove[] buttons;
    public R_MoveList items;
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

        switch (options) {
            case B_OPTIONS_TYPE.MOVES:
                isItem.boolean = false;
                if (currentCharacter.characterRef.moves.Count > 0)
                {
                    for (int i = 0; i < currentCharacter.characterRef.moves.Count; i++)
                    {
                        var button = buttons[i];
                        button.SetBattleButton(currentCharacter.characterRef.moves[i]);
                        button.gameObject.SetActive(true);
                    }
                }
                break;

            case B_OPTIONS_TYPE.ITEMS:
                isItem.boolean = true;
                if (items.moveListRef.Count > 0)
                {
                    Dictionary<O_Move, int> itemsList = new Dictionary<O_Move, int>();
                    for (int i = 0; i < items.moveListRef.Count; i++)
                    {
                        var item = items.moveListRef[i];
                        if (itemsList.ContainsKey(item))
                        {
                            itemsList[item]++;
                        }
                        else
                        {
                            itemsList.Add(item, 1);
                        }
                    }
                    int ind = 0;
                    foreach (var item in itemsList) {

                        var button = buttons[ind];
                        button.SetBattleButton(item.Key);
                        button.SetButonText(item.Key.name + " x " + item.Value);
                        button.gameObject.SetActive(true);
                        ind++;
                    }
                }
                break;
        }
    }
    */
}
