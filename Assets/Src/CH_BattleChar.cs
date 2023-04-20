using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character reference", menuName = "ChRef")]
public class CH_BattleChar : ScriptableObject
{
    public string cName;
    public int health, maxHealth, stamina, maxStamina;
    public float exp;
    public int level;
    public Vector2 position;
    public o_battleCharPartyData characterData;
    public List<s_statusEff> statusEffects = new List<s_statusEff>();
    public Tuple<S_Element, float> sheildAffinity;

    public bool HasStatus(S_StatusEffect statEff)
    {
        if (statusEffects.Find(x => x.status == statEff) != null)
        {
            return true;
        }
        return false;
    }
    public int strengthNet
    {
        get;
        private set;
    }
    public int vitalityNet
    {
        get;
        private set;
    }
    public int dexterityNet
    {
        get;
        private set;
    }
    public int agiNet
    {
        get;
        private set;
    }
    public int intelligenceNet
    {
        get;
        private set;
    }
    public int luckNet
    {
        get;
        private set;
    }

    public List<s_move> GetAllMoves() {
        return characterData.AllSkills;
    }

    public void SetStats(o_battleCharacter bc)
    {
        cName = bc.name;
        health = bc.health;
        maxHealth = bc.maxHealth;
        stamina = bc.stamina;
        maxStamina = bc.maxStamina;
        exp = bc.experiencePoints;
        level = bc.level;
        strengthNet = bc.strengthNet;
        dexterityNet = bc.dexterityNet;
        intelligenceNet = bc.intelligenceNet;
        vitalityNet = bc.vitalityNet;
        agiNet = bc.agiNet;
        luckNet = bc.luckNet;
    }
}
