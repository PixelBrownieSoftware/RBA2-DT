using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PartyStatus : S_MenuSystem
{
    public R_CharacterList players;
    public R_Character selectedCharacter;
    public CH_BattleCharacter selected;
    public CH_Text changeMenu;
    public B_BattleTarget[] buttons;
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
        for (int i = 0; i < players.characterListRef.Count; i++)
        {
            var button = buttons[i];
            button.gameObject.SetActive(true);
            button.SetTargetButton(players.GetChracter(i));
        }
    }

    public void SelectCharacter(O_BattleCharacter bc) {
        selectedCharacter.SetCharacter(bc);
        changeMenu.RaiseEvent(menuToGoTo);
    }
}
