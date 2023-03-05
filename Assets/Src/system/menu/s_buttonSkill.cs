using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoundation2.System.Core;
using MagnumFoundation2.System;

public class s_buttonSkill : s_button
{
    public s_battleMenu BMenu;
    public s_move moveButton;
    public s_passive passiveButton;
    public o_weapon weaponButton;
    public s_consumable consumable;
    public R_Items inventory;
    public Image element; 
    public Image buttonImg; 
    public enum SKILL_TYPE {
        NONE,
        BATTLE,
        GROUP_SELECT,
        EXTRA_SKILLS,
        ITEM,
        ITEM_BATTLE,
        WEAPON_EQUIP,
        SHOP,
        EXTRA_PASSIVES,
        TAVERN,
        WEAPON_EQUIP_BATTLE
    }
    public SKILL_TYPE typeOfButton;
    public bool isComb = false;
    public bool isUsable = true;

    public Text costTxt;
    public Image costGUI;
    float cost;

    public s_moveComb moveCombination;

    protected override void OnHover()
    {
        switch (typeOfButton) {
            case SKILL_TYPE.EXTRA_SKILLS:
                s_menuhandler.GetInstance().
                    GetMenu<s_battleMenu>("OverworldExtraSkill").
                    ChangeRequirements(moveButton);
                break;
        }

        if (isComb)
        {
            BMenu.moveDescription.text =
                moveCombination.user1.name + " (" + moveCombination.user1Move.name + ") " +
                moveCombination.user2.name + " (" + moveCombination.user2Move.name + ") ";
        }
        base.OnHover();
    }

    public void SetCost(int a) {
        cost = a;
    }
    public void SetCost(float a)
    {
        cost = a;
    }

    private void Update()
    {
        switch (typeOfButton)
        {
            case SKILL_TYPE.SHOP:
                if (cost > s_rpgGlobals.money)
                {
                    buttonImg.color = Color.grey;
                }
                else
                {
                    buttonImg.color = Color.white;
                }
                costTxt.text = cost + "";
                break;

            case SKILL_TYPE.TAVERN:
                if (consumable.price > s_rpgGlobals.money)
                {
                    buttonImg.color = Color.grey;
                }
                else
                {
                    buttonImg.color = Color.white;
                }
                costTxt.text = cost + "";
                break;

            case SKILL_TYPE.WEAPON_EQUIP:

                o_battleCharPartyData d = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill").partyCharacter;
                if (d.currentPhysWeapon == weaponButton)
                {
                    buttonImg.color = Color.green;
                }
                else
                {
                    if (d.currentRangeWeapon == weaponButton)
                    {
                        buttonImg.color = Color.magenta;
                    }
                    else
                    {
                        buttonImg.color = Color.white;
                    }
                }
                break;

            case SKILL_TYPE.WEAPON_EQUIP_BATTLE:
                o_battleCharacter bc = s_battleEngine.GetInstance().currentCharacter;
                if (bc.physWeapon == weaponButton)
                {
                    buttonImg.color = Color.green;
                }
                else
                {
                    if (bc.rangedWeapon == weaponButton)
                    {
                        buttonImg.color = Color.magenta;
                    }
                    else
                    {
                        buttonImg.color = Color.white;
                    }
                }
                break;

            case SKILL_TYPE.EXTRA_PASSIVES:

                d = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill").partyCharacter;
                if (isGoodForAssignmentPassive(d))
                {
                    if (hasPassives(d))
                    {
                        buttonImg.color = Color.green;
                    }
                    else
                    {
                        buttonImg.color = Color.white;
                    }
                }
                else
                {
                    if (d.passives.Find(x => x == passiveButton) == null)
                        buttonImg.color = Color.black;
                    else
                        buttonImg.color = Color.grey;
                }
                break;

            case SKILL_TYPE.EXTRA_SKILLS:

                d = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill").partyCharacter;
                if (isGoodForAssignment(d))
                {
                    if (hasSkill(d))
                    {
                        buttonImg.color = Color.green;
                    }
                    else
                    {
                        buttonImg.color = Color.white;
                    }
                }
                else
                {
                    if (d.extraSkills.Find(x => x == moveButton) == null)
                        buttonImg.color = Color.black;
                    else
                        buttonImg.color = Color.grey;
                }
                break;

            case SKILL_TYPE.BATTLE:
                if (menuButton != null)
                {
                    switch (moveButton.moveType)
                    {
                        case s_move.MOVE_TYPE.PHYSICAL:
                            cost = Mathf.RoundToInt((float)(moveButton.cost / 100f) * s_battleEngine.engineSingleton.currentCharacter.maxHealth);
                            costTxt.text = cost + " <color=#05E5B6>HP</color>";
                            break;

                        case s_move.MOVE_TYPE.SPECIAL:
                        case s_move.MOVE_TYPE.STATUS:
                            costTxt.text = cost + " <color=#E705A3>SP</color>";
                            break;
                    }
                }
                break;

            case SKILL_TYPE.ITEM:
            case SKILL_TYPE.ITEM_BATTLE:
                costTxt.text = "" + cost;
                break;

        }
    }

