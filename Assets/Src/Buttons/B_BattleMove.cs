using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_BattleMove: O_Button
{
    public O_Move move;
    public CH_Move moveClickEvent;

    public void OnClickEvent() {
        moveClickEvent.RaiseEvent(move);
    }

    public void SetBattleButton(O_Move move) {
        this.move = move;
        if(text != null)
            text.text = "" + move.name;
    }

}
