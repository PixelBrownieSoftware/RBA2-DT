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
