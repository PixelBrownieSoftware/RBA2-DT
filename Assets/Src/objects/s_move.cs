using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Move")]
public class s_move : s_ability
{
    [System.Serializable]
    public struct moveRquirementList
    {
        public MOVE_QUANITY_TYPE comboType;
        public moveRequirement Req1;
        public moveRequirement Req2;
        public moveRequirement Req3;
        public moveRequirement Req4;
        public moveRequirement Req5;
    }

    [System.Serializable]
    public class moveRequirement {
        public enum MOVE_REQ_TYPE {
            ELEMENTAL,
            BUFF,
            DEBUFF,
            SPECIFIC,
            WEAPON_PHYS,
            WEAPON_RANGE,
            HEAL_HP,
            HEAL_SP,
            HEAL_ANY,
        }
        public o_weapon.WEAPON_TYPE weapType;
        public ELEMENT element;
        public MOVE_REQ_TYPE type;
        public s_move move;
        public bool requirePower = false;
        public int lowerPowerReq;
        public int upperPowerReq = -1;  // 0 > means that there is no upper bound
    }
    [System.Serializable]
    public struct statusInflict
    {
        public STATUS_EFFECT status_effect;
        public float status_inflict_chance;
        public int duration;
        public int damage;
    }

    public enum MOVE_TYPE
    {
        PHYSICAL,
        SPECIAL,
        STATUS
    }
    public enum MOVE_TARGET
    {
        SINGLE,
        ALL,
        RANDOM,
        SELF
    }
    public int power;
    public MOVE_TYPE moveType;
    public MOVE_TARGET moveTarg;
    public ELEMENT element;
    public s_actionAnim[] preAnimations;
    public s_actionAnim[] animations;
    public s_actionAnim[] endAnimations;
    public statusInflict[] statusInflictChance;
    public bool onParty;
    public bool fixedValue = false;
    public int cost = 0;

    public enum STATUS_TYPE {
        NONE = 0,
        HEAL_HEALTH,
        HEAL_STAMINA,
        CURE,
        CUSTOM,
        BUFF,
        DEBUFF,
        HEAL_HP_BUFF,
        HEAL_SP_BUFF,
    }
    public STATUS_TYPE statusType;

    public int strBuff;
    public int vitBuff;
    public int dexBuff;
    public int agiBuff;

    public moveRquirementList[] moveRequirements;

    public enum MOVE_QUANITY_TYPE {
        MONO_TECH,
        DUAL_TECH,
        TRIPLE_TECH,
        QUAD_TECH,
        PENTA_TECH
    }
}