    public bool hasSkill(o_battleCharPartyData d)
    {
        if (d.extraSkills.Find(x => x == moveButton))
        {
            return true;
        }
        return false;
    }

    public bool hasPassives(o_battleCharPartyData d)
    {
        if (d.passives.Find(x => x == passiveButton))
        {
            return true;
        }
        return false;
    }

    public bool isGoodForAssignmentPassive(o_battleCharPartyData d)
    {
        if (d.passives.Find(x => x == passiveButton) != null)
            return false;

        /*
        if (d.strength >= passiveButton.strReq
        && d.vitality >= passiveButton.vitReq
        && d.dexterity >= passiveButton.dxReq
        && d.agility >= passiveButton.agiReq)
            return true;
        else
        */
            return false;
    }

    public bool isGoodForAssignment(o_battleCharPartyData d)
    {
        if (d.currentMoves.Find(x => x == moveButton) != null)
            return false;
        /*
        if (d.strength >= moveButton.strReq
        && d.vitality >= moveButton.vitReq
        && d.dexterity >= moveButton.dxReq
        && d.agility >= moveButton.agiReq)
            return true;
        else
        */
            return false;
    }

    public bool canEquipWeapon(o_battleCharPartyData d, o_weapon weap)
    {
        switch (weap.weaponType)
        {
            case o_weapon.WEAPON_TYPE.FIST:
                return d.characterDataSource.equip_fist;

            case o_weapon.WEAPON_TYPE.GUN:
                return d.characterDataSource.equip_gun;

            case o_weapon.WEAPON_TYPE.STAFF:
                return d.characterDataSource.equip_staff;

            case o_weapon.WEAPON_TYPE.SWORD:
                return d.characterDataSource.equip_sword;
        }
        return false;
    }

    public bool canEquipWeapon(o_battleCharacter d, o_weapon weap)
    {
        switch (weap.weaponType)
        {
            case o_weapon.WEAPON_TYPE.FIST:
                return d.battleCharData.equip_fist;

            case o_weapon.WEAPON_TYPE.GUN:
                return d.battleCharData.equip_gun;

            case o_weapon.WEAPON_TYPE.STAFF:
                return d.battleCharData.equip_staff;

            case o_weapon.WEAPON_TYPE.SWORD:
                return d.battleCharData.equip_sword;
        }
        return false;
    }

