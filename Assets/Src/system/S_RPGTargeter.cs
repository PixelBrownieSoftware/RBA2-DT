using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RPGTargeter : MonoBehaviour
{
    [SerializeField]
    private R_Move selectedMove;
    [SerializeField]
    private R_Character currentCharacter;
    [SerializeField]
    private R_Character target;
    [SerializeField]
    private R_BattleCharacterList players;
    [SerializeField]
    private R_BattleCharacterList enemies;

    public void OnMoveSelected() {
/*
        switch (selectedMove.move.moveTarg) { 
            case s_move.MOVE_TARGET.
        }
*/
    }

}
