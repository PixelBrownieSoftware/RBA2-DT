using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "System/Function Channel")]
public class CH_Func : SO_ChannelDefaut
{
    public UnityAction OnFunctionEvent;
    public void RaiseEvent()
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke();
    }
}
