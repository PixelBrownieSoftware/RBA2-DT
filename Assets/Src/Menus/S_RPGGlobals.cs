using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Rpg Global")]
public class S_RPGGlobals : ScriptableObject
{
    public R_CharacterSetterList partyMembersStart;
    public R_BattleCharacterList partyMembers;
    public R_CharacterSetterList allCharactersData;
    public R_Items inventory;
    public List<string> weapons = new List<string>();
    public List<s_move> itemDatabase = new List<s_move>();
    public List<o_weapon> weaponDatabase = new List<o_weapon>();
    public R_MoveList moveDatabase;

    public bool ContainsPartyMember(o_battleCharDataN partyMember) {
        return partyMembers.battleCharList.Find(x => x.characterDataSource == partyMember) != null;
    }

    public void AddPartyMember(s_RPGSave.sav_party data)
    {
        o_battleCharPartyData newCharacter = new o_battleCharPartyData();
        {
            newCharacter.name = data.name;
            int tempHP = data.health;
            int tempSP = data.stamina;
            int tempStr = data.strength;
            int tempVit = data.vitality;
            int tempDx = data.dexterity;
            int tempAgi = data.agility;
            foreach (var pd in allCharactersData.characterSetters)
            {

                if (pd.name == data.characterDataSource)
                {
                    newCharacter.characterDataSource = pd;
                    break;
                }
            }

            if (data.currentPhysWeapon != "")
            {
                newCharacter.currentPhysWeapon = GetWeapon(data.currentPhysWeapon);
            }
            if (data.currentRangedWeapon != "")
            {
                newCharacter.currentRangeWeapon = GetWeapon(data.currentRangedWeapon);
            }

            //newCharacter.inBattle = true;

            newCharacter.currentMoves = new List<s_move>();
            foreach (string pd in data.currentMoves)
            {
                newCharacter.currentMoves.Add(moveDatabase.GetMove(pd));
            }
            newCharacter.health = newCharacter.maxHealth = tempHP;
            newCharacter.stamina = newCharacter.maxStamina = tempSP;

            newCharacter.strength = tempStr;
            newCharacter.vitality = tempVit;
            newCharacter.dexterity = tempDx;
            newCharacter.agility = tempAgi;
            newCharacter.inBattle = data.inBattle;
        }
        partyMembers.Add(newCharacter);
    }

    public void AddPartyMember(o_battleCharDataN data, int level)
    {
        o_battleCharPartyData newCharacter = new o_battleCharPartyData();
        {
            {
                if (partyMembers.GetActiveCount() < 5)
                {
                    newCharacter.inBattle = true;
                }
                else
                {
                    newCharacter.inBattle = false;
                }
            }
            newCharacter.name = data.name;
            int tempHP = data.maxHitPointsB;
            int tempSP = data.maxSkillPointsB;

            int tempHPMin = data.maxHitPointsGMin;
            int tempSPMin = data.maxSkillPointsGMin;
            int tempHPMax = data.maxHitPointsGMax;
            int tempSPMax = data.maxSkillPointsGMax;

            int tempStr = data.strengthB;
            int tempVit = data.vitalityB;
            int tempDx = data.dexterityB;
            int tempLuc = data.luckB;
            int tempAgi = data.agilityB;
            int tempInt = data.intelligenceB;

            for (int i = 0; i < level; i++)
            {
                tempHP += UnityEngine.Random.Range(tempHPMin, tempHPMax);
                tempSP += UnityEngine.Random.Range(tempSPMin, tempSPMax);

                if (data.strengthGT % level == 0)
                    tempStr++;
                if (data.vitalityGT % level == 0)
                    tempVit++;
                if (data.dexterityGT % level == 0)
                    tempDx++;
                if (data.luckGT % level == 0)
                    tempLuc++;
                if (data.agilityGT % level == 0)
                    tempAgi++;
                if (data.intelligenceGT % level == 0)
                    tempInt++;
                newCharacter.level++;
            }

            if (data.defaultRangedWeapon != null)
            {
                AddWeapon(data.defaultRangedWeapon.name);
                newCharacter.currentRangeWeapon = data.defaultRangedWeapon;
            }
            if (data.defaultRangedWeapon != null)
            {
                AddWeapon(data.defaultRangedWeapon.name);
                newCharacter.currentRangeWeapon = data.defaultRangedWeapon;
            }

            //newCharacter.inBattle = true;
            //newCharacter.elementAffinities = data.elementAffinities;
            newCharacter.currentMoves = new List<s_move>();

            foreach (s_move mov in data.moveLearn)
            {
                if (mov == null)
                    continue;
                if (mov.MeetsRequirements(newCharacter))
                {
                    newCharacter.currentMoves.Add(mov);
                }
            }
            newCharacter.health = newCharacter.maxHealth = tempHP;
            newCharacter.stamina = newCharacter.maxStamina = tempSP;

            newCharacter.strength = tempStr;
            newCharacter.vitality = tempVit;
            newCharacter.dexterity = tempDx;
            newCharacter.agility = tempAgi;
        }
        newCharacter.characterDataSource = data;
        partyMembers.Add(newCharacter);
    }
    public void SetPartyMemberStats(o_battleCharacter data)
    {
        o_battleCharPartyData newCharacter = partyMembers.Get(data.name);
        {
            newCharacter.name = data.name;

            newCharacter.strength = data.strength;
            newCharacter.vitality = data.vitality;
            newCharacter.dexterity = data.dexterity;


            newCharacter.health = data.health;
            newCharacter.maxHealth = data.maxHealth;
            newCharacter.stamina = data.stamina;
            newCharacter.maxStamina = data.maxStamina;

            newCharacter.currentMoves = data.currentMoves;
        }
    }

