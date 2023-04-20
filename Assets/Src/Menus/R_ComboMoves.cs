using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[CreateAssetMenu(menuName = "Registers/Combo moves")]
public class R_ComboMoves : R_Default
{
    public Dictionary<s_move, List<s_moveComb>> combinations = new Dictionary<s_move, List<s_moveComb>>();
    public Dictionary<CH_BattleChar, List<s_move>> characterMoveCombos = new Dictionary<CH_BattleChar, List<s_move>>();
    public List<s_move> allComboMoves = new List<s_move>();

    private void OnDisable()
    {
        combinations.Clear();
    }

    private void OnEnable()
    {
        combinations.Clear();
        combinations = new Dictionary<s_move, List<s_moveComb>>();
    }

    //I may continue on this in due course, it's a lot of headache.
    public void SetCombos() {
        foreach (var comboMove in allComboMoves) { 
            //comboMove.moveRequirements.
        }
    }

    public Dictionary<s_move, List<s_moveComb>> FindComboMovesUser(CH_BattleChar reference) {
        Dictionary<s_move, List<s_moveComb>> returnedCombinations = new Dictionary<s_move, List<s_moveComb>>();
        foreach (var item in combinations) {
            List<s_moveComb> combos = new List<s_moveComb>();
            foreach (var combo in item.Value)
            {
                if (combo.user1 == reference)
                {
                    combos.Add(combo);
                }
            }
            returnedCombinations.Add(item.Key, combos);
        }
        return returnedCombinations;
    }

    public void AddCombo(s_move mv,s_moveComb comb) {
        if (combinations.ContainsKey(mv))
        {
            combinations[mv].Add(comb);
        }
        else
        {
            List<s_moveComb> combine = new List<s_moveComb>();
            combine.Add(comb);
            combinations.Add(mv, combine);
        }
    }
}
*/