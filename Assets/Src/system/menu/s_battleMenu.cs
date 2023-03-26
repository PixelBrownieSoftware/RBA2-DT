using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2;
using UnityEngine.UI;
/*
public class s_battleMenu : s_menucontroller
{
    public List<s_move> rpgSkills = new List<s_move>();
    public List<s_passive> passiveSkills = new List<s_passive>();
    public List<o_weapon> weapons = new List<o_weapon>();
    public List<s_moveComb> combinations = new List<s_moveComb>();
    public List<s_consumable> consumables = new List<s_consumable>();
    public Text moveDescription;
    public o_battleCharPartyData partyCharacter;
    public R_BattleCharacterList partyMembers;
    public R_Items inventory;
    public R_Character currentCharacterRef;
    private CH_BattleChar currentCharacter;

    public float[] prices;

    public enum MENU_TYPE {
        BATTLE,
        BATTLE_COMBO,
        EXTRA_SKILL,
        ITEMS,
        ITEMS_BATTLE,
        ITEMS_EQUIP,
        SHOP,
        EXTRA_PASSIVES,
        TAVERN,
        ITEMS_EQUIP_BATTLE
    }
    public MENU_TYPE menType;

    public Sprite fire_picture;
    public Sprite water_picture;
    public Sprite ice_picture;
    public Sprite electric_picture;
    public Sprite wind_picture;
    public Sprite earth_picture;
    public Sprite dark_picture;
    public Sprite light_picture;
    public Sprite bio_picture;
    public Sprite neuclear_picture;
    public Sprite psychic_picture;
    public Sprite strike_picture;
    public Sprite force_picture;
    public Sprite perice_picture;

    public Sprite support_picture;
    public Sprite heal_HP_picture;
    public Sprite heal_SP_picture;

    public s_statReq str;
    public s_statReq vit;
    public s_statReq dex;
    public s_statReq agi;

    public bool PhysWeapEqupped(o_weapon weap) {

        switch (menType)
        {
            case MENU_TYPE.ITEMS_EQUIP_BATTLE:
                foreach (o_battleCharacter bc in s_battleEngine.GetInstance().playerCharacters)
                {
                    if (bc == s_battleEngine.GetInstance().currentCharacter)
                        continue;
                    if (bc.physWeapon == weap)
                        return true;
                }
                break;

            default:
                foreach (o_battleCharPartyData bc in partyMembers.battleCharList)
                {
                    if (bc == partyCharacter)
                        continue;
                    if (bc.currentPhysWeapon == weap)
                        return true;
                }
                break;
        }
        return false;
    }
    public bool RangeWeapEqupped(o_weapon weap)
    {
        switch (menType) {
            case MENU_TYPE.ITEMS_EQUIP_BATTLE:
                foreach (o_battleCharacter bc in s_battleEngine.GetInstance().playerCharacters)
                {
                    if (bc == s_battleEngine.GetInstance().currentCharacter)
                        continue;
                    if (bc.rangedWeapon == weap)
                        return true;
                }
                break;

            default:
                foreach (o_battleCharPartyData bc in partyMembers.battleCharList)
                {
                    if (bc == partyCharacter)
                        continue;
                    if (bc.currentRangeWeapon == weap)
                        return true;
                }
                break;
        }
        return false;
    }

    public void SetButtonElement(ref s_buttonSkill img) {

        Sprite draw = null;
        switch (img.moveButton.element)
        {
            case ELEMENT.STRIKE:
                draw = strike_picture;
                break;
            case ELEMENT.FIRE:
                draw = fire_picture;
                break;
            case ELEMENT.ICE:
                draw = ice_picture;
                break;
            case ELEMENT.BIO:
                draw = bio_picture;
                break;
            case ELEMENT.ELECTRIC:
                draw = electric_picture;
                break;
            case ELEMENT.PEIRCE:
                draw = perice_picture;
                break;
            case ELEMENT.WATER:
                draw = water_picture;
                break;
            case ELEMENT.EARTH:
                draw = earth_picture;
                break;
            case ELEMENT.DARK:
                draw = dark_picture;
                break;
            case ELEMENT.LIGHT:
                draw = light_picture;
                break;
            case ELEMENT.PSYCHIC:
                draw = psychic_picture;
                break;
            case ELEMENT.WIND:
                draw = wind_picture;
                break;
        }
        switch (img.moveButton.moveType) {
            case s_move.MOVE_TYPE.HP_RECOVER:
                draw = heal_HP_picture;
                break;
            case s_move.MOVE_TYPE.SP_RECOVER:
            case s_move.MOVE_TYPE.HP_SP_RECOVER:
                draw = heal_SP_picture;
                break;
            case s_move.MOVE_TYPE.NONE:
                draw = support_picture;
                break;
        }
        img.element.color = Color.white;
        img.costGUI.color = Color.white;
        img.element.sprite = draw;
    }
    */

    /*
    public override void OnOpen()
    {
        base.OnOpen();
        currentCharacter = currentCharacterRef.characterRef;
        ResetButton();
        int ind = 0;
        switch (menType)
        {
            case MENU_TYPE.SHOP:
                List<s_shopItem> shopItems = s_rpgGlobals.rpgGlSingleton.shopItems;
                foreach (var a in shopItems)
                {
                    s_buttonSkill sb = GetButton<s_buttonSkill>(ind);
                    sb.BMenu = this;
                    sb.SetCost(a.price);
                    sb.moveButton = a.item;
                    sb.txt.text = a.item.name;
                    sb.isUsable = true;
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.SHOP;
                    Sprite draw = null;
                    sb.element.sprite = draw;
                    sb.costGUI.color = Color.white;
                    ind++;
                }
                break;

            case MENU_TYPE.TAVERN:
                foreach (var a in consumables)
                {
                    s_buttonSkill sb = GetButton<s_buttonSkill>(ind);
                    sb.BMenu = this;
                    sb.SetCost(a.price);
                    sb.consumable = a;
                    sb.txt.text = a.name;
                    sb.isUsable = true;
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.TAVERN;
                    Sprite draw = null;
                    sb.element.sprite = draw;
                    sb.costGUI.color = Color.white;
                    ind++;
                }
                break;

            case MENU_TYPE.ITEMS_EQUIP_BATTLE:
                weapons = s_rpgGlobals.rpgGlSingleton.GetWeapons();
                for (int i = 0; i < weapons.Count; i++)
                {
                    if (PhysWeapEqupped(weapons[i]))
                        continue;
                    if (RangeWeapEqupped(weapons[i]))
                        continue;
                    s_buttonSkill sb = GetButton<s_buttonSkill>(i);
                    sb.BMenu = this;
                    sb.gameObject.SetActive(true);
                    sb.weaponButton = weapons[i];
                    sb.txt.text = weapons[i].name;
                    sb.isUsable = true;
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.WEAPON_EQUIP_BATTLE;
                    Sprite draw = null;
                    switch (sb.weaponButton.element)
                    {
                        case ELEMENT.STRIKE:
                            draw = strike_picture;
                            break;
                        case ELEMENT.FIRE:
                            draw = fire_picture;
                            break;
                        case ELEMENT.ICE:
                            draw = ice_picture;
                            break;
                        case ELEMENT.BIO:
                            draw = water_picture;
                            break;
                        case ELEMENT.ELECTRIC:
                            draw = electric_picture;
                            break;
                        case ELEMENT.PEIRCE:
                            draw = perice_picture;
                            break;
                        case ELEMENT.EARTH:
                            draw = earth_picture;
                            break;
                        case ELEMENT.DARK:
                            draw = dark_picture;
                            break;
                        case ELEMENT.LIGHT:
                            draw = light_picture;
                            break;
                        case ELEMENT.PSYCHIC:
                            draw = psychic_picture;
                            break;
                        case ELEMENT.WIND:
                            draw = wind_picture;
                            break;
                    }
                    sb.element.sprite = draw;
                    sb.costGUI.color = Color.white;
                }
                break;

            case MENU_TYPE.ITEMS_EQUIP:
                weapons = s_rpgGlobals.rpgGlSingleton.GetWeapons();
                for (int i = 0; i < weapons.Count; i++)
                {
                    if (PhysWeapEqupped(weapons[i]))
                        continue;
                    if (RangeWeapEqupped(weapons[i]))
                        continue;
                    s_buttonSkill sb = GetButton<s_buttonSkill>(i);
                    sb.BMenu = this;
                    sb.gameObject.SetActive(true);
                    sb.weaponButton = weapons[i];
                    sb.txt.text = weapons[i].name;
                    sb.isUsable = true;
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.WEAPON_EQUIP;
                    Sprite draw = null;
                    switch (sb.weaponButton.element)
                    {
                        case ELEMENT.STRIKE:
                            draw = strike_picture;
                            break;
                        case ELEMENT.FIRE:
                            draw = fire_picture;
                            break;
                        case ELEMENT.ICE:
                            draw = ice_picture;
                            break;
                        case ELEMENT.BIO:
                            draw = water_picture;
                            break;
                        case ELEMENT.ELECTRIC:
                            draw = electric_picture;
                            break;
                        case ELEMENT.PEIRCE:
                            draw = perice_picture;
                            break;
                        case ELEMENT.EARTH:
                            draw = earth_picture;
                            break;
                        case ELEMENT.DARK:
                            draw = dark_picture;
                            break;
                        case ELEMENT.LIGHT:
                            draw = light_picture;
                            break;
                        case ELEMENT.PSYCHIC:
                            draw = psychic_picture;
                            break;
                        case ELEMENT.WIND:
                            draw = wind_picture;
                            break;
                    }
                    sb.element.sprite = draw;
                    sb.costGUI.color = Color.white;
                }
                break;

            case MENU_TYPE.ITEMS:
                rpgSkills = inventory.GetItems();
                for (int i = 0; i < rpgSkills.Count; i++)
                {
                    if (inventory.GetItem(rpgSkills[i]).Item2 == 0)
                        continue;
                    s_buttonSkill sb = GetButton<s_buttonSkill>(i);
                    sb.BMenu = this;
                    sb.gameObject.SetActive(true);
                    sb.moveButton = rpgSkills[i];
                    sb.txt.text = rpgSkills[i].name;
                    sb.isUsable = true;
                    sb.SetCost(inventory.GetItem(rpgSkills[i]).Item2);
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.ITEM;
                    Sprite draw = null;
                    switch (sb.moveButton.element)
                    {
                        case ELEMENT.STRIKE:
                            draw = strike_picture;
                            break;
                        case ELEMENT.FIRE:
                            draw = fire_picture;
                            break;
                        case ELEMENT.ICE:
                            draw = ice_picture;
                            break;
                        case ELEMENT.BIO:
                            draw = water_picture;
                            break;
                        case ELEMENT.ELECTRIC:
                            draw = electric_picture;
                            break;
                        case ELEMENT.PEIRCE:
                            draw = perice_picture;
                            break;
                        case ELEMENT.EARTH:
                            draw = earth_picture;
                            break;
                        case ELEMENT.DARK:
                            draw = dark_picture;
                            break;
                        case ELEMENT.LIGHT:
                            draw = light_picture;
                            break;
                        case ELEMENT.PSYCHIC:
                            draw = psychic_picture;
                            break;
                        case ELEMENT.WIND:
                            draw = wind_picture;
                            break;
                    }
                    sb.element.sprite = draw;
                    sb.costGUI.color = Color.white;
                }
                break;

            case MENU_TYPE.ITEMS_BATTLE:
                {
                    int i = 0;
                    rpgSkills = inventory.GetItems();
                    foreach (var it in inventory.inventory)
                    {
                        if (it.Value == 0)
                            continue;
                        s_buttonSkill sb = GetButton<s_buttonSkill>(i);
                        sb.BMenu = this;
                        sb.gameObject.SetActive(true);
                        sb.moveButton = it.Key;
                        sb.txt.text = it.Key.name;
                        sb.SetCost(it.Value);
                        sb.typeOfButton = s_buttonSkill.SKILL_TYPE.ITEM_BATTLE;
                        Sprite draw = null;
                        switch (sb.moveButton.element)
                        {
                            case ELEMENT.STRIKE:
                                draw = strike_picture;
                                break;
                            case ELEMENT.FIRE:
                                draw = fire_picture;
                                break;
                            case ELEMENT.ICE:
                                draw = ice_picture;
                                break;
                            case ELEMENT.BIO:
                                draw = water_picture;
                                break;
                            case ELEMENT.ELECTRIC:
                                draw = electric_picture;
                                break;
                            case ELEMENT.PEIRCE:
                                draw = perice_picture;
                                break;
                            case ELEMENT.EARTH:
                                draw = earth_picture;
                                break;
                            case ELEMENT.DARK:
                                draw = dark_picture;
                                break;
                            case ELEMENT.LIGHT:
                                draw = light_picture;
                                break;
                            case ELEMENT.PSYCHIC:
                                draw = psychic_picture;
                                break;
                            case ELEMENT.WIND:
                                draw = wind_picture;
                                break;
                        }
                        sb.element.sprite = draw;
                        sb.costGUI.color = Color.white;
                        i++;
                    }
                }
                break;

            case MENU_TYPE.BATTLE_COMBO:
                
                for (int i = 0; i < rpgSkills.Count; i++)
                {
                    s_buttonSkill sb = GetButton<s_buttonSkill>(i);
                    sb.BMenu = this;
                    sb.moveCombination = combinations[i];
                    sb.gameObject.SetActive(true);
                    sb.moveButton = rpgSkills[i];
                    sb.isUsable = false;
                    sb.txt.text = rpgSkills[i].name;
                    sb.isComb = true;
                    int cost = 0;
                    bool satisifedCondition = false;
                    switch (sb.moveButton.moveType)
                    {
                        case s_move.MOVE_TYPE.PHYSICAL:
                            cost = Mathf.RoundToInt((sb.moveButton.cost / 100) * currentCharacter.maxHealth);
                            if (currentCharacter.health > cost)
                            {
                                sb.isUsable = true;
                            }
                            break;
                        case s_move.MOVE_TYPE.SPECIAL:
                        case s_move.MOVE_TYPE.STATUS:
                            cost = sb.moveButton.cost;
                            switch (sb.moveCombination.comboType) {
                                case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:
                                    satisifedCondition = sb.moveCombination.user1.stamina >= cost && 
                                        sb.moveCombination.user2.stamina >= cost;
                                    break;
                                case s_move.MOVE_QUANITY_TYPE.TRIPLE_TECH:
                                    satisifedCondition = sb.moveCombination.user1.stamina >= cost &&
                                        sb.moveCombination.user2.stamina >= cost &&
                                        sb.moveCombination.user3.stamina >= cost;
                                    break;
                            }
                            if (satisifedCondition)
                            {
                                sb.isUsable = true;
                            }
                            break;
                    }
                    SetButtonElement(ref sb);
                    sb.SetCost(cost);
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.BATTLE;
                }
                break;

            case MENU_TYPE.BATTLE:
                rpgSkills = new List<s_move>();
                List<s_move> mv = new List<s_move>();
                mv.AddRange(s_battleEngine.engineSingleton.currentCharacter.currentMoves);
                mv.AddRange(s_battleEngine.engineSingleton.currentCharacter.extraSkills);
                rpgSkills = mv;
                for (int i = 0; i < rpgSkills.Count; i++)
                {
                    s_buttonSkill sb = GetButton<s_buttonSkill>(i);
                    sb.BMenu = this;
                    if (combinations.Count > i)
                        sb.moveCombination = combinations[i];
                    sb.gameObject.SetActive(true);
                    sb.moveButton = rpgSkills[i];
                    sb.isUsable = false;
                    sb.isComb = false;
                    sb.txt.text = rpgSkills[i].name;
                    int cost = 0;
                    switch(sb.moveButton.moveType) {
                        case s_move.MOVE_TYPE.PHYSICAL:
                            cost = Mathf.RoundToInt((float)(sb.moveButton.cost / 100f) * currentCharacter.maxHealth);
                            if (currentCharacterRef.characterRef.health > cost)
                            {
                                sb.isUsable = true;
                            }
                            break;
                        case s_move.MOVE_TYPE.SPECIAL:
                        case s_move.MOVE_TYPE.STATUS:
                            cost = sb.moveButton.cost;
                            if (currentCharacterRef.characterRef.stamina >= cost)
                            {
                                sb.isUsable = true;
                            }
                            break;
                    }
                    sb.SetCost(cost);
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.BATTLE;
                    SetButtonElement(ref sb);
                }
                break;

            case MENU_TYPE.EXTRA_SKILL:
                rpgSkills = s_rpgGlobals.rpgGlSingleton.extraSkills.moveListRef;
                for (int i = 0; i < rpgSkills.Count; i++)
                {
                    if (partyMembers.battleCharList.Find(
                        x => (x.extraSkills.Contains(rpgSkills[i]) && x != partyCharacter)
                        || x.currentMoves.Contains(rpgSkills[i])) != null)
                    {
                        continue;
                    }
                    s_buttonSkill sb = GetButton<s_buttonSkill>(ind);
                    sb.BMenu = this;
                    sb.gameObject.SetActive(true);
                    sb.moveButton = rpgSkills[i];
                    sb.txt.text = rpgSkills[i].name;
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.EXTRA_SKILLS;
                    Sprite draw = null;
                    switch (sb.moveButton.element)
                    {
                        case ELEMENT.STRIKE:
                            draw = strike_picture;
                            break;
                        case ELEMENT.FIRE:
                            draw = fire_picture;
                            break;
                        case ELEMENT.ICE:
                            draw = ice_picture;
                            break;
                        case ELEMENT.BIO:
                            draw = water_picture;
                            break;
                        case ELEMENT.ELECTRIC:
                            draw = electric_picture;
                            break;
                        case ELEMENT.PEIRCE:
                            draw = perice_picture;
                            break;
                        case ELEMENT.EARTH:
                            draw = earth_picture;
                            break;
                        case ELEMENT.DARK:
                            draw = dark_picture;
                            break;
                        case ELEMENT.LIGHT:
                            draw = light_picture;
                            break;
                        case ELEMENT.PSYCHIC:
                            draw = psychic_picture;
                            break;
                        case ELEMENT.WIND:
                            draw = wind_picture;
                            break;
                    }
                    sb.element.sprite = draw;
                    ind++;
                }
                break;

            case MENU_TYPE.EXTRA_PASSIVES:
                passiveSkills = s_rpgGlobals.rpgGlSingleton.extraPassives;
                for (int i = 0; i < passiveSkills.Count; i++)
                {
                    if (partyMembers.battleCharList.Find(
                        x => (x.passives.Contains(passiveSkills[i]) && x != partyCharacter)) != null)
                    {
                        continue;
                    }
                    s_buttonSkill sb = GetButton<s_buttonSkill>(ind);
                    sb.BMenu = this;
                    sb.gameObject.SetActive(true);
                    sb.passiveButton = passiveSkills[i];
                    sb.txt.text = passiveSkills[i].name;
                    sb.typeOfButton = s_buttonSkill.SKILL_TYPE.EXTRA_PASSIVES;
                    Sprite draw = null;
                    sb.element.sprite = draw;
                    ind++;
                }
                break;
        }
    }

    public void ChangeRequirements(s_move mov)
    {
        str.statNum = partyCharacter.strength;
        vit.statNum = partyCharacter.vitality;
        dex.statNum = partyCharacter.dexterity;
        agi.statNum = partyCharacter.agility;
        
        str.requirementNum = mov.strReq;
        vit.requirementNum = mov.vitReq;
        dex.requirementNum = mov.dxReq;
        agi.requirementNum = mov.agiReq;

        if (str.requirementNum > 0)
            str.gameObject.SetActive(true);
        else
            str.gameObject.SetActive(false);

        if (vit.requirementNum > 0)
            vit.gameObject.SetActive(true);
        else
            vit.gameObject.SetActive(false);

        if (dex.requirementNum > 0)
            dex.gameObject.SetActive(true);
        else
            dex.gameObject.SetActive(false);

        if (agi.requirementNum > 0)
            agi.gameObject.SetActive(true);
        else
            agi.gameObject.SetActive(false);
    }

    public void ChangeRequirements(s_passive mov)
    {
        str.statNum = partyCharacter.strength;
        vit.statNum = partyCharacter.vitality;
        dex.statNum = partyCharacter.dexterity;
        agi.statNum = partyCharacter.agility;

        str.requirementNum = mov.strReq;
        vit.requirementNum = mov.vitReq;
        dex.requirementNum = mov.dxReq;
        agi.requirementNum = mov.agiReq;

        if (str.requirementNum > 0)
            str.gameObject.SetActive(true);
        else
            str.gameObject.SetActive(false);

        if (vit.requirementNum > 0)
            vit.gameObject.SetActive(true);
        else
            vit.gameObject.SetActive(false);

        if (dex.requirementNum > 0)
            dex.gameObject.SetActive(true);
        else
            dex.gameObject.SetActive(false);

        if (agi.requirementNum > 0)
            agi.gameObject.SetActive(true);
        else
            agi.gameObject.SetActive(false);
    }

    public override void ResetButton<T>(T b)
    {
        base.ResetButton(b);
        s_buttonSkill d = b.GetComponent<s_buttonSkill>();
        d.typeOfButton = s_buttonSkill.SKILL_TYPE.NONE;
        d.element.sprite = null;
        d.element.color = Color.clear;
        d.costGUI.color = Color.clear;
        d.costTxt.text = "";
    }
}
*/