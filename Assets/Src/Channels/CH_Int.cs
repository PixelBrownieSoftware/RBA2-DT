using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Integer Channel")]
public class CH_Int : SO_ChannelDefaut
{
    public UnityAction<int> OnFunctionEvent;
    public void RaiseEvent(int _int)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_int);
    }
}