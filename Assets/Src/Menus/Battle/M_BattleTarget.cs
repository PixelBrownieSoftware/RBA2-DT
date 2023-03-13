using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.System;

public class M_BattleTarget : S_MenuSystem
{
    public R_CharacterList opponents;
    public R_CharacterList players;
    public R_CharacterList battleCharacters;
    public R_Move moveRef;
    public List<o_battleCharDataN> battleChr;
    public R_Character currentCharacter;
    public B_Int[] buttons;

    private s_move mov;

    public enum SKILL_TYPE
    {
        BATTLE,
        GROUP_SELECT,
        DEPLOY_PARTY
    }
    public SKILL_TYPE skillType;


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
                s_camera.cam.cameraMode = s_camera.CAMERA_MODE.LERPING;
                B_Int tg = null;
                switch (mov.moveTarg)
                {
                    default:
                        {
                            StartCoroutine(s_camera.cam.MoveCamera(getCentroid(), 0.9f));
                        }
                        break;

                    case s_move.MOVE_TARGET.SELF:
                        StartCoroutine(s_camera.cam.MoveCamera(
                            s_battleEngine.GetInstance().currentCharacter.transform.position, 0.9f));
                        break;
                }
                switch (mov.moveTarg)
                {
                    case s_move.MOVE_TARGET.SELF:

                        tg = buttons[0];

                        tg.SetIntButton(players.characterListRef.IndexOf(currentCharacter.characterRef));
                        tg.SetButonText(currentCharacter.name);
                        tg.gameObject.SetActive(true);
                        break;

                    case s_move.MOVE_TARGET.SINGLE:
                        for (int i = 0; i < battleCharacters.characterListRef.Count; i++)
                        {
                            CH_BattleChar battleChar = battleCharacters.GetChracter(i);
                            tg = buttons[i];
                            //bool plContain = s_battleEngine.GetInstance().playerCharacters.Contains(bcs.GetChracter(i));
                            bool plContain = players.characterListRef.Contains(battleChar);
                            bool isStatus = mov.moveType == s_move.MOVE_TYPE.STATUS;

                            tg.SetIntButton(players.characterListRef.IndexOf(currentCharacter.characterRef));
                            tg.SetButonText(currentCharacter.name);
                            tg.gameObject.SetActive(true);
                        }
                        break;

                    case s_move.MOVE_TARGET.RANDOM:
                        tg = buttons[0];
                        tg.SetButonText("Random");
                        tg.gameObject.SetActive(true);
                        break;

                    case s_move.MOVE_TARGET.ALL:
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
        B_Int tg = null;
        switch (skillType)
        {
            case SKILL_TYPE.BATTLE:

                switch (mov.moveTarg)
                {
                    case s_move.MOVE_TARGET.SINGLE:
                        for (int i = 0; i < battleCharacters.characterListRef.Count; i++)
                        {
                            CH_BattleChar battleChar = battleCharacters.GetChracter(i);
                            tg = buttons[i];
                            tg.transform.position = Camera.main.WorldToScreenPoint(battleChar.position + new Vector2(0, 30));
                        }
                        break;

                    case s_move.MOVE_TARGET.RANDOM:
                        tg = buttons[0];
                        tg.transform.position = Camera.main.WorldToScreenPoint(getCentroid());
                        break;

                    case s_move.MOVE_TARGET.ALL:
                        tg = buttons[0];
                        tg.transform.position = Camera.main.WorldToScreenPoint(getCentroid());
                        break;
                }
                break;
        }

    }
}
