using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "System/Move Channel")]
public class CH_Move : SO_ChannelDefaut
{
    public UnityAction<s_move> OnMoveFunctionEvent;
    public void RaiseEvent(s_move _move)
    {
        if (OnMoveFunctionEvent != null)
            OnMoveFunctionEvent.Invoke(_move);
    }
}
