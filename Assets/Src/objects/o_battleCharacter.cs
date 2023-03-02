using MagnumFoundation2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O_BattleCharacterStats
{
    public int level;
    public int maxHealth, health;
    public int maxStamina, stamina;

    public int strength;
    public int vitality;
    public int dexterity;
    public int agility;
    public int intelligence;
    public int luck;

    public int strengthBuff;
    public int vitalityBuff;
    public int dexterityBuff;
    public int agilityBuff;
    public int intelligenceBuff;
    public int luckBuff;
    public CH_BattleChar referencePoint;

    public List<s_move> currentMoves;
    public List<s_move> extraSkills;
    public List<s_passive> extraPassives;

    public o_battleCharDataN battleCharData;
    public element_weaknesses elementals;
    public List<s_statusEff> statusEffects = new List<s_statusEff>();

    public o_weapon physWeapon;
    public o_weapon rangedWeapon;
}

public enum ELEMENT_WEAKNESS {
    NONE,
    FRAIL,
    NULL,
    REFLECT,
    ABSORB
}

public enum STATUS_EFFECT
{
    NONE = -1,
    POISON,
    BURN,
    STUN,
    FROZEN,
    NAUSEA,
    CONFUSED,
    INFATUATED,
    SMIRK,
    RAGE,
    STRAIN,
    DEPRESSED
}
public enum ELEMENT { 
    NONE,
    STRIKE,
    PEIRCE,
    FIRE,
    ICE,
    WATER,
    ELECTRIC,
    EARTH,
    WIND,
    BIO,
    DARK,
    LIGHT,
    PSYCHIC
}

[System.Serializable]
public class o_equip
{
    public string name;
    public int hpInc;
    public int spInc;

    public int strInc;
    public int vitInc;
    public int dxInc;
    public int lcInc;
}
[System.Serializable]
public class charAI
{
    public enum CONDITIONS
    {
        ALWAYS,
        ELEMENT_TARG_FRAIL,
        ELEMENT_TARG_FRAIL_HP_LOWER,
        ELEMENT_TARG_FRAIL_HP_HIGHER,
        ELEMENT_TARG_STRONGEST,
        ELEMENT_TARG_STRONGEST_HP_LOWER,
        ELEMENT_TARG_STRONGEST_HP_HIGHER,
        PARTICULAR_TARG,
        TARGET_PARTY_NUM,
        TARGET_PARTY_HP_LOWER,
        TARGET_PARTY_HP_HIGHER,
        TARGET_PARTY_LEVEL,
        ON_TURN,
        SELF_HP_LOWER,
        SELF_HP_HIGHER,
        SELF_SP_HIGHER,
        SELF_SP_LOWER,
        USER_PARTY_NUM,
        USER_PARTY_HP_HIGHER,
        USER_PARTY_HP_LOWER,
        USER_PARTY_SP_HIGHER,
        USER_PARTY_SP_LOWER,
        NEVER
    }
    public enum TURN_COUNTER
    {
        NONE,
        TURN_COUNTER_EQU,
        ROUND_COUNTER_EQU,
        ROUND_TURN_COUNTER_EQU,
    }
    public enum ACTION
    {
        MOVE,
        GUARD,
        SWITCH_PAGE
    }
    public s_move move;
    public TURN_COUNTER turnCounters;
    public ACTION AIaction = ACTION.MOVE;
    public CONDITIONS conditions;
    public ELEMENT element;
    public float healthPercentage;
    public string name;
    public bool onParty;
    public bool isImportant = false;
    public int number1;
    public int number2;
}
[System.Serializable]
public class s_actionAnim
{
    public string name;
    public enum ACTION_TYPE { 
        WAIT, //either wait for a projectile,
        MOVE,
        FACE,
        PROJECTILE,
        HIT_ANIMATION,
        CALCULATION, //calculations,
        ANIMATION,
        MOVE_CAMERA,
        ZOOM_CAMERA,
        FADE_TARGET
    }
    public ACTION_TYPE actionType;

    public enum MOTION
    {
        FORWARDS,
        BACKWARDS,
        POINT,
        TO_TARGET,
        SELF,
        ALL,
        ALL_SELF,
        ALL_TARGET,
        USER_2,
        USER_3,
        USER_4,
        USER_5
    }
    public bool teleport = false;
    
    public MOTION start;
    public MOTION goal;
    public Vector2 offset;
    public int animation_id;
    public Sprite picture;
    public Animation animframes;
    public float time;
    public float speed;
    public float toZoom;
    public float initialZoom;
    public bool simulteneous = false;

    //Used for the target option
    public enum TARGET_POS  //Places will be relative to the targets direction
    {
        TOP,
        TOP_BACK,
        TOP_FRONT,
        CENTRE,
        CENTRE_BACK,
        CENTRE_FRONT,
        BASE,
        BASE_FRONT,
        BASE_BACK
    }
    public TARGET_POS targetPos;

    public Color startColour;
    public Color endColour;
}

