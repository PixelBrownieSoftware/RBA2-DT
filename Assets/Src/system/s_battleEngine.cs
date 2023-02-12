using MagnumFoundation2.System.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using MagnumFoundation2.System;
using MagnumFoundation2.Objects;

public class s_skillDraw {
    public s_move move;
    public o_battleCharDataN user;
    public bool done = false;
}
public enum DAMAGE_FLAGS {
    NONE,
    FRAIL,
    MISS,
    VOID,
    REFLECT,
    ABSORB,
    PASS = -1
}

public class s_battleEngine : s_singleton<s_battleEngine>
{
    public bool isEnabled = true;
    [System.Serializable]
    public class s_battleAction
    {
        public enum MOVE_TYPE
        {
            MOVE,
            ITEM,
            GUARD,
            PASS
        };
        public MOVE_TYPE type;
        public o_battleCharacter user;
        public o_battleCharacter target;
        public s_move move;
        public bool isCombo = false;
        public bool cureStatus = false;     //This is so that the attack can cure a character and do no damage
        public s_moveComb combo;
    }
    public enum BATTLE_MENU_CHOICES {
        MENU,
        SKILLS,
        ITEMS,
        GUARD,
        RUN,
        COMBO,
        NONE
    }

    public enum BATTLE_ENGINE_STATE {
        NONE,
        SELECT_CHARACTER,   //Checks which characters are there and sets the current character
        DECISION,   //Menus/AI
        TARGET,     //Targeting/AI
        PROCESS_ACTION,     //Attack animations and damage
        END     //Do press turn stuff and switch to opposing side if net turns are zero
    }
    public BATTLE_ENGINE_STATE battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
    public BATTLE_MENU_CHOICES battleDecisionMenu = BATTLE_MENU_CHOICES.MENU;
    public bool isPlayerTurn;
    int battleActionNum;

    public static s_battleEngine engineSingleton;

    #region variables
    public o_battleCharacter[] enemySlots;
    public o_battleCharacter[] playerSlots;

    public Queue<o_battleCharacter> currentPartyCharactersQueue = new Queue<o_battleCharacter>();
    public List<o_battleCharacter> playerCharacters;
    public List<o_battleCharacter> oppositionCharacters;

    public o_battleCharacter currentCharacter;
    public s_battleAction battleAction;
    
    public int roundNum;

    public int fullTurn;
    public int halfTurn;
    public int netTurn
    {
        get
        {
            return fullTurn + halfTurn;
        }
    }
    public DAMAGE_FLAGS damageFlag = DAMAGE_FLAGS.NONE;
    //When the attack ends, this is what determines how the turn icons will go
    public DAMAGE_FLAGS finalDamageFlag = DAMAGE_FLAGS.NONE;

    public int menuchoice;

    public List<o_battleCharDataN> enemyTeam;
    public s_enemyGroup enemyGroup;
    public s_battleEvent[] OnBattleEvents;
    bool[] battleEvDone;
    public bool isCutscene = false;

    public s_move defaultAttack;
    public s_move guard;
    public s_move passMove;
    public s_move nothingMove;
    public bool nonChangablePlayers = false;
    #endregion

    #region graphics
    public Sprite fire_picture;
    public Sprite water_picture;
    public Sprite ice_picture;
    public Sprite electric_picture;
    public Sprite wind_picture;
    public Sprite earth_picture;
    public Sprite dark_picture;
    public Sprite light_picture;
    public Sprite poison_picture;
    public Sprite psychic_picture;
    public Sprite strike_picture;
    public Sprite force_picture;
    public Sprite perice_picture;
    public Sprite hpGUI;

    public s_HPGuiManager HPGUIMan;
    public Image[] PT_GUI;
    public s_hitObj[] damageObjects;

    public Image comboGUI;
    public Text comboGUIText;

    public Image fightButton;
    public Image skillButton;
    public Image comboButton;
    public Image itemButton;
    public Image partyButton;
    public Image passButton;
    public Image fleeButton;
    public Image guardButton;

    public Image buttonSelector;

    public GameObject[] fightMenuButtonsImg;
    skillMenuButton[] fightMenuButtons;
    public GameObject fightMenu;

    public GameObject[] skillButtonsImg;
    [System.Serializable]
    public class skillMenuButton {
        public Image img;
        public Text buttonText;
    }
    skillMenuButton[] skillButtons;
    public GameObject skillMenu;

    public skillMenuButton displayMoveName;

    Image targ ;
    public Sprite targetWeak;
    public Sprite targetNormal;
    public Sprite targetNoDMG;
    //Text targText;

    public SpriteRenderer bg1;
    public SpriteRenderer bg2;
    public SpriteRenderer mainBG;
    #endregion

    public void Awake()
    {
        if (engineSingleton == null)
            engineSingleton = this;
        {
            /*
            int i = 0;
            HP_GUI_TEXT = new Text[HP_GUIS.Length];
            foreach (Image gui in HP_GUIS)
            {
                gui.enabled = false;
                HP_GUI_TEXT[i] = gui.transform.GetChild(0).GetComponent<Text>();
                HP_GUI_TEXT[i].text = "";
                i++;
            }
            */
        }
        {
            int i = 0;
            fightMenuButtons = new skillMenuButton[fightMenuButtonsImg.Length];
            foreach (GameObject fightBtn in fightMenuButtonsImg)
            {
                fightMenuButtons[i] = new skillMenuButton();
                fightMenuButtons[i].img = fightMenuButtonsImg[i].GetComponent<Image>();
                //fightMenuButtons[i].buttonText = fightBtn.transform.GetChild(1).GetComponent<Text>();
                i++;
            }
        }
        {
            /*
            int i = 0;
            skillButtons = new skillMenuButton[skillButtonsImg.Length];
            foreach (GameObject Btn in skillButtonsImg)
            {
                skillButtons[i] = new skillMenuButton();
                skillButtons[i].img = Btn.transform.GetChild(0).GetComponent<Image>();
                skillButtons[i].buttonText = Btn.transform.GetChild(1).GetComponent<Text>();
                i++;
            }
            */
            //skillMenu.SetActive(false);
        }

        /*
        targWeakness = targetObj.transform.GetChild(1).GetComponent<Image>();
        targText = targetObj.transform.GetChild(0).GetComponent<Text>();
        targetObj.SetActive(false);
        */
        //foreach (o_battleCharPartyData cDat in s_rpgGlobals.glSingleton.partyMembers) { }
        
        foreach (o_battleCharacter ob in playerSlots)
        {
            ob.render.sprite = null;
        }
    }

