using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct o_RPGItem
{
    public s_move item;
    public int minAmount, maxAmount;
}

[CreateAssetMenu(fileName = "Battle character bluerpint", menuName = "Character data")]
public class o_battleCharDataN : ScriptableObject
{
    [System.Serializable]
    public class ai_page {

        public charAI[] ai;
    }

    [System.Serializable]
    public struct skill_affinity {
        public ELEMENT el;
        public int points;
    }

    public int level = 1;
    public float money;

    /// Base stats the character starts with
    public int maxSkillPoints = 1;
    public int maxSkillPointsB = 1;
    public int maxHitPoints = 1;
    public int maxHitPointsB = 1;

    public int agilityB;
    public int strengthB;
    public int vitalityB;
    public int luckB;
    public int intelligenceB;
    public int dexterityB;

    public float agilityGP;
    public float strengthGP;
    public float vitalityGP;
    public float luckGP;
    public float intelligenceGP;
    public float dexterityGP;

    public int agility;
    public int strength;
    public int vitality;
    public int luck;
    public int intelligence;
    public int dexterity;

    public s_passive characterPassive;

    public Sprite battleImage;

    public int turnIcons = 1;
    
    public List<s_move> moveLearn;
    public element_affinity elementAffinities;
    public element_weaknesses elementWeaknesses;
    public Color characterColour = new Color(1, 0.95f, 0.75f);
    public Color characterColour2 = new Color(1, 0.95f, 0.75f);

    public ai_page[] aiPages = new ai_page[1];
    public RuntimeAnimatorController anim;
    public o_RPGItem[] items;
    public o_weapon defaultPhysWeapon;
    public o_weapon defaultRangedWeapon;
    public bool persistence;

    public bool equip_fist = true;
    public bool equip_gun = true;
    public bool equip_sword = true;
    public bool equip_staff = true;
}