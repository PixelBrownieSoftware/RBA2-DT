using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class M_BattleOptions : S_MenuSystem
{
    public R_Character currentCharacter;
    public R_CharacterList targetList;
    public R_CharacterList players;
    public R_CharacterList opponents;
    public R_ComboMoves comboMoves;
    public B_BattleMove[] buttons;
    public R_Items items;
    public R_Boolean isItem;
    public R_Move selectedMove;
    public R_MoveList movesList;
    public CH_Move selectMove;
    public CH_Text selectMenu;
    public enum B_OPTIONS_TYPE { 
        MOVES,
        ITEMS
    }
    public B_OPTIONS_TYPE options;

    private void OnEnable()
    {
        selectMove.OnMoveFunctionEvent += SelectMove;
    }

    private void OnDisable()
    {
        selectMove.OnMoveFunctionEvent -= SelectMove;
    }

    private void Awake()
    {
        foreach (var b in buttons) {
            b.gameObject.SetActive(false);
        }
    }

    public void SelectMove(s_move move) {
        selectedMove.move = move;
        switch (move.moveTarg) {
            case s_move.MOVE_TARGET.ALLY:
                targetList.AddCharacters(players.characterListRef);
                break;

            case s_move.MOVE_TARGET.ENEMY:
                targetList.AddCharacters(opponents.characterListRef);
                break;

            case s_move.MOVE_TARGET.ENEMY_ALLY:
                {
                    List<CH_BattleChar> allTargets = new List<CH_BattleChar>();
                    allTargets.AddRange(players.characterListRef);
                    allTargets.AddRange(opponents.characterListRef);
                    targetList.AddCharacters(allTargets);
                }
                break;

            case s_move.MOVE_TARGET.SELF:
                targetList.Add(currentCharacter.characterRef);
                break;
            case s_move.MOVE_TARGET.NONE:

                break;
        }
        switch (move.moveTarg)
        {
            default:
                selectMenu.RaiseEvent("TargetMenu");
                break;
            case s_move.MOVE_TARGET.NONE:

                break;
        }

        switch (move.moveTargScope) {
            case s_move.SCOPE_NUMBER.ALL:
                break;
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
                moves = movesList.moveListRef;
                if (moves.Count > 0)
                {
                    for (int i = 0; i < moves.Count; i++)
                    {
                        var button = buttons[i];
                        button.SetButonText(moves[i].name);
                        button.SetBattleButton(moves[i], moves[i].cost);
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
                        button.SetBattleButton(item.Key, item.Value);
                        button.SetButonText(item.Key.name + " x " + item.Value);
                        button.gameObject.SetActive(true);
                        ind++;
                    }
                }
                break;
        }
    }
}
