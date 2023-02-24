using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Overworld party list")]
public class R_BattleCharacterList : R_Default
{
    public List<o_battleCharPartyData> battleCharList = new List<o_battleCharPartyData>();

    private void OnEnable()
    {
        if (_isReset) {
            Clear();
        }
    }

    public o_battleCharPartyData GetIndex(int ind)
    {
        return battleCharList[ind];
    }

    public int GetActiveCount() {
        return battleCharList.FindAll(x => x.inBattle == true).Count;
    }

    public bool Find(System.Predicate<o_battleCharPartyData> pred, o_battleCharPartyData bc) {
        return pred.Invoke(bc);
    }

    public void Clear() {
        battleCharList.Clear();
    }

    public o_battleCharPartyData Get(string chName) {
        return battleCharList.Find(x => x.name == chName);
    }

    public void Add(o_battleCharPartyData bc) {
        battleCharList.Add(bc);
    }
}