    public IEnumerator StartBattle() {
        fullTurn = 0;
        halfTurn = 0;
        s_menuhandler.GetInstance().SwitchMenu("EMPTY");

        //ffff
        #region CLEAR GUI
        HPGUIMan.ClearHPGui();
        #endregion

        #region GROUP STUFF 
        {
            bg1.material = enemyGroup.material1;
            bg2.material = enemyGroup.material2;
            bg1.sprite = enemyGroup.bg1;
            bg2.sprite = enemyGroup.bg2;
            mainBG.sprite = s_rpgGlobals.GetInstance().GetComponent<s_rpgGlobals>().BGBattle;

            nonChangablePlayers = enemyGroup.fixedPlayers;

            oppositionCharacters = new List<o_battleCharacter>();
            for (int i = 0; i < enemyGroup.members.Length; i++)
            {
                o_battleCharacter c = enemySlots[i];
                s_enemyGroup.s_groupMember mem = enemyGroup.members[i];
                o_battleCharDataN bc = mem.memberDat;
                if (bc.defaultPhysWeapon != null)
                    c.physWeapon = bc.defaultPhysWeapon;
                if (bc.defaultRangedWeapon != null)
                    c.rangedWeapon = bc.defaultRangedWeapon;
                c.animHandler.runtimeAnimatorController = bc.anim;
                c.animHandler.Play("idle");
                //c.animHandler.
                SetStatsOpponent(ref c, mem);
                c.render.color = Color.white;
                //c.PlayAnimation("idle");
            }
            playerCharacters = new List<o_battleCharacter>();
            {
                int charIndex = 0;
                if (nonChangablePlayers)
                {
                    //HPGUIMan.set
                    for (int i = 0; i < enemyGroup.members_Player.Length; i++)
                    {
                        o_battleCharacter c = playerSlots[i];
                        s_enemyGroup.s_groupMember mem = enemyGroup.members_Player[i];
                        o_battleCharDataN bc = mem.memberDat;
                        if (bc.defaultPhysWeapon != null)
                            c.physWeapon = bc.defaultPhysWeapon;
                        if (bc.defaultRangedWeapon != null)
                            c.rangedWeapon = bc.defaultRangedWeapon;
                        c.animHandler.runtimeAnimatorController = bc.anim;
                        c.animHandler.Play("idle");
                        //c.animHandler.
                        SetStatsPlayer(ref c, mem);
                        c.render.color = Color.white;
                        //c.animHandler.runtimeAnimatorController.animationClips[0].wrapMode = WrapMode.Loop;
                        //c.PlayAnimation("idle");
                        HPGUIMan.SetPartyMember(charIndex, c);
                        charIndex++;
                        /*
                        if (c != null && HP_GUIS.Length > charIndex)
                        {
                            if (c.inBattle)
                            {
                                HP_GUIS[charIndex].bc = c;
                                charIndex++;
                            }
                        }

                        */
                    }
                }
                else
                {
                    for (int i = 0; i < s_rpgGlobals.rpgGlSingleton.partyMembers.Count; i++)
                    {
                        o_battleCharacter c = playerSlots[i];
                        c.render.color = Color.clear;
                        o_battleCharPartyData pbc = s_rpgGlobals.rpgGlSingleton.partyMembers[i];
                        o_battleCharDataN bc = pbc.characterDataSource;
                        if (pbc.currentPhysWeapon != null)
                            c.physWeapon = pbc.currentPhysWeapon;
                        if (pbc.currentRangeWeapon != null)
                            c.rangedWeapon = pbc.currentRangeWeapon;
                        c.animHandler.runtimeAnimatorController = bc.anim;
                        c.animHandler.Play("idle");
                        c.inBattle = pbc.inBattle;
                        //c.animHandler.runtimeAnimatorController.animationClips[0].wrapMode = WrapMode.Loop;
                        SetStatsPlayer(ref c, pbc);
                        //c.PlayAnimation("idle");
                        if (c != null)
                        {
                            if (c.inBattle)
                            {
                                HPGUIMan.SetPartyMember(charIndex,c);
                                charIndex++;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region CLEAR ALL STATUS EFFECTS
        {
            for (int i = 0; i < AllCharacters.Count; i++)
            {
                AllCharacters[i].statusEffects.Clear();
            }
        }
        #endregion

        #region SetCam
        {
            List<Vector2> positions = new List<Vector2>();
            foreach (o_battleCharacter bc in AllCharacters) {
                positions.Add(bc.transform.position);
            }
            s_camera.cam.TeleportCamera(s_camera.cam.GetCentroid(positions));
        }
        #endregion

        {
            List<o_battleCharacter> bcs = new List<o_battleCharacter>();
            if (isPlayerTurn)
            {
                bcs = playerCharacters;
            }
            else
            {
                bcs = oppositionCharacters;
            }
            currentPartyCharactersQueue.Clear();
            currentPartyCharactersQueue = new Queue<o_battleCharacter>();
            int i = 0;
            foreach (o_battleCharacter c in bcs)
            {
                o_battleCharDataN bc = c.battleCharData;

                if (c.health > 0 && c.inBattle)
                {
                    for(int i2 =0; i2 < bc.turnIcons; i2++)
                        fullTurn++;
                    StartCoroutine(TurnIconFX(TURN_ICON_FX.APPEAR, i));
                    i++;
                    StartCoroutine(PlayFadeCharacter(c, new Color(1, 1, 1, 0), Color.white));
                    yield return new WaitForSeconds(0.15f);
                    currentPartyCharactersQueue.Enqueue(c);
                }
            }
        }
        StartCoroutine(s_triggerhandler.GetInstance().Fade(Color.clear));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(s_camera.GetInstance().ZoomCamera(-1, 1.5f));
        yield return new WaitForSeconds(1.5f);
        battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
    }

    public List<o_battleCharacter> AllCharacters
    {
        get
        {
            List<o_battleCharacter> allchr = new List<o_battleCharacter>();
            allchr.AddRange(playerCharacters);
            allchr.AddRange(oppositionCharacters);
            return allchr;
        }
    }

    public void SetStatsOpponent(ref o_battleCharacter charObj, s_enemyGroup.s_groupMember mem)
    {
        int tempLvl = 1;

        if (mem.levType == s_enemyGroup.s_groupMember.LEVEL_TYPE.FIXED)
            tempLvl = mem.level;
        else
            tempLvl = UnityEngine.Random.Range(mem.level, mem.maxLevel + 1);
        o_battleCharDataN enem = mem.memberDat;

        if (mem.memberDat.defaultPhysWeapon != null)
            charObj.physWeapon = mem.memberDat.defaultPhysWeapon;
        if (mem.memberDat.defaultRangedWeapon != null)
            charObj.rangedWeapon = mem.memberDat.defaultRangedWeapon;

        int tempHP = enem.maxHitPointsB;
        int tempSP = enem.maxSkillPointsB;

        charObj.name = enem.name;
        charObj.level = enem.level;
        charObj.battleCharData = enem;

        {
            int tempHPMin = enem.maxHitPointsB;
            int tempSPMin = enem.maxSkillPointsB;
            int tempHPMax = enem.maxHitPointsB;
            int tempSPMax = enem.maxSkillPointsB;

            int tempStr = enem.strengthB;
            int tempVit = enem.vitalityB;
            int tempDx = enem.dexterityB;
            int tempLuc = enem.luckB;
            int tempAgi = enem.agilityB;
            int tempInt = enem.intelligenceB;

            for (int i = 0; i < mem.level; i++)
            {
                tempHP += UnityEngine.Random.Range(tempHPMin, tempHPMax);
                tempSP += UnityEngine.Random.Range(tempSPMin, tempSPMax);

                if (i % enem.strengthGT == 0)
                    tempStr++;
                if (i % enem.vitalityGT == 0)
                    tempVit++;
                if (i % enem.dexterityGT == 0)
                    tempDx++;
                if (i % enem.luckGT == 0)
                    tempLuc++;
                if (i % enem.agilityGT == 0)
                    tempAgi++;
                if (i % enem.intelligenceGT == 0)
                    tempInt++;
            }

            charObj.strength = tempStr;
            charObj.vitality = tempVit;
            charObj.dexterity = tempDx;
            charObj.intelligence = tempAgi;
            charObj.luck = tempLuc;
            charObj.agility = tempAgi;

            charObj.health = charObj.maxHealth = tempHP;
            charObj.stamina = charObj.maxStamina = tempSP;
        }
        charObj.currentMoves = new List<s_move>();
        charObj.extraSkills = new List<s_move>();
        foreach (s_move mv in enem.moveLearn) {
            if (mv.MeetsRequirements(charObj)) {
                charObj.currentMoves.Add(mv);
            }
        }
        if (mem.extraSkills != null)
        {
            foreach (s_move mv in mem.extraSkills)
            {
                if (mv.MeetsRequirements(charObj))
                {
                    charObj.extraSkills.Add(mv);
                }
            }
        }
        //charObj.speed = tempAg;
        //charObj.actionTypeCharts = enem.actionTypeCharts;
        charObj.elementals = enem.elementals;
        //charObj.elementalAffinities = enem.elementAffinities;
        /*
        charObj.character_AI = new o_battleCharDataN.ai_page[enem.aiPages.Length];
        for (int i = 0; i < charObj.character_AI.Length; i++)
        {
            charObj.character_AI[i] = new o_battleCharDataN.ai_page();
            List<charAI> ai = new List<charAI>();
            if (enem.aiPages[i].ai != null)
            {
                for (int i2 = 0; i2 < enem.aiPages[i].ai.Length; i2++)
                {
                    ai.Add(enem.aiPages[i].ai[i2]);
                }
            }
            for (int i2 = 0; i2 < charObj.extraSkills.Count; i2++)
            {
                s_move m = charObj.extraSkills[i2];
                charAI chAI = new charAI();
                chAI.AIaction = charAI.ACTION.MOVE;
                chAI.move = m;
                switch (m.moveType) {
                    case s_move.MOVE_TYPE.PHYSICAL:
                    case s_move.MOVE_TYPE.SPECIAL:
                        chAI.onParty = false;
                        break;

                    case s_move.MOVE_TYPE.STATUS:
                        switch (m.statusType) {
                            default:
                                chAI.isImportant = false;
                                chAI.onParty = true;
                                break;
                            case s_move.STATUS_TYPE.HEAL_HP_BUFF:
                            case s_move.STATUS_TYPE.HEAL_HEALTH:
                                chAI.conditions = charAI.CONDITIONS.USER_PARTY_HP_LOWER;
                                chAI.healthPercentage = 0.35f;
                                chAI.isImportant = true;
                                break;
                            case s_move.STATUS_TYPE.HEAL_SP_BUFF:
                            case s_move.STATUS_TYPE.HEAL_STAMINA:
                                chAI.conditions = charAI.CONDITIONS.USER_PARTY_SP_LOWER;
                                chAI.healthPercentage = 0.35f;
                                chAI.onParty = true;
                                chAI.isImportant = true;
                                break;
                            case s_move.STATUS_TYPE.BUFF:
                                chAI.onParty = true;
                                chAI.isImportant = false;
                                break;
                            case s_move.STATUS_TYPE.DEBUFF:
                                chAI.onParty = false;
                                chAI.isImportant = false;
                                break;
                        }
                        break;
                }
                ai.Add(chAI);
            }
            charObj.character_AI[i].ai = new charAI[ai.Count];
            charObj.character_AI[i].ai = ai.ToArray();
        }
        */
        charObj.inBattle = true;
        oppositionCharacters.Add(charObj);
    }
    
    public void SetStatsPlayer(ref o_battleCharacter charObj, o_battleCharPartyData enem)
    {
        int tempHP = enem.maxHealth;
        int tempSP = enem.maxStamina;
        int tempStr = enem.strength;
        int tempVit = enem.vitality;
        int tempDx = enem.dexterity;
        int tempAgi = enem.agility;
        
        charObj.name = enem.name;
        charObj.health = charObj.maxHealth = tempHP;
        charObj.stamina = charObj.maxStamina = tempSP;
        charObj.vitality = tempVit;
        charObj.dexterity = tempDx;
        charObj.strength = tempStr;
        charObj.agility = tempAgi;
        charObj.battleCharData = enem.characterDataSource;
        charObj.currentMoves = new List<s_move>();
        charObj.currentMoves = enem.currentMoves;
        charObj.extraSkills = enem.extraSkills;
        playerCharacters.Add(charObj);
    }

    public void SetStatsPlayer(ref o_battleCharacter charObj, s_enemyGroup.s_groupMember mem)
    {
        int tempLvl = 1;

        if (mem.levType == s_enemyGroup.s_groupMember.LEVEL_TYPE.FIXED)
            tempLvl = mem.level;
        else
            tempLvl = UnityEngine.Random.Range(mem.level, mem.maxLevel + 1);
        o_battleCharDataN enem = mem.memberDat;

        if (mem.memberDat.defaultPhysWeapon != null)
            charObj.physWeapon = mem.memberDat.defaultPhysWeapon;
        if (mem.memberDat.defaultRangedWeapon != null)
            charObj.rangedWeapon = mem.memberDat.defaultRangedWeapon;

        int tempHP = enem.maxHitPointsB;
        int tempSP = enem.maxSkillPointsB;
        
        charObj.name = enem.name;
        charObj.level = enem.level;
        charObj.health = charObj.maxHealth = tempHP;
        charObj.stamina = charObj.maxStamina = tempSP;
        charObj.battleCharData = enem;
        charObj.currentMoves = new List<s_move>();
        charObj.extraSkills = new List<s_move>();
        foreach (s_move mv in enem.moveLearn)
        {
            if (mv.MeetsRequirements(charObj))
            {
                charObj.currentMoves.Add(mv);
            }
        }
        if (mem.extraSkills != null)
        {
            foreach (s_move mv in mem.extraSkills)
            {
                if (mv.MeetsRequirements(charObj))
                {
                    charObj.extraSkills.Add(mv);
                }
            }
        }
        //charObj.speed = tempAg;
        //charObj.actionTypeCharts = enem.actionTypeCharts;
        //charObj.elementals = enem.elementAffinities;
        charObj.character_AI = new o_battleCharDataN.ai_page[enem.aiPages.Length];
        for (int i = 0; i < charObj.character_AI.Length; i++)
        {
            charObj.character_AI[i] = new o_battleCharDataN.ai_page();
            List<charAI> ai = new List<charAI>();
            if (enem.aiPages[i].ai != null)
            {
                for (int i2 = 0; i2 < enem.aiPages[i].ai.Length; i2++)
                {
                    ai.Add(enem.aiPages[i].ai[i2]);
                }
            }
            for (int i2 = 0; i2 < charObj.extraSkills.Count; i2++)
            {
                s_move m = charObj.extraSkills[i2];
                charAI chAI = new charAI();
                chAI.AIaction = charAI.ACTION.MOVE;
                chAI.move = m;
                switch (m.moveType)
                {
                    case s_move.MOVE_TYPE.PHYSICAL:
                    case s_move.MOVE_TYPE.SPECIAL:
                        chAI.onParty = false;
                        break;

                    case s_move.MOVE_TYPE.STATUS:
                        switch (m.statusType)
                        {
                            default:
                                chAI.isImportant = false;
                                chAI.onParty = true;
                                break;
                            case s_move.STATUS_TYPE.HEAL_HP_BUFF:
                            case s_move.STATUS_TYPE.HEAL_HEALTH:
                                chAI.conditions = charAI.CONDITIONS.USER_PARTY_HP_LOWER;
                                chAI.healthPercentage = 0.35f;
                                chAI.isImportant = true;
                                break;
                            case s_move.STATUS_TYPE.HEAL_SP_BUFF:
                            case s_move.STATUS_TYPE.HEAL_STAMINA:
                                chAI.conditions = charAI.CONDITIONS.USER_PARTY_SP_LOWER;
                                chAI.healthPercentage = 0.35f;
                                chAI.onParty = true;
                                chAI.isImportant = true;
                                break;
                            case s_move.STATUS_TYPE.BUFF:
                                chAI.onParty = true;
                                chAI.isImportant = false;
                                break;
                            case s_move.STATUS_TYPE.DEBUFF:
                                chAI.onParty = false;
                                chAI.isImportant = false;
                                break;
                        }
                        break;
                }
                ai.Add(chAI);
            }
            charObj.character_AI[i].ai = new charAI[ai.Count];
            charObj.character_AI[i].ai = ai.ToArray();
            charObj.inBattle = true;
        }
        playerCharacters.Add(charObj);
    }

    #region Aimations
    public void FindProjectile() {

    }
    public IEnumerator AddPartymemberToBattle(o_battleCharacter to)
    {
        List<o_battleCharacter> bc = playerCharacters.FindAll(x => x.inBattle);

        // DAMAGE_FLAGS.PASS
        to.inBattle = true;
        HPGUIMan.SetPartyMember(bc.Count, to);
        to.gameObject.SetActive(true);
        RearangeTurnOrder();
        yield return new WaitForSeconds(0.1f);

    }

    public IEnumerator ChangePartyMember(o_battleCharacter from, o_battleCharacter to) {
        List<o_battleCharacter> bc = playerCharacters.FindAll(x => x.inBattle);

        Vector2 pos = from.transform.position;

        if (bc.Count < 5)
        {
            // HP_GUIS[bc.IndexOf(from)].bc = to;
        }
        HPGUIMan.ChangePartyMemberGUI(ref to, ref from, playerCharacters);
        if (to != null)
        {
            from.gameObject.SetActive(false);
            from.inBattle = false;
            to.transform.position = pos;
            to.inBattle = true;
            to.gameObject.SetActive(true);
        }
        RearangeTurnOrder();
        yield return new WaitForSeconds(0.1f);
        
    }

    public IEnumerator PlayFadeCharacter(o_battleCharacter t, Color from, Color to) {
        t.render.color = from;
        float dt = 0;
        while (t.render.color != to) {
            dt += Time.deltaTime * 1.4f;
            t.render.color = Color.Lerp(from, to, dt);
            yield return new WaitForSeconds(Time.deltaTime);
        };
    }

    public IEnumerator PlayProjectileAnimation(string objName ,Vector2 start, Vector2 target)
    {
        s_moveanim projectile = s_objpooler.GetInstance().SpawnObject<s_moveanim>("Projectile", start);
        Rigidbody2D projRB2d = projectile.GetComponent<Rigidbody2D>();

        projectile.anim.Play(objName);

        Vector2 dir = (target - start).normalized;

        while (Vector2.Distance(target, projectile.transform.position) > 20)
        {
            projRB2d.velocity = (dir * 215);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        projectile.DespawnObject();
    }

    public IEnumerator DamageAnimation(int dmg, o_battleCharacter targ) {
        
        if (battleAction.move.onParty)
        {
            targ.health += dmg;
            targ.health = Mathf.Clamp(targ.health, -targ.maxHealth, targ.maxHealth);

        }
        else
        {
            targ.health -= dmg;
            targ.health = Mathf.Clamp(targ.health, -targ.maxHealth, targ.maxHealth);

            Vector2 characterPos = targ.transform.position;
            if (oppositionCharacters.Contains(targ))
            {
                SpawnDamageObject(dmg, characterPos, true, Color.white);
            }
            else
            {
                SpawnDamageObject(dmg, characterPos, false, targ.battleCharData.characterColour);
            }
            for (int i = 0; i < 2; i++)
            {
                targ.transform.position = characterPos + new Vector2(15, 0);
                yield return new WaitForSeconds(0.02f);
                targ.transform.position = characterPos;
                yield return new WaitForSeconds(0.02f);
                targ.transform.position = characterPos + new Vector2(-15, 0);
                yield return new WaitForSeconds(0.02f);
                targ.transform.position = characterPos;
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

    public IEnumerator PlayAttackAnimation(s_actionAnim[] animations, o_battleCharacter targ, o_battleCharacter user) {

        Vector3 dir = new Vector2(0, 0);
        Vector3 originalPos = new Vector2(0, 0);
        Vector3 targPos = new Vector2(0, 0);
        if (targ != null)
            targPos = targ.transform.position;
        if (user != null)
            originalPos = user.transform.position;

        if (animations.Length > 0)
        {
            foreach (s_actionAnim an in animations)
            {
                float timer = 0;
                switch (an.actionType)
                {
                    case s_actionAnim.ACTION_TYPE.ANIMATION:
                        {
                            Vector2 start = new Vector2(0, 0);
                            switch (an.start)
                            {
                                case s_actionAnim.MOTION.SELF:
                                    start = battleAction.user.transform.position;
                                    break;
                                //If the move was random this sets the target
                                case s_actionAnim.MOTION.TO_TARGET:
                                    start = targ.transform.position;
                                    break;

                                case s_actionAnim.MOTION.USER_2:
                                    start = battleAction.combo.user2.transform.position;
                                    break;
                            }
                            s_moveanim projectile = s_objpooler.GetInstance().SpawnObject<s_moveanim>("Projectile", start);
                            projectile.anim.Play(an.name);

                            while (timer < an.time)
                            {
                                timer += Time.deltaTime;
                                yield return new WaitForSeconds(Time.deltaTime);
                            }
                        }
                        break;

                    case s_actionAnim.ACTION_TYPE.WAIT:
                        yield return new WaitForSeconds(an.time);
                        break;

                    case s_actionAnim.ACTION_TYPE.FADE_TARGET:
                        switch (an.start) {
                            case s_actionAnim.MOTION.SELF:
                                StartCoroutine(PlayFadeCharacter(battleAction.user, an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.TO_TARGET:
                                StartCoroutine(PlayFadeCharacter(battleAction.target, an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.USER_2:
                                StartCoroutine(PlayFadeCharacter(battleAction.combo.user2, an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.USER_3:
                                StartCoroutine(PlayFadeCharacter(battleAction.combo.user3, an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.USER_4:
                                StartCoroutine(PlayFadeCharacter(battleAction.combo.user4, an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.USER_5:
                                StartCoroutine(PlayFadeCharacter(battleAction.combo.user5, an.startColour, an.endColour));
                                break;
                        }

                        yield return new WaitForSeconds(an.time);
                        break;

                    case s_actionAnim.ACTION_TYPE.MOVE_CAMERA:

                        switch (an.goal)
                        {
                            case s_actionAnim.MOTION.ALL_SELF:
                                {
                                    List<Vector2> allPositions = new List<Vector2>();
                                    foreach (var v in AllTargets(true))
                                    {
                                        allPositions.Add(v.transform.position);
                                    }
                                    StartCoroutine(s_camera.cam.MoveCamera(s_camera.cam.GetCentroid(allPositions), 0.9f));
                                }
                                break;
                            case s_actionAnim.MOTION.ALL_TARGET:
                                {
                                    List<Vector2> allPositions = new List<Vector2>();
                                    foreach (var v in AllTargets(false))
                                    {
                                        allPositions.Add(v.transform.position);
                                    }
                                    StartCoroutine(s_camera.cam.MoveCamera(s_camera.cam.GetCentroid(allPositions), 0.9f));
                                }
                                break;

                            case s_actionAnim.MOTION.TO_TARGET:
                                if (an.teleport)
                                    s_camera.cam.TeleportCamera(targ.transform.position);
                                else
                                    StartCoroutine(s_camera.cam.MoveCamera(targ.transform.position, 0.9f));
                                break;

                            case s_actionAnim.MOTION.SELF:
                                if (an.teleport)
                                    s_camera.cam.TeleportCamera(user.transform.position);
                                else
                                    StartCoroutine(s_camera.cam.MoveCamera(user.transform.position, 0.9f));
                                break;
                        }
                        yield return new WaitForSeconds(an.time);
                        break;

                    case s_actionAnim.ACTION_TYPE.ZOOM_CAMERA:

                        switch (an.goal)
                        {
                            case s_actionAnim.MOTION.USER_2:
                                StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom, 
                                    battleAction.combo.user2.transform.position, 0.9f));
                                break;

                            case s_actionAnim.MOTION.USER_3:
                                StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom, 
                                    battleAction.combo.user3.transform.position, 0.9f));
                                break;

                            case s_actionAnim.MOTION.USER_4:
                                StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom, 
                                    battleAction.combo.user4.transform.position, 0.9f));
                                break;
                                
                            case s_actionAnim.MOTION.USER_5:
                                StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom,
                                    battleAction.combo.user5.transform.position, 0.9f));
                                break;

                            case s_actionAnim.MOTION.ALL_SELF:
                                {
                                    List<Vector2> allPositions = new List<Vector2>();
                                    foreach (var v in AllTargets(true))
                                    {
                                        allPositions.Add(v.transform.position);
                                    }
                                    StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom, s_camera.GetInstance().GetCentroid(allPositions), 0.9f));
                                }
                                break;

                            case s_actionAnim.MOTION.ALL_TARGET:
                                {
                                    List<Vector2> allPositions = new List<Vector2>();
                                    foreach (var v in AllTargets(false))
                                    {
                                        allPositions.Add(v.transform.position);
                                    }
                                    StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom, s_camera.GetInstance().GetCentroid(allPositions), 0.9f));
                                }
                                break;

                            case s_actionAnim.MOTION.TO_TARGET:
                                if (an.teleport)
                                    s_camera.GetInstance().TeleportCamera(targ.transform.position);
                                else
                                    StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom, targ.transform.position, 0.9f));
                                break;

                            case s_actionAnim.MOTION.SELF:
                                if (an.teleport)
                                    s_camera.GetInstance().TeleportCamera(user.transform.position);
                                else
                                    StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom, user.transform.position, 0.9f));
                                break;
                        }
                        yield return new WaitForSeconds(an.time);
                        break;

                    case s_actionAnim.ACTION_TYPE.MOVE:

                        print("offk");
                        switch (an.goal)
                        {
                            case s_actionAnim.MOTION.TO_TARGET:
                                dir = (targ.transform.position - user.transform.position).normalized;
                                while (!user.CheckIfCloseToTarget(20, targ.transform.position))
                                {
                                    dir = (targ.transform.position - user.transform.position).normalized;
                                    user.rbody2d.velocity = (dir * 175.65f);
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
                                user.rbody2d.velocity = Vector2.zero;
                                break;
                            case s_actionAnim.MOTION.SELF:
                                dir = (originalPos - user.transform.position).normalized;
                                while (!user.CheckIfCloseToTarget(20, originalPos))
                                {
                                    dir = (originalPos - user.transform.position).normalized;
                                    user.rbody2d.velocity = (dir * 175.65f);
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
                                user.rbody2d.velocity = Vector2.zero;
                                currentCharacter.transform.position = originalPos;
                                break;
                        }
                        break;
                    case s_actionAnim.ACTION_TYPE.CALCULATION:
                        yield return StartCoroutine(CalculateAttk(targ));
                        break;

                    case s_actionAnim.ACTION_TYPE.PROJECTILE:
                        {
                            Vector2 start = new Vector2(0, 0);
                            Vector2 end = new Vector2(0, 0);

                            /*
                            switch (battleAction.move.moveTarg) {
                                case s_move.MOVE_TARGET.RANDOM:
                                    if (isPlayerTurn)
                                    {
                                        if (battleAction.move.onParty)

                                            end = playerCharacters[Random.Range(0, playerCharacters.Count)].transform.position;
                                        else
                                            end = oppositionCharacters[Random.Range(0, oppositionCharacters.Count)].transform.position;
                                    }
                                    break;
                                case s_move.MOVE_TARGET.SINGLE:
                                    if (isPlayerTurn)
                                    {
                                        targetToBeCalculated = battleAction.target.transform.position;
                                    }
                                    break;
                            }
                            */
                            switch (an.start)
                            {
                                case s_actionAnim.MOTION.SELF:
                                    start = battleAction.user.transform.position;
                                    break;
                                //If the move was random this sets the target
                                case s_actionAnim.MOTION.TO_TARGET:
                                    start = battleAction.target.transform.position;
                                    break;

                                case s_actionAnim.MOTION.USER_2:
                                    start = battleAction.combo.user2.transform.position;
                                    break;
                            }

                            switch (an.goal)
                            {
                                case s_actionAnim.MOTION.USER_2:
                                    end = battleAction.combo.user2.transform.position;
                                    break;

                                case s_actionAnim.MOTION.USER_3:
                                    end = battleAction.combo.user3.transform.position;
                                    break;

                                case s_actionAnim.MOTION.USER_4:
                                    end = battleAction.combo.user4.transform.position;
                                    break;

                                case s_actionAnim.MOTION.USER_5:
                                    end = battleAction.combo.user5.transform.position;
                                    break;

                                case s_actionAnim.MOTION.SELF:
                                    end = battleAction.user.transform.position;
                                    break;
                                //If the move was random this sets the target
                                case s_actionAnim.MOTION.TO_TARGET:
                                    end = battleAction.target.transform.position;
                                    break;
                            }
                            yield return StartCoroutine(PlayProjectileAnimation(an.name, start, end));
                        }
                        break;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            yield return StartCoroutine(CalculateAttk(targ));
        }
    }

    public List<o_battleCharacter> AllTargets(bool self) {
        List<o_battleCharacter> bcs = new List<o_battleCharacter>();
        if (isPlayerTurn)
        {
            if (self)
            {
                bcs = playerCharacters;
            }
            else
            {
                bcs = oppositionCharacters;
            }
        }
        else
        {
            if (self)
            {
                bcs = oppositionCharacters;
            }
            else
            {
                bcs = playerCharacters;
            }
        }
        return bcs;
    }

    public IEnumerator DisplayMoveName(string moveName)
    {
        displayMoveName.buttonText.text = moveName;
        float t = 0;
        float spd = 4.75f;
        while (displayMoveName.img.color != Color.white)
        {
            displayMoveName.img.color = Color.Lerp(Color.clear, Color.white, t);
            displayMoveName.buttonText.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime * spd;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        t = 0;
        yield return new WaitForSeconds(0.25f);
        while (displayMoveName.img.color != Color.clear)
        {
            displayMoveName.img.color = Color.Lerp(Color.white, Color.clear, t);
            displayMoveName.buttonText.color = Color.Lerp(Color.white, Color.clear, t);
            t += Time.deltaTime * spd;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public IEnumerator ExcecuteMove()
    {
        s_move mov = battleAction.move;
        o_battleCharacter user = battleAction.user;
        o_battleCharacter targ = battleAction.target;
        s_actionAnim[] preAnimations = null;
        s_actionAnim[] animations = null;
        s_actionAnim[] endAnimations = null;

        switch (battleAction.type)
        {
            case s_battleAction.MOVE_TYPE.ITEM:
                animations = battleAction.move.animations;
                break;

            case s_battleAction.MOVE_TYPE.MOVE:
                if (battleAction.isCombo)
                {
                    for(int i =0; i < 5; i++) 
                    {
                        o_battleCharacter bc = null;
                        s_move combMov = null;
                        switch (i) {
                            case 0:
                                bc = battleAction.combo.user1;
                                combMov = battleAction.combo.user1Move;
                                break;

                            case 1:
                                bc = battleAction.combo.user2;
                                combMov = battleAction.combo.user2Move;
                                break;

                            case 2:
                                bc = battleAction.combo.user3;
                                combMov = battleAction.combo.user3Move;
                                break;

                            case 3:
                                bc = battleAction.combo.user4;
                                combMov = battleAction.combo.user4Move;
                                break;

                            case 4:
                                bc = battleAction.combo.user5;
                                combMov = battleAction.combo.user5Move;
                                break;
                        }

                        if (bc == null)
                            continue;
                        switch (combMov.moveType)
                        {
                            case s_move.MOVE_TYPE.PHYSICAL:
                                bc.health -= Mathf.RoundToInt(((float)(combMov.cost / 100) * bc.maxHealth));
                                break;

                            case s_move.MOVE_TYPE.SPECIAL:
                            case s_move.MOVE_TYPE.STATUS:
                                bc.stamina -= combMov.cost;
                                break;
                        }
                    }
                } else
                {
                    switch (battleAction.move.moveType)
                    {
                        case s_move.MOVE_TYPE.PHYSICAL:
                            battleAction.user.health -=
                                Mathf.RoundToInt((float)(battleAction.move.cost / 100f) * battleAction.user.maxHealth);
                            break;

                        case s_move.MOVE_TYPE.SPECIAL:
                        case s_move.MOVE_TYPE.STATUS:
                            battleAction.user.stamina -= battleAction.move.cost;
                            break;
                    }
                }
                animations = battleAction.move.animations;
                break;
        }

        #region NOTIFICATION
        if (playerCharacters.Contains(battleAction.user))
        {
            s_soundmanager.sound.PlaySound("notif");
        }
        else
        {
            s_soundmanager.sound.PlaySound("notif_enemy");
        }

        /*
        for (int i = 0; i < 2; i++)
        {
            float t = 0;
            float spd = 13.6f;
            while (battleAction.user.rend.color != Color.black)
            {
                battleAction.user.rend.color = Color.Lerp(Color.white, Color.black, t);
                t += Time.deltaTime * spd;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            t = 0;
            while (move.user.rend.color != Color.white)
            {
                move.user.rend.color = Color.Lerp(Color.black, Color.white, t);
                t += Time.deltaTime * spd;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        */
        switch (battleAction.type)
        {
            default:
                yield return StartCoroutine(DisplayMoveName(mov.name));
                break;

            case s_battleAction.MOVE_TYPE.GUARD:
                yield return StartCoroutine(DisplayMoveName("Guard"));
                break;

            case s_battleAction.MOVE_TYPE.PASS:
                yield return StartCoroutine(DisplayMoveName("Pass"));
                break;
        }
        #endregion
        
        switch (battleAction.type) {
            case s_battleAction.MOVE_TYPE.PASS:

                //If there are no full turn icons start taking away instead of turning full icons into half
                if (halfTurn > 0)
                {
                    PassTurn();
                    yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                }
                else
                {
                    PassTurn();
                    yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.HIT, netTurn - halfTurn));
                }
                break;
        }
        
        #region PRE ANIM
        switch (battleAction.type)
        {
            case s_battleAction.MOVE_TYPE.ITEM:
            case s_battleAction.MOVE_TYPE.MOVE:
                preAnimations = battleAction.move.preAnimations;
                if (preAnimations != null)
                {
                    if (preAnimations.Length > 0)
                        yield return StartCoroutine(PlayAttackAnimation(preAnimations, null, battleAction.user));
                }

                battleAction.user.SwitchAnimation("idle");

                yield return new WaitForSeconds(0.3f);
                break;
        }
        #endregion

        #region MAIN ANIM
        switch (battleAction.type)
        {
            case s_battleAction.MOVE_TYPE.ITEM:
            case s_battleAction.MOVE_TYPE.MOVE:
                if (animations != null)
                {
                    switch (battleAction.move.moveTarg)
                    {
                        case s_move.MOVE_TARGET.SINGLE:
                            yield return StartCoroutine(PlayAttackAnimation(animations, targ, battleAction.user));
                            break;

                        case s_move.MOVE_TARGET.ALL:
                            {
                                List<o_battleCharacter> bcs = AllTargets(battleAction.move.onParty);
                                print(bcs.Count);
                                foreach (var b in bcs)
                                {
                                    yield return StartCoroutine(PlayAttackAnimation(animations, b, battleAction.user));
                                }
                            }
                            break;

                        case s_move.MOVE_TARGET.RANDOM:
                            {
                                List<o_battleCharacter> bcs = AllTargets(battleAction.move.onParty);
                                int rand = UnityEngine.Random.Range(2, 5);
                                for (int i = 0; i < rand; i++)
                                {
                                    yield return StartCoroutine(PlayAttackAnimation(animations,
                                        bcs[UnityEngine.Random.Range(0, bcs.Count)],
                                        battleAction.user));
                                }
                            }
                            break;
                    }
                }
                else {

                }

                battleAction.user.SwitchAnimation("idle");

                #region PRESS TURN STUFF
                int numOfTimes = 1;
                switch (battleAction.combo.comboType)
                {
                    case s_move.MOVE_QUANITY_TYPE.MONO_TECH:
                        numOfTimes = 1;
                        break;
                    case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:
                        numOfTimes = 2;
                        break;
                    case s_move.MOVE_QUANITY_TYPE.TRIPLE_TECH:
                        numOfTimes = 3;
                        break;
                    case s_move.MOVE_QUANITY_TYPE.QUAD_TECH:
                        numOfTimes = 4;
                        break;
                    case s_move.MOVE_QUANITY_TYPE.PENTA_TECH:
                        numOfTimes = 5;
                        break;
                }

                for (int i = 0; i < numOfTimes; i++)
                {
                    switch (finalDamageFlag)
                    {
                        case DAMAGE_FLAGS.NONE:
                            NextTurn();
                            yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                            break;
                        case DAMAGE_FLAGS.VOID:
                            NextTurn();
                            yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                            NextTurn();
                            yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                            break;
                        case DAMAGE_FLAGS.FRAIL:

                            //If there are no full turn icons start taking away instead of turning full icons into half
                            if (fullTurn > 0)
                            {
                                s_soundmanager.GetInstance().PlaySound("hitWeak");
                                HitWeakness();
                                yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.HIT, netTurn - halfTurn));
                            }
                            else
                            {
                                HitWeakness();
                                yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                            }
                            break;
                        case DAMAGE_FLAGS.PASS:

                            //If there are no full turn icons start taking away instead of turning full icons into half
                            if (fullTurn > 0)
                            {
                                PassTurn();
                                yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.HIT, netTurn - halfTurn));
                            }
                            else
                            {
                                PassTurn();
                                yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                            }
                            break;
                        case DAMAGE_FLAGS.REFLECT:
                        case DAMAGE_FLAGS.ABSORB:
                            fullTurn = 0;
                            halfTurn = 0;
                            break;
                    }
                    if (netTurn == 0)
                        break;
                }
                #endregion

                yield return new WaitForSeconds(0.3f);
                break;

            case s_battleAction.MOVE_TYPE.GUARD:
                battleAction.user.guardPoints++;
                NextTurn();
                yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                break;
        }
        #endregion

        #region END ANIM
        switch (battleAction.type)
        {
            case s_battleAction.MOVE_TYPE.ITEM:
            case s_battleAction.MOVE_TYPE.MOVE:
                endAnimations = battleAction.move.endAnimations;
                if (endAnimations != null)
                {
                    if (endAnimations.Length > 0)
                        yield return StartCoroutine(PlayAttackAnimation(endAnimations, null, battleAction.user));
                }

                battleAction.user.SwitchAnimation("idle");

                yield return new WaitForSeconds(0.3f);
                break;
        }
        #endregion

        yield return StartCoroutine(CheckStatusEffectAfterAction());
        yield return StartCoroutine(CheckCutscene());
        finalDamageFlag = DAMAGE_FLAGS.NONE;
        battleEngine = BATTLE_ENGINE_STATE.END;
    }
    public IEnumerator CheckCutscene()
    {
        s_camera.cam.SetZoom();
        yield return new WaitForSeconds(0.1f);
    }
    public IEnumerator CheckStatusEffectAfterAction()
    {
        s_camera.cam.SetZoom();
        foreach (s_statusEff eff in currentCharacter.statusEffects) {
            switch (eff.status) {
                case STATUS_EFFECT.POISON:
                    //We'll have some stat calculations as if this status effect is damage, there would be some kind of formula.
                    StartCoroutine(DamageAnimation(eff.damage, currentCharacter));
                    eff.duration--;
                    break;

                case STATUS_EFFECT.BURN:
                    //We'll have some stat calculations as if this status effect is damage, there would be some kind of formula.
                    StartCoroutine(DamageAnimation(eff.damage, currentCharacter));
                    eff.duration--;
                    break;
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
    
    public IEnumerator DoEndTurnAnimation(bool isPturn)
    {
        yield return new WaitForSeconds(0.4f);

        isPlayerTurn = isPturn;
        battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
    }

    public void SpawnDamageObject(float dmg, Vector2 characterPos, Color colour, string damageObjType)
    {
        /*
        for (int i = 0; i < damageObjects.Length; i++)
        {
            if (!damageObjects[i].isDone)
                continue;
            damageObjects[i].transform.position = characterPos;
            damageObjects[i].
            break;
        }
        */
        s_hitObj ob = s_objpooler.GetInstance().SpawnObject<s_hitObj>("HitObj", characterPos);
        ob.PlayAnim(dmg, damageObjType, colour);
    }
    public void SpawnDamageObject(int dmg, Vector2 characterPos, bool enemy, Color colour)
    {
        /*
        for (int i = 0; i < damageObjects.Length; i++)
        {
            if (!damageObjects[i].isDone)
                continue;
            damageObjects[i].transform.position = characterPos;
            damageObjects[i].PlayAnim(dmg, enemy, colour);
            break;
        }
        */
        s_hitObj ob = s_objpooler.GetInstance().SpawnObject<s_hitObj>("HitObj", characterPos);
        ob.PlayAnim(dmg, enemy, colour);
    }
    #endregion

    #region Turn Stuff
    public void HitWeakness()
    {
        if (halfTurn >= netTurn)
        {
            halfTurn--;
        }
        else
        {
            halfTurn++;
            fullTurn--;
        }
    }

    public void NextTurn()
    {
        if (halfTurn > 0)
            halfTurn--;
        else
            fullTurn--;
    }

    public void PassTurn()
    {
        if (halfTurn > 0)
        {
            halfTurn--;
        }
        else
        {
            halfTurn++;
            fullTurn--;
        }
    }
    #endregion

    #region Battle system core
    void Update()
    {
        if (isEnabled)
        {
            switch (battleEngine) {
                case BATTLE_ENGINE_STATE.SELECT_CHARACTER:
                    currentCharacter = currentPartyCharactersQueue.Peek();
                    if (currentCharacter.health <= 0 || !currentCharacter.inBattle) {
                        currentPartyCharactersQueue.Dequeue();
                        currentPartyCharactersQueue.Enqueue(currentCharacter);
                        return;
                    }
                    battleAction.user = currentCharacter;
                    if (currentCharacter.statusEffects != null)
                    {
                        foreach (s_statusEff eff in currentCharacter.statusEffects)
                        {
                            switch (eff.status)
                            {
                                case STATUS_EFFECT.CONFUSED:

                                    battleAction.user = currentCharacter;
                                    battleAction.move = currentCharacter.GetRandomMove;
                                    battleAction.target = AllCharacters[UnityEngine.Random.Range(0, AllCharacters.Count)];
                                    battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
                                    EndAction();
                                    break;

                                case STATUS_EFFECT.STUN:
                                    //TODO: make it so that the character's turn is immediatley cancelled.
                                    if (eff.duration % 2 == 1)
                                    {
                                        battleAction.user = currentCharacter;
                                        battleAction.move = nothingMove;
                                        battleAction.isCombo = false;
                                        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
                                        battleAction.target = currentCharacter;
                                        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
                                        EndAction();
                                    }
                                    break;
                            }
                            switch (eff.status)
                            {
                                case STATUS_EFFECT.CONFUSED:
                                case STATUS_EFFECT.STUN:
                                    eff.duration -= 1;
                                    break;
                            }
                            if (eff.duration == 0)
                                currentCharacter.statusEffects.Remove(eff);
                        }
                    }
                    break;
            }
            if (isPlayerTurn)
            {
                PlayerTurn();
            }
            else
            {
                OppositionTurn();
            }
        }
    }
    public void OppositionTurn()
    {
        switch (battleEngine)
        {
            case BATTLE_ENGINE_STATE.NONE:

                break;

            case BATTLE_ENGINE_STATE.SELECT_CHARACTER:
                battleEngine = BATTLE_ENGINE_STATE.DECISION;
                break;

            case BATTLE_ENGINE_STATE.DECISION:
            case BATTLE_ENGINE_STATE.TARGET:

                battleAction.target = null;
                battleAction.user = currentCharacter;
                bool fufilled = false;
                bool notAlways = false;

                List<s_move> alwaysMoves = new List<s_move>();

                foreach (charAI ai in  currentCharacter.battleCharData.aiPages
                    [currentCharacter.characterPage].ai)
                {
                    o_battleCharacter potentialTrg = null;
                    s_move move = ai.move;
                    if (!currentCharacter.extraSkills.Contains(move) && !currentCharacter.currentMoves.Contains(move)) {
                        continue;
                    }
                    List<o_battleCharacter> targets = new List<o_battleCharacter>();
                    if (ai.move.onParty) {
                        targets = oppositionCharacters;
                    } else {
                        targets = playerCharacters.FindAll(x=> x.inBattle == true && x.health > 0);
                    }

                    battleAction.move = ai.move;
                    float minimumHP = 0;
                    float maximumHP = 0;

                    switch (ai.AIaction) {
                        case charAI.ACTION.MOVE:

                            break;

                        case charAI.ACTION.SWITCH_PAGE:

                            break;
                    }

                    switch (ai.conditions) {

                        case charAI.CONDITIONS.NEVER:
                            continue;

                        case charAI.CONDITIONS.ALWAYS:
                            potentialTrg = targets[UnityEngine.Random.Range(0, targets.Count - 1)];
                            alwaysMoves.Add(move);
                            break;

                        case charAI.CONDITIONS.TARGET_PARTY_HP_HIGHER:
                            maximumHP = 0;
                            foreach (o_battleCharacter bc in targets)
                            {
                                if (maximumHP < bc.health)
                                {
                                    potentialTrg = bc;
                                }
                            }
                            break;

                        case charAI.CONDITIONS.ELEMENT_TARG_FRAIL:
                            foreach (o_battleCharacter bc in targets)
                            {
                                if (bc.elementals[ai.move.element] >= 2) {
                                    potentialTrg = bc;
                                    break;
                                }
                            }
                            break;

                        case charAI.CONDITIONS.ELEMENT_TARG_STRONGEST:
                            foreach (o_battleCharacter bc in targets)
                            {
                                int aff = (int)bc.elementals[ai.move.element];
                                if (aff > 1.999f)
                                {
                                    potentialTrg = bc;
                                    break;
                                }
                            }
                            break;
                        case charAI.CONDITIONS.USER_PARTY_HP_LOWER:
                            potentialTrg = targets.Find(x => x.health < ai.healthPercentage * x.maxHealth);
                            break;

                        case charAI.CONDITIONS.USER_PARTY_HP_HIGHER:
                            minimumHP = float.MaxValue;
                            foreach (o_battleCharacter bc in targets)
                            {
                                if (minimumHP > bc.health)
                                {
                                    potentialTrg = bc;
                                }
                            }
                            break;

                        case charAI.CONDITIONS.TARGET_PARTY_HP_LOWER:
                            minimumHP = float.MaxValue;
                            foreach (o_battleCharacter bc in targets) {
                                if (minimumHP > bc.health) {
                                    potentialTrg = bc;
                                }
                            }
                            break;
                    }
                    if (potentialTrg != null)
                    {
                        switch (ai.conditions)
                        {
                            case charAI.CONDITIONS.ALWAYS:
                                break;

                            default:
                                notAlways = true;
                                break;
                        }
                        fufilled = true;
                        battleAction.target = potentialTrg;
                    }
                }
                if (!fufilled)
                {
                    List<o_battleCharacter> targets = new List<o_battleCharacter>();
                    targets = playerCharacters.FindAll(x => x.inBattle == true && x.health > 0);
                    battleAction.target = targets[UnityEngine.Random.Range(0, targets.Count)];
                    if(currentCharacter.physWeapon != null)
                        battleAction.move = currentCharacter.physWeapon;
                    else
                        battleAction.move = defaultAttack;
                }
                else if(!notAlways)
                {
                    battleAction.move = alwaysMoves[UnityEngine.Random.Range(0, alwaysMoves.Count - 1)];
                    List<o_battleCharacter> targets = new List<o_battleCharacter>();
                    if (battleAction.move.onParty)
                        targets = oppositionCharacters;
                    else
                        targets = playerCharacters.FindAll(x => x.inBattle == true && x.health > 0);
                    battleAction.target = targets[UnityEngine.Random.Range(0, targets.Count)];
                }
                battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
                battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
                battleEngine = BATTLE_ENGINE_STATE.PROCESS_ACTION;
                print(battleAction.target);
                StartCoroutine(ExcecuteMove());
                break;

            case BATTLE_ENGINE_STATE.END:
                StartCoroutine(NextTeamTurn());
                break;
        }
    }
    public void PlayerTurn()
    {
        switch (battleEngine)
        {
            case BATTLE_ENGINE_STATE.NONE:

                break;

            case BATTLE_ENGINE_STATE.SELECT_CHARACTER:

                s_menuhandler.GetInstance().GetMenu<s_mainBattleMenu>("BattleMenu").battleCharacter = currentCharacter;
                s_menuhandler.GetInstance().SwitchMenu("BattleMenu");
                //StartCoroutine(CheckStatusEffectBeforeAction());
                s_camera.cam.TeleportCamera(currentCharacter.transform.position);
                battleDecisionMenu = BATTLE_MENU_CHOICES.MENU;
                battleEngine = BATTLE_ENGINE_STATE.DECISION;
                break;
            //CheckComboRequirementsParty(s_battleEngine.engineSingleton.playerCharacters)
            case BATTLE_ENGINE_STATE.DECISION:

                //This is where you decide your actions
                switch (battleDecisionMenu)
                {
                    case BATTLE_MENU_CHOICES.SKILLS:

                        skillMenu.SetActive(true);
                        MenuControl(currentCharacter.currentMoves.Count, MENU_CONTROLL_TYPE.MULTI_DIRECTIONAL, new Vector2(2, 5));
                        if (Input.GetKeyDown(s_globals.GetKeyPref("select")))
                        {
                            battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
                            battleAction.move = currentCharacter.currentMoves[menuchoice];
                            skillMenu.SetActive(false);
                            battleEngine = BATTLE_ENGINE_STATE.TARGET;
                            menuchoice = 0;
                        }
                        if (Input.GetKeyDown(s_globals.GetKeyPref("back")))
                        {
                            skillMenu.SetActive(false);
                            battleDecisionMenu = BATTLE_MENU_CHOICES.MENU;
                            battleEngine = BATTLE_ENGINE_STATE.DECISION;
                            menuchoice = 0;
                        }
                        break;

                    case BATTLE_MENU_CHOICES.ITEMS:

                        skillMenu.SetActive(true);
                        MenuControl(s_rpgGlobals.rpgGlSingleton.GetItems().Count, MENU_CONTROLL_TYPE.MULTI_DIRECTIONAL, new Vector2(2, 5));
                        if (Input.GetKeyDown(s_globals.GetKeyPref("select")))
                        {
                            battleAction.move = s_rpgGlobals.rpgGlSingleton.GetItems()[menuchoice];
                            battleAction.type = s_battleAction.MOVE_TYPE.ITEM;
                            battleEngine = BATTLE_ENGINE_STATE.TARGET;
                            menuchoice = 0;
                        }
                        if (Input.GetKeyDown(s_globals.GetKeyPref("back")))
                        {
                            skillMenu.SetActive(false);
                            battleDecisionMenu = BATTLE_MENU_CHOICES.MENU;
                            battleEngine = BATTLE_ENGINE_STATE.DECISION;
                            menuchoice = 0;
                        }
                        break;
                }
                break;

            case BATTLE_ENGINE_STATE.TARGET:
                /*
                
                */
                break;

            case BATTLE_ENGINE_STATE.PROCESS_ACTION:

                break;

            case BATTLE_ENGINE_STATE.END:
                StartCoroutine(NextTeamTurn());
                break;
        }
    }

    public List<o_battleCharacter> GetTargets(bool onParty) {
        List<o_battleCharacter> targets = new List<o_battleCharacter>();
        /*
        if (battleAction.type == s_battleAction.MOVE_TYPE.ITEM)
        {
            onParty = battleAction.move.onParty;
        }
        else if (battleAction.type == s_battleAction.MOVE_TYPE.MOVE)
        {
            onParty = battleAction.move.onParty;
        }
        */
        if (!onParty)
        {
            //targetObj.SetActive(true);
            MenuControl(oppositionCharacters.Count, MENU_CONTROLL_TYPE.LINEAR_UP_DOWN, new Vector2(0, 0));
            for (int i = 0; i < oppositionCharacters.Count; i++)
            {
                if (oppositionCharacters[i].health > 0)
                    targets.Add(oppositionCharacters[i]);
            }
            /*
            switch (oppositionCharacters[menuchoice].elementals[(int)battleAction.move.element].flag)
            {
                case s_affinity.DAMAGE_FLAGS.ABSORB:
                case s_affinity.DAMAGE_FLAGS.REFLECT:
                case s_affinity.DAMAGE_FLAGS.VOID:
                    targWeakness.sprite = targetNoDMG;
                    break;

                case s_affinity.DAMAGE_FLAGS.NONE:
                    targWeakness.sprite = targetNormal;
                    break;

                case s_affinity.DAMAGE_FLAGS.FRAIL:
                    targWeakness.sprite = targetWeak;
                    break;
            }
            */
            /*
            hpBar.HP.value = ((float)bc.health / (float)bc.maxHealth) * 100;
            hpBar.SP.value = ((float)bc.stamina / (float)bc.maxStamina) * 100;
            */
            //print((float)bc.health / (float)bc.maxHealth);
        }
        else {
            for (int i = 0; i < playerCharacters.Count; i++)
            {
                targets.Add(playerCharacters[i]);
            }
        }
        /*
        if (Input.GetKeyDown(s_globals.GetKeyPref("back")))
        {
            targetObj.SetActive(false);
            HP_GUIS.ToList().ForEach(x => x.HideComboImage());
            battleEngine = BATTLE_ENGINE_STATE.DECISION;
        }*/
        return targets;
    }

    public void RearangeTurnOrder() {
        List<o_battleCharacter> bcs = playerCharacters.FindAll(x => x.inBattle);
        currentPartyCharactersQueue.Clear();
        int ind = bcs.IndexOf(currentCharacter);
        int initInd = bcs.IndexOf(currentCharacter);
        while (ind != bcs.Count) {
            currentPartyCharactersQueue.Enqueue(bcs[ind]);
            ind++;
        }
        ind = 0;
        while (ind != initInd)
        {
            currentPartyCharactersQueue.Enqueue(bcs[ind]);
            ind++;
        }
    }

    public void SelectTarget(o_battleCharacter bc) {
        battleAction.target = bc;
        EndAction();
    }

    public void EndAction() {
        battleEngine = BATTLE_ENGINE_STATE.PROCESS_ACTION;
        StartCoroutine(ExcecuteMove());
    }

    public void GuardOption()
    {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        battleAction.move = defaultAttack;
        s_menuhandler.GetInstance().SwitchMenu("EMPTY");
        battleEngine = BATTLE_ENGINE_STATE.END;
        menuchoice = 0;
    }

    public void SelectSkillOptionGuard()
    {
        battleAction.type = s_battleAction.MOVE_TYPE.GUARD;
        battleAction.move = guard;
        s_menuhandler.GetInstance().SwitchMenu("EMPTY");
        EndAction();
    }

    public void SelectSkillRangedOption()
    {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        battleAction.move = currentCharacter.rangedWeapon;
        battleAction.isCombo = false;
        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        menuchoice = 0;
    }
    public void SelectSkillOption()
    {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        if(currentCharacter.physWeapon == null)
            battleAction.move = defaultAttack;
        else
            battleAction.move = currentCharacter.physWeapon;
        battleAction.isCombo = false;
        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        menuchoice = 0;
    }

    public void SelectSkillOption(s_move move, s_moveComb combo)
    {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        battleAction.isCombo = true;
        battleAction.combo = combo;
        battleAction.move = move;
        s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").bcs = GetTargets(move.onParty);
        s_menuhandler.GetInstance().SwitchMenu("TargetMenu");
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        menuchoice = 0;
    }

    public void SelectSkillOption(s_move move) {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        battleAction.move = move;
        battleAction.isCombo = false;
        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
        s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").bcs = GetTargets(move.onParty);
        if (move.moveTarg == s_move.MOVE_TARGET.SINGLE)
        {
            switch (move.element)
            {
                case ELEMENT.STRIKE:
                case ELEMENT.FIRE:
                    foreach (o_battleCharacter bc in GetTargets(true))
                    {
                        if (bc.HasStatus(STATUS_EFFECT.FROZEN))
                        {
                            s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").bcs.Add(bc);
                        }
                    }
                    break;

                case ELEMENT.WATER:
                case ELEMENT.ICE:
                    foreach (o_battleCharacter bc in GetTargets(true))
                    {
                        if (bc.HasStatus(STATUS_EFFECT.BURN))
                        {
                            s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").bcs.Add(bc);
                        }
                    }
                    break;
            }
        }
        s_menuhandler.GetInstance().SwitchMenu("TargetMenu");
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        menuchoice = 0;
    }

    public IEnumerator OnAttack() {
        if (isPlayerTurn) {

        } else {
            foreach (o_battleCharacter bc in playerCharacters) {
                if (bc.health <= 0) {
                    continue;
                }
                if (battleAction.move.moveTarg == s_move.MOVE_TARGET.SINGLE) {
                    if (battleAction.target.health > 0) {

                    }
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }

    public float GetElementStat(o_battleCharacter user, s_move move) {
        float pow = 0;
        float basePow = 0;
        float elementalPow = 0;

        int str = user.strengthNet;
        int vit = user.vitalityNet;
        int dex = user.dexterityNet;
        int agi = user.agiNet;

        switch (move.moveType) {
            case s_move.MOVE_TYPE.PHYSICAL:
                basePow = user.strengthNet;
                break;
            case s_move.MOVE_TYPE.SPECIAL:
                basePow = user.dexterityNet;
                break;
        }
        ///Put power
        pow = basePow + elementalPow;
        return pow;
    }

    public int CalculateDamage(o_battleCharacter user, o_battleCharacter target, s_move move)
    {
        ELEMENT el = ELEMENT.NONE;
        el = move.element;
        int dmg = 0;
        if (!move.fixedValue)
        {
            if (move.moveType != s_move.MOVE_TYPE.STATUS)
            {
                float multipler = 1f;
                float elementals = user.elementals[el];
                if (elementals < 0 && elementals > -1)
                    multipler = (elementals * -1);
                else if (elementals <= -1)
                    multipler = ((elementals + 1) * -1);

                dmg = (int)(move.power * (GetElementStat(user, move) / (float)target.vitalityNet) * multipler);
            }
            else
            {
                dmg = (int)(move.power * (user.dexterityNet /2));
            }
        }
        else { dmg = move.power; }
        return dmg;
    }
    
    public void DamageEffect(int dmg, o_battleCharacter target ,Vector2 characterPos, DAMAGE_FLAGS fl) {

        target.health -= dmg;
        if (target.guardPoints > 0)
            target.guardPoints--;
        target.health = Mathf.Clamp(target.health, -target.maxHealth, target.maxHealth);

        switch (fl)
        {
            case DAMAGE_FLAGS.FRAIL:
            case DAMAGE_FLAGS.NONE:
                if (oppositionCharacters.Contains(target))
                {
                    s_soundmanager.GetInstance().PlaySound("rpgHit");
                    SpawnDamageObject(dmg, characterPos, true, Color.white);
                }
                else
                {
                    s_soundmanager.GetInstance().PlaySound("pl_dmg");
                    SpawnDamageObject(dmg, characterPos, false, target.battleCharData.characterColour);
                }
                break;

            case DAMAGE_FLAGS.VOID:
                SpawnDamageObject(0, characterPos, Color.white, "block");
                break;
        }

    }

    public IEnumerator CalculateAttk(o_battleCharacter targ)
    {
        /*TODO: 
         * If this is a combo move, the target's defence is calculated against the user's stats combineed
         * i.e. if there was a ice strike combo (where Blueler does the magic and redler does the physical attack)
         * it would be combo output = Redler physical output + Blueler magic output
        */

        int dmg = 0;
        switch (battleAction.type)
        {
            case s_battleAction.MOVE_TYPE.MOVE:

                if (battleAction.isCombo)
                {
                    int leng = 0;

                    if (battleAction.combo.user2 != null)
                        leng++;
                    if (battleAction.combo.user3 != null)
                        leng++;
                    if (battleAction.combo.user4 != null)
                        leng++;
                    if (battleAction.combo.user5 != null)
                        leng++;

                    int dmg1 = CalculateDamage(battleAction.user, targ, battleAction.combo.user1Move);
                    int dmg2 = 0;
                    int dmg3 = 0;
                    int dmg4 = 0;
                    int dmg5 = 0;

                    if (battleAction.combo.user2 != null)
                        dmg2 = CalculateDamage(battleAction.combo.user2, targ, battleAction.combo.user2Move);
                    if (battleAction.combo.user3 != null)
                        dmg3 = CalculateDamage(battleAction.combo.user3, targ, battleAction.combo.user3Move);
                    if (battleAction.combo.user4 != null)
                        dmg4 = CalculateDamage(battleAction.combo.user4, targ, battleAction.combo.user4Move);
                    if (battleAction.combo.user5 != null)
                        dmg5 = CalculateDamage(battleAction.combo.user5, targ, battleAction.combo.user5Move);
                    for (int i = 0; i < 5; i++) {
                        switch (i) {
                            case 0:
                                dmg += dmg1;
                                break;
                            case 1:
                                if (battleAction.combo.user2 != null)
                                    dmg += dmg2;
                                    break;
                            case 2:
                                if (battleAction.combo.user3 != null)
                                    dmg += dmg3;//Mathf.CeilToInt(dmg3 * 0.5f);
                                break;
                            case 3:
                                if (battleAction.combo.user4 != null)
                                    dmg += dmg4;
                                break;
                            case 4:
                                if (battleAction.combo.user5 != null)
                                    dmg += dmg5;
                                break;
                        }
                    }
                }
                else
                {
                    if (battleAction.cureStatus)
                    {
                        dmg = 0;
                    } else
                    {
                        dmg = CalculateDamage(battleAction.user, targ, battleAction.move);
                    }
                }
                Vector2 characterPos = targ.transform.position;
                switch (battleAction.move.moveType)
                {
                    #region ATTACK
                    case s_move.MOVE_TYPE.PHYSICAL:
                    case s_move.MOVE_TYPE.SPECIAL:

                        #region CHECK FOR SACRIFICE
                        {
                            List<o_battleCharacter> counterOpts;
                            if (isPlayerTurn)
                            {
                                counterOpts = oppositionCharacters;
                            }
                            else
                            {
                                counterOpts = playerCharacters;
                            }
                            float minimum_res = 6;
                            o_battleCharacter sacrifice = null;
                            foreach (o_battleCharacter bc in counterOpts)
                            {
                                if (bc.health <= 0)
                                {
                                    continue;
                                }
                                if (!bc.extraPassives.Find(x => x.passiveSkillType == s_passive.PASSIVE_TYPE.SACRIFICE))
                                {
                                    continue;
                                }
                                if (battleAction.move.moveTarg == s_move.MOVE_TARGET.SINGLE)
                                {
                                    if (battleAction.target.health < dmg && bc.health > CalculateDamage(battleAction.user, bc, battleAction.move))
                                    {
                                        if (bc.elementals[battleAction.move.element] >= 2)
                                        {
                                            /*
                                            if (minimum_res > bc.elementals[(int)battleAction.move.element])
                                            {
                                                sacrifice = bc;
                                                minimum_res = bc.elementals[(int)battleAction.move.element];
                                            }
                                            */
                                        }
                                    }
                                }
                            }
                            if (sacrifice != null)
                            {
                                battleAction.target = sacrifice;
                                dmg = CalculateDamage(battleAction.user, targ, battleAction.move);
                            }
                        }
                        #endregion

                        #region PRESS TURN STUFF
                        {
                            int numOfTimes = 1;
                            switch (battleAction.combo.comboType)
                            {
                                case s_move.MOVE_QUANITY_TYPE.MONO_TECH:
                                    numOfTimes = 1;
                                    break;
                                case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:
                                    numOfTimes = 2;
                                    break;
                                case s_move.MOVE_QUANITY_TYPE.TRIPLE_TECH:
                                    numOfTimes = 3;
                                    break;
                                case s_move.MOVE_QUANITY_TYPE.QUAD_TECH:
                                    numOfTimes = 4;
                                    break;
                                case s_move.MOVE_QUANITY_TYPE.PENTA_TECH:
                                    numOfTimes = 5;
                                    break;
                            }

                            for (int i = 0; i < numOfTimes; i++)
                            {
                                float smirkChance = UnityEngine.Random.Range(0,1);
                                ELEMENT_WEAKNESS fl = 0;
                                float elementWeakness = targ.elementals[battleAction.move.element];
                                if (battleAction.move.moveType == s_move.MOVE_TYPE.STATUS)
                                {
                                    fl = ELEMENT_WEAKNESS.NONE;
                                }
                                else
                                {
                                    if (elementWeakness >= 2)
                                        fl = ELEMENT_WEAKNESS.FRAIL;
                                    else if (elementWeakness < 2 && elementWeakness > 0)
                                        fl = ELEMENT_WEAKNESS.NONE;
                                    else if (elementWeakness == 0)
                                        fl = ELEMENT_WEAKNESS.NULL;
                                    else if (elementWeakness < 0 && elementWeakness > -1)
                                        fl = ELEMENT_WEAKNESS.REFLECT;
                                    else if (elementWeakness <= -1)
                                        fl = ELEMENT_WEAKNESS.ABSORB;
                                }
                                switch (fl) {
                                    case ELEMENT_WEAKNESS.ABSORB:
                                        damageFlag = DAMAGE_FLAGS.ABSORB;
                                        if (finalDamageFlag <= damageFlag)
                                        {
                                            finalDamageFlag = damageFlag;
                                        }
                                        if (smirkChance < 0.65f)
                                        {

                                        }
                                        break;

                                    case ELEMENT_WEAKNESS.REFLECT:
                                        damageFlag = DAMAGE_FLAGS.REFLECT;
                                        if (finalDamageFlag <= damageFlag)
                                        {
                                            finalDamageFlag = damageFlag;
                                        }
                                        break;

                                    case ELEMENT_WEAKNESS.NULL:
                                        damageFlag = DAMAGE_FLAGS.VOID;
                                        if (finalDamageFlag <= damageFlag)
                                        {
                                            finalDamageFlag = damageFlag;
                                        }
                                        break;

                                    case ELEMENT_WEAKNESS.NONE:
                                        damageFlag = DAMAGE_FLAGS.NONE;
                                        if (finalDamageFlag == damageFlag)
                                        {
                                            finalDamageFlag = damageFlag;
                                        }
                                        break;

                                    case ELEMENT_WEAKNESS.FRAIL:
                                        if (targ.guardPoints == 0)
                                        {
                                            damageFlag = DAMAGE_FLAGS.FRAIL;
                                            if (finalDamageFlag <= damageFlag)
                                            {
                                                finalDamageFlag = DAMAGE_FLAGS.FRAIL;
                                            }
                                        }
                                        else
                                        {
                                            damageFlag = DAMAGE_FLAGS.NONE;
                                            if (finalDamageFlag <= damageFlag)
                                            {
                                                finalDamageFlag = DAMAGE_FLAGS.NONE;
                                            }
                                        }
                                        break;
                                }
                                #region HIT FROZEN FOE
                                if (targ.HasStatus(STATUS_EFFECT.FROZEN))
                                {
                                    switch (battleAction.move.element) {
                                        case ELEMENT.STRIKE:
                                        case ELEMENT.PEIRCE:
                                            if (targ.guardPoints == 0)
                                                damageFlag = DAMAGE_FLAGS.FRAIL;
                                            else
                                            {
                                                targ.guardPoints = 0;
                                                damageFlag = DAMAGE_FLAGS.NONE;
                                            }
                                            break;
                                    }
                                }
                                #endregion
                            }
                        }
                        DamageEffect(dmg, targ, characterPos, damageFlag);
                        if (battleAction.move.statusInflictChance != null)
                        {
                            foreach (s_move.statusInflict statusChance in battleAction.move.statusInflictChance)
                            {
                                float ch = UnityEngine.Random.Range(0, 1);
                                if (ch <= statusChance.status_inflict_chance)
                                {
                                    battleAction.target.SetStatus(new s_statusEff(
                                        statusChance.status_effect,
                                        statusChance.duration,
                                        statusChance.damage));
                                }
                            }
                        }
                        #region ELEMENT STATUS
                        {
                            float status_inflict_chance = 0;
                            s_statusEff eff = new s_statusEff();
                            float ch = UnityEngine.Random.Range(0f, 1f);
                            switch (battleAction.move.element)
                            {
                                case ELEMENT.ELECTRIC:
                                    status_inflict_chance = 0.45f;
                                    eff.duration = 4;
                                    eff.status = STATUS_EFFECT.STUN;
                                    break;

                                case ELEMENT.ICE:
                                    status_inflict_chance = 0.35f;
                                    eff.duration = 0;
                                    eff.status = STATUS_EFFECT.FROZEN;
                                    break;

                                case ELEMENT.PSYCHIC:
                                    status_inflict_chance = 0.5f;
                                    eff.duration = 3;
                                    eff.status = STATUS_EFFECT.CONFUSED;
                                    break;

                                case ELEMENT.FIRE:
                                    status_inflict_chance = 0.15f;
                                    eff.duration = 3;
                                    eff.damage = Mathf.FloorToInt(CalculateDamage(battleAction.user, targ, battleAction.move) * 0.15f);
                                    eff.status = STATUS_EFFECT.BURN;
                                    break;
                            }

                            if (!battleAction.cureStatus)
                            {
                                switch (battleAction.move.element)
                                {
                                    case ELEMENT.ELECTRIC:
                                    case ELEMENT.ICE:
                                    case ELEMENT.PSYCHIC:
                                    case ELEMENT.FIRE:
                                        if (ch < status_inflict_chance)
                                        {
                                            targ.SetStatus(eff);
                                        }
                                        break;
                                }
                            }
                            switch (battleAction.move.element)
                            {
                                case ELEMENT.WATER:
                                case ELEMENT.ICE:
                                    targ.RemoveStatus(STATUS_EFFECT.BURN);
                                    break;
                                case ELEMENT.FIRE:
                                    targ.RemoveStatus(STATUS_EFFECT.FROZEN);
                                    break;
                            }
                        }
                        #endregion

                        for (int i = 0; i < 2; i++)
                        {
                            targ.transform.position = characterPos + new Vector2(15, 0);
                            yield return new WaitForSeconds(0.02f);
                            targ.transform.position = characterPos;
                            yield return new WaitForSeconds(0.02f);
                            targ.transform.position = characterPos + new Vector2(-15, 0);
                            yield return new WaitForSeconds(0.02f);
                            targ.transform.position = characterPos;
                            yield return new WaitForSeconds(0.02f);
                        }

                        #region CHECK FOR COUNTER
                        {
                            if (targ.extraPassives.Find
                                (x => x.passiveSkillType == s_passive.PASSIVE_TYPE.COUNTER)) {
                                characterPos = battleAction.user.transform.position;
                                dmg = CalculateDamage(targ, battleAction.user, defaultAttack);

                                DamageEffect(dmg, battleAction.user, characterPos, DAMAGE_FLAGS.NONE);

                                for (int i = 0; i < 2; i++)
                                {
                                    battleAction.user.transform.position = characterPos + new Vector2(15, 0);
                                    yield return new WaitForSeconds(0.02f);
                                    battleAction.user.transform.position = characterPos;
                                    yield return new WaitForSeconds(0.02f);
                                    battleAction.user.transform.position = characterPos + new Vector2(-15, 0);
                                    yield return new WaitForSeconds(0.02f);
                                    battleAction.user.transform.position = characterPos;
                                    yield return new WaitForSeconds(0.02f);
                                }
                            }
                        }
                        #endregion

                        break;
                    #endregion

                    #endregion

                    #region STATUS
                    case s_move.MOVE_TYPE.STATUS:
                        switch (battleAction.move.statusType)
                        {
                            case s_move.STATUS_TYPE.HEAL_HEALTH:
                                battleAction.target.health += dmg;
                                battleAction.target.health = Mathf.Clamp(battleAction.target.health,
                                    -battleAction.target.maxHealth, battleAction.target.maxHealth);
                                SpawnDamageObject(dmg, characterPos, Color.white, "heal_hp");
                                break;

                            case s_move.STATUS_TYPE.HEAL_STAMINA:
                                targ.stamina += dmg;
                                targ.stamina = Mathf.Clamp(targ.stamina,
                                    0, targ.maxStamina);
                                SpawnDamageObject(dmg, characterPos, Color.magenta, "heal_hp");
                                break;

                            case s_move.STATUS_TYPE.HEAL_SP_BUFF:
                                targ.vitalityBuff += battleAction.move.vitBuff;
                                targ.strengthBuff += battleAction.move.strBuff;
                                targ.dexterityBuff += battleAction.move.dexBuff;
                                targ.agilityBuff += battleAction.move.agiBuff;
                                targ.stamina += dmg;
                                targ.stamina = Mathf.Clamp(targ.stamina,
                                    0, targ.maxStamina);
                                SpawnDamageObject(dmg, characterPos, Color.magenta, "heal_hp");
                                break;

                            case s_move.STATUS_TYPE.HEAL_HP_BUFF:
                                targ.vitalityBuff += battleAction.move.vitBuff;
                                targ.strengthBuff += battleAction.move.strBuff;
                                targ.dexterityBuff += battleAction.move.dexBuff;
                                targ.agilityBuff += battleAction.move.agiBuff;
                                targ.health += dmg;
                                targ.health = Mathf.Clamp(targ.health,
                                    -targ.maxHealth, targ.maxHealth);
                                SpawnDamageObject(dmg, characterPos, Color.white, "heal_hp");
                                break;

                            case s_move.STATUS_TYPE.BUFF:
                                targ.vitalityBuff += battleAction.move.vitBuff;
                                if (targ.vitalityBuff > 0)
                                {
                                    SpawnDamageObject(targ.vitalityBuff, characterPos, Color.blue, "buffVit");
                                    yield return new WaitForSeconds(0.05f);
                                }
                                targ.strengthBuff += battleAction.move.strBuff;
                                if (targ.strengthBuff > 0)
                                {
                                    SpawnDamageObject(targ.strengthBuff, characterPos, Color.red, "buffStr");
                                    yield return new WaitForSeconds(0.05f);
                                }
                                targ.dexterityBuff += battleAction.move.dexBuff;
                                if (targ.dexterityBuff > 0)
                                {
                                    SpawnDamageObject(targ.dexterityBuff, characterPos, Color.magenta, "buffDex");
                                    yield return new WaitForSeconds(0.05f);
                                }
                                targ.agilityBuff += battleAction.move.agiBuff;
                                if (targ.agilityBuff > 0)
                                {
                                    SpawnDamageObject(targ.agilityBuff, characterPos, Color.yellow, "buffAgi");
                                    yield return new WaitForSeconds(0.05f);
                                }
                                break;
                        }
                        break;
                        #endregion
                }
                break;
        }
        //Show Damage output here


        yield return new WaitForSeconds(0.18f);
        if (targ.health <= 0)
        {
            if (oppositionCharacters.Contains(targ))
            {
                s_soundmanager.GetInstance().PlaySound("enemy_defeat");
            }
            else {
                s_soundmanager.GetInstance().PlaySound("player_defeat");
            }
            StartCoroutine(PlayFadeCharacter(targ, Color.black, Color.clear));
        }
    }
    public enum MENU_CONTROLL_TYPE {
        LINEAR_UP_DOWN,
        LINEAR_LEFT_RIGHT,
        MULTI_DIRECTIONAL
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="mc"></param>
    /// <param name="limiterMulti">Only use this if the menu control type is Multi-Directional</param>
    public void MenuControl(int limit, MENU_CONTROLL_TYPE mc, Vector2 limiterMulti)
    {
        switch (mc) {
            case MENU_CONTROLL_TYPE.LINEAR_LEFT_RIGHT:

                if (Input.GetKeyDown(s_globals.GetKeyPref("left")))
                {
                    menuchoice--;
                }
                if (Input.GetKeyDown(s_globals.GetKeyPref("right")))
                {
                    menuchoice++;
                }
                break;

            case MENU_CONTROLL_TYPE.LINEAR_UP_DOWN:

                if (Input.GetKeyDown(s_globals.GetKeyPref("up")))
                {
                    menuchoice--;
                }
                if (Input.GetKeyDown(s_globals.GetKeyPref("down")))
                {
                    menuchoice++;
                }
                break;

            case MENU_CONTROLL_TYPE.MULTI_DIRECTIONAL:

                if (Input.GetKeyDown(s_globals.GetKeyPref("up")))
                {
                    menuchoice--;
                }
                if (Input.GetKeyDown(s_globals.GetKeyPref("down")))
                {
                    menuchoice++;
                }
                if (Input.GetKeyDown(s_globals.GetKeyPref("left")))
                {
                    menuchoice += (int)limiterMulti.y;
                }
                if (Input.GetKeyDown(s_globals.GetKeyPref("right")))
                {
                    menuchoice -= (int)limiterMulti.y;
                }
                break;
        }
        if (menuchoice < 0)
            menuchoice = (limit - 1);
        if(menuchoice > (limit - 1))
        menuchoice = 0;

    }
    
    public IEnumerator ConcludeBattle()
    {
        //Fade
        //EXPResults.SetActive(false);
        //oppositionCharacterTurnQueue.Clear();
        //playerCharacterTurnQueue.Clear();
        currentPartyCharactersQueue.Clear();
        battleEngine = BATTLE_ENGINE_STATE.NONE;
        yield return new WaitForSeconds(0.1f);
        //yield return StartCoroutine(s_globals.globalSingle.s(true));

        if (!nonChangablePlayers)
        {
            s_menuhandler.GetInstance().SwitchMenu("ExperienceMenu");

            bool allDefeated = oppositionCharacters.FindAll(x => x.health <= 0).Count == oppositionCharacters.Count;

            /*
            foreach (o_battleCharacter c in playerCharacters)
            {
                float exp = TotalEXP(c, allDefeated);
                //we add the exp and make it so that it checks for a level up
                for (float i = 0; i < exp; i++)
                {
                    c.experiencePoints += 1;
                    if (c.experiencePoints >= 100)
                    {
                        o_battleCharDataN chdat = c.battleCharData;
                        if (i % chdat.strengthGT == 0)
                            c.strength++;
                        if (i % chdat.vitalityGT == 0)
                            c.vitality++;
                        if (i % chdat.dexterityGT == 0)
                            c.dexterity++;
                        c.level++;
                        c.experiencePoints = 0;
                        exp = TotalEXP(c, allDefeated);
                        c.maxHealth += UnityEngine.Random.Range(chdat.maxHitPointsGMin, chdat.maxHitPointsGMax + 1);
                        c.maxStamina += UnityEngine.Random.Range(chdat.maxSkillPointsGMin, chdat.maxSkillPointsGMax + 1);
                    }
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }
            */

            List<string> extSkillLearn = new List<string>();

            foreach (o_battleCharacter en in oppositionCharacters)
            {
                if (en.health > 0)
                    continue;
                s_rpgGlobals.money += en.battleCharData.money;
                foreach (s_move mv in en.currentMoves)
                {
                    if (!s_rpgGlobals.rpgGlSingleton.extraSkills.Contains(mv))
                    {
                        print("Added skill");
                        s_rpgGlobals.rpgGlSingleton.extraSkills.Add(mv);
                        extSkillLearn.Add(mv.name);
                    }
                }
                if (en.extraSkills != null)
                {
                    foreach (s_move mv in en.extraSkills)
                    {
                        if (!s_rpgGlobals.rpgGlSingleton.extraSkills.Contains(mv))
                        {
                            print("Added skill");
                            s_rpgGlobals.rpgGlSingleton.extraSkills.Add(mv);
                            extSkillLearn.Add(mv.name);
                        }
                    }
                }
                foreach (var mv in en.extraPassives)
                {
                    if (!s_rpgGlobals.rpgGlSingleton.extraPassives.Contains(mv))
                    {
                        s_rpgGlobals.rpgGlSingleton.extraPassives.Add(mv);
                        extSkillLearn.Add(mv.name);
                    }
                }
            }

            yield return new WaitForSeconds(2.5f);
            foreach (o_battleCharacter c in playerCharacters)
            {
                s_rpgGlobals.rpgGlSingleton.SetPartyMemberStats(c);
            }
        }
        s_rpgGlobals.rpgGlSingleton.SwitchToOverworld(false);
    }
    public IEnumerator GameOver()
    {
        currentPartyCharactersQueue.Clear();
        yield return StartCoroutine(s_triggerhandler.trigSingleton.Fade(Color.black));
        isPlayerTurn = true;
       // players.Clear();
        isEnabled = false;
        s_rpgGlobals.rpgGlSingleton.ClearAllThings();
        //Destroy(rpg_globals.gl.player.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    public IEnumerator NextTeamTurn()
    {
        //PT_GUI
        battleEngine = BATTLE_ENGINE_STATE.NONE;

        //If all opposition is down then declare victory!
        if (oppositionCharacters.FindAll(x => x.health > 0).Count == 0) {
            yield return StartCoroutine(ConcludeBattle());
        } else if (playerCharacters.FindAll(x => x.health > 0).Count == 0)
        {
            yield return StartCoroutine(GameOver());
        }
        else if (netTurn <= 0)
        {
            List<o_battleCharacter> bcs = new List<o_battleCharacter>();
            yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
            if (isPlayerTurn) {
                bcs = oppositionCharacters;
                isPlayerTurn = false;
            } else {
                bcs = playerCharacters;
                isPlayerTurn = true;
            }
            currentPartyCharactersQueue.Clear();
            int i = 0;
            foreach (o_battleCharacter c in bcs)
            {
                if (c.health > 0 && c.inBattle == true)
                {
                    c.RemoveStatus(STATUS_EFFECT.FROZEN);
                    for (int i2 = 0; i2 < c.battleCharData.turnIcons; i2++)
                    {
                        fullTurn++;
                        yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.APPEAR, i));
                        i++;
                    }
                }
                currentPartyCharactersQueue.Enqueue(c);
            }
            yield return StartCoroutine(PollBattleEvent());
            battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
        }
        else
        {
            //NextTurn();
            //print(netTurn);
            currentPartyCharactersQueue.Dequeue();
            currentPartyCharactersQueue.Enqueue(battleAction.user);
            
            yield return StartCoroutine(s_camera.cam.MoveCamera(currentPartyCharactersQueue.Peek().transform.position, 0.9f));
            yield return new WaitForSeconds(0.25f);
            battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
        }
    }
    public IEnumerator PollBattleEvent()
    {
        bool conditionFufilled = false;
        if (OnBattleEvents != null)
            for (int i = 0; i < OnBattleEvents.Length; i++)
            {
                s_battleEvent be = OnBattleEvents[i];
                if (battleEvDone[i])
                {
                    switch (be.battleCheckCond)
                    {
                        case s_battleEvent.B_CHECK_COND.ON_START:

                            switch (be.battleCond)
                            {
                                /*
                                case s_battleEvents.B_COND.HEALTH:
                                    if (opposition.Find(x => x.name == be.name).hitPoints == be.int0)
                                    {
                                        conditionFufilled = true;
                                    }
                                    break;
                                    */

                                case s_battleEvent.B_COND.TURNS_ELAPSED:
                                    if (roundNum == be.int0)
                                    {
                                        conditionFufilled = true;
                                        roundNum = 0;
                                    }
                                    break;

                            }
                            if (conditionFufilled)
                            {
                                s_rpgEvents.GetInstance().JumpToEvent(be.eventScript);
                                isCutscene = true;
                                battleEvDone[i] = false;

                                yield return null;
                            }
                            break;

                        case s_battleEvent.B_CHECK_COND.PER_TURN:

                            if (be.enabled)
                            {
                                switch (be.battleCond)
                                {
                                    /*
                                    case s_battleEvents.B_COND.HEALTH:
                                        if (opposition.Find(x => x.name == be.name).hitPoints == be.int0)
                                        {
                                            conditionFufilled = true;
                                        }
                                        break;
                                        */
                                    case s_battleEvent.B_COND.TURNS_ELAPSED:
                                        if (roundNum >= be.int0)
                                        {
                                            s_triggerhandler.GetInstance().GetComponent<s_rpgEvents>().JumpToEvent(be.eventScript);//yield return StartCoroutine();
                                            isCutscene = true;
                                            battleEvDone[i] = false;
                                            roundNum = 0;
                                            goto l1a;
                                        }
                                        break;

                                }
                            }
                            else
                                continue;
                            break;

                    }

                }
                else
                    continue;

            }
        l1a:
        battleEngine = BATTLE_ENGINE_STATE.NONE;
    }
    #endregion

    #region DRAW STUFF
    public void DrawButtonIcon(ref Vector2 pos, int i, string buttonName, UnityEngine.Texture2D tex)
    {
        if(tex != null)
            GUI.DrawTexture(new Rect(pos, new Vector2(30, 30)), tex);
        //pos += new Vector2(0, -20);
        if (menuchoice == i)
            GUI.color = Color.red;
        else
            GUI.color = Color.white;

        GUI.Label(new Rect(pos + new Vector2(30, 0), new Vector2(130, 30)), buttonName);
        pos.y += 40;
    }

    public void DrawButtonTarget(ref Vector2 pos, int i, o_battleCharacter bc)
    {
        pos += new Vector2(0, -20);
        if (menuchoice == i)
            GUI.color = Color.red;
        else
            GUI.color = Color.white;

        GUI.Label(new Rect(pos + new Vector2(30, 0), new Vector2(130, 30)), bc.name);
        pos.y += 40;
    }
    
    enum TURN_ICON_FX {
        APPEAR,
        FADE,
        HIT
    }
    IEnumerator TurnIconFX(TURN_ICON_FX fx, int i) {
        float t = 0;
        float speed = 2.85f;
        switch (fx) {
            case TURN_ICON_FX.APPEAR:
                while (PT_GUI[i].color != Color.white)
                {
                    PT_GUI[i].color = Color.Lerp(Color.clear,Color.white,t);
                    t += Time.deltaTime * speed;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;

            case TURN_ICON_FX.HIT:
                if (PT_GUI.Length > i) {

                }
                while (PT_GUI[i].color != Color.magenta)
                {
                    PT_GUI[i].color = Color.Lerp(Color.white, Color.magenta, t);
                    t += Time.deltaTime * speed;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;

            case TURN_ICON_FX.FADE:
                while (PT_GUI[i].color != Color.clear) {
                    PT_GUI[i].color = Color.Lerp(Color.white, Color.clear,t);
                    t += Time.deltaTime * speed;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;
        }
    }
    #endregion
}
/*
public void DisplayAttackComboButtonsGUI() {
    Vector2 pos = new Vector2(20, 20);
    ClearSkillMenuButtons();
    if (currentCharacter.currentMoves != null)
    {
        List<Tuple<s_moveComb, s_move>> listMoves = s_rpgGlobals.rpgGlSingleton.CheckComboRequirementsCharacter2(currentCharacter,playerCharacters);
        for (int i = 0; i < listMoves.Count; i++)
        {
            s_move m = listMoves[i].Item2;
            Sprite draw = null;
            switch (m.element)
            {
                case ELEMENT.NORMAL:
                    draw = strike_picture;
                    break;
                case ELEMENT.FIRE:
                    draw = fire_picture;
                    break;
                case ELEMENT.ICE:
                    draw = ice_picture;
                    break;
                case ELEMENT.BIO:
                    draw = water_picture;
                    break;
                case ELEMENT.ELECTRIC:
                    draw = electric_picture;
                    break;
                case ELEMENT.FORCE:
                    draw = force_picture;
                    break;
                case ELEMENT.EARTH:
                    draw = earth_picture;
                    break;
                case ELEMENT.CURSE:
                    draw = dark_picture;
                    break;
                case ELEMENT.LIGHT:
                    draw = light_picture;
                    break;
                case ELEMENT.PSYCHIC:
                    draw = psychic_picture;
                    break;
                case ELEMENT.WIND:
                    draw = wind_picture;
                    break;
            }
            DrawButtonIcon(ref pos, i, m.name, draw);
        }
    }
}
public void DisplayItemButtonsGUI()
{
    Vector2 pos = new Vector2(20, 20);

    if (s_rpgGlobals.rpgGlSingleton.GetItems() != null)
    {

        for (int i = 0; i < s_rpgGlobals.rpgGlSingleton.GetItems().Count; i++)
        {
             o_RPGitem it = s_rpgGlobals.rpgGlSingleton.GetItems()[i];
            s_move m = it.move;
            Sprite draw = null;
            switch (m.element)
            {
                case ELEMENT.NORMAL:
                    draw = strike_picture;
                    break;
                case ELEMENT.FIRE:
                    draw = fire_picture;
                    break;
                case ELEMENT.ICE:
                    draw = ice_picture;
                    break;
                case ELEMENT.BIO:
                    draw = water_picture;
                    break;
                case ELEMENT.ELECTRIC:
                    draw = electric_picture;
                    break;
                case ELEMENT.FORCE:
                    draw = force_picture;
                    break;
                case ELEMENT.EARTH:
                    draw = earth_picture;
                    break;
                case ELEMENT.CURSE:
                    draw = dark_picture;
                    break;
                case ELEMENT.LIGHT:
                    draw = light_picture;
                    break;
                case ELEMENT.PSYCHIC:
                    draw = psychic_picture;
                    break;
                case ELEMENT.WIND:
                    draw = wind_picture;
                    break;
            }
            DrawButtonIcon(ref pos, i, it.name, draw);
        }
    }
}
public void DrawHP(o_battleCharacter bc, int i)
{
    HP_GUIS[i].enabled = true;
    HP_GUIS[i].HPboxColour.color = bc.battleCharData.characterColour;
    HP_GUIS[i].hpText.text = bc.name + "\nHP: " + bc.health + "\nSP: " + bc.stamina;
}
*/
/*
public void DisplayTargetButtonsGUI()
{
    Vector2 pos = new Vector2(20, 20);
    bool onParty = false;
    if (battleAction.type == s_battleAction.MOVE_TYPE.ITEM) {
        onParty = battleAction.item.move.onParty;
    }
    else if(battleAction.type == s_battleAction.MOVE_TYPE.MOVE) {
        onParty = battleAction.move.onParty;
    }
    if (onParty) {
        for (int i = 0; i < playerCharacters.Count; i++) { DrawButtonTarget(ref pos, i, playerCharacters[i]); }
    } else {
        for (int i = 0; i < oppositionCharacters.Count; i++) { DrawButtonTarget(ref pos, i, oppositionCharacters[i]); }
    }
}
*/
/*
public void FightMenuIcons(List<string> buttonList, string curOpt)
{
fightButton.gameObject.SetActive(false);
skillButton.gameObject.SetActive(false);
comboButton.gameObject.SetActive(false);
itemButton.gameObject.SetActive(false);
partyButton.gameObject.SetActive(false);
guardButton.gameObject.SetActive(false);
fleeButton.gameObject.SetActive(false);
passButton.gameObject.SetActive(false);


for (int i = 0; i < buttonList.Count; i++) {
    //pos += new Vector2(0, -20);
    Image sel = null;
    switch (buttonList[i])
    {
        case "guard":
            guardButton.gameObject.SetActive(true);
            break;
        case "fight":
            fightButton.gameObject.SetActive(true);
            break;
        case "pass":
            passButton.gameObject.SetActive(true);
            break;
        case "combo":
            comboButton.gameObject.SetActive(true);
            break;
        case "skills":
            skillButton.gameObject.SetActive(true);
            break;
        case "items":
            itemButton.gameObject.SetActive(true);
            break;
        case "party":
            partyButton.gameObject.SetActive(true);
            break;
    }
    switch (curOpt)
    {
        case "fight":
            sel = fightButton;
            break;
        case "skills":
            sel = skillButton;
            break;
        case "combo":
            sel = comboButton;
            break;
        case "guard":
            sel = guardButton;
            break;
        case "pass":
            sel = passButton;
            break;
        case "party":
            sel = partyButton;
            break;
        case "items":
            sel = itemButton;
            break;
    }
    if (sel != null)
    {
        buttonSelector.rectTransform.position = sel.rectTransform.position;
        //sel.color = Color.magenta;
    }
}
}
*/
/*
public void ClearSkillMenuButtons()
{
    for (int i = 0; i < skillButtons.Length; i++) {

        skillButtons[i].img.sprite = null;
        skillButtons[i].img.color = Color.clear;
        skillButtons[i].buttonText.text = "";
    }
}
*/
/*
public void DrawButtonIcon(ref Vector2 pos, int i, string buttonName, Sprite tex)
{
if (i > skillButtons.Length-1)
    return;
skillButtons[i].img.sprite = tex;
//pos += new Vector2(0, -20);
if (menuchoice == i)
    skillButtons[i].img.color = Color.red;
else
    skillButtons[i].img.color = Color.white;
skillButtons[i].buttonText.text = buttonName;
}
*/
/*
                    case BATTLE_MENU_CHOICES.MENU:
                        //fightMenu.SetActive(true);
                        List<string> options = new List<string>();

                        options.Add("fight");
                        if (currentCharacter.currentMoves.Count > 0)
                            options.Add("skills");
                        if (s_rpgGlobals.rpgGlSingleton.CheckComboRequirementsCharacter(currentCharacter, playerCharacters).Count > 0)
                            options.Add("combo");
                        options.Add("guard");
                        options.Add("items");
                        options.Add("party");
                        options.Add("pass");
                        options.Add("run");
                        MenuControl(options.Count, MENU_CONTROLL_TYPE.LINEAR_LEFT_RIGHT, new Vector2(0, 0));

                        FightMenuIcons(options, options[menuchoice]);

                        if (Input.GetKeyDown(s_globals.GetKeyPref("select")))
                        {
                            switch (options[menuchoice]) {
                                case "fight":
                                    fightMenu.SetActive(false);
                                    battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
                                    battleAction.move = defaultAttack;
                                    battleEngine = BATTLE_ENGINE_STATE.TARGET;
                                    break;

                                case "skills":
                                    fightMenu.SetActive(false);
                                    battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
                                    //s_menuhandler.GetInstance().SwitchMenu("BattleSkillMenu");
                                    battleDecisionMenu = BATTLE_MENU_CHOICES.SKILLS;
                                    break;

                                case "items":
                                    fightMenu.SetActive(false);
                                    battleDecisionMenu = BATTLE_MENU_CHOICES.ITEMS;
                                    break;

                                case "combo":
                                    fightMenu.SetActive(false);
                                    battleDecisionMenu = BATTLE_MENU_CHOICES.COMBO;
                                    break;

                                case "pass":
                                    fightMenu.SetActive(false);
                                    battleAction.type = s_battleAction.MOVE_TYPE.PASS;
                                    battleEngine = BATTLE_ENGINE_STATE.PROCESS_ACTION;
                                    StartCoroutine(PlayAttackAnimation());
                                    break;

                            }
                            menuchoice = 0;
                        }
                        break;

                    case BATTLE_MENU_CHOICES.COMBO:
                    skillMenu.SetActive(true);
                    List<Tuple<s_moveComb, s_move>> combos = s_rpgGlobals.rpgGlSingleton.CheckComboRequirementsCharacter2(currentCharacter, playerCharacters);
                    MenuControl(combos.Count, MENU_CONTROLL_TYPE.MULTI_DIRECTIONAL, new Vector2(2, 7));
                    if (Input.GetKeyDown(s_globals.GetKeyPref("select")))
                    {
                        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
                        battleAction.move = combos[menuchoice].Item2;
                        skillMenu.SetActive(false);
                        HP_GUIS[playerCharacters.IndexOf(combos[menuchoice].Item1.user1)].ChangeComboImage(combos[menuchoice].Item1.user1Move);
                        HP_GUIS[playerCharacters.IndexOf(combos[menuchoice].Item1.user2)].ChangeComboImage(combos[menuchoice].Item1.user2Move);
                        battleEngine = BATTLE_ENGINE_STATE.TARGET;
                        menuchoice = 0;
                    }
                    if (Input.GetKeyDown(s_globals.GetKeyPref("back")))
                    {
                        skillMenu.SetActive(false);
                        battleDecisionMenu = BATTLE_MENU_CHOICES.MENU;
                        battleEngine = BATTLE_ENGINE_STATE.DECISION;
                        menuchoice = 0;
                    }
                    comboGUI.enabled = true;
                    */
/*
comboGUIText.text = listOfMoves[menuchoice].Item1.user1.name + " Move: " + listOfMoves[menuchoice].Item1.user1Move.name
+ "\n"+listOfMoves[menuchoice].Item1.user2.name + " Move: " + listOfMoves[menuchoice].Item1.user2Move.name;
break;
*/
/*
public void DisplayAttackButtonsGUI() {
    Vector2 pos = new Vector2(20, 20);
    if (currentCharacter.currentMoves != null)
    {
        for (int i = 0; i < currentCharacter.currentMoves.Count; i++)
        {
            s_move m = currentCharacter.currentMoves[i];
            Sprite draw = null;
            switch (m.element)
            {
                case ELEMENT.NORMAL:
                    draw = strike_picture;
                    break;
                case ELEMENT.FIRE:
                    draw = fire_picture;
                    break;
                case ELEMENT.ICE:
                    draw = ice_picture;
                    break;
                case ELEMENT.BIO:
                    draw = water_picture;
                    break;
                case ELEMENT.ELECTRIC:
                    draw = electric_picture;
                    break;
                case ELEMENT.FORCE:
                    draw = force_picture;
                    break;
                case ELEMENT.EARTH:
                    draw = earth_picture;
                    break;
                case ELEMENT.CURSE:
                    draw = dark_picture;
                    break;
                case ELEMENT.LIGHT:
                    draw = light_picture;
                    break;
                case ELEMENT.PSYCHIC:
                    draw = psychic_picture;
                    break;
                case ELEMENT.WIND:
                    draw = wind_picture;
                    break;
            }
            DrawButtonIcon(ref pos, i, m.name, draw);
        }
    }
}
*/
/*
private void OnGUI()
{
    {
        Vector2 pos = new Vector2(20, 400);
        int i = 0;
        foreach (o_battleCharacter bc in playerCharacters)
        {
            //DrawHP(bc, i);
            i++;
        }
    }

    if (isPlayerTurn) {
        switch (battleEngine) {
            case BATTLE_ENGINE_STATE.DECISION:
                switch (battleDecisionMenu)
                {
                    case BATTLE_MENU_CHOICES.MENU:

                        Vector2 pos = new Vector2(20, 20);
                        break;

                    case BATTLE_MENU_CHOICES.SKILLS:
                        DisplayAttackButtonsGUI();
                        break;

                    case BATTLE_MENU_CHOICES.COMBO:
                        //DisplayAttackComboButtonsGUI();
                        break;

                    case BATTLE_MENU_CHOICES.ITEMS:
                        //DisplayItemButtonsGUI();
                        break;
                }
                break;

            case BATTLE_ENGINE_STATE.TARGET:
                //DisplayTargetButtonsGUI();
                break;

        }
    }
}
*/
/*
switch (battleAction.target.elementals[(int)el].flag)
{
    case s_affinity.DAMAGE_FLAGS.FRAIL:
    case s_affinity.DAMAGE_FLAGS.NONE:
        battleAction.target.health -= dmg;
        break;
    case s_affinity.DAMAGE_FLAGS.REFLECT:
        //Recalculate user attack against user defence or something.
        switch (battleAction.move.moveType)
        {
            case s_move.MOVE_TYPE.PHYSICAL:
                dmg = (int)(battleAction.move.power * (battleAction.user.strength / battleAction.user.vitality)
                    * battleAction.target.elementals[(int)battleAction.move.element].affinity);
                break;
            case s_move.MOVE_TYPE.SPECIAL:
                dmg = (int)(battleAction.move.power * (battleAction.user.dexterity / battleAction.user.vitality)
                    * battleAction.target.elementals[(int)battleAction.move.element].affinity);
                break;
        }
        battleAction.user.health -= dmg;
        break;
    case s_affinity.DAMAGE_FLAGS.VOID:
        dmg = 0;
        break;
    case s_affinity.DAMAGE_FLAGS.ABSORB:
        battleAction.target.health += dmg;
        break;
}
*/
/*
TODO: 
Like Digital Devil Saga, depending on how many people were involved in a combo
the amount of press turns may increase or decresase
 */
/*
if (!flagSet)
{
   int numOfTimes = 1;
   switch (battleAction.move.comboType)
   {
       case s_move.MOVE_QUANITY_TYPE.MONO_TECH:
           numOfTimes = 1;
           break;
       case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:
           numOfTimes = 2;
           break;
       case s_move.MOVE_QUANITY_TYPE.TRIPLE_TECH:
           numOfTimes = 3;
           break;
       case s_move.MOVE_QUANITY_TYPE.QUAD_TECH:
           numOfTimes = 4;
           break;
       case s_move.MOVE_QUANITY_TYPE.PENTA_TECH:
           numOfTimes = 5;
           break;
   }

   for (int i = 0; i < numOfTimes; i++)
   {

       damageFlag = battleAction.target.elementals[(int)el].flag;
       switch (battleAction.target.elementals[(int)el].flag)
       {
           case s_affinity.DAMAGE_FLAGS.NONE:
               NextTurn();
               yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
               break;
           case s_affinity.DAMAGE_FLAGS.VOID:
               NextTurn();
               yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
               NextTurn();
               yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
               break;
           case s_affinity.DAMAGE_FLAGS.FRAIL:

               //If there are no full turn icons start taking away instead of turning full icons into half
               if (fullTurn > 0)
               {
                   HitWeakness();
                   yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.HIT, netTurn - halfTurn));
               }
               else
               {
                   HitWeakness();
                   yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
               }
               break;
           case s_affinity.DAMAGE_FLAGS.PASS:

               //If there are no full turn icons start taking away instead of turning full icons into half
               if (fullTurn > 0)
               {
                   PassTurn();
                   yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.HIT, netTurn - halfTurn));
               }
               else
               {
                   PassTurn();
                   yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
               }
               break;
           case s_affinity.DAMAGE_FLAGS.REFLECT:
           case s_affinity.DAMAGE_FLAGS.ABSORB:
               fullTurn = 0;
               halfTurn = 0;
               break;
       }
       flagSet = true;
       if (netTurn == 0)
           break;
   }
}
*/
