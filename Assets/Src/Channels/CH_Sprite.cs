using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "System/Sprite Channel")]
public class CH_Sprite : SO_ChannelDefaut
{
    public UnityAction<Sprite> OnFunctionEvent;
    public void RaiseEvent(Sprite _spr)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_spr);
    }
}
