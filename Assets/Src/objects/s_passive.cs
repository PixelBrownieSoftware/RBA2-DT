using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Passive move")]
public class s_passive : s_ability
{
    public enum PASSIVE_TYPE {
        STAT_BOOST,
        ELEMENT_DMG_BOOST,
        ELEMENT_RES_BOOST,
        SACRIFICE,
        REGEN,
        COUNTER,
        CUSTOM
    }
    public enum COUNTER_TYPE {
        PHYSICAL,
        SPECIAL
    }
    public enum STAT_TYPE
    {
        HEALTH,
        STAMINA,
    }

    public PASSIVE_TYPE passiveSkillType;
    public COUNTER_TYPE counterType;
    public STAT_TYPE stat;
    public S_Element element;
    public float percentage;
    public string customPassive;
}
