using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Battle character Channel")]
public class CH_BattleCharacter : SO_ChannelDefaut
{
    public UnityAction<o_battleCharacter> OnFunctionEvent;
    public void RaiseEvent(o_battleCharacter _battleChar)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_battleChar);
    }
}