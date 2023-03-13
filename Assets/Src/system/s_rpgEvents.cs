using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.System.Core;
using MagnumFoundation2.System;
using MagnumFoundation2.Objects;

public class s_rpgEvents : s_triggerhandler
{
    public R_BattleCharacterList partyMembers;

    public override IEnumerator EventPlay(ev_details current_ev)
    {
        switch ((EVENT_TYPES)current_ev.eventType)
        {
            default:
                s_menuhandler.GetInstance().SwitchMenu("EMPTY");
                yield return StartCoroutine(base.EventPlay(current_ev));
                break;


            case EVENT_TYPES.CUSTOM_FUNCTION:

                switch (current_ev.funcName)
                {
                    case "START_BATTLE":
                        pointer = -1;
                        doingEvents = false;
                        //player.rendererObj.color = Color.clear;
                        /*
                        s_camera.cam.ZoomCamera(10, 250);
                        MagnumFoundation2.System.Core.s_soundmanager.GetInstance().PlaySound("encounter");
                        yield return StartCoroutine(Fade(Color.black));
                        */
                        s_enemyGroup enGr = (s_enemyGroup)current_ev.scrObj;
                        if (enGr != null)
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene("EMPTY_SCENE");
                            StartCoroutine(s_rpgGlobals.rpgGlSingleton.SwitchToBattle(enGr));
                        }
                        break;

                    case "ADD_PARTY_MEMBER":
                        //o_battleCharDataN bcd = current_ev.scrObj as o_battleCharDataN;
                        //if (partyMembers.Get(bcd.name) == null)
                        //    s_rpgGlobals.rpgGlSingleton.AddPartyMember(bcd, 1);
                        break;

                    case "INCREASE_ES_LIMIT":
                        s_rpgGlobals.rpgGlSingleton.extraSkillAmount += current_ev.int0;
                        break;
                }

                break;
        }

    }
}