//The animation of a hit or a projectile
[System.Serializable]
public struct s_moveAnim
{
    public string name;
    public enum MOVEPOSTION
    {
        ON_TARGET,
        FIXED,
        ALL_SAME_TIME,
        ALL_LEFT_TO_RIGHT,
        ALL_RIGHT_TO_LEFT
    }
    public MOVEPOSTION pos;
}

[System.Serializable]
public class o_battleCharPartyData
{
    public string name;
    public int maxHealth, health;
    public int maxStamina, stamina;
    public int level = 1;

    public int strength;
    public int vitality;
    public int dexterity;
    public int agility;
    public int intelligence;
    public int luck;

    public int knowledgePoints;
    public bool inBattle;
    public bool isActive = true;

    public o_battleCharDataN characterDataSource;
    public List<s_move> currentMoves = new List<s_move>();
    public List<s_move> extraSkills = new List<s_move>();
    public List<s_passive> passives = new List<s_passive>();
    public element_affinity elementAffinities;
    public element_weaknesses elementWeakness;
    public o_weapon currentPhysWeapon;
    public o_weapon currentRangeWeapon;
    public s_consumable consumable;
    public int durationConsumable;
}
//[System.Serializable]


[System.Serializable]
public class o_battleCharData 
{
    public string name;
    public int level;
    public int experience;
    public int experienceToNextLevel;
    public int maxHealth, health;
    public int maxStamina, stamina;

    public int strength;
    public int vitality;
    public int dexterity;
    public int agility;
    public int luck;

    /// Base stats the character starts with

    public int maxSkillPointsB;
    public int maxHitPointsB;
    public int strengthB;
    public int vitalityB;
    public int dexterityB;
    public int luckB;
    public int expB;

    //Growth turns

    public int healthGT;
    public int staminaGT;
    public int strengthGT;
    public int vitalityGT;
    public int dexterityGT;
    public int luckGT;

    //Health and Stamina will be determined by 
    public int maxSkillPointsGMax;
    public int maxSkillPointsGMin;

    public int maxHitPointsGMax;
    public int maxHitPointsGMin;

    public List<string> currentMoves;
    public List<s_move> extraSkills;
    public charAI[] character_AI;
    public float[] elementAffinities = new float[12];
}

[System.Serializable]
public class s_statusEff
{
    public s_statusEff()
    {
    }
    public s_statusEff(STATUS_EFFECT status, int duration) {
        this.status = status;
        this.duration = duration;
    }
    public s_statusEff(STATUS_EFFECT status, int duration, int damage)
    {
        this.damage = damage;
        this.status = status;
        this.duration = duration;
    }

    public STATUS_EFFECT status;
    public int duration;
    public int damage;
}

public class o_battleCharacter : MonoBehaviour
{
    public int level;
    public int maxHealth, health;
    public int maxStamina, stamina;

    public int strength;
    public int vitality;
    public int dexterity;
    public int agility;
    public int intelligence;
    public int luck;

    public int strengthBuff;
    public int vitalityBuff;
    public int dexterityBuff;
    public int agilityBuff;
    public int intelligenceBuff;
    public int luckBuff;
    public float experiencePoints;
    public CH_BattleChar referencePoint;

    public List<s_move> currentMoves;
    public List<s_move> extraSkills;
    public List<s_passive> extraPassives;
    public o_battleCharDataN.ai_page[] character_AI;
    public int characterPage;
    public s_rpganim animations;

    public o_battleCharDataN battleCharData;
    public element_weaknesses elementals;
    //public element_affinity elementalAffinities;
    public List<s_statusEff> statusEffects = new List<s_statusEff>();

    public Rigidbody2D rbody2d;
    bool isIdle = true;
    public SpriteRenderer render;
    public SpriteRenderer shadow;

    //When this is on, a character will not be permenantley removed from battle
    //All playables and bosses have this flag on
    public bool persistence = false;
    public bool inBattle;

    public o_weapon physWeapon;
    public o_weapon rangedWeapon;

    public Animation[] customAnims;
    public Animator animHandler;

    #region STATUS GUI STUFF
    public Image statusEffectSpeech;
    public Image statusEffectIcon;

    public Sprite poisionIcon;
    public Sprite stunIcon;
    public Sprite confuseIcon;
    public Sprite burnIcon;
    public Sprite frozenIcon;

    float statusTimer = 0f;
    const float statusFlipTimer = 0.5f;
    int statusFlipIndex = 0;
    int prevStatusFlipCount = 0;
    #endregion

    public int strengthNet {
        get {
            return strength + (int)((float)strengthBuff * 2.5f);
        }
    }
    public int vitalityNet
    {
        get
        {
            return vitality + guardPoints + (int)((float)vitalityBuff * 2.5f);
        }
    }
    public int dexterityNet
    {
        get
        {
            return dexterity + (int)((float)dexterityBuff * 2.5f);
        }
    }
    public int agiNet
    {
        get
        {
            return agility + (int)((float)agilityBuff * 2.5f);
        }
    }

    public int guardPoints = 0;

