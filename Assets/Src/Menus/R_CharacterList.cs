using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Character list")]
public class R_CharacterList : R_Default
{

    public List<CH_BattleChar> characterListRef = new List<CH_BattleChar>();
    [SerializeField]
    private List<CH_BattleChar> characterListRefBase = new List<CH_BattleChar>();

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
    public bool ListContains(CH_BattleChar targ)
    {
        return characterListRef.Contains(targ);
    }

    public void Remove(CH_BattleChar targ)
    {
        characterListRef.Remove(targ);
    }
    public void Add(CH_BattleChar targ)
    {
        characterListRef.Add(targ);
    }

    public CH_BattleChar GetChracter(int index)
    {
        return characterListRef[index];
    }
}
