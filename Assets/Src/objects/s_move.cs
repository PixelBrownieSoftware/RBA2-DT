using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Move")]
public class s_move : s_ability
{
    /*
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
    */
    /*
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
        public S_Element element;
        public MOVE_REQ_TYPE type;
        public s_move move;
        public bool requirePower = false;
        public int lowerPowerReq;
        public int upperPowerReq = -1;  // 0 > means that there is no upper bound
    }
    */

    [System.Serializable]
    public struct statusInflict
    {
        public S_StatusEffect status_effect;
        public float status_inflict_chance;
        public int duration;
        public int damage;
    }

    public enum MOVE_TYPE
    {
        HP_DAMAGE,
        SP_DAMAGE,
        HP_SP_DAMAGE,
        HP_RECOVER,
        SP_RECOVER,
        HP_SP_RECOVER,
        HP_DRAIN,
        SP_DRAIN,
        HP_SP_DRAIN,
        NONE
    }
    public enum MOVE_TARGET
    {
        NONE,
        ENEMY,
        ALLY,
        ENEMY_ALLY,
        SELF
    }
    public enum SCOPE_NUMBER { 
        ONE,
        ALL,
        RANDOM
    }
    public int power;
    public bool fixedValue = false;
    public bool consumeTurn = true;
    public MOVE_TYPE moveType = MOVE_TYPE.HP_DAMAGE;
    public MOVE_TARGET moveTarg = MOVE_TARGET.ENEMY;
    public SCOPE_NUMBER moveTargScope = SCOPE_NUMBER.ONE;
    public S_Element element;
    public S_Element elementsSheild;
    public s_actionAnim[] preAnimations;
    public s_actionAnim[] animations;
    public s_actionAnim[] endAnimations;
    public statusInflict[] statusInflictChance;
    public int cost = 0;

    public string customFunc;

    public int strBuff;
    public int vitBuff;
    public int dexBuff;
    public int agiBuff;
    public int intBuff;
    public int lucBuff;
    public int guardPoints = 0;

    public bool canBuff
    {
        get {
            return
                strBuff > 0 ||
                vitBuff > 0 ||
                dexBuff > 0 ||
                agiBuff > 0 ||
                intBuff > 0 ||
                lucBuff > 0;
        }
    }
    public bool canDebuff
    {
        get
        {
            return
                strBuff < 0 ||
                vitBuff < 0 ||
                dexBuff < 0 ||
                agiBuff < 0 ||
                intBuff < 0 ||
                lucBuff < 0;
        }
    }

    /*
    public moveRquirementList[] moveRequirements;

    public enum MOVE_QUANITY_TYPE {
        MONO_TECH,
        DUAL_TECH,
        TRIPLE_TECH,
        QUAD_TECH,
        PENTA_TECH
    }
    */
}