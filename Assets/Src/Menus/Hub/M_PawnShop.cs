using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PawnShop : S_MenuSystem
{
    public R_AssetList playerAssets;
    public B_AssetItem[] assetButtons;
    public CH_Text changeMenu;
    public CH_Asset selectAsset;
    public R_Asset selectedAsset;

    private void OnEnable()
    {
        selectAsset.OnMoveFunctionEvent += SelectAsset;
    }

    private void OnDisable()
    {
        selectAsset.OnMoveFunctionEvent -= SelectAsset;
    }

    public void SelectAsset(O_Asset asset) {
        selectedAsset._asset = asset;
        changeMenu.RaiseEvent("PawnItemViewer");
    }

    public override void StartMenu()
    {
        foreach (var item in assetButtons) {
            item.gameObject.SetActive(false);
        }
        base.StartMenu();
        int ind = 0;
        foreach (var asset in playerAssets.assetList) {
            if (asset.amount > 0) {
                var button = assetButtons[ind];
                button.gameObject.SetActive(true);
                button.SetButonText(asset.name + " x " + asset.amount + " $" +  asset.currentValue.ToString("F2"));
                button.SetAssetButton(asset);
                ind++;
            }
        }
    }
}
