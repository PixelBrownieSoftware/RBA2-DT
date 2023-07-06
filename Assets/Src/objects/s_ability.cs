using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct s_ability_req { }

public class s_ability : ScriptableObject
{
    public int strReq = 0;
    public int dxReq = 0;
    public int vitReq = 0;
    public int agiReq = 0;
    public int intReq = 0;
    public int lucReq = 0;
    public bool MeetsRequirements(o_battleCharPartyData bc)
    {
        if (strReq <= bc.strength 
            && dxReq <= bc.dexterity
            && vitReq <= bc.vitality 
            && agiReq <= bc.agility 
            && intReq <= bc.intelligence 
            && lucReq <= bc.luck)
            return true;
        return false;
    }
    public bool MeetsRequirements(o_battleCharacter bc)
    {
        if (strReq <= bc.strength
            && dxReq <= bc.dexterity 
            && vitReq <= bc.vitality 
            && agiReq <= bc.agility 
            && intReq <= bc.intelligence 
            && lucReq <= bc.luck )
            return true;
        return false;
    }

    public bool MeetsRequirements(o_battleCharPartyData bc, S_Element element)
    {
        int discount = bc.characterDataSource.GetDiscounts[element] * -1;
        if (strReq <= bc.strength + discount
            && dxReq <= bc.dexterity + discount
            && vitReq <= bc.vitality + discount
            && agiReq <= bc.agility + discount
            && intReq <= bc.intelligence + discount
            && lucReq <= bc.luck + discount)
            return true;
        return false;
    }
    public bool MeetsRequirements(o_battleCharacter bc, S_Element element)
    {
        int discount = bc.battleCharData.GetDiscounts[element] * -1;
        if (strReq <= bc.strength + discount
            && dxReq <= bc.dexterity + discount
            && vitReq <= bc.vitality + discount
            && agiReq <= bc.agility + discount
            && intReq <= bc.intelligence + discount
            && lucReq <= bc.luck + discount)
            return true;
        return false;
    }
}
