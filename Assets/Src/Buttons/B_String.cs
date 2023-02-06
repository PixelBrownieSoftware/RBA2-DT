using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_String : O_Button
{
    public CH_Text stringClickEvent;

    public void OnClickEvent(string text)
    {
        stringClickEvent.RaiseEvent(text);
    }
}