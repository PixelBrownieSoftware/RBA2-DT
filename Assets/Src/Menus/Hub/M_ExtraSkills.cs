using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_ExtraSkills : S_MenuSystem
{
    [SerializeField]
    private R_MoveList availibleSkills;
    [SerializeField]
    private R_BattleCharacter currentCharacter;
    [SerializeField]
    private B_Int[] equipButtons;
    [SerializeField]
    private B_Int[] availibleButtons;

    [SerializeField]
    private CH_Int equip;
    [SerializeField]
    private CH_Int deEquip;

    [SerializeField]
    private CH_Int equipSelect;
    [SerializeField]
    private CH_Int deEquipSelect;

    public TextMeshProUGUI moveDescription;
    public B_Int equipButton;
    public B_Int unequipButton;

    private int page = 0;

    public override void StartMenu()
    {
        page = 0;
        base.StartMenu();
        UpdateButtons();
        unequipButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        equip.OnFunctionEvent += EquipSkill;
        deEquip.OnFunctionEvent += UnequipSkill;
        equipSelect.OnFunctionEvent += GetAvailibleSkill;
        deEquipSelect.OnFunctionEvent += GetEquippedSkill;
    }

    private void OnDisable()
    {
        equip.OnFunctionEvent -= EquipSkill;
        deEquip.OnFunctionEvent -= UnequipSkill;
        equipSelect.OnFunctionEvent -= GetAvailibleSkill;
        deEquipSelect.OnFunctionEvent -= GetEquippedSkill;
    }
    public void GetAvailibleSkill(int i)
    {
        moveDescription.text = "" + availibleSkills.GetMove(i).name;
        equipButton.gameObject.SetActive(true);
        unequipButton.gameObject.SetActive(false);
        equipButton.SetIntButton(i);
    }
    public void GetEquippedSkill(int i)
    {
        moveDescription.text = "" + currentCharacter.battleCharacter.extraSkills[i].name;
        unequipButton.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(false);
        unequipButton.SetIntButton(i);
    }

    public void EquipSkill(int i)
    {
        unequipButton.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(false);
        currentCharacter.battleCharacter.extraSkills.Add(availibleSkills.GetMove(i));
        UpdateButtons();
    }
    public void UnequipSkill(int i)
    {
        equipButton.gameObject.SetActive(true);
        unequipButton.gameObject.SetActive(false);
        currentCharacter.battleCharacter.extraSkills.RemoveAt(i);
        UpdateButtons();
    }

    public void UpdateButtons() {
        int indButton = 0;
        for (int i = 0; i < availibleButtons.Length; i++) {
            if (availibleSkills.moveListRef == null)
            {
                availibleButtons[i].gameObject.SetActive(false);
            }
            else
            {
                if (availibleSkills.moveListRef.Count > i)
                {
                    s_move skill = availibleSkills.moveListRef[i];
                    if (!currentCharacter.battleCharacter.extraSkills.Contains(skill))
                    {
                        availibleButtons[indButton].gameObject.SetActive(true);
                        availibleButtons[indButton].SetIntButton(i);
                        availibleButtons[indButton].SetButonText(skill.name);
                        indButton++;
                    }
                    else
                        continue;
                }
                else
                {
                    availibleButtons[indButton].gameObject.SetActive(false);
                    indButton++;
                }
            }
        }
        indButton = 0;
        for (int i = 0; i < equipButtons.Length; i++)
        {
            if (currentCharacter.battleCharacter.extraSkills.Count > i)
            {
                s_move skill = currentCharacter.battleCharacter.extraSkills[i];
                equipButtons[indButton].gameObject.SetActive(true);
                equipButtons[indButton].SetIntButton(i);
                equipButtons[indButton].SetButonText(skill.name);
                indButton++;
            }
            else
            {
                equipButtons[indButton].gameObject.SetActive(false);
                indButton++;
            }
        }
    }
}
