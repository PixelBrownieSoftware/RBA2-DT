using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PartyStatus : S_MenuSystem
{
    public R_BattleCharacterList players;
    public R_BattleCharacter selectedCharacter;
    public CH_Int selected;
    public CH_Text changeMenu;
    public B_Int[] buttons;
    public string menuToGoTo;

    private void OnEnable()
    {
        selected.OnFunctionEvent += SelectCharacter;
    }

    private void OnDisable()
    {
        selected.OnFunctionEvent -= SelectCharacter;
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
        for (int i = 0; i < players.battleCharList.Count; i++)
        {
            var button = buttons[i];
            button.gameObject.SetActive(true);
            button.SetIntButton(i);
            button.SetButonText(players.GetIndex(i).name);
        }
    }

    public void SelectCharacter(int bcID) {
        selectedCharacter.SetCharacter(players.GetIndex(bcID));
        changeMenu.RaiseEvent(menuToGoTo);
    }
}
