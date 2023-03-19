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
        health.value = isHP ? (target.health / target.maxHealth) : (target.stamina / target.maxStamina);
    }
}
