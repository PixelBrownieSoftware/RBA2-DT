using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public class s_statusMenu : s_menucontroller
{
    public s_guiList str;
    public s_guiList dx;
    public s_guiList vit;
    public s_guiList agi;

    public Text strTXT;
    public Text dxTXT;
    public Text vitTXT;
    public Text agiTXT;

    public Text strShadTXT;
    public Text dxShadTXT;
    public Text vitShadTXT;
    public Text agiShadTXT;

    public o_battleCharPartyData characterData;
    public Text health;
    public Text stamina;
    public Text nameCharacter;
    bool isDirty = true;
    public List<s_elementalWeaknessGUI> affs = new List<s_elementalWeaknessGUI>();

    public void SetCharacter(ref o_battleCharPartyData ch) {
        isDirty = true;
        characterData = ch;
    }

    public override void OnOpen()
    {
        base.OnOpen(); ResetButton();
    }

    private void Update()
    {
        if (characterData != null) {
            if (isDirty)
            {
                health.text = "" + characterData.maxHealth;
                stamina.text = "" + characterData.maxStamina;
                nameCharacter.text = characterData.name;
                str.amount = characterData.strength;
                dx.amount = characterData.dexterity;
                agi.amount = characterData.agility;
                vit.amount = characterData.vitality;

                strTXT.text = strShadTXT.text = "" + characterData.strength;
                vitTXT.text = vitShadTXT.text = "" + characterData.vitality;
                dxTXT.text = dxShadTXT.text = "" + characterData.dexterity;
                agiTXT.text = agiShadTXT.text = "" + characterData.agility;

                //strike_aff.text = characterData.wea
                affs.ForEach(x => x.SetToDat());
                {
                    int i = 0;
                    foreach (s_move m in characterData.currentMoves)
                    {
                        s_button b = GetButton<s_button>(i);
                        b.txt.text = m.name;
                        i++;
                    }
                }
                isDirty = false;
            }
        }
    }

    public void SetButton(s_button b, int i, List<s_move> mov) {
        b.txt.text = mov[i].name;
    }
}
*/