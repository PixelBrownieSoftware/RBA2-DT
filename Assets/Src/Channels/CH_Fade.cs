using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "System/Fade Channel")]
public class CH_Fade : SO_ChannelDefaut
{
    public UnityAction<Color> OnFadeColourEventRaised;

    public void Fade(Color color)
    {
        if (OnFadeColourEventRaised != null)
            OnFadeColourEventRaised.Invoke(color);
    }
}
