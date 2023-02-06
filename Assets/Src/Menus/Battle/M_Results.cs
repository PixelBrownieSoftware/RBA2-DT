using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class M_Results : S_MenuSystem
{
    public CH_Func exitToHub;
    public R_CharacterList playerParty;
    public R_CharacterList enemyParty;
    public CH_Text expList;
    public CH_MapTransfer sceneTransition;
    public R_Int day;
    public R_Boolean passDay;

    private void OnEnable()
    {
        exitToHub.OnFunctionEvent += BackToHub;
    }

    private void OnDisable()
    {
        exitToHub.OnFunctionEvent -= BackToHub;
    }

    public override void StartMenu()
    {
        expList.RaiseEvent("");
        base.StartMenu();
        List<string> characters = new List<string>();
        /*
        for (int i = 0; i < playerParty.characterListRef.Count; i++)
        {
            var bc = playerParty.characterListRef[i];
            int exp = 0;
            foreach (var chTarg in enemyParty.characterListRef)
            {
                exp += bc.CalculateExp(chTarg);
            }
            for (float i2 = 0; i2 < exp; i2 += 0.01f)
            {
                bc.expereince += 0.01f;
                if (bc.expereince >= 1)
                {
                    bc.level++;
                    if (EligibleForIncrease(bc.level, bc.attackG))
                    {
                        bc.strength++;
                    }
                    if (EligibleForIncrease(bc.level, bc.defenceG))
                    {
                        bc.defence++;
                    }
                    if (EligibleForIncrease(bc.level, bc.intelligenceG))
                    {
                        bc.intelligence++;
                    }
                    if (EligibleForIncrease(bc.level, bc.speedG))
                    {
                        bc.agility++;
                    }
                    bc.maxHealth += Random.Range(bc.maxHitPointsGMin, bc.maxHitPointsGMax);
                    bc.maxMana += Random.Range(bc.maxManaGMin, bc.maxManaGMax);


                    bc.expereince = 0;
                    exp = 0;
                    foreach (var chTarg in enemyParty.characterListRef)
                    {
                        exp += bc.CalculateExp(chTarg);
                    }
                }

                //EXPList[i].value = bc.exp;
                //yield return new WaitForSeconds(Time.deltaTime * 3.85f);
            }
            characters.Add(bc.charaName + " Level: " + bc.level);
        }
        */
        /*
        string resultTxt = "";
        foreach (string str in characters) {
            resultTxt += str + "\n";
        }
        expList.RaiseEvent(resultTxt);
        */

        foreach (var chTarg in enemyParty.characterListRef)
        {
            chTarg.WipeClean();
        }
        enemyParty.Clear();
    }

    private void BackToHub() {
        if (passDay.boolean){
            day.integer++;
        }
        sceneTransition.RaiseEvent("HubMenu");
    }

    private bool EligibleForIncrease(int lev, float growthStat)
    {
        float statInc = lev * growthStat;
        float prevStatInc = (lev - 1) * growthStat;
        if (Mathf.Floor(statInc) > Mathf.Floor(prevStatInc))
            return true;
        return false;
    }

}
