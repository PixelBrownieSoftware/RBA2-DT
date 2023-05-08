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
    public class el_weaknesses
    {
        public S_Element element;
        public float value;
    }
    public float money;

    /// Base stats the character starts with
    public int maxSkillPointsB = 1;
    public int maxHitPointsB = 1;

    public int maxHitPointsGMin = 1;
    public int maxHitPointsGMax = 1;
    public int maxSkillPointsGMin = 1;
    public int maxSkillPointsGMax = 1;

    public int agilityB = 1;
    public int strengthB = 1;
    public int vitalityB = 1;
    public int luckB = 1;
    public int intelligenceB = 1;
    public int dexterityB = 1;

    public int agilityGT = 3;
    public int strengthGT = 3;
    public int vitalityGT = 3;
    public int luckGT = 3;
    public int intelligenceGT = 3;
    public int dexterityGT = 3;

    public Sprite battleImage;

    public int turnIcons = 1;
    
    public List<s_move> moveLearn;
    public List<S_Passive> passiveLearn;
    //public element_affinity elementAffinities;
    public el_weaknesses[] elementals;
    public Color characterColour = new Color(1, 0.95f, 0.75f);
    public Color characterColour2 = new Color(1, 0.95f, 0.75f);

    public ai_page[] aiPages = new ai_page[1];
    public RuntimeAnimatorController anim;
    public o_RPGItem[] items;
    public s_move firstMove;
    public s_move secondMove;
    public s_move thirdMove;
    //public o_weapon defaultPhysWeapon;
    //public o_weapon defaultRangedWeapon;
    public bool persistence;

    public bool equip_fist = true;
    public bool equip_gun = true;
    public bool equip_sword = true;
    public bool equip_staff = true;

    public Dictionary<S_Element, float> GetElements {
        get {
            Dictionary<S_Element, float> els = new Dictionary<S_Element, float>();
            foreach (var element in elementals) {
                if (element.element == null)
                {
                    Debug.LogWarning("There is an unset element in " + name + ". Please remove or change it.");
                    continue;
                }
                els.Add(element.element, element.value);
            }
            return els;
        }
    }
}