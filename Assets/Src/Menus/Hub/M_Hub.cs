using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Hub : S_MenuSystem
{
    public R_ShopItem shopItems;
    public CH_Text menuChanger;
    public B_String shopButton;

    public override void StartMenu()
    {
        base.StartMenu();
        if (shopItems.shopItems.Count > 0) {
            shopButton.gameObject.SetActive(true);
        } else {
            shopButton.gameObject.SetActive(false);
        }
    }
}
