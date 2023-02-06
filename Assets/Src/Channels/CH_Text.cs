using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "System/Text Channel")]
public class CH_Text : SO_ChannelDefaut
{
    public UnityAction<string> OnTextEventRaised;
    public void RaiseEvent(string txt)
    {
        if (OnTextEventRaised != null)
            OnTextEventRaised.Invoke(txt);
    }
}