    public o_battleCharPartyData GetPartyData(o_battleCharDataN chara) {
        return partyMembers.battleCharList.Find(x => chara == x.characterDataSource);
    }

    public void AddExpToPartyMember(o_battleCharDataN chara, float expPts) {

        int initailTotal = (int)(expPts * 100);
        Debug.Log(initailTotal);
        o_battleCharPartyData ch = GetPartyData(chara);
        
        if (ch.level == 50) {
            return;
        }

        //we add the exp and make it so that it checks for a level up
        for (int i = 0; i < initailTotal; i++)
        {
            ch.experiencePoints += 0.01f;
            if (ch.experiencePoints >= 1f)
            {
                o_battleCharDataN chdat = ch.characterDataSource;
                if (i % chdat.strengthGT == 0)
                    ch.strength++;
                if (i % chdat.vitalityGT == 0)
                    ch.vitality++;
                if (i % chdat.dexterityGT == 0)
                    ch.dexterity++;
                if (i % chdat.intelligenceGT == 0)
                    ch.intelligence++;
                if (i % chdat.agilityGT == 0)
                    ch.agility++;
                if (i % chdat.luckGT == 0)
                    ch.luck++;
                ch.level++;
                ch.experiencePoints = 0;
                expPts = expPts * (float)((float)i / (float)initailTotal);
                ch.maxHealth += Random.Range(chdat.maxHitPointsGMin, chdat.maxHitPointsGMax + 1);
                ch.maxStamina +=Random.Range(chdat.maxSkillPointsGMin, chdat.maxSkillPointsGMax + 1); 
                List<s_move> mv2Learn = chdat.moveLearn.FindAll(x => x.MeetsRequirements(ch));
                if (mv2Learn != null)
                {
                    ch.currentMoves.AddRange(mv2Learn);
                }
                if (ch.level == 50)
                {
                    ch.experiencePoints = 0;
                    return;
                }
            }
            //yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public List<o_weapon> GetWeapons()
    {
        List<o_weapon> weaps = new List<o_weapon>();
        foreach (string val in weapons)
        {
            weaps.Add(GetWeapon(val));
        }
        return weaps;
    }
    public o_weapon GetWeapon(string itemName)
    {
        if (weapons.Contains(itemName))
        {
            return weaponDatabase.Find(x => x.name == itemName);
        }
        else return null;
    }
    public void AddWeapon(string itemName)
    {
        if (!weapons.Contains(itemName))
        {
            weapons.Add(itemName);
        }
    }
}
