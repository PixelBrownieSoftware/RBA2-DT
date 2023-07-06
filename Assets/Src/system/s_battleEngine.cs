using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class s_skillDraw {
    public s_move move;
    public o_battleCharDataN user;
    public bool done = false;
}
public enum DAMAGE_FLAGS {
    NONE,
    FRAIL,
    CRITICAL,
    LUCKY,
    MISS,
    VOID,
    REFLECT,
    ABSORB,
    PASS = -1
}

public class s_battleEngine : MonoBehaviour
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
    [SerializeField]
    private S_EnemyWeaknessReveal enemyWeaknessReveal;
    [SerializeField]
    private S_HitObjSpawner hitObjectSpawner;
    [SerializeField]
    private S_ProjectileSpawner projectileSpawner;

    public Queue<o_battleCharacter> currentPartyCharactersQueue = new Queue<o_battleCharacter>();
    public List<o_battleCharacter> playerCharacters;
    public List<o_battleCharacter> oppositionCharacters;

    [SerializeField]
    private R_Int hitObjNumber;
    [SerializeField]
    private R_Text hitObjType;
    [SerializeField]
    private R_Vector2 hitObjPosition;
    [SerializeField]
    private R_Colour hitObjColour;

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

    public R_MoveList extraSkills;
    public R_Passives extraPassives;
    public R_EnemyGroupList battleGroupRef;
    public R_EnemyGroupList battleGroupDoneRef;
    public R_EnemyGroup enemyGroupRef;

    private Dictionary<o_battleCharacter, S_Passive> usedPassives = new Dictionary<o_battleCharacter, S_Passive>();
    private Dictionary<CH_BattleChar, List<Tuple<DAMAGE_FLAGS, int>>> totalDamageOutput = new Dictionary<CH_BattleChar, List<Tuple<DAMAGE_FLAGS, int>>>();
    private Dictionary<Tuple<S_Passive.PASSIVE_TRIGGER, o_battleCharacter>, List<S_Passive>> characterPassiveReference = new Dictionary<Tuple<S_Passive.PASSIVE_TRIGGER, o_battleCharacter>, List<S_Passive>>();

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
    bool[] battleEvDone;
    public bool isCutscene = false;

    public s_move defaultAttack;
    public s_move guard;
    public s_move passMove;
    public s_move nothingMove;

    public CH_Text changeMenu;
    public CH_Func perfomMove;
    public CH_Func retreatOption;
    public CH_Fade fadeFunc;
    public CH_Func onMapLoad;
    public CH_SoundPitch playSound;

    public CH_MapTransfer mapTrans;
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

    #region sounds
    public AudioClip notificationPlayerSound;
    public AudioClip notificationEnemySound;
    public AudioClip pressTurnSound;
    public AudioClip playerDefeatSound;
    public AudioClip enemyDefeatSound;
    public AudioClip criticalHitSound;
    public AudioClip luckyHitSound;
    public AudioClip enemyHitSound;
    public AudioClip playerHitSound;
    #endregion

    public o_battleCharacter ReferenceToCharacter(CH_BattleChar refer) {
        return GetAllCharacters().Find(x => x.referencePoint == refer);
    }

    public List<o_battleCharacter> GetAllCharacters() {
        List<o_battleCharacter> bcs = new List<o_battleCharacter>();
        if (hasGuest)
            bcs.Add(guest);
        bcs.AddRange(playerCharacters);
        bcs.AddRange(oppositionCharacters);
        return bcs;
    }

    private void OnEnable()
    {
        perfomMove.OnFunctionEvent += EndAction;
        retreatOption.OnFunctionEvent += RunFromBattle;
        onMapLoad.OnFunctionEvent += StartBattleCoroutine;
    }

    private void OnDisable()
    {
        perfomMove.OnFunctionEvent -= EndAction;
        retreatOption.OnFunctionEvent -= RunFromBattle;
        onMapLoad.OnFunctionEvent -= StartBattleCoroutine;
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
        usedPassives.Clear();
        usedPassives = new Dictionary<o_battleCharacter, S_Passive>();
    }

    public void StartBattleCoroutine() {
        StartCoroutine(StartBattle());
    }

    public IEnumerator StartBattle() {
        fullTurn = 0;
        halfTurn = 0;
        isEnabled = true;
        //s_menuhandler.GetInstance().SwitchMenu("EMPTY");
        allCharacterReferences.Clear();
        playersReference.Clear();
        enemiesReference.Clear();
        //SceneManager.UnloadSceneAsync("Overworld");
        usedPassives.Clear();
        usedPassives = new Dictionary<o_battleCharacter, S_Passive>();

        #region CLEAR GUI
        HPGUIMan.ClearHPGui();
        foreach (var ptIc in PT_GUI) {
            ptIc.color = Color.clear;
        }
        #endregion

        #region GROUP STUFF 
        {
            s_enemyGroup enemyGroup = this.enemyGroupRef.enemyGroup;
            bg1.material = enemyGroup.material1;
            bg2.material = enemyGroup.material2;
            bg1.sprite = enemyGroup.bg1;
            bg2.sprite = enemyGroup.bg2;
            //mainBG.sprite = s_rpgGlobals.GetInstance().GetComponent<s_rpgGlobals>().BGBattle;

            oppositionCharacters = new List<o_battleCharacter>();

            {
                List<Vector2> enPos = new List<Vector2>();
                if (enemyGroup.members_pre_summon != null)
                {
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
                } else
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
                if (enemyGroup.members_pre_summon != null)
                {
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
            }
            playerCharacters = new List<o_battleCharacter>();
            {
                int charIndex = 0;
                int playablesLeng = 1;
                if (enemyGroup.tempOnly)
                    playablesLeng = (partyMembers.battleCharList.Count - 1) + (enemyGroup.members_Player.Length - 1);
                else
                    playablesLeng = (enemyGroup.members_Player.Length - 1);
                playablesLeng = Mathf.Clamp(playablesLeng, 1, 5);
                List<Vector2> plPos = battlePositionsPlayer[playablesLeng];

                if (!enemyGroup.tempOnly)
                    for (int i = 0; i < partyMembers.battleCharList.Count; i++)
                    {
                        o_battleCharacter c = playerSlots[i];
                        c.render.color = Color.clear;
                        o_battleCharPartyData pbc = partyMembers.GetIndex(i);
                        o_battleCharDataN bc = pbc.characterDataSource;
                        c.experiencePoints = pbc.experiencePoints;
                        c.transform.position = plPos[i];
                        if (pbc.characterDataSource.secondMove != null)
                            c.secondMove = pbc.characterDataSource.secondMove;
                        if (pbc.characterDataSource.thirdMove != null)
                            c.thirdMove = pbc.characterDataSource.thirdMove;
                        c.animHandler.runtimeAnimatorController = bc.anim;
                        c.animHandler.Play("idle");
                        c.inBattle = pbc.inBattle;
                        allCharacterReferences.Add(c.referencePoint);
                        SetStatsPlayer(ref c, pbc);
                        c.referencePoint.characterData.elementals = bc.GetElements;
                        if (c != null)
                        {
                            if (c.inBattle)
                            {
                                HPGUIMan.SetPartyMember(charIndex, c);
                                charIndex++;
                            }
                        }
                    }
                for (int i = 0; i < enemyGroup.members_Player.Length; i++)
                {
                    if (charIndex == 5)
                        break;
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

        foreach (var character in GetAllCharacters()) {
            foreach (var passive in character.GetAllPassives)
            {
                Tuple<S_Passive.PASSIVE_TRIGGER, o_battleCharacter> passiveChar = new Tuple<S_Passive.PASSIVE_TRIGGER, o_battleCharacter>(passive.passiveTrigger, character);
                if (!characterPassiveReference.ContainsKey(passiveChar)) {
                    characterPassiveReference.Add(passiveChar, character.GetAllPassives.FindAll(x => x.passiveTrigger == passive.passiveTrigger));
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
            //s_camera.cam.TeleportCamera(s_camera.cam.GetCentroid(positions));
            //s_camera.cam.TeleportCamera(new Vector2(360,310));
        }
        #endregion
        fadeFunc.Fade(Color.clear);
        yield return new WaitForSeconds(0.1f);
        //StartCoroutine(s_camera.GetInstance().ZoomCamera(-1, 1.5f));
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
        s_enemyGroup enemyGroup = this.enemyGroupRef.enemyGroup;
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
        StartCoroutine(PlayFadeCharacter(c, Color.clear, Color.white));
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
            if (oppositionCharacters[i].health <= 0) {
                continue;
            }
            oppositionCharacters[i].render.color = Color.white;
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
            int tempHPMax = enem.maxHitPointsGMax;

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
        charObj.passives = new List<S_Passive>();
        charObj.extraPassives = new List<S_Passive>();
        foreach (s_move mv in enem.moveLearn)
        {
            if (mv.MeetsRequirements(charObj))
            {
                charObj.currentMoves.Add(mv);
            }
        }
        foreach (var mv in enem.passiveLearn)
        {
            if (mv.MeetsRequirements(charObj))
            {
                charObj.passives.Add(mv);
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
        if (mem.passives != null)
        {
            foreach (var mv in mem.passives)
            {
                if (mv.MeetsRequirements(charObj))
                {
                    charObj.extraPassives.Add(mv);
                }
            }
        }
        List<s_move> totalMoves = new List<s_move>();
        totalMoves.AddRange(charObj.currentMoves);
        totalMoves.AddRange(charObj.extraSkills);
        charObj.referencePoint.characterData = rpgManager.SetPartyCharacterStats(charObj);

        charObj.character_AI = GetAIList(totalMoves);
        charObj.referencePoint.characterData.elementals = enem.GetElements;
        if (mem.passives != null)
        {
            AssignPassivesAffinity(mem.passives, ref charObj);
        }
        charObj.inBattle = true;
    }
    public void AssignPassivesAffinity(S_Passive[] passives, ref o_battleCharacter charObj) {
        foreach (var passive in passives)
        {
            if (passive.MeetsRequirements(charObj))
            {
                charObj.extraPassives.Add(passive);
                switch (passive.passiveSkillType)
                {
                    case S_Passive.PASSIVE_TYPE.ABSORB:
                        charObj.referencePoint.characterData.SetElementWeakness(passive.element, -2f);
                        break;
                    case S_Passive.PASSIVE_TYPE.NULL:
                        charObj.referencePoint.characterData.SetElementWeakness(passive.element, 0f);
                        break;
                    case S_Passive.PASSIVE_TYPE.REPEL:
                        charObj.referencePoint.characterData.SetElementWeakness(passive.element, -1f);
                        break;
                    case S_Passive.PASSIVE_TYPE.RESIST:
                        charObj.referencePoint.characterData.SetElementWeakness(passive.element, 0.5f);
                        break;
                }
            }
        }
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
        int tempLuc = enem.luck;
        int tempMag = enem.intelligence;

        charObj.name = enem.name;
        charObj.level = enem.level;
        charObj.health = charObj.maxHealth = tempHP;
        charObj.stamina = charObj.maxStamina = tempSP;
        charObj.vitality = tempVit;
        charObj.dexterity = tempDx;
        charObj.intelligence = tempMag;
        charObj.strength = tempStr;
        charObj.agility = tempAgi;
        charObj.luck = tempLuc;
        charObj.battleCharData = enem.characterDataSource;
        charObj.referencePoint.characterData = enem;
        charObj.currentMoves = new List<s_move>();
        charObj.extraSkills = new List<s_move>();
        charObj.currentMoves.AddRange(enem.currentMoves);
        charObj.extraSkills.AddRange(enem.extraSkills);
        charObj.extraPassives.AddRange(enem.extraPassives);
        charObj.passives.AddRange(enem.passives);

        AssignPassivesAffinity(enem.passives.ToArray(), ref charObj);

        playersReference.Add(charObj.referencePoint);
        playerCharacters.Add(charObj);
    }
    public void SetStatsPlayer(ref o_battleCharacter charObj, s_enemyGroup.s_groupMember mem)
    {
        SetStatsNonChangable(ref charObj, mem);
        foreach (var elem in rpgManager.allElements.elementsList)
        {
            enemyWeaknessReveal.AddElementWeakness(mem.memberDat, elem);
        }
        playersReference.Add(charObj.referencePoint);
        playerCharacters.Add(charObj);
    }
    #endregion

    #region Aimations
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
    public IEnumerator PlayProjectileAnimation(O_ProjectileAnim projectile ,Vector2 start, Vector2 target)
    {
        yield return new WaitForSeconds(Time.deltaTime);
        hitObjPosition.Set(start);
        projectile.transform.position = start;
        if (start != target)
        {
            Vector2 dir = (target - start).normalized;
            Rigidbody2D projRB2d = projectile.rb2d;
            while (Vector2.Distance(target, projectile.transform.position) > 20)
            {
                projRB2d.velocity = (dir * 10);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            projectile.DespawnObject();
        }
    }
    public IEnumerator DamageAnimation(int dmg, o_battleCharacter targ) {

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
                    hitObjColour.Set(Color.white);
                    hitObjNumber.Set(dmg);
                    hitObjPosition.Set(characterPos);
                    hitObjType.Set("damage_enemy");
                    hitObjectSpawner.hitObjectPool.Get();
                }
                else
                {
                    hitObjColour.Set(targ.battleCharData.characterColour);
                    hitObjNumber.Set(dmg);
                    hitObjPosition.Set(characterPos);
                    hitObjType.Set("damage_player");
                    hitObjectSpawner.hitObjectPool.Get();
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
                    case s_actionAnim.ACTION_TYPE.CHAR_ANIMATION:
                        user.SwitchAnimation(an.name);
                        
                        if (an.time > 0)
                        {
                            while (timer < an.time)
                            {
                                timer += Time.deltaTime;
                                yield return new WaitForSeconds(Time.deltaTime);
                            }
                        }    
                        else {
                            while (timer < user.GetAnimHandlerState())
                            {
                                timer += Time.deltaTime;
                                yield return new WaitForSeconds(Time.deltaTime);
                            }
                        }
                        break;

                    case s_actionAnim.ACTION_TYPE.ANIMATION:
                        {
                            Vector2 start = new Vector2(0, 0);
                            switch (an.start)
                            {
                                case s_actionAnim.MOTION.SELF:
                                    start = user.transform.position;
                                    break;
                                case s_actionAnim.MOTION.TO_TARGET:
                                    if(targ != null)
                                        start = targ.transform.position;
                                    break;
                            }
                            hitObjType.Set(an.name);
                            O_ProjectileAnim projectile = projectileSpawner.projectilePool.Get();
                            StartCoroutine(PlayProjectileAnimation(projectile, start, start));
                            if (an.time > 0)
                            {
                                while (timer < an.time)
                                {
                                    timer += Time.deltaTime;
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
                            }
                            else
                            {
                                while (timer < projectile.GetAnimHandlerState())
                                {
                                    timer += Time.deltaTime;
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
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
                        yield return StartCoroutine(DamageAnimation(targ, UnityEngine.Random.Range(an.minimumPowerRandomness, an.maximumPowerRandomness)));
                        break;

                    case s_actionAnim.ACTION_TYPE.FADE_SCREEN:
                        fadeFunc.Fade(an.endColour);
                        yield return new WaitForSeconds(0.75f);
                        break;

                    case s_actionAnim.ACTION_TYPE.PROJECTILE:
                        {
                            Vector2 start = new Vector2(0, 0);
                            Vector2 end = new Vector2(0, 0);

                            switch (an.start)
                            {
                                case s_actionAnim.MOTION.SELF:
                                    start = user.transform.position;
                                    break;
                                case s_actionAnim.MOTION.TO_TARGET:
                                    start = targ.transform.position;
                                    break;
                            }

                            switch (an.goal)
                            {
                                case s_actionAnim.MOTION.SELF:
                                    end = user.transform.position;
                                    break;
                                case s_actionAnim.MOTION.TO_TARGET:
                                    end = targ.transform.position;
                                    break;
                            }
                            hitObjType.Set(an.name);
                            O_ProjectileAnim projectile = projectileSpawner.projectilePool.Get();
                            yield return StartCoroutine(PlayProjectileAnimation(projectile, start, end));
                        }
                        break;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            yield return StartCoroutine(DamageAnimation(targ,0));
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
                case s_move.MOVE_TARGET.ENEMY_ALLY:
                    bcs.AddRange(oppositionCharacters);
                    bcs.AddRange(playerCharacters);
                    if (hasGuest)
                        bcs.Add(guest);
                    break;
                case s_move.MOVE_TARGET.ALLY:
                    bcs.AddRange(playerCharacters);
                    if (hasGuest)
                        bcs.Add(guest);
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
        switch (scope)
        {
            case s_move.MOVE_TARGET.ENEMY_ALLY:
                bcs.AddRange(playerCharacters);
                if (hasGuest)
                    bcs.Add(guest);
                bcs.AddRange(oppositionCharacters);
                break;
            case s_move.MOVE_TARGET.NONE:
            case s_move.MOVE_TARGET.SELF:
                bcs.Add(currentCharacterObject);
                break;
        }
        return bcs;
    }
    public bool DisplayTotalDamage() {
        bool displayDMG = false;
        foreach (var chara in totalDamageOutput) {
            if (playerCharacters.Contains(ReferenceToCharacter(chara.Key)))
                continue;
            List<Tuple<DAMAGE_FLAGS, int>> damageFlags = chara.Value.FindAll(x => x.Item1 < DAMAGE_FLAGS.MISS);
            if (damageFlags.Count < 2)
                continue;
            displayDMG = true;
            int totDmg = 0;
            foreach (var dmg in chara.Value) {
                totDmg += dmg.Item2;
            }
            hitObjColour.Set(Color.white);
            hitObjNumber.Set(totDmg);
            hitObjPosition.Set(chara.Key.position + new Vector2(40, 0));
            hitObjType.Set("total");
            hitObjectSpawner.hitObjectPool.Get();
        }
        totalDamageOutput.Clear();
        return displayDMG;
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
    public IEnumerator CheckStatusEffectAfterAction()
    {
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
                        StartCoroutine(DamageAnimation(eff.damage, currentCharacterObject));
                    }
                    break;
            }
            eff.duration--;
        }
        yield return new WaitForSeconds(0.1f);
    }
    public IEnumerator DoEndTurnAnimation(bool isPturn)
    {
        yield return new WaitForSeconds(0.4f);

        isPlayerTurn = isPturn;
        battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
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
                if(currentCharacter.characterRef != guest.referencePoint)
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
        if (currentCharacter.characterRef != guest.referencePoint)
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

                Dictionary<s_move, float> alwaysMoves = new Dictionary<s_move, float>();

                foreach (charAI ai in  currentCharacterObject.character_AI)
                {
                    float inclination = UnityEngine.Random.Range(0.01f, 1.5f);
                    o_battleCharacter potentialTrg = null;
                    s_move move = ai.move;
                    if (move.element.isMagic) {
                        switch (move.customFunc) {
                            case "callAlly":
                            case "callAllies":
                                if (oppositionCharacters.Count >= 4)
                                {
                                    continue;
                                }
                                break;
                        }
                        if (currentCharacterObject.stamina < move.cost) {
                            continue;
                        }
                    } else {
                        if (currentCharacterObject.health <= move.cost) {
                            continue;
                        }
                    }
                    if (!currentCharacterObject.GetAllMoves.Contains(move)) {
                        continue;
                    }
                    List<o_battleCharacter> targets = new List<o_battleCharacter>();
                    switch (ai.move.moveTarg) {

                        case s_move.MOVE_TARGET.ALLY:
                            if(ai.move.includeDefeated)
                                targets = allies;
                            else
                                targets = allies.FindAll(x => x.inBattle == true && x.health > 0);
                            break;
                        case s_move.MOVE_TARGET.ENEMY:
                            if (ai.move.includeDefeated)
                                targets = baddies;
                            else
                                targets = baddies.FindAll(x => x.inBattle == true && x.health > 0);
                            break;
                        case s_move.MOVE_TARGET.ENEMY_ALLY:
                            targets.AddRange(allies);
                            targets.AddRange(baddies);
                            break;
                        case s_move.MOVE_TARGET.SELF:
                        case s_move.MOVE_TARGET.NONE:
                            targets.Add(currentCharacterObject);
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
                            alwaysMoves.Add(move, (inclination * ai.inclinationPercentage));
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
                                if (bc.referencePoint.characterData.elementals[ai.move.element] >= 2) {
                                    potentialTrg = bc;
                                    break;
                                }
                            }
                            break;

                        case charAI.CONDITIONS.ELEMENT_TARG_STRONGEST:
                            foreach (o_battleCharacter bc in targets)
                            {
                                int aff = (int)bc.referencePoint.characterData.elementals[ai.move.element];
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
                    s_move selectedMV = null;
                    {
                        float bestInc = float.MinValue;
                        foreach (var mov in alwaysMoves) {
                            if (mov.Value > bestInc)
                            {
                                bestInc = mov.Value;
                                selectedMV = mov.Key;
                            }
                        }
                    }
                    currentMove.SetMove(selectedMV);
                    List<o_battleCharacter> targets = new List<o_battleCharacter>();
                    targets = AllTargetsLiving(currentMove.move.moveTarg);
                    targetCharacter.SetCharacter(targets[UnityEngine.Random.Range(0, targets.Count-1)].referencePoint);
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
    public IEnumerator ExcecuteMove()
    {
        s_move mov = currentMove.move;
        o_battleCharacter user = ReferenceToCharacter(currentCharacter.characterRef);
        o_battleCharacter targ = ReferenceToCharacter(targetCharacter.characterRef);
        s_actionAnim[] animations = mov.animations;


        if (playerCharacters.Contains(currentCharacterObject) || currentCharacterObject == guest)
        {
            playSound.RaiseEvent(notificationPlayerSound, 1);
        }
        else
        {
            playSound.RaiseEvent(notificationEnemySound, 1);
        }

        for (int i = 0; i < 2; i++)
        {
            float t = 0;
            float spd = 13.6f;
            while (currentCharacterObject.render.color != Color.black)
            {
                currentCharacterObject.render.color = Color.Lerp(Color.white, Color.black, t);
                t += Time.deltaTime * spd;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            t = 0;
            while (currentCharacterObject.render.color != Color.white)
            {
                currentCharacterObject.render.color = Color.Lerp(Color.black, Color.white, t);
                t += Time.deltaTime * spd;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        if (mov.element.isMagic)
        {
            user.stamina -= mov.cost;
        }
        else
        {
            user.health -= s_calculation.DetermineHPCost(mov, user.strengthNet, user.vitalityNet, user.maxHealth);
        }
        yield return StartCoroutine(DisplayMoveName(mov.name));

        switch (currentMove.move.moveTargScope)
        {
            case s_move.SCOPE_NUMBER.ONE:
                targetCharacterObject = targ;
                yield return StartCoroutine(PlayAttackAnimation(animations, targ, currentCharacterObject));
                break;

            case s_move.SCOPE_NUMBER.ALL:
                {
                    List<o_battleCharacter> bcs = AllTargets(currentMove.move.moveTarg);
                    print(bcs.Count);
                    foreach (var b in bcs)
                    {
                        targetCharacterObject = b;
                        yield return StartCoroutine(PlayAttackAnimation(animations, b, currentCharacterObject));
                    }
                }
                break;

            case s_move.SCOPE_NUMBER.RANDOM:
                {
                    List<o_battleCharacter> bcs = AllTargets(currentMove.move.moveTarg);
                    int rand = UnityEngine.Random.Range(3, 6);
                    for (int i = 0; i < rand; i++)
                    {
                        bcs = AllTargetsLiving(currentMove.move.moveTarg);
                        if (bcs.Count == 0)
                            break;
                        o_battleCharacter bc = bcs[UnityEngine.Random.Range(0, bcs.Count)];
                        targetCharacterObject = bc;
                        yield return StartCoroutine(PlayAttackAnimation(animations, bc, currentCharacterObject));
                    }
                }
                break;
        }

        yield return new WaitForSeconds(1f);
        currentCharacterObject.SwitchAnimation("idle");
        yield return StartCoroutine(CheckStatusEffectAfterAction());
        FigureFinalFlag();
        bool dispTotal = DisplayTotalDamage();
        if (dispTotal) {
            yield return new WaitForSeconds(1.3f);
        }
        //finalDamageFlag = DAMAGE_FLAGS.NONE;

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
                    playSound.RaiseEvent(pressTurnSound, 1);
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
        battleEngine = BATTLE_ENGINE_STATE.END;
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
    public void RunFromBattle() {
        StartCoroutine(ConcludeBattle());
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
        battleEngine = BATTLE_ENGINE_STATE.END;
        menuchoice = 0;
    }
    public void SelectSkillOptionGuard()
    {
        currentMove.SetMove(guard);
        EndAction();
    }
    public void AddToTotalDMG(int damage, DAMAGE_FLAGS flag, CH_BattleChar targ) {
        if (totalDamageOutput == null)
            totalDamageOutput = new Dictionary<CH_BattleChar, List<Tuple<DAMAGE_FLAGS, int>>>();
        if (!totalDamageOutput.ContainsKey(targ)) {
            totalDamageOutput.Add(targ, new List<Tuple<DAMAGE_FLAGS, int>>());
        }
        totalDamageOutput[targ].Add(new Tuple<DAMAGE_FLAGS, int>(flag, damage));
    }
    public bool PredictStatChance(int userVal, int targVal, float connectMax) {
        userVal = Mathf.Clamp(userVal, 1, int.MaxValue);
        targVal = Mathf.Clamp(targVal, 1, int.MaxValue);
        int totalVal = userVal + targVal;
        float userModify = (float)userVal + ((float)userVal * connectMax);
        print("User chance: " + userModify + " enemy chance: " + targVal);
        float attackConnectChance = (userModify / (float)totalVal);
        float gonnaHit = UnityEngine.Random.Range(0f, 1f);
        if (gonnaHit > attackConnectChance)
            return false;
        return true;
    }
    public void FigureFinalFlag() {
        DAMAGE_FLAGS currFlag = DAMAGE_FLAGS.NONE;
        foreach (var entry in totalDamageOutput) {
            foreach (var flg in entry.Value) {
                currFlag = currFlag < flg.Item1 ? flg.Item1 : currFlag; 
            }
        }
        if (currentCharacter.characterRef == guest.referencePoint)
            currFlag = DAMAGE_FLAGS.NONE;
        if (!currentMove.move.consumeTurn)
        {
            currFlag = DAMAGE_FLAGS.PASS;
        }
        finalDamageFlag = currFlag;
    }
    public ELEMENT_WEAKNESS FigureWeakness(o_battleCharacter targ)
    {
        s_move mov = currentMove.move;
        float elementWeakness = 1;
        elementWeakness = targ.referencePoint.characterData.GetElementWeakness(mov.element);
        if (targ.referencePoint.sheildAffinity != null)
        {
            if (targ.referencePoint.sheildAffinity.Item1 == mov.element)
                elementWeakness = targ.referencePoint.sheildAffinity.Item2;
        }
        if (mov.moveType == s_move.MOVE_TYPE.NONE)
        {
            return ELEMENT_WEAKNESS.NONE;
        }
        else
        {
            if (elementWeakness >= 2)
                return ELEMENT_WEAKNESS.FRAIL;
            else if (elementWeakness < 2 && elementWeakness > 0)
                return ELEMENT_WEAKNESS.NONE;
            else if (elementWeakness == 0)
                return ELEMENT_WEAKNESS.NULL;
            else if (elementWeakness < 0 && elementWeakness > -1)
                return ELEMENT_WEAKNESS.REFLECT;
            else if (elementWeakness <= -1)
                return ELEMENT_WEAKNESS.ABSORB;
        }

        return ELEMENT_WEAKNESS.NONE;
    }
    public bool IsCritical(o_battleCharacter targ)
    {
        s_move mov = currentMove.move;
        foreach (var statusEff in targ.statusEffects)
        {
            foreach (var element in statusEff.status.criticalOnHit)
            {
                if (element == mov.element)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public IEnumerator DamageAnimation(o_battleCharacter targ, int randomValue)
    {
        Vector2 characterPos = targ.transform.position;
        s_move mov = currentMove.move;
        int dmg = 0;
        List<float> modifier = new List<float>();
        bool willHit = true;
        switch (currentMove.move.moveType)
        {
            case s_move.MOVE_TYPE.HP_DAMAGE:
            case s_move.MOVE_TYPE.HP_DRAIN:
            case s_move.MOVE_TYPE.HP_SP_DAMAGE:
            case s_move.MOVE_TYPE.HP_SP_DRAIN:
                if (targ.health <= 0)
                    yield break;
                willHit = PredictStatChance(currentCharacterObject.dexterity, targ.agiNet, 0.65f);
                bool isLucky = PredictStatChance(currentCharacterObject.luckNet, targ.luckNet, -0.80f);
                bool isCritical = IsCritical(targ);
                print("Will hit? " + willHit + " Is critical? " + isCritical + " Is lucky? " + isLucky);
                ELEMENT_WEAKNESS weaknessType = FigureWeakness(targ);
                if (weaknessType < ELEMENT_WEAKNESS.NULL)
                {
                    switch (weaknessType) {
                        case ELEMENT_WEAKNESS.NONE:
                            damageFlag = DAMAGE_FLAGS.NONE;
                            break;
                        case ELEMENT_WEAKNESS.ABSORB:
                            damageFlag = DAMAGE_FLAGS.ABSORB;
                            break;
                        case ELEMENT_WEAKNESS.FRAIL:
                            damageFlag = DAMAGE_FLAGS.FRAIL;
                            break;
                        case ELEMENT_WEAKNESS.NULL:
                            damageFlag = DAMAGE_FLAGS.VOID;
                            break;
                        case ELEMENT_WEAKNESS.REFLECT:
                            damageFlag = DAMAGE_FLAGS.REFLECT;
                            break;
                    }
                    if (isCritical)
                    {
                        modifier.Add(0.35f);
                        damageFlag = DAMAGE_FLAGS.CRITICAL;
                    }
                    if (isLucky)
                    {
                        modifier.Add(0.5f);
                        damageFlag = DAMAGE_FLAGS.LUCKY;
                    }
                }
                if (willHit)
                {
                    dmg = CalculateDamage(targ, currentCharacterObject, currentMove.move, modifier, randomValue);
                }
                else
                {
                    damageFlag = DAMAGE_FLAGS.MISS;
                }
                DamageEffect(dmg, targ, characterPos, damageFlag);
                if (!willHit) {
                    yield return StartCoroutine(DodgeAnimation(targ, characterPos));
                }
                break;
            case s_move.MOVE_TYPE.HP_RECOVER:
                dmg = CalculateDamage(targ, currentCharacterObject, currentMove.move, null, 0);
                ReferenceToCharacter(targetCharacter.characterRef).health += dmg;
                targetCharacterObject.health = Mathf.Clamp(targetCharacterObject.health,
                    0, targetCharacterObject.maxHealth);
                hitObjColour.Set(Color.white);
                hitObjNumber.Set(dmg);
                hitObjPosition.Set(characterPos);
                hitObjType.Set("heal_hp");
                hitObjectSpawner.hitObjectPool.Get();
                break;

            case s_move.MOVE_TYPE.SP_RECOVER:
                dmg = CalculateDamage(targ, currentCharacterObject, currentMove.move, null,0);
                targ.stamina += dmg;
                targ.stamina = Mathf.Clamp(targ.stamina,
                    0, targ.maxStamina);
                hitObjColour.Set(Color.magenta);
                hitObjNumber.Set(dmg);
                hitObjPosition.Set(characterPos);
                hitObjType.Set("heal_hp");
                hitObjectSpawner.hitObjectPool.Get();
                break;
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
        if (willHit)
        {
            List<S_Element.effect> statusEffsInflictChance = new List<S_Element.effect>();
            statusEffsInflictChance.AddRange(mov.statusInflictChance);
            statusEffsInflictChance.AddRange(mov.element.statusInflict);
            HashSet<S_StatusEffect> hasInflicted = new HashSet<S_StatusEffect>();
            foreach (var statusInfl in statusEffsInflictChance)
            {
                float infChance = UnityEngine.Random.Range(0, 1f);
                if (infChance <= statusInfl.chance)
                {
                    if (hasInflicted.Contains(statusInfl.statusEffect))
                        continue;
                    if (statusInfl.add_remove)
                    {
                        if (!targ.HasStatus(statusInfl.statusEffect))
                        {
                            hasInflicted.Add(statusInfl.statusEffect);
                            targ.SetStatus(statusInfl.statusEffect, dmg);
                        }
                    }
                    else
                    {
                        if (targ.HasStatus(statusInfl.statusEffect))
                        {
                            hasInflicted.Add(statusInfl.statusEffect);
                            targ.RemoveStatus(statusInfl.statusEffect);
                        }
                    }
                }
            }
        }
        if (finalDamageFlag != DAMAGE_FLAGS.MISS || finalDamageFlag != DAMAGE_FLAGS.VOID ||
            finalDamageFlag != DAMAGE_FLAGS.ABSORB || finalDamageFlag != DAMAGE_FLAGS.REFLECT)
        {
            foreach (var ch in targs)
            {
                if (mov.guardPoints != 0)
                {
                    ch.guardPoints += mov.guardPoints;
                }
                if (currentMove.move.elementsSheild != null)
                {
                    ch.referencePoint.sheildAffinity = new Tuple<S_Element, float>(currentMove.move.elementsSheild, currentMove.move.elementalSheildAffinity);
                }
            }
        }

        //Show Damage output here
        yield return new WaitForSeconds(0.18f);
        if (targ.health <= 0)
        {
            yield return StartCoroutine(TriggerSingleTargetPassives(S_Passive.PASSIVE_TRIGGER.SELF_DEFEAT, targ, characterPos));
        }
        if (targ.health <= 0)
        {
            targ.statusEffects.Clear();
            if (oppositionCharacters.Contains(targ))
            {
                playSound.RaiseEvent(enemyDefeatSound, 1);
            }
            else
            {
                playSound.RaiseEvent(playerDefeatSound, 1);
            }
            yield return StartCoroutine(PlayFadeCharacter(targ, Color.black, Color.clear));
            if (oppositionCharacters.Contains(targ) && !targ.persistence)
            {
                enemiesReference.Remove(targ.referencePoint);
                oppositionCharacters.Remove(targ);
                ReshuffleOpponentPositions();
            }
        }
    }
    public int CalculateDamage(o_battleCharacter user, o_battleCharacter target, s_move move, List<float> modifiers, int randomVal)
    {
        if (target == null)
            return 0;
        S_Element el = move.element;
        int dmg = 0;
        if (!move.fixedValue)
        {
            float multipler = 1f;
            float elementals = user.referencePoint.characterData.GetElementWeakness(el);
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
            int mvPower = move.power + randomVal;
            mvPower = Mathf.Clamp(mvPower, 1, int.MaxValue);
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

                hitObjColour.Set(new Color(95f / 255, 225f / 255f, 215f / 255f));
                hitObjNumber.Set(dmg);
                hitObjPosition.Set(characterPos);
                hitObjType.Set("lucky");
                if (oppositionCharacters.Contains(target))
                {
                    playSound.RaiseEvent(luckyHitSound, 1);
                    playSound.RaiseEvent(enemyHitSound, 1);
                    hitObjectSpawner.hitObjectPool.Get();
                }
                else
                {
                    playSound.RaiseEvent(luckyHitSound, 0.5f);
                    playSound.RaiseEvent(playerHitSound, 1);
                    hitObjectSpawner.hitObjectPool.Get();
                    //SpawnDamageObject(dmg, characterPos, false, target.battleCharData.characterColour);
                }
                break;

            case DAMAGE_FLAGS.CRITICAL:

                hitObjColour.Set(Color.white);
                hitObjNumber.Set(dmg);
                hitObjPosition.Set(characterPos);
                hitObjType.Set("critical");
                if (oppositionCharacters.Contains(target))
                {
                    playSound.RaiseEvent(criticalHitSound, 0.85f);
                    playSound.RaiseEvent(enemyHitSound, 1);
                    hitObjectSpawner.hitObjectPool.Get();
                }
                else
                {
                    playSound.RaiseEvent(criticalHitSound, 0.85f);
                    playSound.RaiseEvent(playerHitSound, 1);
                    hitObjectSpawner.hitObjectPool.Get();
                }
                break;

            case DAMAGE_FLAGS.FRAIL:
            case DAMAGE_FLAGS.NONE:
                hitObjNumber.Set(dmg);
                hitObjPosition.Set(characterPos);
                if (target != guest)
                {
                    if (oppositionCharacters.Contains(target))
                    {
                        playSound.RaiseEvent(enemyHitSound, 1);
                        hitObjColour.Set(Color.white);
                        hitObjType.Set("damage_enemy");
                        hitObjectSpawner.hitObjectPool.Get();
                    }
                    else
                    {
                        playSound.RaiseEvent(playerHitSound, 1);
                        hitObjType.Set("damage_player");
                        hitObjColour.Set(target.battleCharData.characterColour);
                        hitObjectSpawner.hitObjectPool.Get();
                    }
                }
                else
                {
                    playSound.RaiseEvent(playerHitSound, 1);
                }
                break;

            case DAMAGE_FLAGS.VOID:
                hitObjNumber.Set(0);
                hitObjPosition.Set(characterPos);
                hitObjColour.Set(Color.white);
                hitObjType.Set("block");
                hitObjectSpawner.hitObjectPool.Get();
                break;
            case DAMAGE_FLAGS.MISS:
                hitObjNumber.Set(0);
                hitObjPosition.Set(characterPos);
                hitObjColour.Set(Color.white);
                hitObjType.Set("miss_attack");
                hitObjectSpawner.hitObjectPool.Get();
                break;
        }
        AddToTotalDMG(dmg, fl, target.referencePoint);
    }
    public IEnumerator PassiveSkillDo(KeyValuePair<o_battleCharacter, S_Passive> user_skill, Vector2 characterPos)
    {
        o_battleCharacter targ = user_skill.Key;
        int dmg = 0;
        yield return StartCoroutine(DisplayMoveName(user_skill.Value.name));
        if (user_skill.Value.singleUse)
        {
            usedPassives.Add(targ, user_skill.Value);
        }
        switch (user_skill.Value.passiveSkillType)
        {
            case S_Passive.PASSIVE_TYPE.COUNTER:


                characterPos = currentCharacterObject.transform.position;
                dmg = CalculateDamage(targ, currentCharacterObject, targ.battleCharData.firstMove, null, 0);

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
                break;

            case S_Passive.PASSIVE_TYPE.REGEN:
                dmg = Mathf.RoundToInt(targ.maxHealth * user_skill.Value.percentageHeal);
                //int spDmg
                targ.health += dmg;
                targ.health = Mathf.Clamp(targ.health, 0, targetCharacterObject.maxHealth);
                hitObjColour.Set(Color.white);
                hitObjNumber.Set(dmg);
                hitObjPosition.Set(characterPos);
                hitObjType.Set("heal_hp");
                hitObjectSpawner.hitObjectPool.Get();
                break;
        }
    }
    public IEnumerator ApplySmirk(o_battleCharacter target, float chance) {
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator TriggerSingleTargetPassives(S_Passive.PASSIVE_TRIGGER trigger, o_battleCharacter targ, Vector2 characterPos)
    {
        Tuple<S_Passive.PASSIVE_TRIGGER, o_battleCharacter> trigger_char = new Tuple<S_Passive.PASSIVE_TRIGGER, o_battleCharacter>(trigger, targ);
        if (!characterPassiveReference.ContainsKey(trigger_char))
            yield break;
        if (characterPassiveReference[trigger_char] != null)
            yield break;
        List<S_Passive> passives = characterPassiveReference[trigger_char];
        S_Passive passiveToDo = null;
        foreach (var passiveIndex in passives)
        {
            if (usedPassives.ContainsKey(targ) && usedPassives.ContainsValue(passiveIndex))
            {
                continue;
            }
            float perc = UnityEngine.Random.Range(0f, 1f);
            if (perc < passiveIndex.percentage)
            {
                passiveToDo = passiveIndex;
            }
        }
        yield return StartCoroutine(PassiveSkillDo(new KeyValuePair<o_battleCharacter, S_Passive>(targ, passiveToDo), characterPos));
    }

    public IEnumerator ConcludeBattle()
    {
        s_enemyGroup enemyGroup = enemyGroupRef.enemyGroup;
        print(enemyGroup.name);
        //Fade
        //EXPResults.SetActive(false);
        //oppositionCharacterTurnQueue.Clear();
        //playerCharacterTurnQueue.Clear();
        currentPartyCharactersQueue.Clear();
        battleEngine = BATTLE_ENGINE_STATE.NONE;
        yield return new WaitForSeconds(0.1f);
        bool allDefeated = oppositionCharacters.FindAll(x => x.health <= 0).Count == oppositionCharacters.Count;
        print("All gone?" + allDefeated);
        for (int ind = 0; ind < playerCharacters.Count; ind++)
        {
            o_battleCharacter c = playerCharacters[ind];
            if (c.isTemp)
                continue;
            if (c == guest)
                continue;
            float exp = TotalEXP(c);
            int initailTotal = (int)(exp * 100);
            print(initailTotal);
            //we add the exp and make it so that it checks for a level up
            for (int i = 0; i < initailTotal; i++)
            {
                c.experiencePoints += 0.01f;
                c.experiencePoints = MathF.Round(c.experiencePoints * 100f) / 100f;
                print(c.experiencePoints);
                if (c.experiencePoints >= 1f)
                {
                    c.level++;
                    o_battleCharDataN chdat = c.battleCharData;
                    if (c.level % chdat.strengthGT == 0)
                        c.strength++;
                    if (c.level % chdat.vitalityGT == 0)
                        c.vitality++;
                    if (c.level % chdat.dexterityGT == 0)
                        c.dexterity++;
                    if (c.level % chdat.intelligenceGT == 0)
                        c.intelligence++;
                    if (c.level % chdat.agilityGT == 0)
                        c.agility++;
                    if (c.level % chdat.luckGT == 0)
                        c.luck++;
                    c.experiencePoints = 0;
                    exp = TotalEXP(c) * (float)((float)i / (float)initailTotal);
                    c.maxHealth += UnityEngine.Random.Range(chdat.maxHitPointsGMin, chdat.maxHitPointsGMax + 1);
                    List<s_move> mv2Learn = chdat.moveLearn.FindAll(x => x.MeetsRequirements(c) && !c.currentMoves.Contains(x));
                    if (mv2Learn != null)
                    {
                        c.currentMoves.AddRange(mv2Learn);
                    }
                    print(c.name + "Level up! Lv." + c.level);
                }
            }
            rpgManager.SetPartyMemberStats(c);
        }

        List<string> extSkillLearn = new List<string>();
        changeMenu.RaiseEvent("ExperienceMenu");
        foreach (o_battleCharacter en in oppositionCharacters)
        {
            if (en.health > 0)
                continue;
            //For now it's going to be a random amount... later it'll be "en.battleCharData.money"
            float moneyGiven = en.battleCharData.money;
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
            if (en.extraPassives != null)
            {
                foreach (var mv in en.extraPassives)
                {
                    if (!extraPassives.ListContains(mv))
                    {
                        extraPassives.AddMove(mv);
                        extSkillLearn.Add(mv.name);
                    }
                }
            }
        }
        if (allDefeated)
        {
            //battleGroupDoneRef.AddGroup(enemyGroup);
            if (enemyGroup.perishBranches != null)
            {
                foreach (var br in enemyGroup.perishBranches)
                {
                    print(br.name);
                    battleGroupRef.RemoveGroup(br);
                    battleGroupDoneRef.AddGroup(br);
                }
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
        foreach (o_battleCharacter c in playerCharacters)
        {
            c.extraSkills.Clear();
            c.extraPassives.Clear();
            if (c == guest)
                continue;
        }
        if (enemyGroup.unlockCharacters != null)
            if (enemyGroup.unlockCharacters.Length > 0)
            {
                foreach (o_battleCharDataN c in enemyGroup.unlockCharacters)
                {
                    rpgManager.AddPartyMember(c, rpgManager.MeanLevel);
                }
            }
        rpgManager.SaveData();
        //s_rpgGlobals.rpgGlSingleton.SwitchToOverworld(false);
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
        yield return new WaitForSeconds(0.5f);
        isPlayerTurn = true;
        isEnabled = false;
        currentPartyCharactersQueue.Clear();
        battleEngine = BATTLE_ENGINE_STATE.NONE;
        mapTrans.RaiseEvent("Overworld");
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
                    if (currentCharacter.characterRef == guest.referencePoint)
                    {
                        enemiesReference.ClearSheild();
                        bcs.AddRange(oppositionCharacters);
                        isPlayerTurn = false;
                    }
                    else {
                        bcs.Add(guest);
                    }
                }
                else
                {
                    enemiesReference.ClearSheild();
                    bcs.AddRange(oppositionCharacters);
                    isPlayerTurn = false;
                }
            } else
            {
                playersReference.ClearSheild();
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
            battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
        }
        else
        {

            #region CHECK FOR ALWAYS
            if (currentCharacterObject.health > 0)
            {
                yield return StartCoroutine(TriggerSingleTargetPassives(
                    S_Passive.PASSIVE_TRIGGER.ALWAYS,
                    currentCharacterObject,
                    currentCharacterObject.transform.position));
                //targetCharacter.SetCharacter(sacrifice.referencePoint);
            }
            #endregion
            currentPartyCharactersQueue.Dequeue();
            currentPartyCharactersQueue.Enqueue(currentCharacterObject);
            
            //yield return StartCoroutine(s_camera.cam.MoveCamera(currentPartyCharactersQueue.Peek().transform.position, 0.9f));
            yield return new WaitForSeconds(0.25f);
            battleEngine = BATTLE_ENGINE_STATE.SELECT_CHARACTER;
        }
    }
    /*
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
    */
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