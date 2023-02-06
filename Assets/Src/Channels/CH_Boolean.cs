using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "System/Boolean Channel")]
public class CH_Boolean : SO_ChannelDefaut
{
    public UnityAction<bool> OnFunctionEvent;
    public void RaiseEvent(bool _boolean)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_boolean);
    }
}

