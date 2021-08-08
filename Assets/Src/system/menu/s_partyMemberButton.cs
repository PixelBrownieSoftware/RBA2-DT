using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_partyMemberButton : s_button
{
    public o_battleCharPartyData battleCharacter;
    public enum BUTTON_TYPE {
        EXTRA_SKILL,
        OVERWORLD_PARTYMENU,
        PARTY_BATTLE,
        EQUIP_WEAPON,
        PASSIVE_SKILL
    }
    public BUTTON_TYPE btnType; 

    protected override void OnButtonClicked()
    {
        s_battleMenu TM = null;
        switch (btnType) {
            case BUTTON_TYPE.EXTRA_SKILL:
                TM = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill");
                TM.menType = s_battleMenu.MENU_TYPE.EXTRA_SKILL;
                TM.partyCharacter = battleCharacter;
                s_menuhandler.GetInstance().SwitchMenu("OverworldExtraSkill");
                base.OnButtonClicked();
                break;

            case BUTTON_TYPE.PASSIVE_SKILL:
                TM = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill");
                TM.menType = s_battleMenu.MENU_TYPE.EXTRA_PASSIVES;
                TM.partyCharacter = battleCharacter;
                s_menuhandler.GetInstance().SwitchMenu("OverworldExtraSkill");
                base.OnButtonClicked();
                break;

            case BUTTON_TYPE.EQUIP_WEAPON:
                TM = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill");
                TM.menType = s_battleMenu.MENU_TYPE.ITEMS_EQUIP;
                TM.partyCharacter = battleCharacter;
                s_menuhandler.GetInstance().SwitchMenu("OverworldExtraSkill");
                base.OnButtonClicked();
                break;

            case BUTTON_TYPE.OVERWORLD_PARTYMENU:
                s_menuhandler.GetInstance().GetMenu<s_statusMenu>("OverworldStatus").SetCharacter(ref battleCharacter);
                s_menuhandler.GetInstance().SwitchMenu("OverworldStatus");
                break;

            case BUTTON_TYPE.PARTY_BATTLE:

                s_rpgGlobals.rpgGlSingleton.SetActivePartyMember(battleCharacter);
                if (battleCharacter.inBattle == true)
                {
                    GetComponent<Image>().color = Color.white;
                }
                else
                {
                    GetComponent<Image>().color = Color.blue;
                }
                break;
        }
    }
}
