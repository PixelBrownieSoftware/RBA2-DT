using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character reference", menuName = "ChRef")]
public class CH_BattleChar : ScriptableObject
{
    public string cName;
    public int health, maxHealth, stamina, maxStamina;
    public Vector2 position;
    public List<s_statusEff> statusEffects = new List<s_statusEff>();

    public bool HasStatus(STATUS_EFFECT statEff)
    {
        if (statusEffects.Find(x => x.status == statEff) != null)
        {
            return true;
        }
        return false;
    }

    public void SetStats(o_battleCharacter bc)
    {
        cName = bc.name;
        health = bc.health;
        maxHealth = bc.maxHealth;
        stamina = bc.stamina;
        maxStamina = bc.maxStamina;
    }
}
