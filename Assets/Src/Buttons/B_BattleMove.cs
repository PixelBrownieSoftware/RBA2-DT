using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class B_BattleMove: O_Button
{
    public s_move move;
    public CH_Move moveClickEvent;
    public TextMeshProUGUI tmp;

    public void OnClickEvent() {
        moveClickEvent.RaiseEvent(move);
    }

    public void SetBattleButton(s_move move, int cost) {
        this.move = move;
        if(text != null)
            text.text = "" + move.name;
        if (tmp != null)
            tmp.text = "" + cost;
    }

}
