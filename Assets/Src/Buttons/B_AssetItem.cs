using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AssetItem : O_Button
{
    public O_Asset asset;
    public CH_Asset moveClickEvent;

    public void OnClickEvent()
    {
        moveClickEvent.RaiseEvent(asset);
    }

    public void SetAssetButton(O_Asset _asset)
    {
        asset = _asset;
    }
}