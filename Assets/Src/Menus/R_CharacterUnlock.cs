using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Defeated characters")]
public class R_CharacterUnlock : R_Default
{
    public Dictionary<o_battleCharDataN, int> defeatedCharacters = new Dictionary<o_battleCharDataN, int>();

    public void OnEnable()
    {
        defeatedCharacters.Clear();
    }

    public void AddItem(o_battleCharDataN character)
    {
        if (defeatedCharacters.ContainsKey(character))
        {
            defeatedCharacters[character]++;
        }
        else
        {
            defeatedCharacters.Add(character, 1);
        }
    }

    public int GetDefeatedCharacterCount(o_battleCharDataN character) {

        if (defeatedCharacters.ContainsKey(character))
        {
            return defeatedCharacters[character];
        }
        else
        {
            defeatedCharacters.Add(character, 0);
            return defeatedCharacters[character];
        }
    }
}
