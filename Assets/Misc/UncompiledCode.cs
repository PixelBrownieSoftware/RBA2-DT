//16/04/2023
/*
public Dictionary<o_battleCharacter, S_Passive> TriggerOtherCharacterPassives(int dmg, o_battleCharacter targ, S_Passive.PASSIVE_TRIGGER[] passiveTriggers)
{
    Dictionary<o_battleCharacter, S_Passive> counters = new Dictionary<o_battleCharacter, S_Passive>();
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
        S_Passive getPassive()
        {
            List<S_Passive> passives = new List<S_Passive>();
            foreach (S_Passive.PASSIVE_TRIGGER trig in passiveTriggers)
            {
                passives.AddRange(bc.GetAllPassives.FindAll(x => x.passiveTrigger == trig));
            }
            foreach (var passiveIndex in passives)
            {
                float perc = UnityEngine.Random.Range(0f, 1f);
                if (perc > passiveIndex.percentage)
                {
                    return passiveIndex;
                }
            }
            return null;
        }

        S_Passive passiveSelected = getPassive();
        if (!passiveSelected)
        {
            continue;
        }
        if (currentMove.move.moveTargScope == s_move.SCOPE_NUMBER.ONE)
        {
            int newDmg = CalculateDamage(currentCharacterObject, bc, currentMove.move, null);
            switch (passiveSelected.passiveSkillType)
            {
                case S_Passive.PASSIVE_TYPE.SACRIFICE:

                    if (targetCharacter.characterRef.health < newDmg && bc.health > newDmg)
                    {
                        if (bc.referencePoint.characterData.elementals[currentMove.move.element] >= 2)
                        {
                            float elementWeakness = bc.referencePoint.characterData.GetElementWeakness(currentMove.move.element);
                            if (minimum_res > elementWeakness)
                            {
                                sacrifice = bc;
                                minimum_res = elementWeakness;
                            }
                        }
                    }
                    break;
            }
        }
    }
    if (sacrifice != null)
    {
        dmg = CalculateDamage(currentCharacterObject, targ, currentMove.move, null);
        return sacrifice;
    }
}
*/
//14/04/2023
/*
 * press turn combo stuff
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
/* guard turn stuff
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
/*
combo stuff
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
/*
more hardcoded status effect stuff
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
/*Old todo related to the above, I removed combo moves.
 * TODO: 
 * If this is a combo move, the target's defence is calculated against the user's stats combineed
 * i.e. if there was a ice strike combo (where Blueler does the magic and redler does the physical attack)
 * it would be combo output = Redler physical output + Blueler magic output
*/
/*
Hardcoded status effects
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
            if (currentMove.move.moveTarg == s_move.MOVE_TARGET.SINGLE) {
                if (targetCharacterObject.health > 0) {

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

//13/03/2023
/*

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
*/

//12/03/2023

/*
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
*/

//04/03/2023

/*
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
*/
/*
public enum MENU_CONTROLL_TYPE
{
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
    switch (mc)
    {
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
    if (menuchoice > (limit - 1))
        menuchoice = 0;

}
*/
/*
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
*/
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
