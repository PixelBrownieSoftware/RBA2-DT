using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MagnumFoundation2.System;
using MagnumFoundation2.System.Core;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public struct s_moveComb
{
    public s_move.MOVE_QUANITY_TYPE comboType;
    public o_battleCharacter user1;
    public s_move user1Move;
    public o_battleCharacter user2;
    public s_move user2Move;
    public o_battleCharacter user3;
    public s_move user3Move;
    public o_battleCharacter user4;
    public s_move user4Move;
    public o_battleCharacter user5;
    public s_move user5Move;

    public s_moveComb(o_battleCharacter user1, o_battleCharacter user2, s_move user1Move, s_move user2Move)
    {
        this.user1 = user1;
        this.user2 = user2;
        user3 = null;
        user4 = null;
        user5 = null;
        this.user1Move = user1Move;
        this.user2Move = user2Move;
        user3Move = null;
        user4Move = null;
        user5Move = null;
        comboType = s_move.MOVE_QUANITY_TYPE.DUAL_TECH;
    }
    public s_moveComb(o_battleCharacter user1, o_battleCharacter user2, o_battleCharacter user3, s_move user1Move, s_move user2Move, s_move user3Move)
    {
        this.user1 = user1;
        this.user2 = user2;
        this.user3 = user3;
        user4 = null;
        user5 = null;
        this.user1Move = user1Move;
        this.user2Move = user2Move;
        this.user3Move = user3Move;
        user4Move = null;
        user5Move = null;
        comboType = s_move.MOVE_QUANITY_TYPE.TRIPLE_TECH;
    }
}

[System.Serializable]
public class s_RPGSave : dat_save {
    [System.Serializable]
    public struct sav_shopItem
    {
        public string name;
        public float price;
        public sav_shopItem(string name, float price)
        {
            this.name = name;
            this.price = price;
        }
    }
    [System.Serializable]
    public struct sav_item
    {
        public string name;
        public int amount;
        public sav_item(string name, int amount)
        {
            this.name = name;
            this.amount = amount;
        }
    }
    [System.Serializable]
    public struct sav_skill
    {
        public string character;
        public string name;
        public sav_skill(string character, string name) {
            this.character = character;
            this.name = name;
        }
    }
    [System.Serializable]
    public class sav_party
    {
        public string name;
        public int level;
        public float experience;
        public int health;
        public int stamina;

        public int strength;
        public int vitality;
        public int dexterity;
        public int agility;
        public int luck;
        public bool inBattle;

        public string characterDataSource;
        public List<string> currentMoves = new List<string>();
        public string currentPhysWeapon;
        public string currentRangedWeapon;
    }

    public float money;
    public sav_party[] party_members;
    public sav_shopItem[] shop_items;
    public sav_item[] inventory;
    public sav_skill[] extraSkills;
    public string[] weapons;
    public int extraSkillAmount;

    public s_RPGSave(
        List<o_battleCharPartyData> partyMembers,
        List<s_move> extraMoves,
        List<s_shopItem> shopItems,
        Dictionary<string, int> inventoryItems,
        List<string> weapons,
        float money)
    {
        List<sav_party> partySave = new List<sav_party>();
        foreach (var a in partyMembers) {
            sav_party mem = new sav_party();
            mem.health = a.maxHealth;
            mem.stamina = a.maxStamina;
            mem.name = a.name;
            mem.inBattle = a.inBattle;

            mem.strength = a.strength;
            mem.vitality = a.vitality;
            mem.dexterity = a.dexterity;
            mem.agility = a.agility;

            mem.characterDataSource = a.characterDataSource.name;

            foreach (var mv in a.currentMoves) {
                mem.currentMoves.Add(mv.name);
            }
            
            if(a.currentPhysWeapon != null)
                mem.currentPhysWeapon = a.currentPhysWeapon.name;
            if (a.currentRangeWeapon != null)
                mem.currentRangedWeapon = a.currentRangeWeapon.name;

            partySave.Add(mem);
        }

        List<sav_skill> exMoves = new List<sav_skill>();
        foreach (var exMV in extraMoves)
        {
            o_battleCharPartyData dat = partyMembers.Find(x => x.extraSkills.Contains(exMV));
            if (dat != null)
            {
                exMoves.Add(new sav_skill(dat.name, exMV.name));
            }
            else {
                exMoves.Add(new sav_skill("", exMV.name));
            }
        }

        List<sav_shopItem> savedShopItems = new List<sav_shopItem>();
        foreach (var shopIt in shopItems) {
            savedShopItems.Add(new sav_shopItem(shopIt.item.name, shopIt.price));
        }

        List<sav_item> savedItems = new List<sav_item>();
        foreach (var shopIt in inventoryItems)
        {
            savedItems.Add(new sav_item(shopIt.Key, shopIt.Value));
        }

        this.money = money;
        party_members = partySave.ToArray();
        this.weapons = weapons.ToArray();
        shop_items = savedShopItems.ToArray();
        inventory = savedItems.ToArray();
        extraSkills = exMoves.ToArray();
    }
}

