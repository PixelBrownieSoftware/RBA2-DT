using MagnumFoundation2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BC_Stats {

    public int strength;
    public int vitality;
    public int dexterity;
    public int agility;
    public int intelligence;
    public int luck;
}

/*
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
*/

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
/*
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
*/

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
    public S_Element element;
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
        CHAR_ANIMATION,
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

    public float experiencePoints;
    public bool inBattle;
    public bool isActive = true;

    public o_battleCharDataN characterDataSource;
    public List<s_move> currentMoves = new List<s_move>();
    public List<s_move> extraSkills = new List<s_move>();
    public List<S_Passive> passives = new List<S_Passive>();
    public List<S_Passive> extraPassives = new List<S_Passive>();
    public Dictionary<S_Element, float> elementals = new Dictionary<S_Element, float>();

    public void SetElementWeakness(S_Element element, float aff)
    {
        if (!elementals.ContainsKey(element))
        {
            elementals.Add(element, aff);
        }
        else
        {
            elementals[element] = aff;
        }
    }

    public float GetElementWeakness(S_Element element)
    {
        if (!elementals.ContainsKey(element))
        {
            return 1f;
        }
        return elementals[element];
    }

    public List<s_move> AllSkills {
        get {
            List<s_move> skills = new List<s_move>();
            skills.AddRange(currentMoves);
            skills.AddRange(extraSkills);
            return skills;
        }
    }
}
//[System.Serializable]

/*
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
*/

[System.Serializable]
public class s_statusEff
{
    public s_statusEff()
    {
    }
    public s_statusEff(S_StatusEffect status, int duration) {
        this.status = status;
        this.duration = duration;
    }
    public s_statusEff(S_StatusEffect status, int duration, int damage)
    {
        this.damage = damage;
        this.status = status;
        this.duration = duration;
    }

    public S_StatusEffect status;
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

    private BC_Stats statusStats = new BC_Stats();
    public float experiencePoints;
    public CH_BattleChar referencePoint;

    public List<s_move> currentMoves;
    public List<s_move> extraSkills;
    public List<S_Passive> passives;
    public List<S_Passive> extraPassives;
    public o_battleCharDataN.ai_page character_AI;
    public int characterPage;
    public s_rpganim animations;

    public o_battleCharDataN battleCharData;
    //public element_affinity elementalAffinities;
    public List<s_statusEff> statusEffects = new List<s_statusEff>();

    public Rigidbody2D rbody2d;
    bool isIdle = true;
    public SpriteRenderer render;
    public SpriteRenderer shadow;

    public bool persistence = true;
    public bool inBattle;

    public o_weapon physWeapon;
    public o_weapon rangedWeapon;
    public s_move secondMove;
    public s_move thirdMove;

    public Animation[] customAnims;
    public Animator animHandler;

    #region STATUS GUI STUFF
    public Image[] statusEffectIcon;

    public GameObject strengthBuffIcon;
    public GameObject dexterityBuffIcon;
    public GameObject agilityBuffIcon;
    public GameObject vitalityBuffIcon;
    public GameObject intelligenceBuffIcon;
    public GameObject luckBuffIcon;
    public SpriteRenderer sheildFx;

    public Sprite poisionIcon;
    public Sprite stunIcon;
    public Sprite confuseIcon;
    public Sprite burnIcon;
    public Sprite frozenIcon;
    #endregion

    public int strengthNet {
        get
        {
            int net = strength + statusStats.strength;
            net = Mathf.Clamp(net, 1, int.MaxValue);
            return net;
        }
    }
    public int vitalityNet
    {
        get
        {
            int net = vitality + guardPoints + statusStats.vitality;
            net = Mathf.Clamp(net, 1, int.MaxValue);
            return net;
        }
    }
    public int dexterityNet
    {
        get
        {
            int net = dexterity + statusStats.dexterity;
            net = Mathf.Clamp(net, 1, int.MaxValue);
            return net;
        }
    }
    public int agiNet
    {
        get
        {
            int net = agility  + statusStats.agility;
            net = Mathf.Clamp(net, 1, int.MaxValue);
            return net;
        }
    }
    public int intelligenceNet
    {
        get
        {
            int net = intelligence + statusStats.intelligence;
            net = Mathf.Clamp(net, 1, int.MaxValue);
            return net;
        }
    }
    public int luckNet
    {
        get
        {
            int net = luck + statusStats.luck;
            net = Mathf.Clamp(net, 1, int.MaxValue);
            return net;
        }
    }

