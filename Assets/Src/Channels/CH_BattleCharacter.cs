using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Battle character Channel")]
public class CH_BattleCharacter : SO_ChannelDefaut
{
    public UnityAction<CH_BattleChar> OnFunctionEvent;
    public void RaiseEvent(CH_BattleChar _battleChar)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_battleChar);
    }
}