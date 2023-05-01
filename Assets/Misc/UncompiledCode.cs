//29/04/2023
/*
    public void CalculateAttack(o_battleCharacter targ) {

        s_move mov = currentMove.move;
        int dmg = 0;
        switch (currentMove.move.moveType)
        {
            case s_move.MOVE_TYPE.HP_DAMAGE:
            case s_move.MOVE_TYPE.HP_DRAIN:
            case s_move.MOVE_TYPE.HP_SP_DAMAGE:
            case s_move.MOVE_TYPE.HP_SP_DRAIN:
                {
                    float smirkChance = UnityEngine.Random.Range(0, 1);
                    ELEMENT_WEAKNESS fl = 0;
                    float elementWeakness = 1;
                    elementWeakness = targ.referencePoint.characterData.GetElementWeakness(mov.element);
                    if (targ.referencePoint.sheildAffinity != null)
                    {
                        if (targ.referencePoint.sheildAffinity.Item1 == mov.element)
                            elementWeakness = targ.referencePoint.sheildAffinity.Item2;
                    }
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
                    switch (finalDamageFlag)
                    {
                        case DAMAGE_FLAGS.NONE:
                        case DAMAGE_FLAGS.FRAIL:
                        case DAMAGE_FLAGS.CRITICAL:
                            #region LUCK CHECK
                            {
                                int userLuc = currentCharacterObject.luckNet + 2;
                                int targLuc = targ.luckNet - 1;
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
                            break;
                    }
                }
                dmg = CalculateDamage(targ, currentCharacterObject, currentMove.move, modifier);

                {
                    S_Passive.PASSIVE_TRIGGER[] triggers = {
                     S_Passive.PASSIVE_TRIGGER.ALLY_BEFORE_HIT,
                     S_Passive.PASSIVE_TRIGGER.SELF_BEFORE_HIT
                    };
                    //yield return StartCoroutine(TriggerSingleTargetPassives( targ,triggers, characterPos));
                    //targetCharacter.SetCharacter(sacrifice.referencePoint);
                }
                {
                    int userAgil = currentCharacterObject.agiNet + 3;
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
                DamageEffect(dmg, targ, characterPos, damageFlag);

                break;
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
        }
        if (currentMove.move.moveType == s_move.MOVE_TYPE.HP_DAMAGE)
        {
            if (targ.health <= 0)
                yield break;
        }
        List<float> modifier = new List<float>();
        dmg = CalculateDamage(currentCharacterObject, targ, currentMove.move, null);
        Vector2 characterPos = targ.transform.position;
        switch (currentMove.move.moveType)
        {
            #region ATTACK
            case s_move.MOVE_TYPE.HP_DAMAGE:
            case s_move.MOVE_TYPE.HP_DRAIN:
            case s_move.MOVE_TYPE.HP_SP_DAMAGE:
            case s_move.MOVE_TYPE.HP_SP_DRAIN:
                #region PRESS TURN STUFF
                {
                    float smirkChance = UnityEngine.Random.Range(0, 1);
                    ELEMENT_WEAKNESS fl = 0;
                    float elementWeakness = 1;
                    elementWeakness = targ.referencePoint.characterData.GetElementWeakness(mov.element);
                    if (targ.referencePoint.sheildAffinity != null)
                    {
                        if (targ.referencePoint.sheildAffinity.Item1 == mov.element)
                            elementWeakness = targ.referencePoint.sheildAffinity.Item2;
                    }
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
                                int userLuc = currentCharacterObject.luckNet + 2;
                                int targLuc = targ.luckNet - 1;
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

                #region PASSIVE BEFORE HIT CHECK
                {
                    S_Passive.PASSIVE_TRIGGER[] triggers = {
                     S_Passive.PASSIVE_TRIGGER.ALLY_BEFORE_HIT,
                     S_Passive.PASSIVE_TRIGGER.SELF_BEFORE_HIT
                    };
                    //yield return StartCoroutine(TriggerSingleTargetPassives( targ,triggers, characterPos));
                    //targetCharacter.SetCharacter(sacrifice.referencePoint);
                }
                #endregion

                #region AGILITY DODGE CHECK
                {
                    int userAgil = currentCharacterObject.agiNet + 3;
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
                    enemyWeaknessReveal.AddElementWeakness(targ.referencePoint.characterData.characterDataSource, mov.element);
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
                                foreach (var statusChange in statusChance.status_effect.statusReplace)
                                {
                                    if (targetCharacterObject.HasStatus(statusChange.replace))
                                    {
                                        targetCharacterObject.RemoveStatus(statusChange.replace);
                                        targetCharacterObject.SetStatus(new s_statusEff(
                                            statusChange.toReplace,
                                            UnityEngine.Random.Range(
                                                statusChange.toReplace.minDuration,
                                                statusChange.toReplace.maxDuration),
                                            0));
                                    }
                                }
                            }
                        }
                    }
                    #region ELEMENT STATUS
                    foreach (var statusEff in mov.element.statusInflict)
                    {
                        s_statusEff eff = new s_statusEff();
                        float ch = UnityEngine.Random.Range(0f, 1f);
                        S_StatusEffect status = statusEff.statusEffect;
                        if (ch < statusEff.chance)
                        {
                            if (statusEff.add_remove)
                            {
                                eff.damage = Mathf.CeilToInt(dmg * (status.regenPercentage * -1));
                                eff.duration = status.minDuration;
                                eff.status = status;
                                targ.SetStatus(eff);
                            }
                            else
                            {
                                targ.RemoveStatus(status);
                            }
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
                        yield return StartCoroutine(TriggerSingleTargetPassives(S_Passive.PASSIVE_TRIGGER.SELF_HIT, targ, characterPos));
                        //targetCharacter.SetCharacter(sacrifice.referencePoint);
                    }
                    #endregion
                }
                else
                {
                    yield return StartCoroutine(DodgeAnimation(targ, characterPos));
                    yield return new WaitForSeconds(0.02f);
                }


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

        #region CHECK FOR ON DEFEAT
        if (targ.health <= 0)
        {
            S_Passive.PASSIVE_TRIGGER[] triggers = {
                     S_Passive.PASSIVE_TRIGGER.ALLY_DEFEAT,
                     S_Passive.PASSIVE_TRIGGER.SELF_DEFEAT
                    };
            yield return StartCoroutine(TriggerCharacterPassives(targ, triggers, characterPos));
yield return StartCoroutine(TriggerSingleTargetPassives(S_Passive.PASSIVE_TRIGGER.SELF_DEFEAT, targ, characterPos));
            //targetCharacter.SetCharacter(sacrifice.referencePoint);
        }
        #endregion

        if (targ.health <= 0)
{
    targ.statusEffects.Clear();
    if (oppositionCharacters.Contains(targ))
    {
        s_soundmanager.GetInstance().PlaySound("enemy_defeat");
    }
    else
    {
        s_soundmanager.GetInstance().PlaySound("player_defeat");
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
 */
