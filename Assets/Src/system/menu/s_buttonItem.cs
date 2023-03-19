using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.System;

public class s_buttonItem : s_button
{
    public s_move moveButton;
    public R_CharacterList targets;

    protected override void OnButtonClicked()
    {
        //s_battleEngine.engineSingleton.battleAction.move = moveButton;
        s_battleEngine.engineSingleton.battleAction.type = s_battleEngine.s_battleAction.MOVE_TYPE.ITEM;
        s_battleEngine.engineSingleton.SetTargets(moveButton.onParty);
        s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").mov = moveButton;
        base.OnButtonClicked();
    }

}
