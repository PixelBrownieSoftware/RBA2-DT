using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Character list")]
public class R_CharacterList : R_Default
{
    /*
    public List<O_BattleCharacter> characterListRef = new List<O_BattleCharacter>();
    [SerializeField]
    private List<O_BattleCharacter> characterListRefBase = new List<O_BattleCharacter>();

    private void OnEnable()
    {
        if (_isReset) { 
            characterListRef.Clear();
            characterListRef.AddRange(characterListRefBase);
        }
    }

    public void Clear() {
        characterListRef.Clear();
    }
    public bool ListContains(R_Character targ)
    {
        return characterListRef.Contains(targ.characterRef);
    }
    public bool ListContains(O_BattleCharacter targ)
    {
        return characterListRef.Contains(targ);
    }

    public void Remove(R_Character targ) {
        characterListRef.Remove(targ.characterRef);
    }
    public void Remove(O_BattleCharacter targ)
    {
        characterListRef.Remove(targ);
    }
    public void Add(R_Character targ)
    {
        characterListRef.Add(targ.characterRef);
    }
    public void Add(O_BattleCharacter targ)
    {
        characterListRef.Add(targ);
    }

    public O_BattleCharacter GetChracter(int index)
    {
        return characterListRef[index];
    }
    */
}
