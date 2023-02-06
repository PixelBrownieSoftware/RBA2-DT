using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "System/Map Transfer Channel")]
public class CH_MapTransfer : SO_ChannelDefaut
{
    public UnityAction<string> OnMapTransferEvent;
    public void RaiseEvent(string location)
    {
        if (OnMapTransferEvent != null)
            OnMapTransferEvent.Invoke(location);
    }
}
