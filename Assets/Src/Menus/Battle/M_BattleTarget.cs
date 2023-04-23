using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.System;

public class M_BattleTarget : S_MenuSystem
{
    public R_CharacterList battleCharacters;
    public R_Move moveRef;
    public List<o_battleCharDataN> battleChr;
    public R_Character currentCharacter;
    public R_Character selectedCharacter;
    public R_BattleCharacter selectedCharacterDat;
    public B_TargetWithUI[] buttons;
    public R_Text targetMenuTo;

    public CH_BattleCharacter selectTarget;
    public CH_Text switchMenu;
    public CH_Func performMove;
    private s_move mov;
    //"EMPTY"
    public enum SKILL_TYPE
    {
        BATTLE,
        GROUP_SELECT,
        DEPLOY_PARTY
    }
    public SKILL_TYPE skillType;

    public void SetTarget(CH_BattleChar target) {
        switch (mov.moveTargScope) {
            case s_move.SCOPE_NUMBER.ONE:
                selectedCharacter.characterRef = target;
                selectedCharacterDat.battleCharacter = target.characterData;
                break;
        }
        if (targetMenuTo.text != "CharacterStatus")
            performMove.RaiseEvent();
        switchMenu.RaiseEvent(targetMenuTo.text);
    }

    private void OnDisable()
    {
        selectTarget.OnFunctionEvent -= SetTarget;
    }

    private void OnEnable()
    {
        selectTarget.OnFunctionEvent += SetTarget;
    }

    private void Awake()
    {
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
    }
    Vector2 getCentroid()
    {

        List<Vector2> allPositions = new List<Vector2>();
        foreach (var v in battleCharacters.characterListRef)
        {
            allPositions.Add(v.position);
        }
        return s_camera.cam.GetCentroid(allPositions);
    }

    public override void StartMenu()
    {
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        base.StartMenu();
        mov = moveRef.move;

        switch (skillType)
        {
            case SKILL_TYPE.BATTLE:
                //s_camera.cam.cameraMode = s_camera.CAMERA_MODE.LERPING;
                B_TargetWithUI tg = null;
                /*
                switch (mov.moveTarg)
                {
                    default:
                        {
                            StartCoroutine(s_camera.cam.MoveCamera(getCentroid(), 0.9f));
                        }
                        break;

                    case s_move.MOVE_TARGET.SELF:
                        StartCoroutine(s_camera.cam.MoveCamera(currentCharacter.characterRef.position, 0.9f));
                        break;
                }
                */
                switch (mov.moveTargScope)
                {
                    /*
                    case s_move.SCOPE_NUMBER.ONE:

                        tg = buttons[0];

                        tg.SetTargetButton(currentCharacter.characterRef);
                        tg.SetButonText(currentCharacter.name);
                        tg.gameObject.SetActive(true);
                        break;
                        */

                    case s_move.SCOPE_NUMBER.ONE:
                        for (int i = 0; i < battleCharacters.characterListRef.Count; i++)
                        {
                            CH_BattleChar battleChar = battleCharacters.GetChracter(i);
                            tg = buttons[i];
                            //bool plContain = s_battleEngine.GetInstance().playerCharacters.Contains(bcs.GetChracter(i));
                            //bool plContain = players.characterListRef.Contains(battleChar);
                            bool isStatus = mov.moveType == s_move.MOVE_TYPE.NONE;

                            tg.SetTargetButton(battleChar);
                            tg.SetButonText(battleChar.cName);
                            tg.gameObject.SetActive(true);
                        }
                        break;

                    case s_move.SCOPE_NUMBER.RANDOM:
                        tg = buttons[0];
                        tg.SetButonText("Random");
                        tg.gameObject.SetActive(true);
                        break;

                    case s_move.SCOPE_NUMBER.ALL:
                        tg = buttons[0];
                        tg.SetButonText("All");
                        tg.gameObject.SetActive(true);
                        break;
                }
                break;

                /*
            case SKILL_TYPE.GROUP_SELECT:
                ResetButton();
                for (int i = 0; i < battleChr.Count; i++)
                {
                    GetButton<s_targetButton>(i).txt.text = battleChr[i].name;
                    GetButton<s_targetButton>(i).battleCharButton2 = battleChr[i];
                }
                break;
                */

            case SKILL_TYPE.DEPLOY_PARTY:

                break;
        }
        /*
        for (int i = 0; i < targets.characterListRef.Count; i++)
        {
            var button = buttons[i];
            button.gameObject.SetActive(true);
            button.SetTargetButton(targets.characterListRef[i]);
        }
        */
    }


    private void Update()
    {
        B_BattleTarget tg = null;
        switch (skillType)
        {
            case SKILL_TYPE.BATTLE:

                switch (mov.moveTargScope)
                {
                    case s_move.SCOPE_NUMBER.ONE:
                        for (int i = 0; i < battleCharacters.characterListRef.Count; i++)
                        {
                            CH_BattleChar battleChar = battleCharacters.GetChracter(i);
                            tg = buttons[i];
                            tg.transform.position = Camera.main.WorldToScreenPoint(battleChar.position + new Vector2(0, 30));
                        }
                        break;

                    case s_move.SCOPE_NUMBER.RANDOM:
                    case s_move.SCOPE_NUMBER.ALL:
                        tg = buttons[0];
                        tg.transform.position = Camera.main.WorldToScreenPoint(new Vector3(300, 300, 0)); // Camera.main.WorldToScreenPoint(getCentroid());
                        break;
                }
                break;
        }

    }
}
