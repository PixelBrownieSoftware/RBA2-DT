using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_CharacterExtraPassives : S_MenuSystem
{
    [SerializeField]
    private R_Passives availibleSkills;
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
    public R_Int extraPassivesMax;
    public TextMeshProUGUI moveDescription;
    public B_Int equipButton;
    public B_Int unequipButton;
    [SerializeField]
    private CH_Int changePage;

    private int page = 0;

    public override void StartMenu()
    {
        page = 0;
        base.StartMenu();
        UpdateButtons();
        unequipButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
    }
    public void ChangePage(int i)
    {
        if (i == 1)
        {
            if (availibleButtons.Length * (page + 1) < availibleSkills.passives.Count)
            {
                page++;
                UpdateButtons();
            }
        }
        if (i == -1)
        {
            if (page > 0)
            {
                page--;
                UpdateButtons();
            }
        }
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
        moveDescription.text = "" + availibleSkills.GetPassive(i).name;

        equipButton.gameObject.SetActive(true);
        unequipButton.gameObject.SetActive(false);
        equipButton.SetIntButton(i);
    }
    public void GetEquippedSkill(int i)
    {
        o_battleCharPartyData bcDat = currentCharacter.battleCharacter;
        s_move extraSkill = currentCharacter.battleCharacter.extraSkills[i];

        bool strReqFufil = bcDat.strength >= extraSkill.strReq;
        bool vitReqFufil = bcDat.vitality >= extraSkill.vitReq;
        bool dexReqFufil = bcDat.dexterity >= extraSkill.dxReq;
        bool agiReqFufil = bcDat.agility >= extraSkill.agiReq;
        bool lucReqFufil = bcDat.luck >= extraSkill.lucReq;
        bool intReqFufil = bcDat.intelligence >= extraSkill.intReq;

        string strengthReq = "";
        string vitalityReq = "";
        string dexterityReq = "";
        string agilityReq = "";
        string luckReq = "";
        string intelligenceReq = "";

        if(strReqFufil)
            strengthReq = "<color=green>" + extraSkill.strReq + "</color>";
        else
            strengthReq = "<color=red>" + extraSkill.strReq + "</color>";
        if (vitReqFufil)
            vitalityReq = "<color=green>" + extraSkill.vitReq + "</color>";
        else
            vitalityReq = "<color=red>" + extraSkill.vitReq + "</color>";
        if (dexReqFufil)
            dexterityReq = "<color=green>" + extraSkill.dxReq + "</color>";
        else
            dexterityReq = "<color=red>" + extraSkill.dxReq + "</color>";
        if (agiReqFufil)
            agilityReq = "<color=green>" + extraSkill.agiReq + "</color>";
        else
            agilityReq = "<color=red>" + extraSkill.agiReq + "</color>";
        if (lucReqFufil)
            luckReq = "<color=green>" + extraSkill.lucReq + "</color>";
        else
            luckReq = "<color=red>" + extraSkill.lucReq + "</color>";
        if (intReqFufil)
            intelligenceReq = "<color=green>" + extraSkill.intReq + "</color>";
        else
            intelligenceReq = "<color=red>" + extraSkill.intReq + "</color>";

        moveDescription.text = "" + extraSkill.name + "\n" +
            "Strength: "+ strengthReq + "\n" +
            "Vitality: " + vitalityReq + "\n" +
            "Luck: " + luckReq + "\n" +
            "Dexterity: " + dexterityReq + "\n" +
            "Agility: " + agilityReq + "\n"+
            "Intelligence: " + intelligenceReq + "\n";

        unequipButton.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(false);
        unequipButton.SetIntButton(i);
    }

    public void EquipSkill(int i)
    {
        if (extraPassivesMax.integer == currentCharacter.battleCharacter.extraSkills.Count)
            return;
        unequipButton.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(false);
        currentCharacter.battleCharacter.extraPassives.Add(availibleSkills.GetPassive(i));
        UpdateButtons();
    }
    public void UnequipSkill(int i)
    {
        equipButton.gameObject.SetActive(true);
        unequipButton.gameObject.SetActive(false);
        currentCharacter.battleCharacter.extraPassives.RemoveAt(i);
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        int indButton = 0;
        for (int i = 0; i < availibleButtons.Length; i++)
        {
            int index = i + (availibleButtons.Length * page);
            if (availibleSkills.passives == null)
            {
                availibleButtons[i].gameObject.SetActive(false);
            }
            else
            {
                if (availibleSkills.passives.Count > index)
                {
                    S_Passive skill = availibleSkills.passives[index];
                    if (!currentCharacter.battleCharacter.extraPassives.Contains(skill))
                    {
                        availibleButtons[indButton].gameObject.SetActive(true);
                        availibleButtons[indButton].SetIntButton(index);
                        availibleButtons[indButton].SetButonText(skill.name);
                        if (extraPassivesMax.integer == currentCharacter.battleCharacter.extraSkills.Count)
                            availibleButtons[indButton].SetButtonColour(Color.grey);
                        else if (extraPassivesMax.integer > currentCharacter.battleCharacter.extraSkills.Count)
                        {
                            if (skill.MeetsRequirements(currentCharacter.battleCharacter))
                                availibleButtons[indButton].SetButtonColour(Color.white);
                            else
                                availibleButtons[indButton].SetButtonColour(Color.red);
                        }
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
            if (currentCharacter.battleCharacter.extraPassives.Count > i)
            {
                S_Passive skill = currentCharacter.battleCharacter.extraPassives[i];
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
