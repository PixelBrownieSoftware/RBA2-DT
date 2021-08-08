using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food item")]
public class s_consumable : ScriptableObject
{
    public float price;
    public int healthBoost;
    public int staminaBoost;
    public int strBoost;
    public int dexBoost;
    public int agiBoost;
    public int vitBoost;
    public int lucBoost;

    public int duration;
}
