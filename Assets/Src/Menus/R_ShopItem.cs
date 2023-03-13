using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Shop items")]
public class R_ShopItem : R_Default
{
    public List<Shop_item> shopItems = new List<Shop_item>();

    private void OnDisable()
    {
        shopItems.Clear();
    }

    private void OnEnable()
    {
        shopItems.Clear();
        shopItems = new List<Shop_item>();
    }

    public void AddItem(Shop_item shopItem) {
        shopItems.Add(shopItem);
    }
}
