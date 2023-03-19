using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_BattleTarget : O_Button
{
    public CH_BattleChar target;
    public CH_BattleCharacter moveClickEvent;

    public void OnClickEvent()
    {
        moveClickEvent.RaiseEvent(target);
    }

    public void SetTargetButton(CH_BattleChar target)
    {
        this.target = target;
        text.text = "" + target.name + " (" + (target.health * 100) + "%" + ")";
    }

}
