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
    public o_weapon physWeapon;
    public o_weapon rangedWeapon;
    public List<s_statusEff> statusEffects = new List<s_statusEff>();

    public bool HasStatus(S_StatusEffect statEff)
    {
        if (statusEffects.Find(x => x.status == statEff) != null)
        {
            return true;
        }
        return false;
    }

    public List<s_move> GetAllMoves() {
        return characterData.AllSkills;
    }

    public void SetStats(o_battleCharacter bc)
    {
        cName = bc.name;
        health = bc.health;
        physWeapon = bc.physWeapon;
        rangedWeapon = bc.rangedWeapon;
        maxHealth = bc.maxHealth;
        stamina = bc.stamina;
        maxStamina = bc.maxStamina;
        exp = bc.experiencePoints;
        level = bc.level;
    }
}
