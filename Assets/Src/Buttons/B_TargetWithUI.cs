using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class B_TargetWithUI : B_BattleTarget
{
    public Slider health;
    public bool isHP = true;

    public new void SetTargetButton(CH_BattleChar target) {
        base.SetTargetButton(target);
        float HPCompare = (float)((float)target.health / (float)target.maxHealth);
        print("HP: " + HPCompare);
        health.maxValue = 1f;
        health.value = isHP ? HPCompare : (float)((float)target.stamina / (float)target.maxStamina);
    }
}
