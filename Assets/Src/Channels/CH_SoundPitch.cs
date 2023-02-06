using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Sound pitch Channel")]
public class CH_SoundPitch : SO_ChannelDefaut
{
    public UnityAction<AudioClip, float> OnFunctionEvent;
    public void RaiseEvent(AudioClip _audio, float _pitch)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_audio, _pitch);
    }
}
