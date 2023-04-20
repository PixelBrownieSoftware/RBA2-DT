using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class M_BattleOptions : S_MenuSystem
{
    public R_Character currentCharacter;
    public R_CharacterList targetList;
    public R_CharacterList players;
    public R_CharacterList opponents;
    public B_BattleMove[] buttons;
    public R_Items items;
    public R_Boolean isItem;
    public R_Move selectedMove;
    public R_MoveList movesList;
    public CH_Move selectMove;
    public CH_Text selectMenu;
    [SerializeField]
    private R_Text battleMenuType;

    public TextMeshProUGUI comboMoveDesc;

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
                targetList.SetCharacters(players.characterListRef);
                break;

            case s_move.MOVE_TARGET.ENEMY:
                targetList.SetCharacters(opponents.characterListRef);
                break;

            case s_move.MOVE_TARGET.ENEMY_ALLY:
                {
                    List<CH_BattleChar> allTargets = new List<CH_BattleChar>();
                    allTargets.AddRange(players.characterListRef);
                    allTargets.AddRange(opponents.characterListRef);
                    targetList.SetCharacters(allTargets);
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

        switch (battleMenuType.text) {

            case "Skills":
                moves = movesList.moveListRef;
                if (moves.Count > 0)
                {
                    for (int i = 0; i < moves.Count; i++)
                    {
                        var button = buttons[i];
                        button.SetButonText(moves[i].name);
                        int cost = 0;
                        if (moves[i].element.isMagic)
                            cost = moves[i].cost;
                        else
                            cost = s_calculation.DetermineHPCost(moves[i], currentCharacter.characterRef.strengthNet, currentCharacter.characterRef.vitalityNet, currentCharacter.characterRef.maxHealth);
                        bool canUse = true;
                        if (moves[i].element.isMagic)
                            canUse = currentCharacter.characterRef.stamina >= cost;
                        else
                            canUse = currentCharacter.characterRef.health > cost;
                        if (!canUse)
                        {
                            button.SetButtonColour(Color.grey);
                            button.SetButonTextColour(Color.grey);
                            button.SetBattleButton(moves[i], moves[i].cost);
                            button.move = null;
                        }
                        else
                        {
                            button.SetButtonColour(Color.white);
                            button.SetButonTextColour(Color.white);
                            button.SetBattleButton(moves[i], moves[i].cost);
                        }
                        button.gameObject.SetActive(true);
                    }
                }
                break;

                /*
            case "Combos":
                {
                    int ind = 0;
                    Dictionary<s_move, List<s_moveComb>> combos = comboMoves.FindComboMovesUser(currentCharacter.characterRef);
                    foreach (var move in combos)
                    {
                        foreach (var combo in move.Value) {

                            //Yes, I know, dreadful code - I cannot stand looking at this either
                            var button = buttons[ind];
                            button.SetButonText(moves[ind].name);
                            int cost = moves[ind].cost;
                            bool canUse1 = true;
                            bool canUse2 = true;
                            bool canUse3 = true;
                            bool canUse4 = true;
                            bool canUse5 = true;
                            button.combo = combo;

                            if (combo.user1Move != null)
                            {
                                cost = combo.user1Move.cost;
                                if (combo.user1Move.element.isMagic)
                                    canUse1 = currentCharacter.characterRef.stamina >= cost;
                                else
                                    canUse1 = currentCharacter.characterRef.health > cost;
                            }
                            if (combo.user2Move != null)
                            {
                                cost = combo.user2Move.cost;
                                if (combo.user2Move.element.isMagic)
                                    canUse2 = currentCharacter.characterRef.stamina >= cost;
                                else
                                    canUse2 = currentCharacter.characterRef.health > cost;
                            }
                            if (combo.user3Move != null)
                            {
                                cost = combo.user3Move.cost;
                                if (combo.user3Move.element.isMagic)
                                    canUse3 = currentCharacter.characterRef.stamina >= cost;
                                else
                                    canUse3 = currentCharacter.characterRef.health > cost;
                            }
                            if (combo.user4Move != null)
                            {
                                cost = combo.user4Move.cost;
                                if (combo.user4Move.element.isMagic)
                                    canUse4 = currentCharacter.characterRef.stamina >= cost;
                                else
                                    canUse4 = currentCharacter.characterRef.health > cost;
                            }

                            bool canUse = canUse1 && canUse2 && canUse3 && canUse4 && canUse5;

                            if (!canUse)
                            {
                                button.SetButtonColour(Color.grey);
                                button.SetButonTextColour(Color.grey);
                                button.SetBattleButton(move.Key, move.Key.cost);
                                button.move = null;
                            }
                            else
                            {
                                button.SetButtonColour(Color.white);
                                button.SetButonTextColour(Color.white);
                                button.SetBattleButton(move.Key, move.Key.cost);
                            }
                            button.gameObject.SetActive(true);
                            ind++;
                        }
                    }
                }
                break;
                */
            case "Items":
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