    public int guardPoints = 0;

    public void UpdateStatusEffectBuffs() {
        int str = 0;
        int vit = 0;
        int agi = 0;
        int intel = 0;
        int dex = 0;
        int luc = 0;

        foreach (var status in statusEffects) {
            S_StatusEffect statEff = status.status;
            str += statEff.strAffect;
            vit += statEff.vitAffect;
            agi += statEff.agiAffect;
            intel += statEff.intAffect;
            dex += statEff.dexAffect;
            luc += statEff.lucAffect;
        }

        statusStats.strength = str; 
        statusStats.vitality = vit; 
        statusStats.strength = agi; 
        statusStats.strength = intel; 
        statusStats.dexterity = dex;
        statusStats.luck = luc;
    }

    public List<s_move> GetAllMoves
    {
        get
        {
            List<s_move> moves = new List<s_move>();
            moves.AddRange(currentMoves);
            moves.AddRange(extraSkills);
            return moves;
        }
    }
    public List<S_Passive> GetAllPassives
    {
        get
        {
            List<S_Passive> moves = new List<S_Passive>();
            moves.AddRange(passives);
            moves.AddRange(extraPassives);
            return moves;
        }
    }

    public void Awake()
    {
           animations = GetComponent<s_rpganim>();
        rbody2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(health > 0)
            shadow.enabled = true;
        else
            shadow.enabled = false;

        if (referencePoint != null)
        {
            referencePoint.position = transform.position;
            referencePoint.SetStats(this);
        }
        if (referencePoint.sheildAffinity != null)
        {
            sheildFx.gameObject.SetActive(true);
            Color shAffColour = referencePoint.sheildAffinity.Item1.elementColour;
            sheildFx.color = new Color(shAffColour.a, shAffColour.b, shAffColour.g, 0.75f);
        }
        else {
            sheildFx.gameObject.SetActive(false);
        }
        #region STATUS EFFECTS
        for (int i = 0; i < statusEffectIcon.Length; i++)
        {
            if (statusEffects.Count <= i)
            {
                statusEffectIcon[i].enabled = false;
                statusEffectIcon[i].color = Color.clear;
                continue;
            }
            statusEffectIcon[i].sprite = statusEffects[i].status.statusImage;
            statusEffectIcon[i].enabled = true;
            statusEffectIcon[i].color = Color.white;
        }

        #endregion

    }
    public void SetStatsToPartyData(o_battleCharPartyData dat)
    {
        maxHealth = dat.maxHealth;
        health = dat.health;

        stamina = dat.stamina;
    }


    public bool HasStatus(S_StatusEffect statEff) {
        if (statusEffects.Find(x => x.status == statEff) != null) {
            return true;
        }
        return false;
    }

    public void SetStatus(s_statusEff statEff)
    {
        if (statusEffects.Find(x => x.status == statEff.status) == null)
        {
            statusEffects.Add(statEff);
            UpdateStatusEffectBuffs();
        }
    }
    public s_statusEff GetStatus(S_StatusEffect statEff) {
        return statusEffects.Find(x => x.status == statEff);
    }
    public void RemoveStatus(S_StatusEffect statEff)
    {
        if (statusEffects.Find(x => x.status == statEff) != null)
        {
            statusEffects.Remove(statusEffects.Find(x => x.status == statEff));
        }
    }
    public void RemoveStatusOnEndTurn()
    {
        if (statusEffects != null)
        {
            foreach (var status in statusEffects)
            {
                if (status.status.removeOnEndRound)
                {
                    RemoveStatus(status.status);
                }
            }
        }
    }

    public void StopAnimation() {
        animHandler.Play("");
    }
    public void SwitchAnimation(string animName)
    {
        animHandler.Play(animName);
    }

    public float GetAnimHandlerState() {
        return animHandler.GetCurrentAnimatorStateInfo(0).length;
    }

    public s_move GetRandomMove {
        get {
            List<s_move> allMoves = new List<s_move>();
            allMoves.AddRange(currentMoves);
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
