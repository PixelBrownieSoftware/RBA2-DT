using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/*
public class M_CharacterStatus : S_MenuSystem
{
    public R_Character selectedCharacter;
    public R_CharacterList players;
    public R_CharacterList selectedCharacters;
    public R_Move selectedMove;
    public CH_BattleCharacter selected;
    public CH_Text changeMenu;
    public R_Boolean isItem;
    public CH_Move moveEvent;

    public Slider hp;
    public Slider mp;
    public Slider exp;
    public GameObject mpObj;
    public B_BattleMove[] moves;

    private void OnEnable()
    {
        moveEvent.OnMoveFunctionEvent += SelectMove;
    }

    private void OnDisable()
    {
        moveEvent.OnMoveFunctionEvent -= SelectMove;
    }

    public void SelectMove(O_Move move)
    {
        if (selectedCharacter.characterRef.mana <= selectedMove.move.mpCost)
        {
            return;
        }
        selectedCharacters.characterListRef.Clear();
        switch (move.moveElement.name)
        {
            default:
                return;
            case "Recovery":
                selectedCharacters.characterListRef.AddRange(players.characterListRef.FindAll(x => x.health < x.maxHealth));
                break;
            case "Cure":
                selectedCharacters.characterListRef.AddRange(players.characterListRef.FindAll(x => x.statusEffects.ContainsKey(move.statusFX)));
                break;
        }
        isItem.boolean = false;
        selectedMove.move = move;
        changeMenu.RaiseEvent("PartyMenuHeal");
    }

    public override void StartMenu()
    {
        foreach (var move in moves) {
            move.gameObject.SetActive(false);
        }
        base.StartMenu();
        for (int i = 0; i < selectedCharacter.characterRef.moves.Count; i++) {
            var move = moves[i];
            var chMove = selectedCharacter.characterRef.moves[i];
            move.gameObject.SetActive(true);
            move.SetButonText(chMove.name);
            move.SetBattleButton(chMove);

        }
        if (selectedCharacter.characterRef.maxMana > 0)
        {
            mp.value = ((float)selectedCharacter.characterRef.mana/ (float)selectedCharacter.characterRef.maxMana);
            mpObj.SetActive(true);
        }
        else
        {
            mpObj.SetActive(false);
        }
        hp.value = ((float)selectedCharacter.characterRef.health / (float)selectedCharacter.characterRef.maxHealth);
        exp.value = (float)selectedCharacter.characterRef.expereince / 1f;
    }

    public void SelectCharacter(O_BattleCharacter bc)
    {
        selectedCharacter.SetCharacter(bc);
        changeMenu.RaiseEvent("PartyMenu");
    }
}
*/