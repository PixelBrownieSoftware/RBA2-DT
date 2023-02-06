using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_BattleTarget : O_Button
{
    public O_BattleCharacter target;
    public CH_BattleCharacter moveClickEvent;

    public void OnClickEvent()
    {
        moveClickEvent.RaiseEvent(target);
    }

    public void SetTargetButton(O_BattleCharacter target)
    {
        this.target = target;
        text.text = "" + target.charaName + " (" + (target.health * 100) + "%" + ")";
    }

}
