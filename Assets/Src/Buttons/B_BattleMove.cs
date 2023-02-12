using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_BattleMove: O_Button
{
    public s_move move;
    public CH_Move moveClickEvent;

    public void OnClickEvent() {
        moveClickEvent.RaiseEvent(move);
    }

    public void SetBattleButton(s_move move) {
        this.move = move;
        if(text != null)
            text.text = "" + move.name;
    }

}
