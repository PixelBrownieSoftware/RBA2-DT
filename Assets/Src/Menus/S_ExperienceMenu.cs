using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class S_ExperienceMenu : S_MenuSystem
{
    public R_BattleCharacterList players;
    float[] exp;
    public TextMeshProUGUI[] expText;
    public Slider[] sliders;
    public GameObject[] expGUI;
    public CH_Text changeMenu;
    public CH_Func continueButtonFunc;
    public CH_Func overworldButtonFunc;

    public B_Function continueButton;
    public B_Function overworldButton;
    [SerializeField]
    public CH_MapTransfer mapTrans;

    private void OnEnable()
    {
        continueButtonFunc.OnFunctionEvent += SetbattleStatsAfterExp;
        overworldButtonFunc.OnFunctionEvent += SwtichToOverworld;
    }

    private void OnDisable()
    {
        continueButtonFunc.OnFunctionEvent -= SetbattleStatsAfterExp;
        overworldButtonFunc.OnFunctionEvent -= SwtichToOverworld;
    }

    public override void StartMenu()
    {
        base.StartMenu();
        overworldButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
        //continueButton.gameObject.SetActive(true);
        exp = new float[players.battleCharList.Count];
        foreach (var gui in expGUI)
        {
            gui.gameObject.SetActive(false);
        }
        int index = 0;
        foreach (var chara in players.battleCharList)
        {
            exp[index] = chara.experiencePoints;
            expGUI[index].gameObject.SetActive(true);
            exp[index] = chara.experiencePoints;
            expText[index].text = chara.name + " LV: " + chara.level + " EXP: " + (exp[index] * 100) + "%";
            sliders[index].value = chara.experiencePoints / 1;
        }
    }

    public void SwtichToOverworld()
    {
        continueButton.gameObject.SetActive(false);
        overworldButton.gameObject.SetActive(false);
        mapTrans.RaiseEvent("Overworld");
        changeMenu.RaiseEvent("Hub");
    }

    public void SetbattleStatsAfterExp()
    {
        continueButton.gameObject.SetActive(false);
        overworldButton.gameObject.SetActive(true);
        int index = 0;
        foreach (var chara in players.battleCharList)
        {
            expGUI[index].gameObject.SetActive(true);
            exp[index] = chara.experiencePoints;
            expText[index].text = chara.name + " LV: " + chara.level + " EXP: " + (exp[index] * 100) + "%";
            sliders[index].value = chara.experiencePoints / 1;
        }
    }
}
