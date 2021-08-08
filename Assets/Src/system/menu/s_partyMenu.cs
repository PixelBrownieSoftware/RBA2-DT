using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_partyMenu : s_menucontroller
{
    public List<o_battleCharacter> bc = new List<o_battleCharacter>();

    public enum PARTY_MENU_TYPE {
        SWITCH,
        DEPLOY
    }
    public PARTY_MENU_TYPE partyMenuType;

    public override void OnOpen()
    {
        base.OnOpen();
        ResetButton();
        bc = s_battleEngine.engineSingleton.playerCharacters;
        
        for (int i = 0; i < bc.Count; i++)
        {
            o_battleCharacter b = bc[i];
            switch (partyMenuType)
            {
                case PARTY_MENU_TYPE.SWITCH:
                    if (s_battleEngine.engineSingleton.currentCharacter == b && 
                        s_battleEngine.engineSingleton.playerCharacters.FindAll(x=> x.inBattle).Count < 2
                        || b.inBattle == true)
                        continue;
                    break;

                case PARTY_MENU_TYPE.DEPLOY:
                    if (b.inBattle == true)
                        continue;
                    break;
            }

            GetButton<s_targetButton>(i).battleCharButton = b;
            if (s_battleEngine.engineSingleton.currentCharacter == b)
                GetButton<s_targetButton>(i).txt.text = "Withdraw";
            else
                GetButton<s_targetButton>(i).txt.text = b.name;
            switch (partyMenuType) {
                case PARTY_MENU_TYPE.DEPLOY:
                    GetButton<s_targetButton>(i).targType = s_targetButton.TARGET_TYPE.DEPLOY;
                    break;

                case PARTY_MENU_TYPE.SWITCH:
                    GetButton<s_targetButton>(i).targType = s_targetButton.TARGET_TYPE.SWITCH;
                    break;
            }
        }
    }
}
