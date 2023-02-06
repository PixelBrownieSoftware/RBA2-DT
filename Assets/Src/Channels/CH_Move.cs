using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "System/Move Channel")]
public class CH_Move : SO_ChannelDefaut
{
    public UnityAction<O_Move> OnMoveFunctionEvent;
    public void RaiseEvent(O_Move _move)
    {
        if (OnMoveFunctionEvent != null)
            OnMoveFunctionEvent.Invoke(_move);
    }
}
