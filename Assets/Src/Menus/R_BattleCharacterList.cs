using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Overworld party list")]
public class R_BattleCharacterList : R_Default
{
    public List<o_battleCharPartyData> battleCharList = new List<o_battleCharPartyData>();

    public o_battleCharPartyData GetIndex(int ind)
    {
        return battleCharList[ind];
    }

    public void Add(o_battleCharPartyData bc) {
        battleCharList.Add(bc);
    }
}
