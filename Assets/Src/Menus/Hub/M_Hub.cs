using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Hub : S_MenuSystem
{
    public R_CharacterSetterList partyMembersStart;
    public R_EnemyGroupList groupsStart;
    public R_BattleCharacterList partyMembers;
    public S_RPGGlobals rpgManager;
    [SerializeField]
    private R_Boolean isSave;
    [SerializeField]
    private R_Boolean hasStartedGame;
    public R_Float money;
    public R_ShopItem shopItems;
    public CH_Text menuChanger;
    public B_String shopButton;

    public override void StartMenu()
    {
        base.StartMenu();
        if (!hasStartedGame.boolean)
        {
            if (!isSave.boolean)
            {
                money._float = 0;
                foreach (var ind in partyMembersStart.characterSetters)
                {
                    rpgManager.AddPartyMember(ind, 1);
                }
                foreach (var ind in groupsStart.groupList)
                {
                    rpgManager.groupsCurrent.AddGroup(ind);
                }
            }
            else
            {
                rpgManager.LoadSaveData();
            }
            hasStartedGame.boolean = true;
        }
        if (shopItems.shopItems.Count > 0) {
            shopButton.gameObject.SetActive(true);
        } else {
            shopButton.gameObject.SetActive(false);
        }
    }
}
