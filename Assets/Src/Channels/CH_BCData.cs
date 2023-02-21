using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "System/Battle character data Channel")]
public class CH_BCData : SO_ChannelDefaut
{
    public UnityAction<o_battleCharPartyData> OnFunctionEvent;
    public void RaiseEvent(o_battleCharPartyData _battleChar)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_battleChar);
    }
}
