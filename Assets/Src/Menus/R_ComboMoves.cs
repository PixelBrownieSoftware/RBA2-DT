using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Combo moves")]
public class R_ComboMoves : R_Default
{
    public Dictionary<s_move, s_moveComb> combinations = new Dictionary<s_move, s_moveComb>();
    private void OnDisable()
    {
        combinations.Clear();
    }

    private void OnEnable()
    {
        combinations.Clear();
        combinations = new Dictionary<s_move, s_moveComb>();
    }

    public Dictionary<s_move, s_moveComb> FindComboMovesUser(CH_BattleChar reference) {
        Dictionary<s_move, s_moveComb> returnedCombinations = new Dictionary<s_move, s_moveComb>();
        foreach (var item in combinations) {
            if (item.Value.user1 == reference) {
                returnedCombinations.Add(item.Key, item.Value);
            }
        }
        return returnedCombinations;
    }

    public void AddCombo(s_move mv,s_moveComb comb) {
        combinations.Add(mv, comb);
    }
}