public class s_rpgGlobals : s_globals
{
    public static s_rpgGlobals rpgGlSingleton;
    public R_CharacterSetterList allCharactersData;
    public R_CharacterSetterList partyMembersStart;
    public R_BattleCharacterList partyMembers;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public List<string> weapons = new List<string>();
    public List<s_move> itemDatabase = new List<s_move>();
    public List<o_weapon> weaponDatabase = new List<o_weapon>();
    public List<s_move> moveDatabase = new List<s_move>();

    public List<s_shopItem> shopItems = new List<s_shopItem>();

    public List<s_move> extraSkills = new List<s_move>();
    public List<s_passive> extraPassives = new List<s_passive>();

    //So we can set it's object state once a level has been completed
    public o_locationOverworld locationObjectName;
    public static float money;
    public Text moneyTxt;

    public s_move[] comboMoveData;
    public s_passive[] passiveMoveData;
    public s_menuButton[] buttonObjects;
    public int extraSkillAmount = 4;
    public int extraPassiveSkillAmount = 4;
    public Sprite BGBattle;

    public CH_Func gotoBattleChannel;
    public CH_Text changeMenu;
    public R_EnemyGroup enemyGroupSelect;

    private void OnEnable()
    {
        gotoBattleChannel.OnFunctionEvent += SwitchToBattle;
    }

    private void OnDisable()
    {
        gotoBattleChannel.OnFunctionEvent -= SwitchToBattle;
    }

    private new void Update()
    {
        base.Update();
        if (Input.GetKeyDown(GetKeyPref("select"))) {
            //print("COMBO!");
            /*
            foreach (Tuple<s_moveComb, s_move> mov in CheckComboRequirementsParty(s_battleEngine.engineSingleton.playerCharacters))
            {
                string str = "Move name: " + mov.Item2 + " User 1: " + mov.Item1.user1 + " User 2: " + mov.Item1.user2;
                
                //print(str);
            }
            */
        }
        moneyTxt.text = "£" + money;
    }

    public override void SaveData()
    {
        try
        {
            FileStream fs = new FileStream(saveDataName, FileMode.Create);
            BinaryFormatter bin = new BinaryFormatter();

            s_RPGSave sav = new s_RPGSave(partyMembers.battleCharList, extraSkills, shopItems, inventory, weapons, money);

            List<Tuple<string, float>> dvF = new List<Tuple<string, float>>();
            List<Tuple<string, int>> dvI = new List<Tuple<string, int>>();

            sav.gbflg = new dat_globalflags(GlobalFlags);
            sav.currentmap = currentMapName;
            print(objectStates.Count);
            sav.trigStates = objectStates;
            bin.Serialize(fs, sav);
            fs.Close();

        } catch (Exception e) {
            print(e);
        }

        /*s
        new dat_globalflags(GlobalFlags), (int)player.health, (int)player.maxHealth, lev.mapDat.name,  
        
        if (isFixedSaveArea)
            sav.currentmap = fixedSaveAreaName;
        */

    }

