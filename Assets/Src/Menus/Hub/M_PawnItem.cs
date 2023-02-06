using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_PawnItem : S_MenuSystem
{
    public R_Asset assetItem;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDesc;
    public TextMeshProUGUI textPrice;
    public R_Float money;
    public CH_Func back;
    public CH_Asset assetFunc;
    public B_AssetItem assetButton;

    private void OnEnable()
    {
        assetFunc.OnMoveFunctionEvent += SellItem;
    }

    private void OnDisable()
    {
        assetFunc.OnMoveFunctionEvent -= SellItem;
    }

    public void SellItem(O_Asset item) {
        money._float += item.currentValue;
        item.amount = 0;
        back.RaiseEvent();
    }

    public override void StartMenu()
    {
        assetButton.asset = assetItem._asset;
        textName.text = assetItem._asset.name;
        textDesc.text = assetItem._asset.description;
        textPrice.text = "Current value: $" + assetItem._asset.currentValue.ToString("F2") + "(" + assetItem._asset.currentValuePercentage.ToString("F2") + "%)";
        base.StartMenu();
    }
}
