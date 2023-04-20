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

        o_battleCharPartyData bcDat = currentCharacter.battleCharacter;
        s_move extraSkill = availibleSkills.GetMove(i);

        bool strReqFufil = bcDat.strength >= extraSkill.strReq;
        bool vitReqFufil = bcDat.vitality >= extraSkill.vitReq;
        bool dexReqFufil = bcDat.dexterity >= extraSkill.dxReq;
        bool agiReqFufil = bcDat.agility >= extraSkill.agiReq;
        bool lucReqFufil = bcDat.luck >= extraSkill.lucReq;
        bool intReqFufil = bcDat.intelligence >= extraSkill.intReq;

        bool canEquip = strReqFufil && vitReqFufil && dexReqFufil && agiReqFufil && lucReqFufil && intReqFufil;

        string strengthReq = "";
        string vitalityReq = "";
        string dexterityReq = "";
        string agilityReq = "";
        string luckReq = "";
        string intelligenceReq = "";

        if (strReqFufil)
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
            "Strength: " + strengthReq + "\n" +
            "Vitality: " + vitalityReq + "\n" +
            "Luck: " + luckReq + "\n" +
            "Dexterity: " + dexterityReq + "\n" +
            "Agility: " + agilityReq + "\n" +
            "Intelligence: " + intelligenceReq + "\n";

        if (canEquip)
        {
            equipButton.gameObject.SetActive(true);
        }
        else
        {
            equipButton.gameObject.SetActive(false);
        }
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