    public override void StartStuff()
    {
        base.StartStuff();


        if (s_mainmenu.isload)
        {
            s_RPGSave sav = (s_RPGSave)s_mainmenu.save;
            weapons.AddRange(sav.weapons);
            foreach (var s in sav.party_members)
            {
                AddPartyMember(s);
            }
            player.transform.position = new Vector3(sav.location.x, sav.location.y);
            extraSkillAmount = sav.extraSkillAmount;
            money = sav.money;
            if (sav.extraSkills != null)
            {
                foreach (var it in sav.extraSkills)
                {
                    s_move mov = moveDatabase.Find(x => x.name == it.name);
                    extraSkills.Add(mov);
                    if (it.character != "") {
                        o_battleCharPartyData pc = partyMembers.Get(it.character);
                        pc.extraSkills.Add(mov);
                    }
                }
            }
            if (sav.shop_items != null)
            {
                foreach (var it in sav.shop_items)
                {
                    shopItems.Add(new s_shopItem(it.price, itemDatabase.Find(x => x.name == it.name)));
                }
            }
            if (sav.inventory != null)
            {
                foreach (var it in sav.inventory)
                {
                    AddItem(it.name, it.amount);
                }
            }
        }
        else{
            AddItem("Medicine", 5);
            AddItem("Energy drink", 5);
            foreach (var ind in partyMembersStart.characterSetters)
            {
                AddPartyMember(ind, 1);
            }
        }
        s_menuhandler.GetInstance().SwitchMenu("OverworldSelection");
    }

    public new void Awake()
    {
        base.Awake();
        if (rpgGlSingleton == null) {
            rpgGlSingleton = this;
            DontDestroyOnLoad(gameObject);
            //AddPartyMember(partyMemberBaseData[3], 35);
            //AddPartyMember(partyMemberBaseData[4], 55);
            //AddPartyMember(partyMemberBaseData[5], 45);
            //AddPartyMember(partyMemberBaseData[6], 35);
            AddItem("Medicine", 5);
            AddItem("Energy drink", 5);
            foreach (var ind in partyMembersStart.characterSetters)
            {
                AddPartyMember(ind, 1);
            }
            print("This is cool");
        } else {
            Destroy(gameObject);
        }
    }

    public void AddExtraSkill(s_move mov) {
        if (!extraSkills.Contains(mov))
            extraSkills.Add(mov);
    }

    public void SwitchToOverworld(bool isFlee)
    {
        SceneManager.UnloadSceneAsync("battle_scene");
        if (s_battleEngine.engineSingleton.enemyGroup.sceneToGoTo == "")
        {
            s_menuhandler.GetInstance().SwitchMenu("OverworldSelection");
        } 
        if (!isFlee)
            if (locationObjectName != null)
                locationObjectName.isDone = true;
        s_menuhandler.GetInstance().SwitchMenu("EMPTY");
        //rpgScene.SetActive(false);
        //rpgSceneGUI.SetActive(false);
        //overWorld.SetActive(true);
        /*
        if(!isFlee)
            locationObjectName.isDone = true;
        
        */
        s_battleEngine.engineSingleton.isEnabled = false;
        s_camera.cam.ZoomCamera(-1);
        s_camera.cam.cameraMode = s_camera.CAMERA_MODE.CHARACTER_FOCUS;
        if (locationObjectName != null)
            AddTriggerState(new triggerState(locationObjectName.name, "Overworld", true));
        SaveData();

        SceneManager.LoadSceneAsync("Overworld", LoadSceneMode.Additive);
        changeMenu.RaiseEvent("Hub");
        /*
        if (s_battleEngine.engineSingleton.enemyGroup.sceneToGoTo != "")
            SceneManager.LoadScene(s_battleEngine.engineSingleton.enemyGroup.sceneToGoTo);
        */
    }
    public override void ClearAllThings()
    {
        base.ClearAllThings();
        partyMembers.Clear();
    }

    public void SetLocationObject(o_locationOverworld lc)
    {
        locationObjectName = lc;
    }

    public IEnumerator SwitchToBattle(s_enemyGroup gr)
    {
        StartCoroutine(s_triggerhandler.GetInstance().Fade(Color.black, 0.25f));
        yield return StartCoroutine(s_camera.GetInstance().ZoomCamera(20, 0.6f));
        //yield return SceneManager.UnloadSceneAsync("Overworld", UnloadSceneOptions.None);
        yield return SceneManager.LoadSceneAsync("battle_scene", LoadSceneMode.Additive);
        s_battleEngine.engineSingleton.enemyGroup = gr;
        s_battleEngine.engineSingleton.isEnabled = true;
        s_battleEngine.engineSingleton.nonChangablePlayers = false;
        s_battleEngine.engineSingleton.StartCoroutine(s_battleEngine.engineSingleton.StartBattle());
    }
    public IEnumerator SwitchToBattle(s_enemyGroup gr, o_locationOverworld lc) {
        locationObjectName = lc;
        yield return SwitchToBattle(gr);
    }

