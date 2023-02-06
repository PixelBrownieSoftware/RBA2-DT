using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_SceneTrans : O_Button
{
    public CH_MapTransfer transfer;
    public string scene;
    public void OnClickEvent()
    {
        transfer.RaiseEvent(scene);
    }
}
