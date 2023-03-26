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
    LUCKY,
    CRITICAL,
    MISS,
    VOID,
    REFLECT,
    ABSORB,
    PASS = -1
}

public class s_battleEngine : s_singleton<s_battleEngine>
{
    public bool isEnabled = true;
    /*
    [System.Serializable]
    public class s_battleAction
    {
        public enum MOVE_TYPE
        {
            MOVE,
            ITEM,
            GUARD
        };
        public MOVE_TYPE type;
        public R_Character user;
        public R_Character target;
        public R_Move move;
        public bool isCombo = false;
        public bool cureStatus = false;     //This is so that the attack can cure a character and do no damage
        public s_moveComb combo;
    }
    */
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
    public R_BattleCharacterList partyMembers;
    public R_Float money;

    #region variables
    [SerializeField]
    private o_battleCharacter[] enemySlots;
    [SerializeField]
    private o_battleCharacter[] playerSlots;
    [SerializeField]
    private o_battleCharacter guest;
    public bool hasGuest = false;

    [SerializeField]
    private S_RPGGlobals rpgManager;
    [SerializeField]
    private R_ShopItem shopItems;

    public Queue<o_battleCharacter> currentPartyCharactersQueue = new Queue<o_battleCharacter>();
    public List<o_battleCharacter> playerCharacters;
    public List<o_battleCharacter> oppositionCharacters;

    [SerializeField]
    private R_CharacterList playersReference;
    [SerializeField]
    private R_CharacterList enemiesReference;

    public R_CharacterList allCharacterReferences;
    public R_CharacterList targetCharacters;

    private o_battleCharacter currentCharacterObject;
    private o_battleCharacter targetCharacterObject;

    [SerializeField]
    public R_Character currentCharacter;
    [SerializeField]
    public R_Character targetCharacter;
    [SerializeField]
    private R_Move currentMove;
    [SerializeField]
    private R_ComboMoves currentC;

    public R_MoveList extraSkills;
    public R_EnemyGroupList battleGroupRef;
    public R_EnemyGroupList battleGroupDoneRef;
    //public s_battleAction battleAction;
    
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

    public CH_Text changeMenu;
    public CH_Func perfomMove;
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
    List<List<Vector2>> battlePositionsPlayer = new List<List<Vector2>> { 
        new List<Vector2>(){
            new Vector2(240, 295) 
        },
        new List<Vector2>(){
            new Vector2(245, 290),
            new Vector2(255, 255)
        },
        new List<Vector2>(){
            new Vector2(325, 290),
            new Vector2(275, 355),
            new Vector2(255, 255)
        },
        new List<Vector2>(){
            new Vector2(250, 378),
            new Vector2(250, 322),
            new Vector2(245, 265),
            new Vector2(250, 215)
        },
        new List<Vector2>(){
            new Vector2(247, 302),
            new Vector2(250, 358),
            new Vector2(200, 330),
            new Vector2(250, 245),
            new Vector2(200, 270)
        }
    };
    #endregion

    public o_battleCharacter ReferenceToCharacter(CH_BattleChar refer) {
        return GetAllCharacters().Find(x => x.referencePoint == refer);
    }

    public List<o_battleCharacter> GetAllCharacters() {
        List<o_battleCharacter> bcs = new List<o_battleCharacter>();
        bcs.AddRange(playerCharacters);
        bcs.AddRange(oppositionCharacters);
        return bcs;
    }

    private void OnEnable()
    {
        perfomMove.OnFunctionEvent += EndAction;
    }

    private void OnDisable()
    {
        perfomMove.OnFunctionEvent -= EndAction;
    }

    public void Awake()
    {
        if (engineSingleton == null)
            engineSingleton = this;
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
        foreach (o_battleCharacter ob in playerSlots)
        {
            ob.render.sprite = null;
        }
    }