    public void Awake()
    {
        render = GetComponent<SpriteRenderer>();
           animations = GetComponent<s_rpganim>();
        rbody2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        shadow.enabled = inBattle;

        if (referencePoint != null)
        {
            referencePoint.position = transform.position;
            referencePoint.SetStats(this);
        }

        #region STATUS EFFECTS
        List<string> statusEffs = new List<string>();
        foreach (s_statusEff stat in statusEffects)
        {
            switch (stat.status)
            {
                case STATUS_EFFECT.SMIRK:
                    statusEffs.Add("smk");
                    break;
                case STATUS_EFFECT.POISON:
                    statusEffs.Add("psn");
                    break;
                case STATUS_EFFECT.STUN:
                    statusEffs.Add("stn");
                    break;

                case STATUS_EFFECT.CONFUSED:
                    statusEffs.Add("con");
                    break;

                case STATUS_EFFECT.FROZEN:
                    statusEffs.Add("frz");
                    break;

                case STATUS_EFFECT.BURN:
                    statusEffs.Add("brn");
                    break;
            }
        }
        if (statusEffs.Count > 0)
        {
            if (prevStatusFlipCount != statusEffs.Count)
            {
                statusFlipIndex = 0;
            }
            if (statusTimer > 0)
            {
                statusTimer -= Time.deltaTime;
            }
            else
            {
                statusTimer = statusFlipTimer;
                if (statusFlipIndex < statusEffs.Count - 1)
                    statusFlipIndex++;
                else
                    statusFlipIndex = 0;
            }
            switch (statusEffs[statusFlipIndex])
            {
                case "smk":
                    statusEffectIcon.sprite = poisionIcon;
                    break;
                case "psn":
                    statusEffectIcon.sprite = poisionIcon;
                    break;
                case "stn":
                    statusEffectIcon.sprite = stunIcon;
                    break;
                case "con":
                    statusEffectIcon.sprite = confuseIcon;
                    break;
                case "brn":
                    statusEffectIcon.sprite = burnIcon;
                    break;
                case "frz":
                    statusEffectIcon.sprite = frozenIcon;
                    break;
            }
            statusEffectSpeech.enabled = true;
            statusEffectIcon.color = Color.white;
        }
        else
        {
            statusEffectSpeech.enabled = false;
            statusEffectIcon.color = Color.clear;
        }

        #endregion
    }
    public void SetStatsToPartyData(o_battleCharPartyData dat)
    {
        maxHealth = dat.maxHealth;
        health = dat.health;

        stamina = dat.stamina;
    }

    public void SetStatsToPartyData()
    {

    }

    public void SetStatsToBattleCharData()
    {

    }

    public bool HasStatus(STATUS_EFFECT statEff) {
        if (statusEffects.Find(x => x.status == statEff) != null) {
            return true;
        }
        return false;
    }

    public void SetStatus(s_statusEff statEff)
    {
        if (statusEffects.Find(x => x.status == statEff.status) == null)
        {
            if (statEff.status == STATUS_EFFECT.SMIRK)
            {
                if (statusEffects.Count > 0)
                    statusEffects.Add(statEff);
            }
            else
            {
                statusEffects.Add(statEff);
                if (statusEffects.Find(x => x.status == STATUS_EFFECT.SMIRK) != null) {
                    statusEffects.Remove(statusEffects.Find(x => x.status == STATUS_EFFECT.SMIRK));
                }
            }
        }
    }
    public s_statusEff GetStatus(STATUS_EFFECT statEff) {
        return statusEffects.Find(x => x.status == statEff);
    }
    public void RemoveStatus(STATUS_EFFECT statEff)
    {
        if (statusEffects.Find(x => x.status == statEff) != null)
        {
            statusEffects.Remove(statusEffects.Find(x => x.status == statEff));
        }
    }

    public void StopAnimation() {
        animHandler.Play("");
    }
    public void SwitchAnimation(string animName)
    {
        animHandler.Play(animName);
    }

    public s_move GetRandomMove {
        get {
            List<s_move> allMoves = new List<s_move>();
            allMoves.AddRange(extraSkills);
            allMoves.AddRange(extraSkills);
            return allMoves[Random.Range(0, allMoves.Count)];
        }
    }

    public bool CheckIfCloseToTarget(float distance, Vector2 targPos)
    {
        if (Vector2.Distance(transform.position, targPos) <= distance)
        {
            return true;
        }
        return false;
    }

    /*
    public void PlayAnimation(string animStr) {
        switch (animStr) {

            default:

                break;
                
            case "idle":
                if (battleCharData.idleAnim)
                    animations.PlayAnimation(battleCharData.idleAnim, true);
                else
                    animations.done = true;
                break;

            case "attack":
                if (battleCharData.attackAnim)
                    animations.PlayAnimation(battleCharData.attackAnim, false);
                else
                    animations.done = true;
                break;

            case "hurt":
                if (battleCharData.hurtAnim)
                    animations.PlayAnimation(battleCharData.hurtAnim, false);
                else
                    animations.done = true;
                break;

            case "cast":
                if (battleCharData.castAnim)
                    animations.PlayAnimation(battleCharData.castAnim, false);
                else
                    animations.done = true;
                break;
        }
    }
    */

}
