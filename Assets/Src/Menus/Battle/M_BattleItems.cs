using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BattleItems : S_MenuSystem
{
    /*
    public R_MoveList moveList;
    public B_BattleMove[] buttons;
    public CH_Move onButtonClickEvents;

    private void OnEnable()
    {
        onButtonClickEvents.OnMoveFunctionEvent += UseItem;
    }

    private void OnDisable()
    {
        onButtonClickEvents.OnMoveFunctionEvent -= UseItem;
    }

    public void UseItem(s_move mov) {
        moveList.RemoveMove(mov);
    }

    private void Awake()
    {
        foreach (var b in buttons)
        {
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

        if (moveList.moveListRef.Count > 0)
        {
            for (int i = 0; i < moveList.moveListRef.Count; i++)
            {
                var button = buttons[i];
                button.SetBattleButton(moveList.moveListRef[i]);
                button.gameObject.SetActive(true);
            }
        }
    }
    */
}