    public IEnumerator StartBattle() {
        fullTurn = 0;
        halfTurn = 0;
        s_menuhandler.GetInstance().SwitchMenu("EMPTY");
        allCharacterReferences.Clear();
        playersReference.Clear();
        enemiesReference.Clear();
        SceneManager.UnloadSceneAsync("Overworld");

        //ffff
        #region CLEAR GUI
        HPGUIMan.ClearHPGui();
        foreach (var ptIc in PT_GUI) {
            ptIc.color = Color.clear;
        }
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

            {
                List<Vector2> enPos = new List<Vector2>();
                if (enemyGroup.members_pre_summon.Length > 0)
                {
                    int leng = (enemyGroup.members.Length) + (enemyGroup.members_pre_summon.Length);
                    print("length: " + leng);
                    enPos.AddRange(battlePositionsPlayer[leng - 1]);
                }
                else
                {
                    enPos.AddRange(battlePositionsPlayer[enemyGroup.members.Length - 1]);
                }
                for (int i = 0; i < enemyGroup.members.Length; i++)
                {
                    o_battleCharacter c = enemySlots[i];
                    s_enemyGroup.s_groupMember mem = enemyGroup.members[i];
                    o_battleCharDataN bc = mem.memberDat;
                    c.persistence = true;
                    if (bc.secondMove != null)
                        c.secondMove = bc.secondMove;
                    if (bc.thirdMove != null)
                        c.thirdMove = bc.thirdMove;
                    c.transform.position = new Vector2((enPos[i].x * -1) + 725f, enPos[i].y);
                    c.animHandler.runtimeAnimatorController = bc.anim;
                    c.animHandler.Play("idle");
                    allCharacterReferences.Add(c.referencePoint);
                    SetStatsOpponent(ref c, mem);
                    c.render.color = Color.white;
                }
                if (enemyGroup.members_pre_summon.Length > 0)
                {
                    for (int i = 0; i < enemyGroup.members_pre_summon.Length; i++)
                    {
                        s_enemyGroup.s_groupMember mem = enemyGroup.members_pre_summon[i];
                        o_battleCharacter c = AddSummonable(mem);
                        o_battleCharDataN bc = mem.memberDat;
                        c.persistence = false;
                        if (bc.secondMove != null)
                            c.secondMove = bc.secondMove;
                        if (bc.thirdMove != null)
                            c.thirdMove = bc.thirdMove;
                        c.animHandler.runtimeAnimatorController = bc.anim;
                        c.animHandler.Play("idle");
                        allCharacterReferences.Add(c.referencePoint);
                        c.render.color = Color.white;
                        ReshuffleOpponentPositions();
                    }
                }
            }
            playerCharacters = new List<o_battleCharacter>();
            {
                int charIndex = 0;
                if (nonChangablePlayers)
                {
                    List<Vector2> plPos = battlePositionsPlayer[enemyGroup.members_Player.Length - 1];
                    for (int i = 0; i < enemyGroup.members_Player.Length; i++)
                    {
                        o_battleCharacter c = playerSlots[i];
                        s_enemyGroup.s_groupMember mem = enemyGroup.members_Player[i];
                        o_battleCharDataN bc = mem.memberDat;
                        c.transform.position = plPos[i];
                        if (bc.secondMove != null)
                            c.secondMove = bc.secondMove;
                        if (bc.thirdMove != null)
                            c.thirdMove = bc.thirdMove;
                        c.animHandler.runtimeAnimatorController = bc.anim;
                        c.animHandler.Play("idle");
                        allCharacterReferences.Add(c.referencePoint);
                        SetStatsPlayer(ref c, mem);
                        c.render.color = Color.white;
                        HPGUIMan.SetPartyMember(charIndex, c);
                        charIndex++;
                    }
                }
                else
                {
                    List<Vector2> plPos = battlePositionsPlayer[partyMembers.battleCharList.Count - 1];
                    for (int i = 0; i < partyMembers.battleCharList.Count; i++)
                    {
                        o_battleCharacter c = playerSlots[i];
                        c.render.color = Color.clear;
                        o_battleCharPartyData pbc = partyMembers.GetIndex(i);
                        o_battleCharDataN bc = pbc.characterDataSource;
                        c.transform.position = plPos[i];
                        if (pbc.secondMove != null)
                            c.secondMove = pbc.secondMove;
                        if (pbc.thirdMove != null)
                            c.thirdMove = pbc.thirdMove;
                        c.animHandler.runtimeAnimatorController = bc.anim;
                        c.animHandler.Play("idle");
                        c.inBattle = pbc.inBattle;
                        allCharacterReferences.Add(c.referencePoint);
                        SetStatsPlayer(ref c, pbc);
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
            hasGuest = enemyGroup.guestInvolved;
            if (enemyGroup.guestInvolved)
            {
                o_battleCharacter c = guest;
                s_enemyGroup.s_groupMember mem = enemyGroup.member_Guest;
                o_battleCharDataN bc = mem.memberDat;
                if (bc.secondMove != null)
                    c.secondMove = bc.secondMove;
                if (bc.thirdMove != null)
                    c.thirdMove = bc.thirdMove;
                c.animHandler.runtimeAnimatorController = bc.anim;
                c.animHandler.Play("idle");
                allCharacterReferences.Add(c.referencePoint);
                SetStatsNonChangable(ref c, mem);
                playersReference.Add(c.referencePoint);
                c.render.color = Color.white;
            }
        }
        #endregion

        //combo

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

        StartCoroutine(s_triggerhandler.GetInstance().Fade(Color.clear));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(s_camera.GetInstance().ZoomCamera(-1, 1.5f));
        yield return new WaitForSeconds(0.2f);
        {
            List<o_battleCharacter> bcs = new List<o_battleCharacter>();
            if (isPlayerTurn)
            {
                bcs = playerCharacters;
            }
            else {
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
                    for (int i2 = 0; i2 < bc.turnIcons; i2++)
                    {
                        fullTurn++;
                        StartCoroutine(TurnIconFX(TURN_ICON_FX.APPEAR, i));
                        i++;
                    }
                    StartCoroutine(PlayFadeCharacter(c, new Color(1, 1, 1, 0), Color.white));
                    yield return new WaitForSeconds(0.15f);
                    currentPartyCharactersQueue.Enqueue(c);
                }
            }
        }
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

    public o_battleCharacter AddSummonableRand()
    {
        s_enemyGroup.s_groupMember mem = enemyGroup.members_summonable[UnityEngine.Random.Range(0, enemyGroup.members_summonable.Length)];
        o_battleCharacter c = AddSummonable(mem);
        return c;
    }
    public o_battleCharacter AddSummonable(s_enemyGroup.s_groupMember mem)
    {
        int allyCount = oppositionCharacters.Count;
        o_battleCharacter c = enemySlots[allyCount];
        o_battleCharDataN bc = mem.memberDat;
        if (bc.secondMove != null)
            c.secondMove = bc.secondMove;
        if (bc.thirdMove != null)
            c.thirdMove = bc.thirdMove;
        c.animHandler.runtimeAnimatorController = bc.anim;
        c.animHandler.Play("idle");
        c.persistence = false;
        allCharacterReferences.Add(c.referencePoint);
        SetStatsOpponent(ref c, mem);
        ReshuffleOpponentPositions();
        c.render.color = Color.white;
        currentPartyCharactersQueue.Enqueue(c);
        return c;
    }

    public void ReshuffleOpponentPositions()
    {
        int allyCount = oppositionCharacters.Count;
        List<Vector2> enPos = battlePositionsPlayer[allyCount - 1];
        for (int i = 0; i < oppositionCharacters.Count; i++)
        {
            oppositionCharacters[i].transform.position = new Vector2((enPos[i].x * -1) + 725f, enPos[i].y);
        }
    }

    #region Set stats

    public charAI[] GetAIList(List<s_move> moves)
    {
        charAI[] lists = new charAI[moves.Count];
        for (int i = 0; i < moves.Count; i++)
        {
            s_move mov = moves[i];
            lists[i] = new charAI();
            lists[i].move = mov;
            switch (mov.moveType)
            {
                case s_move.MOVE_TYPE.HP_DAMAGE:
                    lists[i].conditions = charAI.CONDITIONS.ALWAYS;
                    break;

               default:
                    switch (mov.customFunc)
                    {
                        default:
                            switch (mov.moveType) {
                                case s_move.MOVE_TYPE.HP_DRAIN:
                                case s_move.MOVE_TYPE.HP_SP_DRAIN:
                                case s_move.MOVE_TYPE.SP_DRAIN:
                                case s_move.MOVE_TYPE.HP_RECOVER:
                                case s_move.MOVE_TYPE.SP_RECOVER:
                                    lists[i].isImportant = true;
                                    lists[i].onParty = true;
                                    lists[i].conditions = charAI.CONDITIONS.USER_PARTY_HP_LOWER;
                                    lists[i].healthPercentage = 0.5f;
                                    break;
                            }
                            break;

                        case "callAlly":
                        case "callAllies":
                            lists[i].onParty = true;
                            lists[i].conditions = charAI.CONDITIONS.ALWAYS;
                            break;
                    }
                    break;
            }
        }
        return lists;
    }

    public void SetStatsNonChangable(ref o_battleCharacter charObj, s_enemyGroup.s_groupMember mem) {
        int tempLvl = 1;

        if (mem.levType == s_enemyGroup.s_groupMember.LEVEL_TYPE.FIXED)
            tempLvl = mem.level;
        else
            tempLvl = UnityEngine.Random.Range(mem.level, mem.maxLevel + 1);
        o_battleCharDataN enem = mem.memberDat;

        if (mem.memberDat.secondMove != null)
            charObj.secondMove = mem.memberDat.secondMove;
        if (mem.memberDat.thirdMove != null)
            charObj.thirdMove = mem.memberDat.thirdMove;

        int tempHP = enem.maxHitPointsB;
        int tempSP = enem.maxSkillPointsB;

        charObj.name = enem.name;
        charObj.level = tempLvl;
        charObj.battleCharData = enem;

        {
            int tempHPMin = enem.maxHitPointsGMin;
            int tempSPMin = enem.maxSkillPointsGMin;
            int tempHPMax = enem.maxHitPointsGMax;
            int tempSPMax = enem.maxSkillPointsGMax;

            int tempStr = enem.strengthB;
            int tempVit = enem.vitalityB;
            int tempDx = enem.dexterityB;
            int tempLuc = enem.luckB;
            int tempAgi = enem.agilityB;
            int tempInt = enem.intelligenceB;

            for (int i = 1; i < tempLvl; i++)
            {
                //print("Level "  + i + " name: " + enem.name);
                tempHP += UnityEngine.Random.Range(tempHPMin, tempHPMax);
                tempSP += UnityEngine.Random.Range(tempSPMin, tempSPMax);
                //print("HP " + tempHP + " name: " + enem.name);

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
            charObj.intelligence = tempInt;
            charObj.luck = tempLuc;
            charObj.agility = tempAgi;

            charObj.health = charObj.maxHealth = tempHP;
            charObj.stamina = charObj.maxStamina = tempSP;
        }
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
        List<s_move> totalMoves = new List<s_move>();
        totalMoves.AddRange(charObj.currentMoves);
        totalMoves.AddRange(charObj.extraSkills);
        charObj.referencePoint.characterData = rpgManager.SetPartyCharacterStats(charObj);

        charObj.character_AI.ai = GetAIList(totalMoves);
        charObj.elementals = enem.GetElements;
        charObj.inBattle = true;
    }

    public void SetStatsOpponent(ref o_battleCharacter charObj, s_enemyGroup.s_groupMember mem)
    {
        SetStatsNonChangable(ref charObj, mem);
        enemiesReference.Add(charObj.referencePoint);
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
        charObj.level = enem.level;
        charObj.health = charObj.maxHealth = tempHP;
        charObj.stamina = charObj.maxStamina = tempSP;
        charObj.vitality = tempVit;
        charObj.dexterity = tempDx;
        charObj.strength = tempStr;
        charObj.agility = tempAgi;
        charObj.battleCharData = enem.characterDataSource;
        charObj.referencePoint.characterData = enem;
        charObj.currentMoves = new List<s_move>();
        charObj.extraSkills = new List<s_move>();
        charObj.currentMoves.AddRange(enem.currentMoves);
        charObj.extraSkills.AddRange(enem.extraSkills);

        playersReference.Add(charObj.referencePoint);
        playerCharacters.Add(charObj);
    }

    public void SetStatsPlayer(ref o_battleCharacter charObj, s_enemyGroup.s_groupMember mem)
    {
        SetStatsNonChangable(ref charObj, mem);
        playersReference.Add(charObj.referencePoint);
        playerCharacters.Add(charObj);
    }
    #endregion

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

    public IEnumerator DamageAnimation(int dmg, o_battleCharacter targ, string dmgType) {

        switch (currentMove.move.moveType) {
            case s_move.MOVE_TYPE.HP_DAMAGE:
            case s_move.MOVE_TYPE.SP_DAMAGE:
            case s_move.MOVE_TYPE.HP_SP_DAMAGE:
            case s_move.MOVE_TYPE.HP_DRAIN:
            case s_move.MOVE_TYPE.SP_DRAIN:
            case s_move.MOVE_TYPE.HP_SP_DRAIN:
                targ.health -= dmg;
                targ.health = Mathf.Clamp(targ.health, 0, targ.maxHealth);

                Vector2 characterPos = targ.transform.position;
                if (oppositionCharacters.Contains(targ))
                {
                    SpawnDamageObject(dmg, characterPos, true, Color.white, dmgType);
                }
                else
                {
                    SpawnDamageObject(dmg, characterPos, false, targ.battleCharData.characterColour, dmgType);
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
                break;

            case s_move.MOVE_TYPE.SP_RECOVER:
            case s_move.MOVE_TYPE.HP_RECOVER:
                targ.health += dmg;
                targ.health = Mathf.Clamp(targ.health, 0, targ.maxHealth);
                break;
        }
    }

    public IEnumerator DodgeAnimation(o_battleCharacter targ, Vector2 characterPos) {

        SpawnDamageObject(0, characterPos, Color.white, "miss_attack");
        float desirableTime = 0.07f;
        float elapsed = 0f;
        float percentage = 0;
        float distance = 55;
        Vector2 targPos = characterPos;
        if (playerCharacters.Contains(targ) || targ == guest)
        {
            targPos = characterPos - new Vector2(distance, 0);
        }
        else
        {
            targPos = characterPos + new Vector2(distance, 0);
        }
        while (percentage < 1)
        {
            elapsed += Time.deltaTime;
            percentage = elapsed / desirableTime;
            targ.transform.position = Vector2.Lerp(characterPos, targPos, Mathf.SmoothStep(0, 1, percentage));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(0.15f);
        desirableTime = 0.15f;
        percentage = 0; 
        elapsed = 0f;
        while (percentage < 1)
        {
            elapsed += Time.deltaTime;
            percentage = elapsed / desirableTime;
            targ.transform.position = Vector2.Lerp(targPos , characterPos, Mathf.SmoothStep(0, 1, percentage));
            yield return new WaitForSeconds(Time.deltaTime);
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
                                    start = user.transform.position;
                                    break;
                                //If the move was random this sets the target
                                case s_actionAnim.MOTION.TO_TARGET:
                                    start = targ.transform.position;
                                    break;
                                    /*
                                case s_actionAnim.MOTION.USER_2:
                                    start = ReferenceToCharacter(currComb.user2).transform.position;
                                    break;
                                    */
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
                                StartCoroutine(PlayFadeCharacter(currentCharacterObject, an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.TO_TARGET:
                                StartCoroutine(PlayFadeCharacter(targetCharacterObject, an.startColour, an.endColour));
                                break;
                                /*
                            case s_actionAnim.MOTION.USER_2:
                                StartCoroutine(PlayFadeCharacter(ReferenceToCharacter(currComb.user2), an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.USER_3:
                                StartCoroutine(PlayFadeCharacter(ReferenceToCharacter(currComb.user3), an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.USER_4:
                                StartCoroutine(PlayFadeCharacter(ReferenceToCharacter(currComb.user4), an.startColour, an.endColour));
                                break;
                            case s_actionAnim.MOTION.USER_5:
                                StartCoroutine(PlayFadeCharacter(ReferenceToCharacter(currComb.user5), an.startColour, an.endColour));
                                break;
                                */
                        }

                        yield return new WaitForSeconds(an.time);
                        break;

                    case s_actionAnim.ACTION_TYPE.MOVE_CAMERA:

                        switch (an.goal)
                        {
                            case s_actionAnim.MOTION.ALL_SELF:
                                {
                                    /*
                                    List<Vector2> allPositions = new List<Vector2>();
                                    foreach (var v in AllTargets(s_move.MOVE_TARGET.ALLY))
                                    {
                                        allPositions.Add(v.transform.position);
                                    }
                                    StartCoroutine(s_camera.cam.MoveCamera(s_camera.cam.GetCentroid(allPositions), 0.9f));
                                    */
                                }
                                break;
                            case s_actionAnim.MOTION.ALL_TARGET:
                                {
                                    /*
                                    List<Vector2> allPositions = new List<Vector2>();
                                    foreach (var v in AllTargets(s_move.MOVE_TARGET.ENEMY))
                                    {
                                        allPositions.Add(v.transform.position);
                                    }
                                    StartCoroutine(s_camera.cam.MoveCamera(s_camera.cam.GetCentroid(allPositions), 0.9f));
                                    */
                                }
                                break;

                            case s_actionAnim.MOTION.TO_TARGET:
                                /*
                                if (an.teleport)
                                    s_camera.cam.TeleportCamera(targ.transform.position);
                                else
                                    StartCoroutine(s_camera.cam.MoveCamera(targ.transform.position, 0.9f));
                                */
                                break;

                            case s_actionAnim.MOTION.SELF:
                                /*
                                if (an.teleport)
                                    s_camera.cam.TeleportCamera(user.transform.position);
                                else
                                    StartCoroutine(s_camera.cam.MoveCamera(user.transform.position, 0.9f));
                                */
                                break;

                            case s_actionAnim.MOTION.USER_2:
                                /*
                                if (an.teleport)
                                    s_camera.cam.TeleportCamera(user.transform.position);
                                else
                                    StartCoroutine(s_camera.cam.MoveCamera(ReferenceToCharacter(currComb.user2).transform.position, 0.9f));
                                */
                                break;
                        }
                        yield return new WaitForSeconds(an.time);
                        break;

                    case s_actionAnim.ACTION_TYPE.ZOOM_CAMERA:

                        /*
                        switch (an.goal)
                        {
                            case s_actionAnim.MOTION.USER_2:
                                StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom,
                                    ReferenceToCharacter(currComb.user2).transform.position, 0.9f));
                                break;

                            case s_actionAnim.MOTION.USER_3:
                                StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom,
                                    ReferenceToCharacter(currComb.user3).transform.position, 0.9f));
                                break;

                            case s_actionAnim.MOTION.USER_4:
                                StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom,
                                    ReferenceToCharacter(currComb.user4).transform.position, 0.9f));
                                break;
                                
                            case s_actionAnim.MOTION.USER_5:
                                StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom,
                                    ReferenceToCharacter(currComb.user5).transform.position, 0.9f));
                                break;

                            case s_actionAnim.MOTION.ALL_SELF:
                                {
                                    List<Vector2> allPositions = new List<Vector2>();
                                    foreach (var v in AllTargets(s_move.MOVE_TARGET.ALLY))
                                    {
                                        allPositions.Add(v.transform.position);
                                    }
                                    StartCoroutine(s_camera.GetInstance().ZoomCamera(an.toZoom, s_camera.GetInstance().GetCentroid(allPositions), 0.9f));
                                }
                                break;

                            case s_actionAnim.MOTION.ALL_TARGET:
                                {
                                    List<Vector2> allPositions = new List<Vector2>();
                                    foreach (var v in AllTargets(s_move.MOVE_TARGET.ENEMY))
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
                        */
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
                                currentCharacterObject.transform.position = originalPos;
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
                                    start = user.transform.position;
                                    break;
                                //If the move was random this sets the target
                                case s_actionAnim.MOTION.TO_TARGET:
                                    start = targ.transform.position;
                                    break;

                                    /*
                                case s_actionAnim.MOTION.USER_2:
                                    start = ReferenceToCharacter(currComb.user2).transform.position;
                                    break;
                                    */
                            }

                            switch (an.goal)
                            {
                                /*
                                case s_actionAnim.MOTION.USER_2:
                                    end = ReferenceToCharacter(currComb.user2).transform.position;
                                    break;

                                case s_actionAnim.MOTION.USER_3:
                                    end = ReferenceToCharacter(currComb.user3).transform.position;
                                    break;

                                case s_actionAnim.MOTION.USER_4:
                                    end = ReferenceToCharacter(currComb.user4).transform.position;
                                    break;

                                case s_actionAnim.MOTION.USER_5:
                                    end = ReferenceToCharacter(currComb.user5).transform.position;
                                    break;
                                    */
                                case s_actionAnim.MOTION.SELF:
                                    end = user.transform.position;
                                    break;
                                //If the move was random this sets the target
                                case s_actionAnim.MOTION.TO_TARGET:
                                    end = targ.transform.position;
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

    public List<o_battleCharacter> AllTargetsLiving(s_move.MOVE_TARGET scope)
    {
        List<o_battleCharacter> bcs = AllTargets(scope);
        return bcs.FindAll(x => x.health > 0);
    }

    public List<o_battleCharacter> AllTargets(s_move.MOVE_TARGET scope) {
        List<o_battleCharacter> bcs = new List<o_battleCharacter>();
        if (isPlayerTurn)
        {
            switch (scope) {
                case s_move.MOVE_TARGET.ALLY:
                    bcs.AddRange(playerCharacters);
                    if (hasGuest)
                    {
                        bcs.Add(guest);
                    }
                    break;
                case s_move.MOVE_TARGET.ENEMY:
                    bcs.AddRange(oppositionCharacters);
                    break;
            }
        }
        else
        {
            switch (scope)
            {
                case s_move.MOVE_TARGET.ALLY:
                    bcs.AddRange(oppositionCharacters);
                    break;
                case s_move.MOVE_TARGET.ENEMY:
                    bcs.AddRange(playerCharacters);
                    if (hasGuest)
                    {
                        bcs.Add(guest);
                    }
                    break;
            }
        }
        if (scope == s_move.MOVE_TARGET.ENEMY_ALLY)
        {
            bcs.AddRange(playerCharacters);
            if (hasGuest)
            {
                bcs.Add(guest);
            }
            bcs.AddRange(oppositionCharacters);
        }
        return bcs;
    }

    public IEnumerator DisplayMoveName(string moveName)
    {
        displayMoveName.img.gameObject.SetActive(true);
        displayMoveName.buttonText.gameObject.SetActive(true);

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
        displayMoveName.img.gameObject.SetActive(false);
        displayMoveName.buttonText.gameObject.SetActive(false);
    }
    public IEnumerator ExcecuteMove()
    {
        s_move mov = currentMove.move;
        o_battleCharacter user = ReferenceToCharacter(currentCharacter.characterRef);
        o_battleCharacter targ = ReferenceToCharacter(targetCharacter.characterRef);
        s_actionAnim[] preAnimations = null;
        s_actionAnim[] animations = null;
        s_actionAnim[] endAnimations = null;

        /*
        if (currComb.comboType == s_move.MOVE_QUANITY_TYPE.MONO_TECH)
        {
            for (int i = 0; i < 5; i++)
            {
                o_battleCharacter bc = null;
                s_move combMov = null;
                switch (i)
                {
                    case 0:
                        bc = ReferenceToCharacter(currComb.user1);
                        combMov = currComb.user1Move;
                        break;

                    case 1:
                        bc = ReferenceToCharacter(currComb.user2);
                        combMov = currComb.user2Move;
                        break;

                    case 2:
                        bc = ReferenceToCharacter(currComb.user3);
                        combMov = currComb.user3Move;
                        break;

                    case 3:
                        bc = ReferenceToCharacter(currComb.user4);
                        combMov = currComb.user4Move;
                        break;

                    case 4:
                        bc = ReferenceToCharacter(currComb.user5);
                        combMov = currComb.user5Move;
                        break;
                }

                if (bc == null)
                    continue;
                if (combMov.element.isMagic)
                {
                    bc.stamina -= combMov.cost;
                }
                else
                {
                    bc.health -= combMov.cost;
                }
            }
        }
        else
        {
        }
        */
        if (mov.element.isMagic)
        {
            user.stamina -= mov.cost;
        }
        else
        {
            user.health -= mov.cost;
        }
        animations = mov.animations;

        #region NOTIFICATION
        if (playerCharacters.Contains(currentCharacterObject) || currentCharacterObject == guest)
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
        yield return StartCoroutine(DisplayMoveName(mov.name));
        #endregion

        if (!mov.consumeTurn)
        {
            finalDamageFlag = DAMAGE_FLAGS.PASS;
            //If there are no full turn icons start taking away instead of turning full icons into half
            /*
            if (fullTurn > 0)
            {
                s_soundmanager.GetInstance().PlaySound("weakness_smtIV");
                HitWeakness();
                yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.HIT, netTurn - halfTurn));
            }
            else
            {
                HitWeakness();
                yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
            }
            */
        }

        #region PRE ANIM
        preAnimations = currentMove.move.preAnimations;
        if (preAnimations != null)
        {
            if (preAnimations.Length > 0)
                yield return StartCoroutine(PlayAttackAnimation(preAnimations, null, currentCharacterObject));
        }

        currentCharacterObject.SwitchAnimation("idle");

        yield return new WaitForSeconds(0.3f);
        #endregion

        #region MAIN ANIM
        /*
        switch (battleAction.type)
        {
            case s_battleAction.MOVE_TYPE.ITEM:
            case s_battleAction.MOVE_TYPE.MOVE:
                break;

            case s_battleAction.MOVE_TYPE.GUARD:
                currentCharacterObject.guardPoints++;
                int spRestTurn = Mathf.CeilToInt((float)currentCharacterObject.maxStamina * UnityEngine.Random.Range(0.05f, 0.12f));
                currentCharacterObject.stamina += spRestTurn;
                currentCharacterObject.stamina = Mathf.Clamp(currentCharacterObject.stamina, 0, currentCharacterObject.maxStamina);
                NextTurn();
                yield return StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                break;
        }
        */
        if (animations != null)
        {
            switch (currentMove.move.moveTargScope)
            {
                case s_move.SCOPE_NUMBER.ONE:
                    yield return StartCoroutine(PlayAttackAnimation(animations, targ, currentCharacterObject));
                    break;

                case s_move.SCOPE_NUMBER.ALL:
                    {
                        List<o_battleCharacter> bcs = AllTargets(currentMove.move.moveTarg);
                        print(bcs.Count);
                        foreach (var b in bcs)
                        {
                            yield return StartCoroutine(PlayAttackAnimation(animations, b, currentCharacterObject));
                        }
                    }
                    break;

                case s_move.SCOPE_NUMBER.RANDOM:
                    {
                        List<o_battleCharacter> bcs = AllTargets(currentMove.move.moveTarg);
                        int rand = UnityEngine.Random.Range(2, 5);
                        for (int i = 0; i < rand; i++)
                        {
                            bcs = AllTargetsLiving(currentMove.move.moveTarg);
                            if (bcs.Count == 0)
                                break;
                            o_battleCharacter bc = bcs[UnityEngine.Random.Range(0, bcs.Count)];
                            yield return StartCoroutine(PlayAttackAnimation(animations, bc, currentCharacterObject));
                        }
                    }
                    break;
            }
        }
        else
        {

        }

        currentCharacterObject.SwitchAnimation("idle");

        #region PRESS TURN STUFF
        /*
        int numOfTimes = 1;
        switch (currComb.comboType)
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
        */

        if (currentCharacter != guest)
        {
            switch (finalDamageFlag)
            {
                case DAMAGE_FLAGS.NONE:
                    NextTurn();
                    StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                    break;
                case DAMAGE_FLAGS.MISS:
                case DAMAGE_FLAGS.VOID:
                    NextTurn();
                    StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                    if (netTurn > 0)
                    {
                        NextTurn();
                        StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                    }
                    break;
                case DAMAGE_FLAGS.PASS:
                case DAMAGE_FLAGS.FRAIL:
                case DAMAGE_FLAGS.LUCKY:
                case DAMAGE_FLAGS.CRITICAL:
                    //If there are no full turn icons start taking away instead of turning full icons into half
                    if (fullTurn > 0)
                    {
                        s_soundmanager.GetInstance().PlaySound("weakness_smtIV");
                        HitWeakness();
                        StartCoroutine(TurnIconFX(TURN_ICON_FX.HIT, netTurn - halfTurn));
                    }
                    else
                    {
                        HitWeakness();
                        StartCoroutine(TurnIconFX(TURN_ICON_FX.FADE, netTurn));
                    }
                    break;
                case DAMAGE_FLAGS.REFLECT:
                case DAMAGE_FLAGS.ABSORB:
                    fullTurn = 0;
                    halfTurn = 0;
                    break;
            }
        }
        else
        {
            NextTurn();
        }
        //if (netTurn == 0)
         //   break;
        #endregion

        yield return new WaitForSeconds(0.3f);
        #endregion

        #region END ANIM
        endAnimations = currentMove.move.endAnimations;
        if (endAnimations != null)
        {
            if (endAnimations.Length > 0)
                yield return StartCoroutine(PlayAttackAnimation(endAnimations, null, currentCharacterObject));
        }
        currentCharacterObject.SwitchAnimation("idle");

        yield return new WaitForSeconds(0.3f);
        #endregion

        yield return StartCoroutine(CheckStatusEffectAfterAction());
        yield return StartCoroutine(CheckCutscene());
        finalDamageFlag = DAMAGE_FLAGS.NONE;
        battleEngine = BATTLE_ENGINE_STATE.END;
    }
    public IEnumerator CheckCutscene()
    {
        //s_camera.cam.SetZoom();
        yield return new WaitForSeconds(0.1f);
    }
    public IEnumerator CheckStatusEffectAfterAction()
    {
        //s_camera.cam.SetZoom();
        foreach (s_statusEff eff in currentCharacterObject.statusEffects) {
            switch (eff.status.variableChange) {
                case S_StatusEffect.VARIABLE_CHANGE.HP_REGEN:
                case S_StatusEffect.VARIABLE_CHANGE.SP_REGEN:
                case S_StatusEffect.VARIABLE_CHANGE.HP_SP_REGEN:
                    if (eff.status.regenPercentage > 0)
                    {
                        //StartCoroutine(DamageAnimation(eff.damage, currentCharacterObject, ""));
                    }
                    else
                    {
                        StartCoroutine(DamageAnimation(eff.damage, currentCharacterObject, ""));
                    }
                    break;
            }
            eff.duration--;
            /*
            switch (eff.status) {
                case STATUS_EFFECT.POISON:
                    //We'll have some stat calculations as if this status effect is damage, there would be some kind of formula.
                    StartCoroutine(DamageAnimation(eff.damage, currentCharacterObject, ""));
                    eff.duration--;
                    break;

                case STATUS_EFFECT.BURN:
                    yield return StartCoroutine(s_camera.GetInstance().MoveCamera(currentCharacterObject.transform.position, 0.6f));
                    if (enemiesReference.ListContains(currentCharacter.characterRef))
                    {
                        s_soundmanager.GetInstance().PlaySound("hurt_burn");
                    }
                    else
                    {
                        s_soundmanager.GetInstance().PlaySound("pl_dmg");
                    }
                    //We'll have some stat calculations as if this status effect is damage, there would be some kind of formula.
                    StartCoroutine(DamageAnimation(
                        eff.damage
                        , currentCharacterObject, ""));
                    eff.duration--;
                    break;
            }
            */
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
    public void SpawnDamageObject(int dmg, Vector2 characterPos, bool enemy, Color colour, string dmgType)
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
        ob.PlayAnim(dmg, enemy, colour, dmgType);
    }
    #endregion

    #region Press Turn Stuff
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
        if (fullTurn < 0)
            fullTurn = 0;
    }

    #endregion

    #region Battle system core
    void Update()
    {
        if (isEnabled)
        {
            switch (battleEngine) {
                case BATTLE_ENGINE_STATE.SELECT_CHARACTER:
                    currentCharacterObject = currentPartyCharactersQueue.Peek();
                    currentCharacter.SetCharacter(currentCharacterObject.referencePoint);
                    if (currentCharacter.characterRef.health <= 0 //|| !currentCharacter.inBattle
                        ) {
                        currentPartyCharactersQueue.Dequeue();
                        if(currentCharacterObject.persistence)
                            currentPartyCharactersQueue.Enqueue(currentCharacterObject);
                        return;
                    }
                    List<s_statusEff> statusEffects = currentCharacterObject.statusEffects;
                    if (statusEffects != null)
                    {
                        foreach (s_statusEff eff in statusEffects)
                        {
                            switch (eff.status.restriction) {
                                case S_StatusEffect.RESTRICTION.CANNOT_MOVE:
                                    currentMove.SetMove(nothingMove);
                                    targetCharacter.SetCharacter(currentCharacter.characterRef);
                                    EndAction();
                                    break;

                                case S_StatusEffect.RESTRICTION.RANDOM_ALL:
                                    currentMove.SetMove(currentCharacterObject.GetRandomMove);
                                    targetCharacter.SetCharacter(AllCharacters[UnityEngine.Random.Range(0, AllCharacters.Count)].referencePoint);
                                    EndAction();
                                    break;
                                case S_StatusEffect.RESTRICTION.RANDOM_ALLY:
                                    currentMove.SetMove(currentCharacterObject.GetRandomMove);
                                    if (oppositionCharacters.Contains(currentCharacterObject))
                                    {
                                        targetCharacter.SetCharacter(AllCharacters[UnityEngine.Random.Range(0, oppositionCharacters.Count)].referencePoint);
                                    }
                                    else
                                    {
                                        targetCharacter.SetCharacter(AllCharacters[UnityEngine.Random.Range(0, playerCharacters.Count)].referencePoint);
                                    }
                                    EndAction();
                                    break;

                                case S_StatusEffect.RESTRICTION.RANDOM_FOE:
                                    currentMove.SetMove(currentCharacterObject.GetRandomMove);
                                    if (oppositionCharacters.Contains(currentCharacterObject))
                                    {
                                        targetCharacter.SetCharacter(AllCharacters[UnityEngine.Random.Range(0, playerCharacters.Count)].referencePoint);
                                    }
                                    else
                                    {
                                        targetCharacter.SetCharacter(AllCharacters[UnityEngine.Random.Range(0, oppositionCharacters.Count)].referencePoint);
                                    }
                                    EndAction();
                                    break;
                            }
                            /*
                            switch (eff.status)
                            {
                                case STATUS_EFFECT.CONFUSED:
                                    break;

                                case STATUS_EFFECT.STUN:
                                    //TODO: make it so that the character's turn is immediatley cancelled.
                                    if (eff.duration % 2 == 1)
                                    {
                                        //currentCharacterObject
                                        currentMove.SetMove(nothingMove);
                                        battleAction.isCombo = false;
                                        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
                                        targetCharacter.SetCharacter(currentCharacter.characterRef);
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
                            */
                            if (eff.duration == 0)
                                currentCharacterObject.statusEffects.Remove(eff);
                        }
                    }
                    break;
            }
            if (isPlayerTurn)
            {
                if(currentCharacter != guest)
                    PlayerTurn();
                else
                    AITurn();
            }
            else
            {
                AITurn();
            }
        }
    }
    public void AITurn()
    {
        List<o_battleCharacter> allies = new List<o_battleCharacter>();
        List<o_battleCharacter> baddies = new List<o_battleCharacter>();
        if (currentCharacter != guest)
        {
            allies.AddRange(oppositionCharacters);
            baddies.AddRange(playerCharacters.FindAll(x => x.inBattle == true && x.health > 0));
            if (hasGuest)
                baddies.Add(guest);
        }
        else
        {
            allies.AddRange(playerCharacters);
            if (hasGuest)
                allies.Add(guest);
            baddies.AddRange(oppositionCharacters.FindAll(x => x.inBattle == true && x.health > 0));
        }

        switch (battleEngine)
        {
            case BATTLE_ENGINE_STATE.NONE:

                break;

            case BATTLE_ENGINE_STATE.SELECT_CHARACTER:
                battleEngine = BATTLE_ENGINE_STATE.DECISION;
                break;

            case BATTLE_ENGINE_STATE.DECISION:
            case BATTLE_ENGINE_STATE.TARGET:

                bool fufilled = false;
                bool notAlways = false;

                List<s_move> alwaysMoves = new List<s_move>();

                foreach (charAI ai in  currentCharacterObject.character_AI.ai)
                {
                    o_battleCharacter potentialTrg = null;
                    s_move move = ai.move;
                    if (move.element.isMagic)
                    {
                        switch (move.customFunc)
                        {
                            case "callAlly":
                            case "callAllies":
                                if (oppositionCharacters.Count >= 4)
                                {
                                    continue;
                                }
                                break;
                        }
                        if (currentCharacterObject.stamina < move.cost)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (currentCharacterObject.health <= move.cost)
                        {
                            continue;
                        }
                    }
                    if (!currentCharacterObject.GetAllMoves.Contains(move)) {
                        continue;
                    }
                    List<o_battleCharacter> targets = new List<o_battleCharacter>();
                    switch (ai.move.moveTarg) {
                        case s_move.MOVE_TARGET.ALLY:
                            targets = allies;
                            break;
                        case s_move.MOVE_TARGET.ENEMY:
                            targets = baddies;
                            break;

                        case s_move.MOVE_TARGET.ENEMY_ALLY:
                            targets.AddRange(allies);
                            targets.AddRange(baddies);
                            break;

                        case s_move.MOVE_TARGET.SELF:

                            break;
                    }

                    currentMove.SetMove(ai.move);
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
                            print(move.name);
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
                        targetCharacter.SetCharacter(potentialTrg.referencePoint);
                    }
                }
                if (!fufilled)
                {
                    List<o_battleCharacter> targets = new List<o_battleCharacter>();
                    targets = baddies;
                    targetCharacter.SetCharacter(targets[UnityEngine.Random.Range(0, targets.Count)].referencePoint);
                    if(currentCharacterObject.physWeapon != null)
                        currentMove.SetMove(currentCharacterObject.physWeapon);
                    else
                        currentMove.SetMove(defaultAttack);
                }
                else if(!notAlways)
                {
                    currentMove.SetMove(alwaysMoves[UnityEngine.Random.Range(0, alwaysMoves.Count - 1)]);
                    List<o_battleCharacter> targets = new List<o_battleCharacter>();
                    targets = AllTargetsLiving(currentMove.move.moveTarg);
                    targetCharacter.SetCharacter(targets[UnityEngine.Random.Range(0, targets.Count)].referencePoint);
                }
                //battleAction.isCombo = battleAction.combo.comboType != s_move.MOVE_QUANITY_TYPE.MONO_TECH;
                battleEngine = BATTLE_ENGINE_STATE.PROCESS_ACTION;
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
                changeMenu.RaiseEvent("MainBattleMenu");
                //s_menuhandler.GetInstance().GetMenu<s_mainBattleMenu>("BattleMenu").battleCharacter = currentCharacter;
                //s_menuhandler.GetInstance().SwitchMenu("BattleMenu");
                //StartCoroutine(CheckStatusEffectBeforeAction());
                //s_camera.cam.TeleportCamera(currentCharacterObject.transform.position);
                battleDecisionMenu = BATTLE_MENU_CHOICES.MENU;
                battleEngine = BATTLE_ENGINE_STATE.DECISION;
                break;

            case BATTLE_ENGINE_STATE.END:
                StartCoroutine(NextTeamTurn());
                battleEngine = BATTLE_ENGINE_STATE.NONE;
                break;
        }
    }
    public List<CH_BattleChar> GetTargets(s_move.MOVE_TARGET scope)
    {
        List<CH_BattleChar> targs = new List<CH_BattleChar>();
        switch (scope) {
            case s_move.MOVE_TARGET.ALLY:
                for (int i = 0; i < playerCharacters.Count; i++)
                {
                    targs.Add(playerCharacters[i].referencePoint);
                }
                break;

            case s_move.MOVE_TARGET.ENEMY:
                for (int i = 0; i < oppositionCharacters.Count; i++)
                {
                    if (oppositionCharacters[i].health > 0)
                        targs.Add(oppositionCharacters[i].referencePoint);
                }
                break;

            case s_move.MOVE_TARGET.ENEMY_ALLY:

                for (int i = 0; i < playerCharacters.Count; i++)
                {
                    targs.Add(playerCharacters[i].referencePoint);
                }
                for (int i = 0; i < oppositionCharacters.Count; i++)
                {
                    if (oppositionCharacters[i].health > 0)
                        targs.Add(oppositionCharacters[i].referencePoint);
                }
                break;
        }
        return targs;
    }
    public void SetTargets(List<CH_BattleChar> targs)
    {
        targetCharacters.characterListRef = targs;
    }

    public void SetTargets(bool onParty) {
        targetCharacters.Clear();
        if (!onParty)
        {
            //targetObj.SetActive(true);
            //MenuControl(oppositionCharacters.Count, MENU_CONTROLL_TYPE.LINEAR_UP_DOWN, new Vector2(0, 0));
            for (int i = 0; i < oppositionCharacters.Count; i++)
            {
                if (oppositionCharacters[i].health > 0)
                    targetCharacters.Add(oppositionCharacters[i].referencePoint);
            }
        }
        else
        {
            for (int i = 0; i < playerCharacters.Count; i++)
            {
                targetCharacters.Add(playerCharacters[i].referencePoint);
            }
            if (hasGuest)
                targetCharacters.Add(guest.referencePoint);
        }
    }

    public void RearangeTurnOrder() {
        List<o_battleCharacter> bcs = playerCharacters.FindAll(x => x.inBattle);
        currentPartyCharactersQueue.Clear();
        int ind = bcs.IndexOf(currentCharacterObject);
        int initInd = bcs.IndexOf(currentCharacterObject);
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

    public void SelectTarget(CH_BattleChar bc) {
        targetCharacter.SetCharacter(bc);
        EndAction();
    }
    public void EndAction() {
        battleEngine = BATTLE_ENGINE_STATE.PROCESS_ACTION;
        StartCoroutine(ExcecuteMove());
    }

    public void GuardOption()
    {
        currentMove.SetMove(guard);
        s_menuhandler.GetInstance().SwitchMenu("EMPTY");
        battleEngine = BATTLE_ENGINE_STATE.END;
        menuchoice = 0;
    }

    public void SelectSkillOptionGuard()
    {
        currentMove.SetMove(guard);
        s_menuhandler.GetInstance().SwitchMenu("EMPTY");
        EndAction();
    }

    /*
    public void SelectSkillRangedOption()
    {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        currentMove.move = currentCharacterObject.rangedWeapon;
        battleAction.isCombo = false;
        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        menuchoice = 0;
    }
    public void SelectSkillOption()
    {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        if(currentCharacterObject.physWeapon == null)
            currentMove.SetMove(defaultAttack);
        else
            currentMove.SetMove(currentCharacterObject.physWeapon);
        battleAction.isCombo = false;
        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        menuchoice = 0;
    }
    public void SelectSkillOptionNew()
    {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        currentMove.SetMove(currentMove.move);
        battleAction.isCombo = false;
        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        changeMenu.RaiseEvent("TargetMenu");
        //menuchoice = 0;
    }

    public void SelectSkillOption(s_move move, s_moveComb combo)
    {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        battleAction.isCombo = true;
        battleAction.combo = combo;
        currentMove.SetMove(move); 
        GetTargets(move.moveTarg);
        //s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").bcs = 
        s_menuhandler.GetInstance().SwitchMenu("TargetMenu");
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        menuchoice = 0;
    }
    public void SelectSkillOption(s_move move) {
        battleAction.type = s_battleAction.MOVE_TYPE.MOVE;
        currentMove.SetMove(move);
        battleAction.isCombo = false;
        battleAction.combo.comboType = s_move.MOVE_QUANITY_TYPE.MONO_TECH;
        SetTargets(move.onParty);
        //s_menuhandler.GetInstance().GetMenu<s_targetMenu>("TargetMenu").bcs = GetTargets(move.onParty);
        if (move.moveTarg == s_move.MOVE_TARGET.SINGLE)
        {
            switch (move.element)
            {
                case ELEMENT.STRIKE:
                case ELEMENT.FIRE:
                    {
                        List<CH_BattleChar> targs = targetCharacters.characterListRef;
                        foreach (CH_BattleChar bc in playersReference.characterListRef)
                        {
                            if (bc.HasStatus(STATUS_EFFECT.FROZEN))
                            {
                                targs.Add(bc);
                            }
                        }
                        SetTargets(targs);
                    }
                    break;

                case ELEMENT.WATER:
                case ELEMENT.ICE:
                    {
                        List<CH_BattleChar> targs = targetCharacters.characterListRef;
                        foreach (CH_BattleChar bc in playersReference.characterListRef)
                        {
                            if (bc.HasStatus(STATUS_EFFECT.BURN))
                            {
                                targs.Add(bc);
                            }
                        }
                        SetTargets(targs);
                    }
                    break;
            }
        }
        s_menuhandler.GetInstance().SwitchMenu("TargetMenu");
        battleEngine = BATTLE_ENGINE_STATE.TARGET;
        menuchoice = 0;
    }
    */

    /// <summary>
    /// Probably for counters or something
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnAttack() {
        if (isPlayerTurn) {

        } else {
            foreach (o_battleCharacter bc in playerCharacters) {
                if (bc.health <= 0) {
                    continue;
                }
                /*
                if (currentMove.move.moveTarg == s_move.MOVE_TARGET.SINGLE) {
                    if (targetCharacterObject.health > 0) {

                    }
                }
                */
            }
        }
        yield return new WaitForSeconds(0.1f);
    }

    /*
    public float GetElementStat(o_battleCharacter user, s_move move) {
        float pow = 0;
        float basePow = 0;
        float elementalPow = 0;


        int str = user.strengthNet;
        int vit = user.vitalityNet;
        int dex = user.dexterityNet;
        int agi = user.agiNet;
        int luc = user.agiNet;
        int intel = user.agiNet;


        switch (move.element)
        {
            case ELEMENT.FIRE:
                elementalPow = str / 2;
                break;

            case ELEMENT.WIND:
                elementalPow = agi / 2;
                break;

            case ELEMENT.ELECTRIC:
                elementalPow = dex / 1.5f;
                break;
        }

        switch (move.moveType) {
            case s_move.MOVE_TYPE.PHYSICAL:
                basePow = str;
                break;
            case s_move.MOVE_TYPE.SPECIAL:
                basePow = intel;
                break;
        }
        ///Put power
        pow = basePow + elementalPow;
        return pow;
    }
    */

    public int CalculateDamage(o_battleCharacter user, o_battleCharacter target, s_move move, List<float> modifiers)
    {
        S_Element el = move.element;
        int dmg = 0;
        if (!move.fixedValue)
        {
            float multipler = 1f;
            float elementals = user.GetElementWeakness(el);
            if (elementals < 0 && elementals > -1)
                multipler = (elementals * -1);
            else if (elementals <= -1)
                multipler = ((elementals + 1) * -1);

            if (modifiers != null)
            {
                foreach (float mod in modifiers)
                {
                    multipler += mod;
                }
            }
            float elementalDamage =
                (user.strengthNet * el.strength) +
                (user.vitalityNet * el.vitality) +
                (user.dexterityNet * el.dexterity) +
                (user.intelligenceNet * el.intelligence) +
                (user.luckNet * el.luck) +
                (user.agiNet * el.agility);
            dmg = (int)((move.power * elementalDamage / (float)target.vitalityNet) * multipler);
        }
        else { dmg = move.power; }
        return dmg;
    }
    
    public void DamageEffect(int dmg, o_battleCharacter target ,Vector2 characterPos, DAMAGE_FLAGS fl) {
        switch (fl)
        {
            default:

                target.health -= dmg;
                if (target.guardPoints > 0)
                    target.guardPoints--;
                break;

            case DAMAGE_FLAGS.MISS:
            case DAMAGE_FLAGS.VOID:
                break;
        }
        target.health = Mathf.Clamp(target.health, 0, target.maxHealth);
        switch (fl)
        {
            case DAMAGE_FLAGS.LUCKY:

                if (oppositionCharacters.Contains(target))
                {
                    s_soundmanager.GetInstance().PlaySound("mal_lucky");
                    s_soundmanager.GetInstance().PlaySound("rpgHit");
                    SpawnDamageObject(dmg, characterPos, true, Color.white, "lucky");
                }
                else
                {
                    s_soundmanager.GetInstance().PlaySound("mal_lucky");
                    s_soundmanager.GetInstance().PlaySound("pl_dmg");
                    SpawnDamageObject(dmg, characterPos, true, Color.white, "lucky");
                    //SpawnDamageObject(dmg, characterPos, false, target.battleCharData.characterColour);
                }
                break;

            case DAMAGE_FLAGS.CRITICAL:

                break;

            case DAMAGE_FLAGS.FRAIL:
            case DAMAGE_FLAGS.NONE:
                if (target != guest)
                {
                    if (oppositionCharacters.Contains(target))
                    {
                        s_soundmanager.GetInstance().PlaySound("rpgHit");
                        SpawnDamageObject(dmg, characterPos, true, Color.white, "");
                    }
                    else
                    {
                        s_soundmanager.GetInstance().PlaySound("pl_dmg");
                        SpawnDamageObject(dmg, characterPos, false, target.battleCharData.characterColour, "");
                    }
                }
                else
                {
                    s_soundmanager.GetInstance().PlaySound("pl_dmg");
                    SpawnDamageObject(dmg, characterPos, false, Color.clear, "");
                }
                break;

            case DAMAGE_FLAGS.VOID:
                SpawnDamageObject(0, characterPos, Color.white, "block");
                break;
            case DAMAGE_FLAGS.MISS:
                SpawnDamageObject(0, characterPos, Color.white, "miss_attack");
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

        if (currentMove.move.moveType == s_move.MOVE_TYPE.HP_DAMAGE) {
            if (targ.health <= 0)
                yield break;
        }
        s_move mov = currentMove.move;
        int dmg = 0;
        List<float> modifier = new List<float>();
        /*
        if (combination.comboType != s_move.MOVE_QUANITY_TYPE.MONO_TECH)
        {
            int leng = 0;

            if (combination.user2 != null)
                leng++;
            if (combination.user3 != null)
                leng++;
            if (combination.user4 != null)
                leng++;
            if (combination.user5 != null)
                leng++;

            int dmg1 = CalculateDamage(currentCharacterObject, targ, combination.user1Move, null);
            int dmg2 = 0;
            int dmg3 = 0;
            int dmg4 = 0;
            int dmg5 = 0;

            if (combination.user2 != null)
                dmg2 = CalculateDamage(ReferenceToCharacter(combination.user2), targ, combination.user2Move, null);
            if (combination.user3 != null)
                dmg3 = CalculateDamage(ReferenceToCharacter(combination.user3), targ, combination.user3Move, null);
            if (combination.user4 != null)
                dmg4 = CalculateDamage(ReferenceToCharacter(combination.user4), targ, combination.user4Move, null);
            if (combination.user5 != null)
                dmg5 = CalculateDamage(ReferenceToCharacter(combination.user5), targ, combination.user5Move, null);
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        dmg += dmg1;
                        break;
                    case 1:
                        if (combination.user2 != null)
                            dmg += dmg2;
                        break;
                    case 2:
                        if (combination.user3 != null)
                            dmg += dmg3;//Mathf.CeilToInt(dmg3 * 0.5f);
                        break;
                    case 3:
                        if (combination.user4 != null)
                            dmg += dmg4;
                        break;
                    case 4:
                        if (combination.user5 != null)
                            dmg += dmg5;
                        break;
                }
            }
        }
        else
        {
        }
        */
        dmg = CalculateDamage(currentCharacterObject, targ, currentMove.move, null);
        Vector2 characterPos = targ.transform.position;
        switch (currentMove.move.moveType)
        {
            #region ATTACK
            case s_move.MOVE_TYPE.HP_DAMAGE:
            case s_move.MOVE_TYPE.HP_DRAIN:
            case s_move.MOVE_TYPE.HP_SP_DAMAGE:
            case s_move.MOVE_TYPE.HP_SP_DRAIN:

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
                        if (currentMove.move.moveTargScope == s_move.SCOPE_NUMBER.ONE)
                        {
                            if (targetCharacter.characterRef.health < dmg && bc.health >
                                CalculateDamage(currentCharacterObject, bc, currentMove.move, null))
                            {
                                if (bc.elementals[currentMove.move.element] >= 2)
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
                        targetCharacter.SetCharacter(sacrifice.referencePoint);
                        dmg = CalculateDamage(currentCharacterObject, targ, currentMove.move, null);
                    }
                }
                #endregion

                #region PRESS TURN STUFF
                {
                    float smirkChance = UnityEngine.Random.Range(0, 1);
                    ELEMENT_WEAKNESS fl = 0;
                    float elementWeakness = targ.GetElementWeakness(mov.element);
                    if (mov.moveType == s_move.MOVE_TYPE.NONE)
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

                    switch (fl)
                    {
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
                    #region CALCULATE CRITICALS

                    foreach (var statusEff in targ.statusEffects)
                    {
                        foreach (var element in statusEff.status.criticalOnHit)
                        {
                            if (element == mov.element)
                            {
                                modifier.Add(0.3f);
                                damageFlag = DAMAGE_FLAGS.CRITICAL;
                            }
                        }
                    }
                    #endregion
                    switch (finalDamageFlag)
                    {
                        case DAMAGE_FLAGS.NONE:
                        case DAMAGE_FLAGS.FRAIL:
                        case DAMAGE_FLAGS.CRITICAL:
                            #region LUCK CHECK
                            {
                                int userLuc = currentCharacterObject.agiNet + 2;
                                int targLuc = targ.agiNet - 1;
                                targLuc = Mathf.Clamp(targLuc, 1, int.MaxValue);
                                int totalLuck = userLuc + targLuc;

                                float luckyChance = ((float)userLuc / (float)totalLuck);
                                luckyChance = Mathf.Clamp(luckyChance, 0, 0.95f);
                                float attackConnect = UnityEngine.Random.Range(0f, 1f);

                                if (attackConnect > luckyChance)
                                {
                                    if (targ.guardPoints == 0)
                                    {
                                        damageFlag = DAMAGE_FLAGS.LUCKY;
                                        finalDamageFlag = DAMAGE_FLAGS.LUCKY;
                                    }
                                    modifier.Add(0.65f);
                                }
                            }
                            #endregion
                            break;
                    }
                }
                dmg = CalculateDamage(targ, currentCharacterObject, currentMove.move, modifier);
                #region AGILITY DODGE CHECK
                {
                    int userAgil = currentCharacterObject.agiNet + 2;
                    int targAgil = targ.agiNet - 1;
                    targAgil = Mathf.Clamp(targAgil, 1, int.MaxValue);
                    int totalAgil = userAgil + targAgil;

                    float attackConnectChance = ((float)userAgil / (float)totalAgil);
                    attackConnectChance = Mathf.Clamp(attackConnectChance, 0, 0.95f);
                    float attackConnect = UnityEngine.Random.Range(0f, 1f);

                    print("user: " + userAgil + " targ: " + targAgil + " dodge: " + attackConnectChance + " total: " + totalAgil);
                    print("targ: " + targ.agiNet + " targ (rigged): " + targAgil);
                    if (attackConnect > attackConnectChance)
                    {
                        damageFlag = DAMAGE_FLAGS.MISS;
                        finalDamageFlag = DAMAGE_FLAGS.MISS;
                        dmg = 0;
                    }
                }

                #endregion
                DamageEffect(dmg, targ, characterPos, damageFlag);
                if (damageFlag != DAMAGE_FLAGS.MISS)
                {
                    if (mov.statusInflictChance != null)
                    {
                        foreach (s_move.statusInflict statusChance in mov.statusInflictChance)
                        {
                            float ch = UnityEngine.Random.Range(0, 1);
                            if (ch <= statusChance.status_inflict_chance)
                            {
                                targetCharacterObject.SetStatus(new s_statusEff(
                                    statusChance.status_effect,
                                    statusChance.duration,
                                    statusChance.damage));
                            }
                        }
                    }
                    #region ELEMENT STATUS
                    foreach (var statusEff in mov.element.statusInflict)
                    {
                        float status_inflict_chance = 0;
                        s_statusEff eff = new s_statusEff();
                        float ch = UnityEngine.Random.Range(0f, 1f);
                        S_StatusEffect status = statusEff.statusEffect;
                        if (ch < statusEff.chance)
                        {
                            if (statusEff.add_remove)
                            {
                                eff.damage = Mathf.CeilToInt(dmg * status.regenPercentage);
                                eff.duration = status.minDuration;
                                eff.status = status;
                                targ.SetStatus(eff);
                            }
                            else {
                                targ.RemoveStatus(status);
                            }
                        }
                    }
                    /*
                    {
                        float status_inflict_chance = 0;
                        s_statusEff eff = new s_statusEff();
                        float ch = UnityEngine.Random.Range(0f, 1f);
                        switch (mov.element)
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
                                eff.damage = Mathf.FloorToInt(CalculateDamage(currentCharacterObject, targ, currentMove.move, null) * 0.15f);
                                eff.status = STATUS_EFFECT.BURN;
                                break;
                        }

                        if (!battleAction.cureStatus)
                        {
                            switch (currentMove.move.element)
                            {
                                case ELEMENT.ELECTRIC:
                                case ELEMENT.ICE:
                                case ELEMENT.PSYCHIC:
                                case ELEMENT.FIRE:
                                    if (ch < status_inflict_chance)
                                    {
                                        if (currentMove.move.element == ELEMENT.ICE)
                                        {
                                            targ.guardPoints = 0;
                                        }
                                        targ.SetStatus(eff);
                                    }
                                    break;
                            }
                        }
                        switch (mov.element)
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
                    */
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
                }
                else
                {
                    yield return StartCoroutine(DodgeAnimation(targ, characterPos));
                    yield return new WaitForSeconds(0.02f);
                }

                #region CHECK FOR COUNTER
                {
                    if (targ.extraPassives.Find
                        (x => x.passiveSkillType == s_passive.PASSIVE_TYPE.COUNTER))
                    {
                        characterPos = currentCharacterObject.transform.position;
                        dmg = CalculateDamage(targ, currentCharacterObject, defaultAttack, null);

                        DamageEffect(dmg, currentCharacterObject, characterPos, DAMAGE_FLAGS.NONE);

                        for (int i = 0; i < 2; i++)
                        {
                            currentCharacterObject.transform.position = characterPos + new Vector2(15, 0);
                            yield return new WaitForSeconds(0.02f);
                            currentCharacterObject.transform.position = characterPos;
                            yield return new WaitForSeconds(0.02f);
                            currentCharacterObject.transform.position = characterPos + new Vector2(-15, 0);
                            yield return new WaitForSeconds(0.02f);
                            currentCharacterObject.transform.position = characterPos;
                            yield return new WaitForSeconds(0.02f);
                        }
                    }
                }
                #endregion

                break;
            #endregion

            #endregion

            #region STATUS
            case s_move.MOVE_TYPE.HP_RECOVER:
                ReferenceToCharacter(targetCharacter.characterRef).health += dmg;
                targetCharacterObject.health = Mathf.Clamp(targetCharacterObject.health,
                    0, targetCharacterObject.maxHealth);
                SpawnDamageObject(dmg, characterPos, Color.white, "heal_hp");
                break;

            case s_move.MOVE_TYPE.SP_RECOVER:

                targ.stamina += dmg;
                targ.stamina = Mathf.Clamp(targ.stamina,
                    0, targ.maxStamina);
                SpawnDamageObject(dmg, characterPos, Color.magenta, "heal_hp");
                break;
                #endregion
        }
        List<o_battleCharacter> targs = new List<o_battleCharacter>();
        switch (mov.customFunc)
        {
            case "callAlly":
                targs.Add(AddSummonableRand());
                break;
            case "callAllies":
                targs.Add(AddSummonableRand());
                targs.Add(AddSummonableRand());
                break;
            default:
                targs.Add(targ);
                break;
        }
        foreach (var ch in targs)
        {
            if (mov.intBuff != 0)
            {
                ch.intelligenceBuff += mov.intBuff;
                /*
                if (targ.intelligenceBuff > 0)
                {
                    SpawnDamageObject(targ.intelligenceBuff, characterPos, Color.blue, "buffVit");
                    yield return new WaitForSeconds(0.05f);
                }
                */
            }
            if (mov.lucBuff != 0)
            {
                ch.luckBuff += mov.lucBuff;
            }
            if (mov.vitBuff != 0)
            {
                ch.vitalityBuff += mov.vitBuff;
            }
            if (mov.strBuff != 0)
            {
                ch.strengthBuff += mov.strBuff;
            }
            if (mov.dexBuff != 0)
            {
                ch.dexterityBuff += mov.dexBuff;
            }
            if (mov.agiBuff != 0)
            {
                ch.agilityBuff += mov.agiBuff;
            }
            if (mov.guardPoints != 0)
            {
                ch.guardPoints += mov.guardPoints;
            }
        }
        //Show Damage output here


        yield return new WaitForSeconds(0.18f);
        if (targ.health <= 0)
        {
            targ.statusEffects.Clear();
            if (oppositionCharacters.Contains(targ))
            {
                s_soundmanager.GetInstance().PlaySound("enemy_defeat");
            }
            else {
                s_soundmanager.GetInstance().PlaySound("player_defeat");
            }
            yield return StartCoroutine(PlayFadeCharacter(targ, Color.black, Color.clear));
            if (oppositionCharacters.Contains(targ) && !targ.persistence) {
                enemiesReference.Remove(targ.referencePoint);
                oppositionCharacters.Remove(targ);
                ReshuffleOpponentPositions();
            }
        }
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

            foreach (o_battleCharacter c in playerCharacters)
            {
                if (c == guest)
                    continue;
                float exp = TotalEXP(c);
                int initailTotal = (int)(exp * 100);
                print(initailTotal);

                //we add the exp and make it so that it checks for a level up
                for (int i = 0; i < initailTotal; i++)
                {
                    c.experiencePoints += 0.01f;
                    print(c.experiencePoints);
                    if (c.experiencePoints >= 1f)
                    {
                        o_battleCharDataN chdat = c.battleCharData;
                        if (i % chdat.strengthGT == 0)
                            c.strength++;
                        if (i % chdat.vitalityGT == 0)
                            c.vitality++;
                        if (i % chdat.dexterityGT == 0)
                            c.dexterity++;
                        if (i % chdat.intelligenceGT == 0)
                            c.intelligence++;
                        if (i % chdat.agilityGT == 0)
                            c.agility++;
                        if (i % chdat.luckGT == 0)
                            c.luck++;
                        c.level++;
                        c.experiencePoints = 0;
                        exp = TotalEXP(c) * (float)((float)i / (float)initailTotal);
                        c.maxHealth += UnityEngine.Random.Range(chdat.maxHitPointsGMin, chdat.maxHitPointsGMax + 1);
                        c.maxStamina += UnityEngine.Random.Range(chdat.maxSkillPointsGMin, chdat.maxSkillPointsGMax + 1);
                        List<s_move> mv2Learn = chdat.moveLearn.FindAll(x => x.MeetsRequirements(c) && !c.currentMoves.Contains(x));
                        if (mv2Learn != null) {
                            c.currentMoves.AddRange(mv2Learn);
                        }
                    }
                    //yield return new WaitForSeconds(Time.deltaTime);
                }
            }

            List<string> extSkillLearn = new List<string>();

            foreach (o_battleCharacter en in oppositionCharacters)
            {
                if (en.health > 0)
                    continue;
                //For now it's going to be a random amount... later it'll be "en.battleCharData.money"
                float moneyGiven = Mathf.Round(UnityEngine.Random.Range(0.05f, 1.25f) * 100.0f) * 0.01f;
                money._float += moneyGiven;
                foreach (s_move mv in en.currentMoves)
                {
                    if (!extraSkills.ListContains(mv))
                    {
                        print("Added skill");
                        extraSkills.AddMove(mv);
                        extSkillLearn.Add(mv.name);
                    }
                }
                if (en.extraSkills != null)
                {
                    foreach (s_move mv in en.extraSkills)
                    {
                        if (!extraSkills.ListContains(mv))
                        {
                            print("Added skill");
                            extraSkills.AddMove(mv);
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
            if (allDefeated & !battleGroupDoneRef.groupList.Contains(enemyGroup)) {
                battleGroupDoneRef.AddGroup(enemyGroup);
                if (enemyGroup.perishIfDone) {
                    battleGroupRef.RemoveGroup(enemyGroup);
                }
                if (enemyGroup.shopItems != null)
                {
                    foreach (var item in enemyGroup.shopItems)
                        shopItems.AddItem(item);
                }
                if (enemyGroup.branches != null)
                {
                    foreach (var battle in enemyGroup.branches)
                        battleGroupRef.AddGroup(battle);
                }
            }
            yield return new WaitForSeconds(2.5f);
            foreach (o_battleCharacter c in playerCharacters)
            {
                c.extraSkills.Clear();
                if (c == guest)
                    continue;
                rpgManager.SetPartyMemberStats(c);
            }
            if (enemyGroup.unlockCharacters.Length > 0)
            {
                foreach (o_battleCharDataN c in enemyGroup.unlockCharacters)
                {
                    rpgManager.AddPartyMember(c, rpgManager.MeanLevel);
                }
            }
        }
        s_rpgGlobals.rpgGlSingleton.SwitchToOverworld(false);
    }

    public float TotalEXP(o_battleCharacter recipent) {
        List<o_battleCharacter> opponents = oppositionCharacters.FindAll(x => x.health <= 0);
        float expTotal = 0f;
        foreach (var bc in opponents) {
            float amount = ((float)bc.level / (float)recipent.level) * 0.5f;
            expTotal += amount;
            print("EXP: " + amount);
        }
        return expTotal;
    }

    public IEnumerator GameOver()
    {
        yield return StartCoroutine(s_triggerhandler.trigSingleton.Fade(Color.black));
        isPlayerTurn = true;
       // players.Clear();
        isEnabled = false;
        //s_rpgGlobals.rpgGlSingleton.ClearAllThings();
        //Destroy(rpg_globals.gl.player.gameObject);
        currentPartyCharactersQueue.Clear();
        battleEngine = BATTLE_ENGINE_STATE.NONE;
        s_rpgGlobals.rpgGlSingleton.SwitchToOverworld(false);
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
                if (hasGuest)
                {
                    if (currentCharacter == guest)
                    {
                        bcs.AddRange(oppositionCharacters);
                        isPlayerTurn = false;
                    }
                    else {
                        bcs.Add(guest);
                    }
                }
                else
                {
                    bcs.AddRange(oppositionCharacters);
                    isPlayerTurn = false;
                }
            } else {
                bcs.AddRange(playerCharacters);
                isPlayerTurn = true;
            }
            currentPartyCharactersQueue.Clear();
            int i = 0;
            print("chara count: " + bcs.Count);
            foreach (o_battleCharacter c in bcs)
            {
                if (c.health > 0 && c.inBattle == true)
                {
                    int spRestTurn = Mathf.CeilToInt((float)c.maxStamina * UnityEngine.Random.Range(0.02f, 0.06f));
                    print(spRestTurn);
                    c.stamina += spRestTurn;
                    c.stamina = Mathf.Clamp(c.stamina, 0, c.maxStamina );
                    c.RemoveStatusOnEndTurn();
                    for (int i2 = 0; i2 < c.battleCharData.turnIcons; i2++)
                    {
                        fullTurn++;
                        if (c != guest)
                        {
                            StartCoroutine(TurnIconFX(TURN_ICON_FX.APPEAR, i));
                        }
                        i++;
                        print("OK " + i);
                    }
                }
                currentPartyCharactersQueue.Enqueue(c);
            }
            yield return StartCoroutine(PollBattleEvent());
            battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
        }
        else
        {
            currentPartyCharactersQueue.Dequeue();
            currentPartyCharactersQueue.Enqueue(currentCharacterObject);
            
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
    enum TURN_ICON_FX {
        APPEAR,
        FADE,
        HIT
    }
    IEnumerator TurnIconFX(TURN_ICON_FX fx, int i) {
        float t = 0;
        float speed = 3.25f;
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