using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Character setter list")]
public class R_CharacterSetterList : R_Default
{
    public List<o_battleCharDataN> characterSetters = new List<o_battleCharDataN>();

    public o_battleCharDataN GetCharacter(string characterName) {
        return characterSetters.Find(x => x.name == characterName);
    }
}
