using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_HubBattleSelect : S_MenuSystem
{
    public B_Int[] buttons;
    public CH_Int selectBattle;
    public CH_Int increment;
    public CH_Func startBattle;
    public CH_Text menuChanger;
    public R_EnemyGroupList groupList;
    public R_EnemyGroup selectedGroup;
    public B_Int forwardButton;
    public B_Int backButton;
    int page = 0;

    int pageShift { get { return page * buttons.Length; } }

    private void OnEnable()
    {
        selectBattle.OnFunctionEvent += SelectBattle;
        increment.OnFunctionEvent += IndexPage;
    }

    private void OnDisable()
    {
        selectBattle.OnFunctionEvent -= SelectBattle;
        increment.OnFunctionEvent -= IndexPage;
    }

    public override void StartMenu()
    {
        page = 0;
        backButton.gameObject.SetActive(false);
        base.StartMenu();
        ShowButtons();
    }

    public void IndexPage(int i) {
        page += i;
        if (pageShift > groupList.groupList.Count)
            forwardButton.gameObject.SetActive(false);
        else
            forwardButton.gameObject.SetActive(true);
        if (page == 0)
            backButton.gameObject.SetActive(false);
        else
            backButton.gameObject.SetActive(true);
        ShowButtons();
    }

    public void ShowButtons() {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);
        for (int i = 0; i < buttons.Length; i++) {
            int index = i + pageShift;
            if (index > groupList.groupList.Count) {
                break;
            }
                buttons[i].gameObject.SetActive(true);
            buttons[i].SetIntButton(index);
            buttons[i].SetButonText(groupList.groupList[index].name);
        }
    }

    public void SelectBattle(int selectBattle)
    {
        menuChanger.RaiseEvent("");
        selectedGroup.enemyGroup = groupList.groupList[selectBattle];
        startBattle.RaiseEvent();
    }
}
