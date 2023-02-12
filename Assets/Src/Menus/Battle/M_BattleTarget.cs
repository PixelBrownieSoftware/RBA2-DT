using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BattleTarget : S_MenuSystem
{
    /*
    public R_CharacterList targets;
    public B_BattleTarget[] buttons;

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
        for (int i = 0; i < targets.characterListRef.Count; i++)
        {
            var button = buttons[i];
            button.gameObject.SetActive(true);
            button.SetTargetButton(targets.characterListRef[i]);
        }
    }
    */
}
