using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Items")]
public class R_Items : R_Default
{
    public Dictionary<s_move, int> inventory = new Dictionary<s_move, int>();
    private void OnDisable()
    {
        inventory.Clear();
    }

    public Tuple<s_move, int> GetItem(s_move item) {
        return new Tuple<s_move, int>(item, inventory[item]);
    }

    public List<s_move> GetItems() {
        List<s_move> items = new List<s_move>();
        foreach (var item in inventory) {
            items.Add(item.Key);
        }
        return items;
    }

    public void AddItem(s_move item) {
        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory.Add(item, 1);
        }
    }

    public void RemoveItem(s_move item)
    {
        if(inventory[item] > 0)
            inventory[item]--;
    }
}
