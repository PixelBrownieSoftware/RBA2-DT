/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.System;

public class s_targetMenu : s_menucontroller
{
    public enum SKILL_TYPE
    {
        BATTLE,
        GROUP_SELECT,
        DEPLOY_PARTY
    }
    public SKILL_TYPE skillType;

    public s_move mov;

    public List<o_battleCharDataN> battleChr;
    public R_CharacterList battleCharacters;
    public R_CharacterList players;
    public R_Character currentCharacter;

    Vector2 getCentroid() {

        List<Vector2> allPositions = new List<Vector2>();
        foreach (var v in battleCharacters.characterListRef)
        {
            allPositions.Add(v.position);
        }
        return s_camera.cam.GetCentroid(allPositions);
    }

    private void Update()
    {
        s_targetButton tg = null;
        switch (skillType)
        {
            case SKILL_TYPE.BATTLE:

                switch (mov.moveTarg) {
                    case s_move.MOVE_TARGET.SINGLE:
                        for (int i = 0; i < battleCharacters.characterListRef.Count; i++)
                        {
                            CH_BattleChar battleChar = battleCharacters.GetChracter(i);
                            tg = GetButton<s_targetButton>(i);
                            tg.transform.position = Camera.main.WorldToScreenPoint(battleChar.position + new Vector2(0, 30));
                        }
                        break;

                    case s_move.MOVE_TARGET.RANDOM:
                        tg = GetButton<s_targetButton>(0);
                        tg.transform.position = Camera.main.WorldToScreenPoint(getCentroid());
                        break;

                    case s_move.MOVE_TARGET.ALL:
                        tg = GetButton<s_targetButton>(0);
                        tg.transform.position = Camera.main.WorldToScreenPoint(getCentroid());
                        break;
                }
                break;
        }

    }
    public override void OnOpen()
    {
        ResetButton();
        base.OnOpen();
        switch (skillType) {
            case SKILL_TYPE.BATTLE:
                s_camera.cam.cameraMode = s_camera.CAMERA_MODE.LERPING;
                s_targetButton tg = null;
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
                switch (mov.moveTarg)
                {
                    case s_move.MOVE_TARGET.SELF:

                        tg = GetButton<s_targetButton>(0);

                        tg.battleCharButton = currentCharacter.characterRef;
                        tg.txt.text = tg.battleCharButton.name;
                        break;

                    case s_move.MOVE_TARGET.SINGLE:
                        for (int i = 0; i < battleCharacters.characterListRef.Count; i++)
                        {
                            CH_BattleChar battleChar = battleCharacters.GetChracter(i);
                            tg = GetButton<s_targetButton>(i);
                            //bool plContain = s_battleEngine.GetInstance().playerCharacters.Contains(bcs.GetChracter(i));
                            bool plContain = players.characterListRef.Contains(battleChar);
                            bool isStatus = mov.moveType == s_move.MOVE_TYPE.STATUS;

                            if (plContain) {
                                if(isStatus)
                                    tg.isCure = false;
                                else
                                    tg.isCure = true;
                            } else {
                                tg.isCure = false;
                            }

                            tg.battleCharButton = battleChar;
                            tg.txt.text = battleChar.cName;
                            tg.gameObject.SetActive(true);
                        }
                        break;

                    case s_move.MOVE_TARGET.RANDOM:
                        tg = GetButton<s_targetButton>(0);
                        tg.txt.text = "Random";
                        tg.gameObject.SetActive(true);
                        break;

                    case s_move.MOVE_TARGET.ALL:
                        tg = GetButton<s_targetButton>(0);
                        tg.txt.text = "All";
                        tg.gameObject.SetActive(true);
                        break;
                }
                break;

            case SKILL_TYPE.GROUP_SELECT:
                ResetButton();
                for (int i = 0; i < battleChr.Count; i++) {
                    GetButton<s_targetButton>(i).txt.text = battleChr[i].name;
                    GetButton<s_targetButton>(i).battleCharButton2 = battleChr[i];
                }
                break;

            case SKILL_TYPE.DEPLOY_PARTY:

                break;
        }

    }
}
*/