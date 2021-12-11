using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_HPGuiManager : MonoBehaviour
{

    public s_hpBoxGUI[] HP_GUIS;
    public o_battleCharacter[] characters;

    public void ClearHPGui() {
        foreach (var a in HP_GUIS) {
            a.bc = null;
        }
    }

    public void SetPartyMember(int index, o_battleCharacter to) {
        if (HP_GUIS.Length < index)
            return;
        HP_GUIS[ index].bc = to;
        HP_GUIS[index].SetMaterialDirty();
        HP_GUIS[index].SetMaterial();
    }

    public void ChangePartyMemberGUI(ref o_battleCharacter to, ref o_battleCharacter from, List<o_battleCharacter> playerCharacters) {
        List<o_battleCharacter> bc = playerCharacters.FindAll(x => x.inBattle);
        if (to == null)
        {
            foreach (s_hpBoxGUI c in HP_GUIS)
            {
                c.SetMaterialDirty();
                c.bc = null;
                c.SetMaterial();
            }
            from.inBattle = false;
            bc = playerCharacters.FindAll(x => x.inBattle);
            int ind = 0;
            foreach (o_battleCharacter c in bc)
            {

                HP_GUIS[ind].bc = c;
                HP_GUIS[ind].SetMaterialDirty();
                HP_GUIS[ind].SetMaterial();
                ind++;
            }
        }
        else
        {
            HP_GUIS[bc.IndexOf(from)].bc = to;
            HP_GUIS[bc.IndexOf(from)].SetMaterialDirty();
        }
    }
}
