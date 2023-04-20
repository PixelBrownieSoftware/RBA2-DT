using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_CharacterPassives : S_MenuSystem
{
    [SerializeField]
    private R_Passives skills;
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
        skills.AddMoves(currentCharacter.battleCharacter.passives);
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
        moveDescription.text = "" + skills.GetPassive(i).name;
    }

    public void UpdateButtons()
    {
        int indButton = 0;
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (skills.passives == null)
            {
                skillButtons[i].gameObject.SetActive(false);
            }
            else
            {
                if (skills.passives.Count > i)
                {
                    S_Passive skill = skills.GetPassive(i);
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