    public void SwitchToBattle() {
        StartCoroutine(SwitchToBattle(enemyGroupSelect.enemyGroup));
    }

    public void SetActivePartyMember(o_battleCharPartyData bc) {
        if (bc.inBattle) {
            if (partyMembers.GetActiveCount() > 1)
                bc.inBattle = false;
        }
        else
        {
            if (partyMembers.GetActiveCount() < 5)
            {
                bc.inBattle = true;
            }
        }
    }

    public void AddPartyMember(s_RPGSave.sav_party data)
    {
        o_battleCharPartyData newCharacter = new o_battleCharPartyData();
        {
            newCharacter.name = data.name;
            int tempHP = data.health;
            int tempSP = data.stamina;
            int tempStr = data.strength;
            int tempVit = data.vitality;
            int tempDx = data.dexterity;
            int tempAgi = data.agility;
            foreach (var pd in allCharactersData.characterSetters) {

                if (pd.name == data.characterDataSource)
                {
                    newCharacter.characterDataSource = pd;
                    break;
                } 
            }

            if (data.currentPhysWeapon != "")
            {
                newCharacter.currentPhysWeapon = GetWeapon(data.currentPhysWeapon);
            }
            if (data.currentRangedWeapon != "")
            {
                newCharacter.currentRangeWeapon = GetWeapon(data.currentRangedWeapon);
            }
            
            //newCharacter.inBattle = true;

            newCharacter.currentMoves = new List<s_move>();
            foreach (string pd in data.currentMoves)
            {
                newCharacter.currentMoves.Add(moveDatabase.Find(x => x.name == pd));
            }
            newCharacter.health = newCharacter.maxHealth = tempHP;
            newCharacter.stamina = newCharacter.maxStamina = tempSP;

            newCharacter.strength = tempStr;
            newCharacter.vitality = tempVit;
            newCharacter.dexterity = tempDx;
            newCharacter.agility = tempAgi;
            newCharacter.inBattle = data.inBattle;
        }
        partyMembers.Add(newCharacter);
    }
    public void AddPartyMember(o_battleCharDataN data, int level) {

        o_battleCharPartyData newCharacter = new o_battleCharPartyData();
        {
            {
                if (partyMembers.GetActiveCount() < 5)
                {
                    newCharacter.inBattle = true;
                }
                else {
                    newCharacter.inBattle = false;
                }
            }
            newCharacter.name = data.name;
            int tempHP = data.maxHitPointsB;
            int tempSP = data.maxSkillPointsB;

            int tempHPMin = data.maxHitPointsGMin;
            int tempSPMin = data.maxSkillPointsGMin;
            int tempHPMax = data.maxHitPointsGMax;
            int tempSPMax = data.maxSkillPointsGMax;

            int tempStr = data.strengthB;
            int tempVit = data.vitalityB;
            int tempDx = data.dexterityB;
            int tempLuc = data.luckB;
            int tempAgi = data.agilityB;
            int tempInt = data.intelligenceB;

            for (int i = 0; i < level; i++)
            {
                tempHP += UnityEngine.Random.Range(tempHPMin, tempHPMax);
                tempSP += UnityEngine.Random.Range(tempSPMin, tempSPMax);

                if (data.strengthGT % level == 0)
                    tempStr++;
                if (data.vitalityGT % level == 0)
                    tempVit++;
                if (data.dexterityGT % level == 0)
                    tempDx++;
                if (data.luckGT % level == 0)
                    tempLuc++;
                if (data.agilityGT % level == 0)
                    tempAgi++;
                if (data.intelligenceGT % level == 0)
                    tempInt++;
            }

            if (data.defaultRangedWeapon != null)
            {
                AddWeapon(data.defaultRangedWeapon.name);
                newCharacter.currentRangeWeapon = data.defaultRangedWeapon;
            }
            if (data.defaultRangedWeapon != null)
            {
                AddWeapon(data.defaultRangedWeapon.name);
                newCharacter.currentRangeWeapon = data.defaultRangedWeapon;
            }

            //newCharacter.inBattle = true;
            //newCharacter.elementAffinities = data.elementAffinities;
            newCharacter.currentMoves = new List<s_move>();
            
            foreach (s_move mov in data.moveLearn)
            {
                if (mov == null)
                    continue;
                /*
                if (mov.strReq <= tempStr
                    && mov.dxReq <= tempDx
                    && mov.vitReq <= tempVit
                    && mov.agiReq <= tempAgi)
                {
                }
                */
                    newCharacter.currentMoves.Add(mov);
            }
            newCharacter.health = newCharacter.maxHealth = tempHP;
            newCharacter.stamina = newCharacter.maxStamina = tempSP;

            newCharacter.strength = tempStr;
            newCharacter.vitality = tempVit;
            newCharacter.dexterity = tempDx;
            newCharacter.agility = tempAgi;
        }
        newCharacter.characterDataSource = data;
        partyMembers.Add(newCharacter);
    }
    public void SetPartyMemberStats(o_battleCharacter data)
    {
        o_battleCharPartyData newCharacter = partyMembers.Get(data.name);
        {
            newCharacter.name = data.name;

            newCharacter.strength = data.strength;
            newCharacter.vitality = data.vitality;
            newCharacter.dexterity = data.dexterity;


            newCharacter.health = data.health;
            newCharacter.maxHealth = data.maxHealth;
            newCharacter.stamina = data.stamina;
            newCharacter.maxStamina = data.maxStamina;

            newCharacter.currentMoves = data.currentMoves;
        }
    }
    List<s_move> FindComboMoveReqList(s_move.moveRequirement req, o_battleCharacter pc)
    {
        List<s_move> mov = new List<s_move>();
        List<s_move> userMoves = new List<s_move>();
        userMoves.AddRange(pc.currentMoves);
        userMoves.AddRange(pc.extraSkills);
        switch (req.type)
        {
            case s_move.moveRequirement.MOVE_REQ_TYPE.ELEMENTAL:
                mov.AddRange(userMoves.FindAll(x => x.element == req.element));
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.WEAPON_PHYS:
                if (pc.physWeapon != null)
                {
                    if (pc.physWeapon.weaponType == req.weapType)
                    {
                        mov.Add(pc.physWeapon);
                    }
                }
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.WEAPON_RANGE:
                if (pc.rangedWeapon != null)
                {
                    if (pc.rangedWeapon.weaponType == req.weapType)
                    {
                        mov.Add(pc.rangedWeapon);
                    }
                }
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.HEAL_SP:
                {
                    s_move mv = userMoves.Find(x => x.moveType == s_move.MOVE_TYPE.STATUS &&
                (x.statusType == s_move.STATUS_TYPE.HEAL_STAMINA
                || x.statusType == s_move.STATUS_TYPE.HEAL_SP_BUFF));

                    if (mv != null)
                    {
                        mov.Add(mv);
                    }
                }
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.HEAL_HP:
                {
                    s_move mv = userMoves.Find(x => x.moveType == s_move.MOVE_TYPE.STATUS &&
                (x.statusType == s_move.STATUS_TYPE.HEAL_HEALTH
                || x.statusType == s_move.STATUS_TYPE.HEAL_HP_BUFF));

                    if (mv != null)
                    {
                        mov.Add(mv);
                    }
                }
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.SPECIFIC:
                if (userMoves.Find(x => x == req.move) != null)
                {
                    mov.Add(userMoves.Find(x => x == req.move));
                }
                break;
        }
        return mov;
    }

