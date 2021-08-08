using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2;

public class s_overworldObj : s_mapholder
{
    public Sprite BGtex;

    protected new void Start()
    {
        base.Start();
        s_rpgGlobals.GetInstance().GetComponent<s_rpgGlobals>().BGBattle = BGtex;
    }
}
