using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;


[System.Serializable]
public struct Shop_item
{
    public s_move item;
    public float price;
}


public class M_ItemShop : S_MenuSystem
{
    public CH_Func buyFunction;
    public CH_Int itemSelectFunction;
    public R_Float money;
    public R_Items inventory;
    public R_ShopItem shopItems;

    public Shop_item currentItem;

    public B_Int[] shopItemButtons;
    public B_Function rollButton;
    public Slider tokenSlider;
    int amount;

    private void OnEnable()
    {
        itemSelectFunction.OnFunctionEvent += SelectItem;
        buyFunction.OnFunctionEvent += BuyItem;
    }

    private void OnDisable()
    {
        itemSelectFunction.OnFunctionEvent -= SelectItem;
        buyFunction.OnFunctionEvent -= BuyItem;
    }

    public override void StartMenu()
    {
        base.StartMenu();
        int index = 0;
        foreach (var button in shopItemButtons) {

            if (shopItems.shopItems.Count <= index) {
                button.gameObject.SetActive(false);
                index++;
                continue;
            }
            Shop_item it = shopItems.shopItems[index];
            if (it.item != null)
            {
                button.gameObject.SetActive(true);
                button.SetButonText(it.item.name + " £" + it.price);
                button.SetIntButton(index);
                index++;
            }
        }
    }

    public void BuyItem()
    {
        for (int i = 0; i < amount; i++)
        {
            money._float -= currentItem.price;
            inventory.AddItem(currentItem.item);
        }
    }

    public void SelectItem(int itemNumber) {
        currentItem = shopItems.shopItems[itemNumber];
    }

    private void Update()
    {
        if (currentItem.item != null)
        {
            int afford = Mathf.FloorToInt(money._float / currentItem.price);
            if (afford > 0)
            {
                tokenSlider.gameObject.SetActive(true);
                rollButton.gameObject.SetActive(true);
                tokenSlider.maxValue = afford;
                amount = (int)tokenSlider.value;
                rollButton.SetButonText(tokenSlider.value + " £" + currentItem.price * tokenSlider.value);
            }
            else
            {
                tokenSlider.gameObject.SetActive(false);
                rollButton.gameObject.SetActive(false);
            }
        }
    }
}
