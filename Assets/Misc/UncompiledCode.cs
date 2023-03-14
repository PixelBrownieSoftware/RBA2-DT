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
