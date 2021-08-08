using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_partyOverworldMenu : s_menucontroller
{
    public List<o_battleCharPartyData> partyMembers = new List<o_battleCharPartyData>();
    public PARTY_MENU_TYPE ptMode;
    public enum PARTY_MENU_TYPE {
        STATUS,
        REARANGE,
        EXTRA_SKILL,
        WEAPONS,
        PASSIVES
    }

    public override void OnOpen()
    {
        base.OnOpen();
        ResetButton();
        partyMembers = s_rpgGlobals.rpgGlSingleton.partyMembers;

        for (int i = 0; i < partyMembers.Count; i++) {
            s_partyMemberButton pm = GetButton<s_partyMemberButton>(i);
            pm.battleCharacter = partyMembers[i];
            pm.txt.text = pm.battleCharacter.name;
            switch (ptMode) {
                case PARTY_MENU_TYPE.REARANGE:
                    if (pm.battleCharacter.inBattle == true)
                        pm.GetComponent<Image>().color = Color.white;
                    else
                        pm.GetComponent<Image>().color = Color.blue;
                    pm.btnType = s_partyMemberButton.BUTTON_TYPE.PARTY_BATTLE;
                    break;

                case PARTY_MENU_TYPE.WEAPONS:
                    pm.btnType = s_partyMemberButton.BUTTON_TYPE.EQUIP_WEAPON;
                    break;

                case PARTY_MENU_TYPE.STATUS:
                    pm.btnType = s_partyMemberButton.BUTTON_TYPE.OVERWORLD_PARTYMENU;
                    break;

                case PARTY_MENU_TYPE.EXTRA_SKILL:
                    pm.btnType = s_partyMemberButton.BUTTON_TYPE.EXTRA_SKILL;
                    break;

                case PARTY_MENU_TYPE.PASSIVES:
                    pm.btnType = s_partyMemberButton.BUTTON_TYPE.PASSIVE_SKILL;
                    break;
            }
        }
    }
}
