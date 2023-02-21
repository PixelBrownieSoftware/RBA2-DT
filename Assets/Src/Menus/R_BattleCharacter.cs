using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Overworld party")]
public class R_BattleCharacter : R_Default
{
    public o_battleCharPartyData battleCharacter;

    public void SetCharacter(o_battleCharPartyData bcD) {
        battleCharacter = bcD;
    }
}