/*
public IEnumerator ExcecuteMove()
{
    s_move mov = currentMove.move;
    o_battleCharacter user = ReferenceToCharacter(currentCharacter.characterRef);
    o_battleCharacter targ = ReferenceToCharacter(targetCharacter.characterRef);
    s_actionAnim[] preAnimations = null;
    s_actionAnim[] animations = null;
    s_actionAnim[] endAnimations = null;

    if (mov.element.isMagic)
    {
        print("User: " + user);
        print("Target: " + targ);
        print("Move: " + mov);
        user.stamina -= mov.cost;
    }
    else
    {
        user.health -= s_calculation.DetermineHPCost(mov, user.strengthNet, user.vitalityNet, user.maxHealth);
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
    yield return StartCoroutine(DisplayMoveName(mov.name));
    #endregion

    if (!mov.consumeTurn)
    {
        finalDamageFlag = DAMAGE_FLAGS.PASS;
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
    if (animations != null)
    {
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
                    int rand = UnityEngine.Random.Range(2, 5);
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
    }
    else
    {

    }

    currentCharacterObject.SwitchAnimation("idle");

    #region PRESS TURN STUFF

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
    finalDamageFlag = DAMAGE_FLAGS.NONE;
    battleEngine = BATTLE_ENGINE_STATE.END;
}
*/
/// <summary>
/// This returns anyone with an ally trigger
/// </summary>
/// <param name="dmg"></param>
/// <param name="targ">The triggerer</param>
/// <param name="passiveTriggers"></param>
/// <returns></returns>
/*
IEnumerator TriggerCharacterPassives(o_battleCharacter targ, S_Passive.PASSIVE_TRIGGER[] passiveTriggers, Vector2 characterPos) {
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
        S_Passive getPassive() {
            List<S_Passive> passives = new List<S_Passive>();
            foreach (S_Passive.PASSIVE_TRIGGER trig in passiveTriggers)
            {
                passives.AddRange(bc.GetAllPassives.FindAll(x => x.passiveTrigger == trig));
                print(passives[0].name);
            }
            foreach (var passiveIndex in passives) {

                if (usedPassives.ContainsKey(bc) && usedPassives.ContainsValue(passiveIndex))
                {
                    continue;
                }
                float perc = UnityEngine.Random.Range(0f,1f);
                if (perc < passiveIndex.percentage) {
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
            counters.Add(bc, passiveSelected);
        }
    }
    if (counters.Count > 0) {

        foreach (var item in counters)
        {
            yield return StartCoroutine(PassiveSkillDo(item, characterPos));
        }
    }
}
*/
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(o_locationOverworld))]
[CanEditMultipleObjects]
public class ed_tp : Editor
{
    public o_locationOverworld[] Locations;
    private void OnSceneGUI()
    {
        if (Locations == null)
            Locations = FindObjectsOfType<o_locationOverworld>();
        else if (Locations.Length > 0)
        {

            for (int i = 0; i < Locations.Length; i++)
            {
                if (Locations[i].south != null) {
                    Handles.DrawLine(Locations[i].transform.position, Locations[i].south.transform.position);
                }
                if (Locations[i].north != null)
                {
                    Handles.DrawLine(Locations[i].transform.position, Locations[i].north.transform.position);
                }
                if (Locations[i].west != null)
                {
                    Handles.DrawLine(Locations[i].transform.position, Locations[i].west.transform.position);
                }
                if (Locations[i].east != null)
                {
                    Handles.DrawLine(Locations[i].transform.position, Locations[i].east.transform.position);
                }
            }
        }
    }


