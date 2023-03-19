using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoundation2.System;
using MagnumFoundation2.System.Core;

public class s_targetButton : s_button
{
    public CH_BattleChar battleCharButton;
    public o_battleCharDataN battleCharButton2;
    public enum TARGET_TYPE {
        BATTLE,
        OPTIONAL_BATTLE,
        DEPLOY,
        SWITCH
    }
    public TARGET_TYPE targType;
    public Slider hpSlider;
    public Slider spSlider;
    public bool isCure = false;

    protected override void OnHover()
    {
        base.OnHover();
        switch (targType)
        {
            case TARGET_TYPE.BATTLE:
                //StopAllCoroutines();
                s_soundmanager.GetInstance().PlaySound("cursor_move");
                //StartCoroutine(s_camera.cam.MoveCamera(battleCharButton.transform.position, 1.2f));
                break;

            case TARGET_TYPE.OPTIONAL_BATTLE:
                //s_battleEngine.engineSingleton.
                break;
        }
    }

    private void Update()
    {
        if (targType == TARGET_TYPE.BATTLE)
        {
            if (battleCharButton != null)
            {
                hpSlider.gameObject.SetActive(true);
                spSlider.gameObject.SetActive(true);
                hpSlider.value = ((float)battleCharButton.health / (float)battleCharButton.maxHealth) * 100;
                spSlider.value = ((float)battleCharButton.stamina / (float)battleCharButton.maxStamina) * 100;
            }
        }
        else
        {
            hpSlider.gameObject.SetActive(false);
            spSlider.gameObject.SetActive(false);
        }
    }

    protected override void OnButtonClicked()
    {
        switch (targType) {
            /*
            case TARGET_TYPE.SWITCH:
                if (battleCharButton == s_battleEngine.engineSingleton.currentCharacter)
                {
                    s_battleEngine.engineSingleton.battleAction.user = s_battleEngine.engineSingleton.currentCharacter;
                    s_battleEngine.engineSingleton.StartCoroutine(s_battleEngine.engineSingleton.ChangePartyMember(s_battleEngine.engineSingleton.currentCharacter, null));
                    s_soundmanager.GetInstance().PlaySound("selectOption");
                    s_battleEngine.engineSingleton.EndAction();
                    s_menuhandler.GetInstance().SwitchMenu("EMPTY");
                    s_camera.cam.SetTargPos(s_battleEngine.engineSingleton.currentCharacter.transform.position, 0.6f);
                }
                else
                {
                    s_battleEngine.engineSingleton.battleAction.user = s_battleEngine.engineSingleton.currentCharacter;
                    //s_battleEngine.engineSingleton.StartCoroutine(s_battleEngine.engineSingleton.ChangePartyMember(s_battleEngine.engineSingleton.currentCharacter, battleCharButton));
                    s_soundmanager.GetInstance().PlaySound("selectOption");
                    s_battleEngine.engineSingleton.EndAction();
                    s_menuhandler.GetInstance().SwitchMenu("EMPTY");
                    s_camera.cam.SetTargPos(s_battleEngine.engineSingleton.currentCharacter.transform.position, 0.6f);
                }
                break;
                */

            /*
            case TARGET_TYPE.DEPLOY:
                s_battleEngine.engineSingleton.EndAction();
                s_menuhandler.GetInstance().SwitchMenu("EMPTY");
                s_camera.cam.SetTargPos(c.transform.position, 0.6f);
                s_soundmanager.GetInstance().PlaySound("selectOption");
                //s_battleEngine.engineSingleton.StartCoroutine(s_battleEngine.engineSingleton.AddPartymemberToBattle(battleCharButton));
                break;
                */

            case TARGET_TYPE.BATTLE:
                s_battleEngine.engineSingleton.battleAction.cureStatus = isCure;
                s_battleEngine.engineSingleton.SelectTarget(battleCharButton);
                s_menuhandler.GetInstance().SwitchMenu("EMPTY");
                s_soundmanager.GetInstance().PlaySound("selectOption");
                break;

            case TARGET_TYPE.OPTIONAL_BATTLE:
                //s_battleEngine.engineSingleton.
                break;
        }
    }
}
