using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_overworldPlayerTargetMenu : s_menucontroller
{
    public s_enemyGroup group;
    public o_battleCharDataN playerToPick;

    public override void OnOpen()
    {
        ResetButton();
        List<o_battleCharPartyData> bc = s_rpgGlobals.rpgGlSingleton.partyMembers;
        int i = 0;
        foreach (o_battleCharPartyData a in bc) {
            GetButton<s_partyMemberButton>(i).battleCharacter = a;
            GetButton<s_partyMemberButton>(i).txt.text = a.name;
            i++;
        }
        base.OnOpen();
    }
}