    s_move FindComboMoveReq(s_move.moveRequirement req, o_battleCharacter pc) {
        List<s_move> allMV = new List<s_move>();
        allMV.AddRange(pc.currentMoves);
        allMV.AddRange(pc.extraSkills);
        switch (req.type)
        {
            case s_move.moveRequirement.MOVE_REQ_TYPE.ELEMENTAL:
                return allMV.Find(x => x.element == req.element);

            case s_move.moveRequirement.MOVE_REQ_TYPE.SPECIFIC:
                return allMV.Find(x => x == req.move);

            case s_move.moveRequirement.MOVE_REQ_TYPE.WEAPON_PHYS:
                if(pc.physWeapon != null)
                    return pc.physWeapon;
                break;
            case s_move.moveRequirement.MOVE_REQ_TYPE.WEAPON_RANGE:
                if (pc.rangedWeapon != null)
                    return pc.rangedWeapon;
                break;
            case s_move.moveRequirement.MOVE_REQ_TYPE.BUFF:
                return allMV.Find(x => x.moveType == s_move.MOVE_TYPE.STATUS && x.statusType == s_move.STATUS_TYPE.BUFF);
            case s_move.moveRequirement.MOVE_REQ_TYPE.DEBUFF:
                return allMV.Find(x => x.moveType == s_move.MOVE_TYPE.STATUS && x.statusType == s_move.STATUS_TYPE.DEBUFF);
            case s_move.moveRequirement.MOVE_REQ_TYPE.HEAL_ANY:
                return allMV.Find(x => x.moveType == s_move.MOVE_TYPE.STATUS && 
                (x.statusType == s_move.STATUS_TYPE.HEAL_HEALTH
                || x.statusType == s_move.STATUS_TYPE.HEAL_HP_BUFF
                || x.statusType == s_move.STATUS_TYPE.HEAL_STAMINA
                || x.statusType == s_move.STATUS_TYPE.HEAL_SP_BUFF));
            case s_move.moveRequirement.MOVE_REQ_TYPE.HEAL_HP:
                return allMV.Find(x => x.moveType == s_move.MOVE_TYPE.STATUS &&
                (x.statusType == s_move.STATUS_TYPE.HEAL_HEALTH
                || x.statusType == s_move.STATUS_TYPE.HEAL_HP_BUFF));
            case s_move.moveRequirement.MOVE_REQ_TYPE.HEAL_SP:
                return allMV.Find(x => x.moveType == s_move.MOVE_TYPE.STATUS &&
                (x.statusType == s_move.STATUS_TYPE.HEAL_STAMINA
                || x.statusType == s_move.STATUS_TYPE.HEAL_SP_BUFF));
        }
        return null;
    }

