using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_mainBattleMenu : s_menucontroller
{
    public o_battleCharacter battleCharacter;
    public Sprite fistIcon;
    public Sprite swordIcon;
    public Sprite gunIcon;
    public Sprite staffIcon;
    public R_Items inventory;

    public override void OnOpen()
    {
        base.OnOpen();
        ResetButton();
        //s_menuhandler.GetInstance().GetMenu<s_battleMenu>("SkillMenu").rpgSkills =
        List<Tuple<s_moveComb, s_move>> moves = s_rpgGlobals.rpgGlSingleton.CheckComboRequirementsCharacter3(
        battleCharacter.referencePoint, s_battleEngine.engineSingleton.playerCharacters);

        #region PHYSICAL WEAPON
        {
            Image img = GetButton<s_button>(0).GetComponent<Image>();
            if (battleCharacter.physWeapon != null)
            {
                switch (battleCharacter.physWeapon.weaponType)
                {
                    case o_weapon.WEAPON_TYPE.FIST:
                        img.sprite = fistIcon;
                        break;
                    case o_weapon.WEAPON_TYPE.SWORD:
                        img.sprite = swordIcon;
                        break;
                    case o_weapon.WEAPON_TYPE.GUN:
                        img.sprite = gunIcon;
                        break;
                    case o_weapon.WEAPON_TYPE.STAFF:
                        img.sprite = staffIcon;
                        break;
                }
            }
            else
            {
                img.sprite = fistIcon;
            }
        }
        #endregion

        #region RANGED WEAPON
        if (battleCharacter.rangedWeapon != null)
        {
            Image img = GetButton<s_button>(1).GetComponent<Image>();
            switch (battleCharacter.rangedWeapon.weaponType)
            {
                case o_weapon.WEAPON_TYPE.GUN:
                    img.sprite = gunIcon;
                    break;
                case o_weapon.WEAPON_TYPE.STAFF:
                    img.sprite = staffIcon;
                    break;
            }
        }
        #endregion

        if (battleCharacter.extraSkills.Count > 0
            || battleCharacter.currentMoves.Count > 0) {
            GetButton<s_button>(2);
        }

        if (moves.Count > 0 && s_battleEngine.GetInstance().netTurn > 1) {
            GetButton<s_button>(3);
        } 
        GetButton<s_button>(4);

        if (inventory.inventory.Count > 0)
            GetButton<s_button>(5);

        GetButton<s_button>(6);
        GetButton<s_button>(7);
        GetButton<s_button>(8);

        if (s_battleEngine.engineSingleton.enemyGroup.fleeable)
            GetButton<s_button>(9).gameObject.SetActive(true);
        else
            GetButton<s_button>(9).gameObject.SetActive(false);

    }
}
