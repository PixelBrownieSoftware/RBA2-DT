using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Character")]
public class R_Character : R_Default
{
    public CH_BattleChar characterRef;
    public void SetCharacter(CH_BattleChar targ) {
        characterRef = targ;
    }
}
