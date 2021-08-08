using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_ability : ScriptableObject
{
    public int strReq = 0;
    public int dxReq = 0;
    public int vitReq = 0;
    public int agiReq = 0;

    public bool MeetsRequirements(o_battleCharacter bc)
    {
        if (strReq <= bc.strength
            && dxReq <= bc.dexterity
            && vitReq <= bc.vitality
            && agiReq <= bc.agility)
            return true;
        return false;
    }
}
