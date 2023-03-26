using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoundation2.System.Core;
using MagnumFoundation2.System;
/*
[RequireComponent(typeof(Button))]
public class s_rpgbutton : s_button
{
    public s_button backButton;
    public string buttonT;
    public s_move passAction;
    public R_Move currentMove;
    public R_MoveList characterMoves;
    public R_Character currentCharacter;

    public override void OnStart()
    {
        base.OnStart();
        switch (buttonT) {
            case "deploy":
                if (s_battleEngine.engineSingleton.playerCharacters.FindAll(x => x.inBattle == true).Count > 4)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
                break;
        }
    }

    protected override void OnButtonClicked()
    {
        switch (buttonT) {
            case "fight":
                backButton.buttonType = "BattleMenu";
                s_battleEngine.engineSingleton.SelectSkillOption();
                s_battleEngine.engineSingleton.SetTargets(false);
                //s_menuhandler.GetInstance().GetMenu<s_targetMenu>
                //("TargetMenu").bcs = s_battleEngine.engineSingleton.GetTargets(false);
                if (currentCharacter.characterRef.physWeapon != null)
                {
                    currentMove.move = currentCharacter.characterRef.physWeapon;
                } else
                {
                    currentMove.move = s_battleEngine.engineSingleton.defaultAttack;
                }
                

                s_menuhandler.GetInstance().SwitchMenu("TargetMenu");
                s_soundmanager.GetInstance().PlaySound("selectOption");
                break;

            case "fight2":
                backButton.buttonType = "BattleMenu";
                s_battleEngine.engineSingleton.SelectSkillRangedOption();
                s_battleEngine.engineSingleton.SetTargets(false);
                //s_menuhandler.GetInstance().GetMenu<s_targetMenu>
                //("TargetMenu").bcs = s_battleEngine.engineSingleton.GetTargets(false);
                currentMove.move = currentCharacter.characterRef.rangedWeapon;

                s_menuhandler.GetInstance().SwitchMenu("TargetMenu");
                s_soundmanager.GetInstance().PlaySound("selectOption");
                break;

            case "skill":
                backButton.buttonType = "SkillMenu";
                s_menuhandler.GetInstance().GetMenu<s_battleMenu>("SkillMenu").menType = s_battleMenu.MENU_TYPE.BATTLE;
                characterMoves.AddMoves(currentCharacter.characterRef.GetAllMoves());
                s_menuhandler.GetInstance().SwitchMenu("SkillMenu");
                s_soundmanager.GetInstance().PlaySound("selectOption");
                break;

            case "combo":
                backButton.buttonType = "SkillMenu";
                //s_menuhandler.GetInstance().GetMenu<s_battleMenu>("SkillMenu").rpgSkills =
                List<Tuple<s_moveComb, s_move>> moves = s_rpgGlobals.rpgGlSingleton.CheckComboRequirementsCharacter3(
                         currentCharacter.characterRef, 
                         s_battleEngine.engineSingleton.playerCharacters);

                List<s_moveComb> cmb = new List<s_moveComb>();
                List<s_move> mvs = new List<s_move>();

                foreach (Tuple<s_moveComb, s_move> a in moves)
                {
                    cmb.Add(a.Item1);
                    mvs.Add(a.Item2);
                }
                s_menuhandler.GetInstance().GetMenu<s_battleMenu>("SkillMenu")
                    .menType = s_battleMenu.MENU_TYPE.BATTLE_COMBO;
                s_menuhandler.GetInstance().GetMenu<s_battleMenu>("SkillMenu").rpgSkills = new List<s_move>();

                s_menuhandler.GetInstance().GetMenu<s_battleMenu>("SkillMenu").rpgSkills = mvs;
                s_menuhandler.GetInstance().GetMenu<s_battleMenu>("SkillMenu").combinations = cmb;

                //s_battleEngine.engineSingleton.currentCharacter;
                s_menuhandler.GetInstance().SwitchMenu("SkillMenu");
                s_soundmanager.GetInstance().PlaySound("selectOption");
                break;

            case "item":
                backButton.buttonType = "SkillMenu";
                s_menuhandler.GetInstance().
                    GetMenu<s_battleMenu>("SkillMenu").menType = s_battleMenu.MENU_TYPE.ITEMS_BATTLE;
                s_menuhandler.GetInstance().SwitchMenu("SkillMenu");
                s_soundmanager.GetInstance().PlaySound("selectOption");
                break;

            case "party":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                base.OnButtonClicked();
                break;

            case "guard":
                s_battleEngine.GetInstance().SelectSkillOptionGuard();
                s_soundmanager.GetInstance().PlaySound("selectOption");
                base.OnButtonClicked();
                break;

            case "equip":
                backButton.buttonType = "SkillMenu";
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().
                    GetMenu<s_battleMenu>("SkillMenu").menType = s_battleMenu.MENU_TYPE.ITEMS_EQUIP_BATTLE;
                s_menuhandler.GetInstance().SwitchMenu("SkillMenu");
                break;

            case "pass":
                currentMove.SetMove(passAction);
                s_battleEngine.engineSingleton.battleAction.target = s_battleEngine.engineSingleton.battleAction.user;
                s_battleEngine.engineSingleton.EndAction();
                s_menuhandler.GetInstance().SwitchMenu("EMPTY");
                s_camera.cam.SetTargPos(currentCharacter.characterRef.position, 0.9f);
                s_soundmanager.GetInstance().PlaySound("selectOption");
                break;

            case "back":
                s_camera.cam.cameraMode = s_camera.CAMERA_MODE.LERPING;
                StartCoroutine(s_camera.cam.MoveCamera(currentCharacter.characterRef.position, 0.9f));
                s_soundmanager.GetInstance().PlaySound("back");
                base.OnButtonClicked();
                break;

            case "arrange":
                s_menuhandler.GetInstance().GetMenu<s_partyOverworldMenu>("OverworldPartyMenu").ptMode = s_partyOverworldMenu.PARTY_MENU_TYPE.REARANGE;
                base.OnButtonClicked();
                break;

            case "run":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().SwitchMenu("EMPTY");
                s_battleEngine.engineSingleton.StartCoroutine(s_battleEngine.engineSingleton.ConcludeBattle());
                break;

            case "status":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().GetMenu<s_partyOverworldMenu>("OverworldPartyMenu").ptMode = s_partyOverworldMenu.PARTY_MENU_TYPE.STATUS;
                base.OnButtonClicked();
                break;


            case "deploy":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().GetMenu<s_partyMenu>("PartyMenu").partyMenuType = s_partyMenu.PARTY_MENU_TYPE.DEPLOY;
                base.OnButtonClicked();
                break;


            case "switch":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().GetMenu<s_partyMenu>("PartyMenu").partyMenuType = s_partyMenu.PARTY_MENU_TYPE.SWITCH;
                base.OnButtonClicked();
                break;

            case "OVWeap":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().GetMenu<s_partyOverworldMenu>
                    ("OverworldPartyMenu").ptMode = s_partyOverworldMenu.PARTY_MENU_TYPE.WEAPONS;
                s_menuhandler.GetInstance().SwitchMenu("OverworldPartyMenu");
                break;

            case "OVExtra":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().GetMenu<s_partyOverworldMenu>("OverworldPartyMenu").ptMode = s_partyOverworldMenu.PARTY_MENU_TYPE.EXTRA_SKILL;
                s_menuhandler.GetInstance().SwitchMenu("OverworldPartyMenu");//            
                break;

            case "OVPassive":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().GetMenu<s_partyOverworldMenu>("OverworldPartyMenu").ptMode = s_partyOverworldMenu.PARTY_MENU_TYPE.PASSIVES;
                s_menuhandler.GetInstance().SwitchMenu("OverworldPartyMenu");//            
                break;

            case "OVitem":
                s_soundmanager.GetInstance().PlaySound("selectOption");
                s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill").menType = s_battleMenu.MENU_TYPE.ITEMS;
                s_menuhandler.GetInstance().SwitchMenu("OverworldExtraSkill");        
                break;
        }
    }

}
*/