    protected override void OnButtonClicked()
    {
        switch (typeOfButton) {

            case SKILL_TYPE.SHOP:
                if (cost <= s_rpgGlobals.money)
                {
                    s_rpgGlobals.money -= cost;
                    s_rpgGlobals.rpgGlSingleton.AddItem(moveButton.name, 1);
                }
                break;

            case SKILL_TYPE.ITEM_BATTLE:
            case SKILL_TYPE.BATTLE:

                if (isUsable)
                {
                    s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").mov = moveButton;
                    if (typeOfButton == SKILL_TYPE.ITEM_BATTLE)
                    {
                        inventory.RemoveItem(moveButton);
                    }
                    if (isComb)
                    {
                        s_battleEngine.engineSingleton.SelectSkillOption(moveButton, moveCombination);
                    }
                    else
                    {
                        s_battleEngine.engineSingleton.SelectSkillOption(moveButton);
                    }
                    s_soundmanager.GetInstance().PlaySound("selectOption");
                }
                break;


            case SKILL_TYPE.GROUP_SELECT:
                base.OnButtonClicked();
                break;

            case SKILL_TYPE.EXTRA_PASSIVES:
                {
                    s_soundmanager.GetInstance().PlaySound("selectOption");
                    o_battleCharPartyData d = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill").partyCharacter;
                    if (isGoodForAssignmentPassive(d))
                    {
                        if (d.passives.Find(x => x == passiveButton) == null)
                        {
                            d.passives.Add(passiveButton);
                        }
                        else
                        {
                            d.passives.Remove(passiveButton);
                        }
                    }
                    else
                    {
                        if (d.passives.Find(x => x == passiveButton) != null)
                        {
                            d.passives.Remove(passiveButton);
                        }
                    }
                }
                break;

            case SKILL_TYPE.EXTRA_SKILLS:
                {
                    s_soundmanager.GetInstance().PlaySound("selectOption");
                    o_battleCharPartyData d = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill").partyCharacter;
                    if (isGoodForAssignment(d))
                    {
                        if (d.extraSkills.Find(x => x == moveButton) == null)
                        {
                            d.extraSkills.Add(moveButton);
                        }
                        else
                        {
                            d.extraSkills.Remove(moveButton);
                        }
                    }
                    else
                    {
                        if (d.extraSkills.Find(x => x == moveButton) != null)
                        {
                            d.extraSkills.Remove(moveButton);
                        }
                    }
                }
                break;

            case SKILL_TYPE.WEAPON_EQUIP_BATTLE:
                {
                    o_battleCharacter d = s_battleEngine.GetInstance().currentCharacter;
                    if (canEquipWeapon(d, weaponButton))
                    {
                        switch (weaponButton.weaponType)
                        {
                            case o_weapon.WEAPON_TYPE.FIST:
                            case o_weapon.WEAPON_TYPE.SWORD:
                                if (d.physWeapon == weaponButton)
                                {
                                    d.physWeapon = null;
                                }
                                else
                                {
                                    s_soundmanager.GetInstance().PlaySound("weap_equip");
                                    d.physWeapon = weaponButton;
                                }
                                break;

                            case o_weapon.WEAPON_TYPE.STAFF:
                            case o_weapon.WEAPON_TYPE.GUN:
                                if (d.rangedWeapon == weaponButton)
                                {
                                    d.rangedWeapon = null;
                                }
                                else
                                {
                                    s_soundmanager.GetInstance().PlaySound("weap_equip");
                                    d.rangedWeapon = weaponButton;
                                }
                                break;
                        }
                    }
                }
                break;

            case SKILL_TYPE.WEAPON_EQUIP:
                {
                    o_battleCharPartyData d = s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill").partyCharacter;
                    if (canEquipWeapon(d, weaponButton))
                    {
                        switch (weaponButton.weaponType)
                        {
                            case o_weapon.WEAPON_TYPE.FIST:
                            case o_weapon.WEAPON_TYPE.SWORD:
                                if (d.currentPhysWeapon == weaponButton)
                                {
                                    d.currentPhysWeapon = null;
                                }
                                else
                                {
                                    s_soundmanager.GetInstance().PlaySound("weap_equip");
                                    d.currentPhysWeapon = weaponButton;
                                }
                                break;

                            case o_weapon.WEAPON_TYPE.STAFF:
                            case o_weapon.WEAPON_TYPE.GUN:
                                if (d.currentRangeWeapon == weaponButton)
                                {
                                    d.currentRangeWeapon = null;
                                }
                                else
                                {
                                    s_soundmanager.GetInstance().PlaySound("weap_equip");
                                    d.currentRangeWeapon = weaponButton;
                                }
                                break;
                        }
                    }
                }
                break;
        }
    }
}