    /*
    public List<Tuple<s_moveComb, s_move>> CheckComboRequirementsParty(List<o_battleCharacter> members) {
        List<Tuple<s_moveComb, s_move>> movs = new List<Tuple<s_moveComb, s_move>>();
        foreach (s_move m in comboMoveData)
        {
            //Check if any party member fufils the first condition
            switch (m.comboType) {
                case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:

                    foreach (o_battleCharacter pc in members)
                    {
                        //Try and find all the possible combinations for character 1
                        List<s_move> movList1a= FindComboMoveReq(m.Req1, pc);
                        List<s_move> movList1b= FindComboMoveReq(m.Req2, pc);
                        List<s_move> movList1 = null;

                        if (movList1a != null)
                        {
                            if (movList1 == null)
                                movList1 = new List<s_move>();
                            movList1.AddRange(movList1a);
                        }

                        if (movList1b != null) {
                            if (movList1 == null)
                                movList1 = new List<s_move>();
                            movList1.AddRange(movList1b);
                        }

                        if (movList1 != null)
                        {
                            if (movList1.Count > 0)
                            {
                                foreach (s_move m1 in movList1)
                                {
                                    //If this condition is fufilled, then find the requirement for the 2nd party member
                                    foreach (o_battleCharacter pc2 in members)
                                    {
                                        if (pc2 == pc)
                                            continue;
                                        List<s_move> movList2 = FindComboMoveReq(m.Req2, pc2);
                                        if (movList2 != null)
                                        {
                                            if (movList2.Count > 0)
                                            {
                                                foreach (s_move m2 in movList2)
                                                {
                                                    if (m2 == null)
                                                        continue;
                                                    //print(m2);
                                                    movs.Add(new Tuple<s_moveComb, s_move>(new s_moveComb(pc, pc2, m1, m2), m));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

        }
        return movs;
    }
    public List<Tuple<s_moveComb, s_move>> CheckComboRequirementsCharacter(o_battleCharacter PriUser,List<o_battleCharacter> members)
    {
        List<Tuple<s_moveComb, s_move>> movs = new List<Tuple<s_moveComb, s_move>>();
        foreach (s_move m in comboMoveData)
        {
            //Check if any party member fufils the first condition
            switch (m.comboType)
            {
                case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:

                    //Try and find all the possible combinations for character 1
                    List<s_move> movList1 = FindComboMoveReq(m.Req1, PriUser);
                    if (movList1 != null)
                    {
                        if (movList1.Count > 0)
                        {
                            foreach (s_move m1 in movList1)
                            {
                                //If this condition is fufilled, then find the requirement for the 2nd party member
                                foreach (o_battleCharacter pc2 in members)
                                {
                                    if (pc2 == PriUser)
                                        continue;
                                    List<s_move> movList2 = FindComboMoveReq(m.Req2, pc2);
                                    if (movList2 != null)
                                    {
                                        if (movList2.Count > 0)
                                        {
                                            foreach (s_move m2 in movList2)
                                            {
                                                if (m2 == null)
                                                    continue;
                                                //print(m2);
                                                movs.Add(new Tuple<s_moveComb, s_move>(new s_moveComb(PriUser, pc2, m1, m2), m));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

        }
        return movs;
    }
    public List<Tuple<s_moveComb, s_move>> CheckComboRequirementsCharacter2(o_battleCharacter PriUser, List<o_battleCharacter> members)
    {
        List<Tuple<s_moveComb, s_move>> movs = new List<Tuple<s_moveComb, s_move>>();
        foreach (s_move m in comboMoveData)
        {
            //Check if any party member fufils the first condition
            switch (m.comboType)
            {
                case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:

                    //Try and find all the possible combinations for character 1
                    List<s_move> movList1 = FindComboMoveReq(m.Req1, PriUser);
                    if (movList1 != null)
                    {
                        if (movList1.Count > 0)
                        {
                            foreach (s_move m1 in movList1)
                            {
                                //If this condition is fufilled, then find the requirement for the 2nd party member
                                foreach (o_battleCharacter pc2 in members)
                                {
                                    if (pc2 == PriUser)
                                        continue;
                                    List<s_move> movList2 = FindComboMoveReq(m.Req2, pc2);
                                    if (movList2 != null)
                                    {
                                        if (movList2.Count > 0)
                                        {
                                            foreach (s_move m2 in movList2)
                                            {
                                                if (m2 == null)
                                                    continue;
                                                print(m1.name + " and " + m2.name);
                                                movs.Add(new Tuple<s_moveComb, s_move>(new s_moveComb(PriUser, pc2, m1, m2), m));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    List<s_move> movList1b = FindComboMoveReq(m.Req2, PriUser);
                    if (movList1b != null)
                    {
                        if (movList1b.Count > 0)
                        {
                            foreach (s_move m1 in movList1b)
                            {
                                //If this condition is fufilled, then find the requirement for the 2nd party member
                                foreach (o_battleCharacter pc2 in members)
                                {
                                    if (pc2 == PriUser)
                                        continue;
                                    List<s_move> movList2 = FindComboMoveReq(m.Req1, pc2);
                                    if (movList2 != null)
                                    {
                                        if (movList2.Count > 0)
                                        {
                                            foreach (s_move m2 in movList2)
                                            {
                                                if (m2 == null)
                                                    continue;
                                                print(m1.name + " and " + m2.name);
                                                movs.Add(new Tuple<s_moveComb, s_move>(new s_moveComb(PriUser, pc2, m1, m2), m));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

        }
        return movs;
    }
    */
    public List<Tuple<s_moveComb, s_move>> CheckComboRequirementsCharacter3(o_battleCharacter PriUser, List<o_battleCharacter> members)
    {
        List<Tuple<s_moveComb, s_move>> movs = new List<Tuple<s_moveComb, s_move>>();
        List<s_move> allMoveData = new List<s_move>();
        allMoveData.AddRange(comboMoveData);
        allMoveData.AddRange(moveDatabase);

        foreach (s_move m in allMoveData)
        {
            if (m.moveRequirements != null)
            {
                if (m.moveRequirements.Length > 0)
                {
                    foreach (var cmbo in m.moveRequirements)
                    {
                        switch (cmbo.comboType)
                        {
                            case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:
                                List<s_move> mo = FindComboMoveReqList(cmbo.Req1, PriUser);
                                if (mo != null)
                                {
                                    foreach (s_move mov in mo)
                                    {
                                        foreach (o_battleCharacter bc in members)
                                        {
                                            if (bc == PriUser)
                                                continue;
                                            s_move mo2 = FindComboMoveReq(cmbo.Req2, bc);
                                            if (mo2 == null)
                                                continue;
                                            s_moveComb cmb = new s_moveComb(PriUser, bc, mov, mo2);
                                            //print(
                                            //    cmb.user1.name + " (" + cmb.user1Move.name + ") " +
                                            //    cmb.user2.name + " (" + cmb.user2Move.name + ") ");
                                            Tuple<s_moveComb, s_move> moveThing = new Tuple<s_moveComb, s_move>(cmb, m);
                                            movs.Add(moveThing);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }

        }
        return movs;
    }

    public void AddItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]+= amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }
    }
    public void AddItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else {
            inventory.Add(itemName, 1);
        }
    }
    public Tuple<s_move, int> GetItem(string itemName)
    {
        if (!inventory.ContainsKey(itemName)) {
            return null;
        }
        return new Tuple<s_move, int>(itemDatabase.Find(x => x.name == itemName), inventory[itemName]);
    }
    public void UseItem(string itemName)
    {
        inventory[itemName]--;
    }
    public List<s_move> GetItems() {
        List<s_move> rpgItems = new List<s_move>();
        foreach (KeyValuePair<string, int> val in inventory) {
            if (val.Value == 0)
                continue;
            rpgItems.Add(GetItem(val.Key).Item1);
        }
        return rpgItems;
    }

    public List<o_weapon> GetWeapons()
    {
        List<o_weapon> weaps = new List<o_weapon>();
        foreach (string val in weapons)
        {
            weaps.Add(GetWeapon(val));
        }
        return weaps;
    }
    public o_weapon GetWeapon(string itemName) {
        if (weapons.Contains(itemName))
        {
            return weaponDatabase.Find(x => x.name == itemName);
        }
        else return null;
    }
    public void AddWeapon(string itemName)
    {
        if (!weapons.Contains(itemName))
        {
            weapons.Add(itemName);
        }
    }
    /*
    [MenuItem("AssetDatabase/LoadAssetExample")]
    static void ImportExample()
    {
        Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/texture.jpg", typeof(Texture2D));
    }

    public void AddMemeber( data, int level)
    {
        o_battleCharData newCharacter = new o_battleCharData();
        {
            newCharacter.level = level;
            newCharacter.name = data.name;
            int tempHP = data.maxHitPointsB;
            int tempSP = data.maxSkillPointsB;
            int tempStr = charaData.strengthB;
            int tempVit = charaData.vitalityB;
            int tempDx = charaData.dexterityB;
            int tempAg = charaData.agilityB;
            int tempGut = data.gutsB;
            int tempLuc = data.luckB;

            newCharacter.inBattle = true;

            newCharacter.currentMoves = new List<s_move>();

            foreach (o_battleChar.move_learn mov in data.moveDatabase)
            {
                if (mov.level <= level)
                    newCharacter.currentMoves.Add(mov.move);
            }

            for (int i = 1; i < level; i++)
            {
                if (i % data.attackG == 0)
                    tempStr++;
                if (i % data.defenceG == 0)
                    tempVit++;
                if (i % data.intelligenceG == 0)
                    tempDx++;
                if (i % data.speedG == 0)
                    tempAg++;
                if (i % data.gutsG == 0)
                    tempGut++;
                if (i % data.luckG == 0)
                    tempLuc++;

                tempHP += UnityEngine.Random.Range(data.maxHitPointsGMin, data.maxHitPointsGMax);
                tempSP += UnityEngine.Random.Range(data.maxSkillPointsGMin, data.maxSkillPointsGMax);
            }
            newCharacter.hitPoints = newCharacter.maxHitPoints = tempHP;
            newCharacter.skillPoints = newCharacter.maxSkillPoints = tempSP;

            newCharacter.attack = tempStr;
            newCharacter.defence = tempVit;
            newCharacter.intelligence = tempDx;
            newCharacter.speed = tempAg;
            newCharacter.guts = tempGut;
            newCharacter.luck = tempLuc;
        }
        newCharacter.dataSrc = data;
        partyMembers.Add(newCharacter);
    }
    */

    public IEnumerator BattleTransition() {

        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("BattleScene");
    }
}
