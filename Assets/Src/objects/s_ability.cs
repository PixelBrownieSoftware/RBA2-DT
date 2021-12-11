using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct s_ability_req { }

public class s_ability : ScriptableObject
{
   // public int ;

    public bool MeetsRequirements(o_battleCharacter bc)
    {
        /*
        if (strReq <= bc.strength
            && dxReq <= bc.dexterity
            && vitReq <= bc.vitality
            && agiReq <= bc.agility)
            return true;
        */
        return false;
    }
}
