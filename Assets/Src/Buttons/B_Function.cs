using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Function : O_Button
{
    public CH_Func stringClickEvent;

    public void OnClickEvent()
    {
        stringClickEvent.RaiseEvent();
    }
}
