using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_OverworldSkills : S_MenuSystem
{
    [SerializeField]
    private R_MoveList skills;
    [SerializeField]
    private R_BattleCharacter currentCharacter;
    [SerializeField]
    private B_Int[] skillButtons;

    [SerializeField]
    private CH_Int select;

    public TextMeshProUGUI moveDescription;

    public override void StartMenu()
    {
        base.StartMenu();
        skills.Clear();
        skills.AddMoves(currentCharacter.battleCharacter.currentMoves);
        UpdateButtons();
    }

    private void OnEnable()
    {
        select.OnFunctionEvent += GetAvailibleSkill;
    }

    private void OnDisable()
    {
        select.OnFunctionEvent -= GetAvailibleSkill;
    }
    public void GetAvailibleSkill(int i)
    {
        moveDescription.text = "" + skills.GetMove(i).name;
    }

    public void UpdateButtons()
    {
        int indButton = 0;
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (skills.moveListRef == null)
            {
                skillButtons[i].gameObject.SetActive(false);
            }
            else
            {
                if (skills.moveListRef.Count > i)
                {
                    s_move skill = skills.GetMove(i);
                    skillButtons[indButton].gameObject.SetActive(true);
                    skillButtons[indButton].SetIntButton(i);
                    skillButtons[indButton].SetButonText(skill.name);
                    indButton++;
                }
                else
                {
                    skillButtons[indButton].gameObject.SetActive(false);
                    indButton++;
                }
            }
        }
    }
}
