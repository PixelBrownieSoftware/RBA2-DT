using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class s_skillDrawMenu : s_menucontroller
{
    public s_enemyGroup SkDr;
    public List<s_move> skills;

    public void GetSkillsFromGroup()
    {
        foreach (s_enemyGroup.s_groupMember bc in SkDr.members)
        {
            o_battleCharDataN no = bc.memberDat;
            List<s_move> movesLearn = no.moveLearn;
            foreach (s_move mov in movesLearn)
            {
                if (!skills.Contains(mov))
                {
                    skills.Add(mov);
                }
            }
        }
    }

    public override void OnOpen()
    {
        ResetButton();
        GetSkillsFromGroup();
        for (int i = 0; i < skills.Count; i++)
        {
            s_buttonSkill sk = GetButton<s_buttonSkill>(i);
            sk.moveButton = skills[i];
            sk.txt.text = skills[i].name;
        }

        base.OnOpen();
    }
}
*/