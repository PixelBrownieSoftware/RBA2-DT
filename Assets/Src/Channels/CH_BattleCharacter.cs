using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Battle character Channel")]
public class CH_BattleCharacter : SO_ChannelDefaut
{
    public UnityAction<O_BattleCharacter> OnFunctionEvent;
    public void RaiseEvent(O_BattleCharacter _battleChar)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_battleChar);
    }
}