using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Status effect", menuName = "Status effect")]
public class S_StatusEffect : ScriptableObject
{
    public enum RESTRICTION { NONE, CANNOT_MOVE, RANDOM_FOE, RANDOM_ALLY, RANDOM_ALL }
    public RESTRICTION restriction;
    public VARIABLE_CHANGE variableChange;
    public int maxDuration;
    public int minDuration;

    public enum VARIABLE_CHANGE { HP_REGEN, SP_REGEN, HP_SP_REGEN}
    public float regenPercentage; //A value from -1.0 - 1.0, this is a percentage of the power of the "damage" dealt by the move 
    
    public int strAffect;
    public int vitAffect;
    public int dexAffect;
    public int agiAffect;
    public int intAffect;
    public int lucAffect;

    public bool removeOnEndRound = false;
    
    //If a character with this status effect gets hit by a move with one of these elements,
    //a critical hit will occur
    public S_Element[] criticalOnHit;
    public Sprite statusImage;

    public S_StatusEffect[] statusRemove;
}
