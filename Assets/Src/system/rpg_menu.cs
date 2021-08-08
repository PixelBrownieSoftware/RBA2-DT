using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.System.Core;
using MagnumFoundation2.System;

public class rpg_menu : s_mainmenu
{
    public override void CallLoadSave()
    {
        LoadSave<s_RPGSave>();
    }
}