    public o_locationOverworld[] otherLocations;
    public o_locationOverworld targ;
    public enum DIR {
        NORTH,
        SOUTH,
        WEST,
        EAST
    }
    public DIR direction;

    public bool IsDir(o_locationOverworld loc) {
        if (targ.north != loc && 
            targ.south != loc && 
            targ.east != loc && 
            targ.west != loc)
            return false;
        return true;
    }

    public override void OnInspectorGUI()
    {
        if (targ == null)
            targ = (o_locationOverworld)target;
        else
        {
            if (otherLocations == null)
                otherLocations = FindObjectsOfType<o_locationOverworld>();
            else
            {
                EditorGUILayout.LabelField("Current connections");

                if (targ.north != null)
                {
                    if (GUILayout.Button(targ.north.mapName))
                    {
                        targ.north.south = null;
                        targ.north = null;
                    }
                }
                if (targ.west != null)
                {
                    if (GUILayout.Button(targ.west.mapName))
                    {
                        targ.west.east = null;
                        targ.west = null;
                    }
                }
                if (targ.south != null)
                {
                    if (GUILayout.Button(targ.south.mapName))
                    {
                        targ.south.north = null;
                        targ.south = null;
                    }
                }
                if (targ.east != null)
                {
                    if (GUILayout.Button(targ.east.mapName))
                    {
                        targ.east.west = null;
                        targ.east = null;
                    }
                }

                EditorGUILayout.LabelField("Unconnected Nodes");
                direction = (DIR)EditorGUILayout.EnumPopup("", direction);
                for (int i = 0; i < otherLocations.Length; i++)
                {
                    if (otherLocations[i] == targ && IsDir(otherLocations[i]))
                        continue; 
                    if (GUILayout.Button(otherLocations[i].mapName))
                    {
                        switch (direction)
                        {
                            case DIR.NORTH:
                                if (targ.north == null)
                                {
                                    targ.north = otherLocations[i];
                                    targ.north.south = targ;
                                }
                                break;
                            case DIR.WEST:
                                if (targ.west == null)
                                {
                                    targ.west = otherLocations[i];
                                    targ.west.east = targ;
                                }
                                break;
                            case DIR.SOUTH:
                                if (targ.south == null)
                                {
                                    targ.south = otherLocations[i];
                                    targ.south.north = targ;
                                }
                                break;
                            case DIR.EAST:
                                if (targ.east == null)
                                {
                                    targ.east = otherLocations[i];
                                    targ.east.west = targ;
                                }
                                break;
                        }
                    }
                }
            }
        }
        Repaint();
        base.OnInspectorGUI();
    }
}
*/
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
