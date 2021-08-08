using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.System;

public class s_buttonItem : s_button
{
    public s_move moveButton;

    protected override void OnButtonClicked()
    {
        s_battleEngine.engineSingleton.battleAction.move = moveButton;
        s_battleEngine.engineSingleton.battleAction.type = s_battleEngine.s_battleAction.MOVE_TYPE.ITEM;
        s_menuhandler.GetInstance().GetMenu<s_targetMenu>
("TargetMenu").bcs = s_battleEngine.engineSingleton.GetTargets(moveButton.onParty);
        s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").mov = moveButton;
        base.OnButtonClicked();
    }

}
