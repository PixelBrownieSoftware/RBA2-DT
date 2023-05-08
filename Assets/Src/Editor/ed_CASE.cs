using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

/*
[System.Serializable]
public struct s_characterData {

    public s_characterData(List<o_battleCharData> characterdata) {
        this.characterdata = new List<o_battleCharData>();
        this.characterdata = characterdata;
    }
    public List<o_battleCharData> characterdata;
}
*/
[System.Serializable]
public struct s_skillData
{
    public s_skillData(List<s_move> skilldata)
    {
        this.skilldata = new List<s_move>();
        this.skilldata = skilldata;
    }
    public List<s_move> skilldata;
}

[System.Serializable]
public class s_characterElementalAffinities
{
    public s_characterElementalAffinities(
        int strike,
        int peirce,
        int fire,
        int ice,
        int water,
        int electric,
        int wind,
        int earth,
        int psychic,
        int light,
        int dark,
        int heal,
        int support)
    {
        this.strike = strike;
        this.peirce = peirce;
        this.fire = fire;
        this.ice = ice;
        this.water = water;
        this.electric = electric;
        this.wind = wind;
        this.earth = earth;
        this.psychic = psychic;
        this.light = light;
        this.dark = dark;
        this.heal = heal;
        this.support = support;
    }


    public int strike;
    public int peirce;

    public int fire;
    public int ice;
    public int water;
    public int electric;
    public int wind;
    public int earth;
    public int psychic;
    public int light;
    public int dark;
    public int heal;
    public int support;
}

[CanEditMultipleObjects]
[CustomEditor(typeof(o_battleCharDataN))]
public class ed_CASE : Editor
{
    public enum STAT_DIST_AMOUNT
    {
        VERY_LOW = -2,
        LOW = -1,
        AVERAGE = 0,
        HIGH,
        VERY_HIGH
    }
    public STAT_DIST_AMOUNT hpDist;
    public STAT_DIST_AMOUNT hpGDist;

    public STAT_DIST_AMOUNT spDist;
    public STAT_DIST_AMOUNT spGDist;
    public STAT_DIST_AMOUNT hpDistG;
    public STAT_DIST_AMOUNT spDistG;

    public STAT_DIST_AMOUNT strDist;
    public STAT_DIST_AMOUNT vitDist;
    public STAT_DIST_AMOUNT intDist;
    public STAT_DIST_AMOUNT dexDist;
    public STAT_DIST_AMOUNT agiDist;
    public STAT_DIST_AMOUNT lucDist;

    private int statDist = 10;
    
    public List<charAI> characterAI = null;
    Sprite[] sprites;
    int listArray;
    int superTab = 0;
    int tabSkill = 0;
    int tabChar = 0;
    int levelChar = 0;
    int charAIPageNum = 0;
    s_move moveData = null;
    o_battleCharDataN charaData = null;
    bool isLoadedCharacter = false;
    string directoryMove;
    string element;

    //public List<o_ite> itemdata;
    public List<s_move> skilldata;
    Vector2 scrollPos;
    public bool[] aiBoolList;
    private S_Element[] elementList;
    private List<o_battleCharDataN.el_weaknesses> elWeaknesses;

    public void OnEnable()
    {
        charaData = (o_battleCharDataN)target;
        string[] fileNames = EditorAssetHelper.GetFileNames("S_Element", "Assets/Data/Elements/");
        List<string> fn = fileNames.ToList();
        fn.Remove("Heal");
        fn.Remove("Status");
        fileNames = fn.ToArray();
        elementList = new S_Element[fn.Count];
        for (int i = 0; i < fileNames.Length; i++)
        {
            elementList[i] = AssetDatabase.LoadAssetAtPath<S_Element>("Assets/Data/Elements/" + fileNames[i] + ".asset");
        }
    }

    public charAI[] GetAIList() {
        charAI[] lists = new charAI[charaData.moveLearn.Count];
        for (int i = 0; i < charaData.moveLearn.Count; i++)
        {
            s_move mov = charaData.moveLearn[i];
            lists[i] = new charAI();
            lists[i].move = mov;
            switch (mov.moveType)
            {
                case s_move.MOVE_TYPE.HP_DAMAGE:
                case s_move.MOVE_TYPE.HP_SP_DAMAGE:
                    lists[i].conditions = charAI.CONDITIONS.ALWAYS;
                    break;

                case s_move.MOVE_TYPE.HP_DRAIN:
                case s_move.MOVE_TYPE.SP_DRAIN:
                case s_move.MOVE_TYPE.HP_RECOVER:
                case s_move.MOVE_TYPE.SP_RECOVER:
                    lists[i].isImportant = true;
                    lists[i].onParty = true;
                    lists[i].conditions = charAI.CONDITIONS.USER_PARTY_HP_LOWER;
                    lists[i].healthPercentage = 0.5f;
                    break;
            }
        }
        return lists;
    }
    public void ChangeStatElementWeakness(ref ELEMENT_WEAKNESS stat, string elName)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(elName);
        stat = (ELEMENT_WEAKNESS)EditorGUILayout.EnumPopup(stat);
        EditorGUILayout.EndHorizontal();
    }
    public void ChangeStatElemenAffinities(ref int stat, string elName)
    {
        EditorGUILayout.BeginHorizontal();
        int amountDamagePlus = 0;
        int amountDiscount = 0;

        bool statNegative = stat < 0;

        if (statNegative)
        {
            for (int i = 0; i < Mathf.Abs(stat); i++)
            {
                if (i % 3 == 0)
                {
                    amountDiscount++;
                    continue;
                }
                amountDamagePlus++;
            }
        }
        else
        {
            for (int i = 0; i < stat; i++)
            {
                if (i % 3 == 0)
                {
                    amountDiscount++;
                    continue;
                }
                amountDamagePlus++;
            }
        }

        string colBad = ColorUtility.ToHtmlStringRGB(Color.red);
        string colGood = ColorUtility.ToHtmlStringRGB(Color.red);

        if (statNegative)
        {
            EditorGUILayout.LabelField(elName + " - " + amountDamagePlus + " Damage" + " + " + amountDiscount + " Cost");
        }
        else
        {
            if (amountDamagePlus > 0)
            {
                EditorGUILayout.LabelField(elName + " + " + amountDamagePlus + " Damage" + " - " + amountDiscount + " Cost");
            }
            else {
                EditorGUILayout.LabelField(elName);
            }
        }
        /*
        if (charaData.elementAffinities.CalculateTotal() >= statDist)
        {
            int prevStat = stat;
            stat = EditorGUILayout.IntSlider(stat, -15, 15);
            if(stat > prevStat)
                stat = prevStat;
        }
        else
        {
            stat = EditorGUILayout.IntSlider(stat, -15, 15);
        }
        */
        EditorGUILayout.EndHorizontal();
    }

    public float ChangeStatFloatSlider(ref float stat1, float max, float min)
    {
        float curStat = stat1;
        float prevStat = stat1;
        curStat = EditorGUILayout.Slider(curStat, min, max);
        /*
        if (curStat != prevStat)
        {
            EditorUtility.SetDirty(charaData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        */
        return curStat;
    }
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(charaData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        tabChar = GUILayout.Toolbar(tabChar, new string[] { "Overview", "Stats", "Moves", "Elements", "AI", "Raw data" });
        switch (tabChar)
        {
            case 0:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Name: " + charaData.name);
                levelChar = (int)EditorGUILayout.Slider(levelChar, 1, 50);
                {
                    int tempHPMin = charaData.maxHitPointsB;
                    int tempSPMin = charaData.maxSkillPointsB;
                    int tempHPMax = charaData.maxHitPointsB;
                    int tempSPMax = charaData.maxSkillPointsB;

                    int tempStr = charaData.strengthB;
                    int tempVit = charaData.vitalityB;
                    int tempDx = charaData.dexterityB;
                    int tempLuc = charaData.luckB;
                    int tempAgi = charaData.agilityB;
                    int tempInt = charaData.intelligenceB;

                    for (int i = 1; i < levelChar; i++)
                    {
                        tempHPMax += charaData.maxHitPointsGMax;
                        tempHPMin += charaData.maxHitPointsGMin;
                        
                        tempSPMax += charaData.maxSkillPointsGMax;
                        tempSPMin += charaData.maxSkillPointsGMin;
                        
                        if (i % charaData.strengthGT == 0)
                            tempStr++;
                        if (i % charaData.vitalityGT == 0)
                            tempVit++;
                        if (i % charaData.dexterityGT == 0)
                            tempDx++;
                        if (i % charaData.luckGT == 0)
                            tempLuc++;
                        if (i % charaData.agilityGT == 0)
                            tempAgi++;
                        if (i % charaData.intelligenceGT == 0)
                            tempInt++;
                    }
                    EditorGUILayout.LabelField("Health (HP): " + tempHPMin + " - " + tempHPMax);
                    EditorGUILayout.LabelField("Stamina (SP): " + tempSPMin + " - " + tempSPMax);

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Strength: " + tempStr);
                    EditorGUILayout.LabelField("Vitality: " + tempVit);
                    EditorGUILayout.LabelField("Dexterity: " + tempDx);
                    EditorGUILayout.LabelField("Agility: " + tempAgi);
                    EditorGUILayout.LabelField("Intelligence: " + tempInt);
                    EditorGUILayout.LabelField("Luck: " + tempLuc);
                    EditorGUILayout.Space();

                }
                charaData.strengthGT = (int)EditorGUILayout.Slider("Strength: ",charaData.strengthGT, 1,6);
                charaData.vitalityGT = (int)EditorGUILayout.Slider("Vitality: ", charaData.vitalityGT, 1, 6);
                charaData.dexterityGT = (int)EditorGUILayout.Slider("Dexterity: ", charaData.dexterityGT, 1, 6);
                charaData.intelligenceGT = (int)EditorGUILayout.Slider("Intelligence: ", charaData.intelligenceGT, 1, 6);
                charaData.agilityGT = (int)EditorGUILayout.Slider("Agility: ", charaData.agilityGT, 1, 6);
                charaData.luckGT = (int)EditorGUILayout.Slider("Luck: ", charaData.luckGT, 1, 6);
                EditorGUILayout.Space();
                charaData.turnIcons = (int)EditorGUILayout.Slider("Turn icons: ", charaData.turnIcons, 1, 4);
                break;

            #region Stats
            case 1:
                #region STAT DISTRIBUTION STUFF

                #region STAT VARIABLES
                Vector2Int BSvlowBoundPTS = new Vector2Int(4, 9);
                Vector2Int BSlowBoundPTS = new Vector2Int(6, 12);
                Vector2Int BSavgBoundPTS = new Vector2Int(8, 15);
                Vector2Int BShighBoundPTS = new Vector2Int(11, 18);
                Vector2Int BSvhighBoundPTS = new Vector2Int(14, 25);

                Vector2Int LWvlowBoundPTS = new Vector2Int(1, 1);
                Vector2Int LWlowBoundPTS = new Vector2Int(1, 3);
                Vector2Int LWavgBoundPTS = new Vector2Int(1, 4);
                Vector2Int LWhighBoundPTS = new Vector2Int(2, 4);
                Vector2Int LWvhighBoundPTS = new Vector2Int(3, 4);

                Vector2Int HGvlowBoundPTS = new Vector2Int(1, 2);
                Vector2Int HGlowBoundPTS = new Vector2Int(1, 3);
                Vector2Int HGavgBoundPTS = new Vector2Int(1, 4);
                Vector2Int HGhighBoundPTS = new Vector2Int(2, 5);
                Vector2Int HGvhighBoundPTS = new Vector2Int(4, 7);

                Vector2Int vlowBoundB = new Vector2Int(1, 2);
                Vector2Int lowBoundB = new Vector2Int(1, 4);
                Vector2Int avgBoundB = new Vector2Int(2, 5);
                Vector2Int highBoundB = new Vector2Int(3, 6);
                Vector2Int vhighBoundB = new Vector2Int(4, 6);

                Vector2Int vlowBoundG = new Vector2Int(4, 5);
                Vector2Int lowBoundG = new Vector2Int(3, 5);
                Vector2Int avgBoundG = new Vector2Int(2, 3);
                Vector2Int highBoundG = new Vector2Int(2, 2);
                Vector2Int vhighBoundG = new Vector2Int(1, 2);
                #endregion

                if (GUILayout.Button("Generate Stat distribution"))
                {
                    #region HP
                    switch (hpDist)
                    {
                        case STAT_DIST_AMOUNT.VERY_LOW:
                            charaData.maxHitPointsB = Random.Range(BSvlowBoundPTS.x, BSvlowBoundPTS.y);
                            break;
                        case STAT_DIST_AMOUNT.LOW:
                            charaData.maxHitPointsB = Random.Range(BSlowBoundPTS.x, BSlowBoundPTS.y);
                            break;
                        case STAT_DIST_AMOUNT.AVERAGE:
                            charaData.maxHitPointsB = Random.Range(BSavgBoundPTS.x, BSavgBoundPTS.y);
                            break;
                        case STAT_DIST_AMOUNT.HIGH:
                            charaData.maxHitPointsB = Random.Range(BShighBoundPTS.x, BShighBoundPTS.y);
                            break;
                        case STAT_DIST_AMOUNT.VERY_HIGH:
                            charaData.maxHitPointsB = Random.Range(BSvhighBoundPTS.x, BSvhighBoundPTS.y);
                            break;
                    }
                    switch (hpDistG)
                    {
                        case STAT_DIST_AMOUNT.VERY_LOW:
                            charaData.maxHitPointsGMin = Random.Range(LWvlowBoundPTS.x, LWvlowBoundPTS.y);
                            if (charaData.maxHitPointsGMin > HGvlowBoundPTS.x)
                            {
                                charaData.maxHitPointsGMax = Random.Range(charaData.maxHitPointsGMin, HGvlowBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxHitPointsGMax = Random.Range(HGvlowBoundPTS.x, HGvlowBoundPTS.y);
                            }
                            break;

                        case STAT_DIST_AMOUNT.LOW:
                            charaData.maxHitPointsGMin = Random.Range(LWlowBoundPTS.x, LWlowBoundPTS.y);
                            if (charaData.maxHitPointsGMin > HGlowBoundPTS.x)
                            {
                                charaData.maxHitPointsGMax = Random.Range(charaData.maxHitPointsGMin, HGlowBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxHitPointsGMax = Random.Range(HGlowBoundPTS.x, HGlowBoundPTS.y);
                            }
                            break;

                        case STAT_DIST_AMOUNT.AVERAGE:
                            charaData.maxHitPointsGMin = Random.Range(LWavgBoundPTS.x, LWavgBoundPTS.y);
                            if (charaData.maxHitPointsGMin > HGavgBoundPTS.x)
                            {
                                charaData.maxHitPointsGMax = Random.Range(charaData.maxHitPointsGMin, HGavgBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxHitPointsGMax = Random.Range(HGavgBoundPTS.x, HGavgBoundPTS.y);
                            }
                            break;

                        case STAT_DIST_AMOUNT.HIGH:
                            charaData.maxHitPointsGMin = Random.Range(LWhighBoundPTS.x, LWhighBoundPTS.y);
                            if (charaData.maxHitPointsGMin > HGhighBoundPTS.x)
                            {
                                charaData.maxHitPointsGMax = Random.Range(charaData.maxHitPointsGMin, HGhighBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxHitPointsGMax = Random.Range(HGhighBoundPTS.x, HGhighBoundPTS.y);
                            }
                            break;

                        case STAT_DIST_AMOUNT.VERY_HIGH:
                            charaData.maxHitPointsGMin = Random.Range(LWvhighBoundPTS.x, LWvhighBoundPTS.y);
                            if (charaData.maxHitPointsGMin > HGvhighBoundPTS.x)
                            {
                                charaData.maxHitPointsGMax = Random.Range(charaData.maxHitPointsGMin, HGvhighBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxHitPointsGMax = Random.Range(HGvhighBoundPTS.x, HGvhighBoundPTS.y);
                            }
                            break;
                    }
                    #endregion
                    
                    #region STRENGTH
                    {
                        int statB = 0;
                        int statG = 0;
                        switch (strDist)
                        {
                            case STAT_DIST_AMOUNT.VERY_LOW:
                                statB = Random.Range(vlowBoundB.x, vlowBoundB.y);
                                statG = Random.Range(vlowBoundG.x, vlowBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.LOW:
                                statB = Random.Range(lowBoundB.x, lowBoundB.y);
                                statG = Random.Range(lowBoundG.x, lowBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.AVERAGE:
                                statB = Random.Range(avgBoundB.x, avgBoundB.y);
                                statG = Random.Range(avgBoundB.x, avgBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.HIGH:
                                statB = Random.Range(highBoundB.x, highBoundB.y);
                                statG = Random.Range(highBoundG.x, highBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.VERY_HIGH:
                                statB = Random.Range(vhighBoundB.x, vhighBoundB.y);
                                statG = Random.Range(vhighBoundG.x, vhighBoundG.y);
                                break;
                        }
                        charaData.strengthB = statB;
                        charaData.strengthGT = statG;
                    }
                    #endregion

                    #region VITALITY
                    {
                        int statB = 0;
                        int statG = 0;
                        switch (strDist)
                        {
                            case STAT_DIST_AMOUNT.VERY_LOW:
                                statB = Random.Range(vlowBoundB.x, vlowBoundB.y);
                                statG = Random.Range(vlowBoundG.x, vlowBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.LOW:
                                statB = Random.Range(lowBoundB.x, lowBoundB.y);
                                statG = Random.Range(lowBoundG.x, lowBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.AVERAGE:
                                statB = Random.Range(avgBoundB.x, avgBoundB.y);
                                statG = Random.Range(avgBoundB.x, avgBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.HIGH:
                                statB = Random.Range(highBoundB.x, highBoundB.y);
                                statG = Random.Range(highBoundG.x, highBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.VERY_HIGH:
                                statB = Random.Range(vhighBoundB.x, vhighBoundB.y);
                                statG = Random.Range(vhighBoundG.x, vhighBoundG.y);
                                break;
                        }
                        charaData.vitalityB = statB;
                        charaData.vitalityGT = statG;
                    }
                    #endregion

                    #region DEXTERITY
                    {
                        int statB = 0;
                        int statG = 0;
                        switch (strDist)
                        {
                            case STAT_DIST_AMOUNT.VERY_LOW:
                                statB = Random.Range(vlowBoundB.x, vlowBoundB.y);
                                statG = Random.Range(vlowBoundG.x, vlowBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.LOW:
                                statB = Random.Range(lowBoundB.x, lowBoundB.y);
                                statG = Random.Range(lowBoundG.x, lowBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.AVERAGE:
                                statB = Random.Range(avgBoundB.x, avgBoundB.y);
                                statG = Random.Range(avgBoundB.x, avgBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.HIGH:
                                statB = Random.Range(highBoundB.x, highBoundB.y);
                                statG = Random.Range(highBoundG.x, highBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.VERY_HIGH:
                                statB = Random.Range(vhighBoundB.x, vhighBoundB.y);
                                statG = Random.Range(vhighBoundG.x, vhighBoundG.y);
                                break;
                        }
                        charaData.dexterityB = statB;
                        charaData.dexterityGT = statG;
                    }
                    #endregion

                    #region AGILITY
                    {
                        int statB = 0;
                        int statG = 0;
                        switch (strDist)
                        {
                            case STAT_DIST_AMOUNT.VERY_LOW:
                                statB = Random.Range(vlowBoundB.x, vlowBoundB.y);
                                statG = Random.Range(vlowBoundG.x, vlowBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.LOW:
                                statB = Random.Range(lowBoundB.x, lowBoundB.y);
                                statG = Random.Range(lowBoundG.x, lowBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.AVERAGE:
                                statB = Random.Range(avgBoundB.x, avgBoundB.y);
                                statG = Random.Range(avgBoundB.x, avgBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.HIGH:
                                statB = Random.Range(highBoundB.x, highBoundB.y);
                                statG = Random.Range(highBoundG.x, highBoundG.y);
                                break;

                            case STAT_DIST_AMOUNT.VERY_HIGH:
                                statB = Random.Range(vhighBoundB.x, vhighBoundB.y);
                                statG = Random.Range(vhighBoundG.x, vhighBoundG.y);
                                break;
                        }
                        charaData.agilityB = statB;
                        charaData.agilityGT = statG;
                    }
                    #endregion
                }

                EditorGUILayout.LabelField("Base stats");
                hpDist = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Health distribution", hpDist);
                spDist = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Stamina distribution", spDist);

                EditorGUILayout.LabelField("Growth stats");
                hpDistG = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Health growth distribution", hpDistG);
                spDistG = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Stamina growth distribution", spDistG);



                #endregion
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name: ");
                charaData.name = EditorGUILayout.TextArea(charaData.name);
                EditorGUILayout.EndHorizontal();
                charaData.anim = EditorGUILayout.ObjectField(charaData.anim, typeof(RuntimeAnimatorController), false) as RuntimeAnimatorController;

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Base stats: ");
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Health: ");
                charaData.maxHitPointsB = EditorGUILayout.IntSlider(charaData.maxHitPointsB, 1, 60);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Health increase: ");
                charaData.maxHitPointsGMin = EditorGUILayout.IntSlider(charaData.maxHitPointsGMin, 0, 20);
                charaData.maxHitPointsGMax = EditorGUILayout.IntSlider(charaData.maxHitPointsGMin, charaData.maxHitPointsGMin, 30);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Stamina: ");
                charaData.maxSkillPointsB = EditorGUILayout.IntSlider(charaData.maxSkillPointsB, 1, 50);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Stamina increase: ");
                charaData.maxSkillPointsGMin = EditorGUILayout.IntSlider(charaData.maxSkillPointsGMin, 0, 10);
                charaData.maxSkillPointsGMax = EditorGUILayout.IntSlider(charaData.maxSkillPointsGMax, charaData.maxSkillPointsGMin, 15);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Base Strength: ");
                charaData.strengthB = EditorGUILayout.IntSlider(charaData.strengthB, 1, 10);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Base Vitality: ");
                charaData.vitalityB = EditorGUILayout.IntSlider(charaData.vitalityB, 1, 10);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Base Intelligence: ");
                charaData.intelligenceB = EditorGUILayout.IntSlider(charaData.intelligenceB, 1, 10);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Base Dexterity: ");
                charaData.dexterityB = EditorGUILayout.IntSlider(charaData.dexterityB, 1, 10);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Base Agility: ");
                charaData.agilityB = EditorGUILayout.IntSlider(charaData.agilityB, 1, 10);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Base Luck: ");
                charaData.luckB = EditorGUILayout.IntSlider(charaData.luckB, 1, 10);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                /*
                EditorGUILayout.LabelField("Total stats " + charaData.elementAffinities.CalculateTotal() + "/ " + statDist);
                

                ChangeStatElemenAffinities(ref charaData.elementAffinities.strike, "Strike");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.peirce, "Peirce/Gun");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.fire, "Fire");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.water, "Water");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.ice, "Ice");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.electric, "Electric");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.wind, "Wind");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.earth, "Earth");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.psychic, "Psychic");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.light, "Light");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.dark, "Dark");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.heal, "Healing");
                ChangeStatElemenAffinities(ref charaData.elementAffinities.support, "Support");
                */
                /*
                */
                break;
            #endregion

            #region Moves
            case 2:

                if (charaData.moveLearn == null)
                {
                    charaData.moveLearn = new List<s_move>();
                }
                else
                {
                    for (int i = 0; i < charaData.moveLearn.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        charaData.moveLearn[i] = EditorGUILayout.ObjectField(charaData.moveLearn[i], typeof(s_move), false) as s_move;
                        if (GUILayout.Button("Delete"))
                        {
                            charaData.moveLearn.RemoveAt(i);
                            //To reset the whole thing
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("Add new move"))
                    {
                        charaData.moveLearn.Add(new s_move());
                        //To reset the whole thing
                        break;
                    }
                }
                break;
            #endregion

            case 3:

                elWeaknesses = charaData.elementals.ToList();
                for (int i = 0; i < elementList.Length; i++)
                {
                    S_Element curEl = elementList[i];
                    EditorGUILayout.BeginHorizontal();
                    GUI.color = curEl.elementColour;
                    EditorGUILayout.LabelField(curEl.name, GUILayout.Width(120f));
                    GUI.color = Color.white;
                    o_battleCharDataN.el_weaknesses elWeak = elWeaknesses.Find(x => x.element == curEl);
                    if (elWeak != null)
                    {
                        GUI.backgroundColor = Color.white;
                        float prevVal = elWeak.value;
                        elWeak.value = EditorGUILayout.Slider(elWeak.value, -2, 2.5f);
                        elWeak.value = Mathf.Round(elWeak.value * 100f) / 100f;
                        float currVal = elWeak.value;
                        if (currVal != prevVal)
                        {
                            charaData.elementals = elWeaknesses.ToArray();
                        }
                        if (GUILayout.Button("-"))
                        {
                            elWeaknesses.Remove(elWeak);
                            charaData.elementals = elWeaknesses.ToArray();
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    else
                    {
                        GUI.backgroundColor = Color.grey;
                        if (GUILayout.Button(""))
                        {
                            o_battleCharDataN.el_weaknesses wk = new o_battleCharDataN.el_weaknesses();
                            wk.value = 1f;
                            wk.element = curEl;
                            elWeaknesses.Add(wk);
                            Debug.Log("OK");
                            charaData.elementals = elWeaknesses.ToArray();
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                break;

            #region AI
            case 4:
                if (charaData.aiPages != null)
                {
                    if (charaData.aiPages.Length > 1)
                        charAIPageNum = EditorGUILayout.IntSlider(charAIPageNum, 0, charaData.aiPages.Length);
                    else
                        charAIPageNum = 0;
                    o_battleCharDataN.ai_page page = charaData.aiPages[charAIPageNum];
                    if (charaData.aiPages[charAIPageNum].ai.Length <= 0) {
                        charaData.aiPages[charAIPageNum].ai = GetAIList();
                    }

                    if (aiBoolList == null || aiBoolList.Length != charaData.aiPages[charAIPageNum].ai.Length)
                    {
                        aiBoolList = new bool[charaData.moveLearn.Count];
                    }
                    else
                    {
                        int ind = 0;

                        for (int im = 0; im < page.ai.Length; im++)
                        {
                            charAI ai = page.ai[im];
                            EditorGUILayout.BeginHorizontal();
                            aiBoolList[ind] = EditorGUILayout.Toggle(aiBoolList[ind]);
                            EditorGUILayout.EndHorizontal();

                            if (aiBoolList[ind])
                            {
                                EditorGUILayout.Space();
                                EditorGUILayout.LabelField("Important");
                                ai.isImportant = EditorGUILayout.Toggle(ai.isImportant);
                                ai.conditions = (charAI.CONDITIONS)EditorGUILayout.EnumPopup(ai.conditions);
                                switch (ai.conditions)
                                {

                                    case charAI.CONDITIONS.ALWAYS:
                                        for (int i = 0; i < charaData.moveLearn.Count; i++)
                                        {
                                            if (charaData.moveLearn[i] != null)
                                            {
                                                if (GUILayout.Button(charaData.moveLearn[i].name))
                                                {
                                                    //ml.move = charaData.moveLearn[i].move;
                                                }
                                            }
                                        }
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;
                                    case charAI.CONDITIONS.USER_PARTY_HP_LOWER:
                                        EditorGUILayout.LabelField("If party member's health is lower than " + ai.healthPercentage * 100 + "%, use ");
                                        //ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                        ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;
                                    case charAI.CONDITIONS.TARGET_PARTY_HP_HIGHER:
                                        EditorGUILayout.LabelField("If target's health is higher than " + ai.healthPercentage * 100 + "%, use ");
                                        //ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                        ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;
                                    case charAI.CONDITIONS.USER_PARTY_HP_HIGHER:
                                        EditorGUILayout.LabelField("If party member's health is higher than " + ai.healthPercentage * 100 + "%, use ");
                                        //ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                        ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;
                                    case charAI.CONDITIONS.TARGET_PARTY_HP_LOWER:
                                        EditorGUILayout.LabelField("If target's health is lower than " + ai.healthPercentage * 100 + "%, use ");

                                        //ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                        ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;

                                    case charAI.CONDITIONS.SELF_HP_HIGHER:
                                        EditorGUILayout.LabelField("If the user's health is higher than " + ai.healthPercentage * 100 + "%, use ");

                                        //ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                        ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;

                                    case charAI.CONDITIONS.SELF_SP_LOWER:
                                        EditorGUILayout.LabelField("If the user's stamina is higher than " + ai.healthPercentage * 100 + "%, use ");

                                        // ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                        ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;

                                    case charAI.CONDITIONS.SELF_HP_LOWER:
                                        EditorGUILayout.LabelField("If the user's health is higher than " + ai.healthPercentage * 100 + "%, use ");

                                        //ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                        ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;

                                    case charAI.CONDITIONS.SELF_SP_HIGHER:
                                        EditorGUILayout.LabelField("If the user's stamina is higher than " + ai.healthPercentage * 100 + "%, use ");

                                        //ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                        ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                        EditorGUILayout.LabelField("On Party member?");
                                        ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                        break;
                                }
                                EditorGUILayout.LabelField("Always use [NO MOVE SELECTED]");
                                ai.turnCounters = (charAI.TURN_COUNTER)EditorGUILayout.EnumPopup(ai.turnCounters);
                                switch (ai.turnCounters)
                                {
                                    case charAI.TURN_COUNTER.TURN_COUNTER_EQU:
                                        EditorGUILayout.LabelField("Turn counter");
                                        ai.number1 = EditorGUILayout.IntField(ai.number1);
                                        break;

                                    case charAI.TURN_COUNTER.ROUND_COUNTER_EQU:
                                        EditorGUILayout.LabelField("Round counter");
                                        ai.number2 = EditorGUILayout.IntField(ai.number2);
                                        break;

                                    case charAI.TURN_COUNTER.ROUND_TURN_COUNTER_EQU:
                                        EditorGUILayout.LabelField("Turn counter");
                                        ai.number1 = EditorGUILayout.IntField(ai.number1);
                                        EditorGUILayout.Space();
                                        EditorGUILayout.LabelField("Round counter");
                                        ai.number2 = EditorGUILayout.IntField(ai.number2);
                                        break;
                                }
                                EditorGUILayout.Space();
                            }
                            ind++;
                        }
                    }

                    /*
                    foreach (o_battleCharDataN.move_learn ml in charaData.aiPages)
                    {
                        charAI ai = ml.ai; ai_page
                        string blurb = "";
                        /*
                        EditorGUILayout.BeginHorizontal();
                        if (ml.move != null)
                        {
                            #region DESCRIPTION
                            switch (ai.conditions)
                            {
                                case charAI.CONDITIONS.ALWAYS:
                                    for (int i = 0; i < charaData.moveLearn.Count; i++)
                                    {
                                        if (charaData.moveLearn[i] != null)
                                        {
                                            if (GUILayout.Button(charaData.moveLearn[i].move.name))
                                            {
                                                ml.move = charaData.moveLearn[i].move;
                                            }
                                        }
                                    }
                                    EditorGUILayout.LabelField("On Party member?");
                                    ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;
                                case charAI.CONDITIONS.USER_PARTY_HP_LOWER:
                                    blurb = "If party member's health is lower than " + ai.healthPercentage * 100 + "%, use ";
                                    ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                    //ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                    //EditorGUILayout.LabelField("On Party member?");
                                    //ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;
                                case charAI.CONDITIONS.TARGET_PARTY_HP_HIGHER:
                                    blurb = "If target's health is higher than " + ai.healthPercentage * 100 + "%, use ";
                                    ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                   // ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                   // EditorGUILayout.LabelField("On Party member?");
                                   // ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;
                                case charAI.CONDITIONS.USER_PARTY_HP_HIGHER:
                                    EditorGUILayout.LabelField("If party member's health is higher than " + ai.healthPercentage * 100 + "%, use ");
                                    ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                    ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                    EditorGUILayout.LabelField("On Party member?");
                                    ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;
                                case charAI.CONDITIONS.TARGET_PARTY_HP_LOWER:
                                    EditorGUILayout.LabelField("If target's health is lower than " + ai.healthPercentage * 100 + "%, use ");

                                    ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                    ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                    EditorGUILayout.LabelField("On Party member?");
                                    ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;

                                case charAI.CONDITIONS.SELF_HP_HIGHER:
                                    EditorGUILayout.LabelField("If the user's health is higher than " + ai.healthPercentage * 100 + "%, use ");

                                    ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                    ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                    EditorGUILayout.LabelField("On Party member?");
                                    ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;

                                case charAI.CONDITIONS.SELF_SP_LOWER:
                                    EditorGUILayout.LabelField("If the user's stamina is higher than " + ai.healthPercentage * 100 + "%, use ");

                                    ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                    ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                    EditorGUILayout.LabelField("On Party member?");
                                    ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;

                                case charAI.CONDITIONS.SELF_HP_LOWER:
                                    EditorGUILayout.LabelField("If the user's health is higher than " + ai.healthPercentage * 100 + "%, use ");

                                    ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                    ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                    EditorGUILayout.LabelField("On Party member?");
                                    ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;

                                case charAI.CONDITIONS.SELF_SP_HIGHER:
                                    EditorGUILayout.LabelField("If the user's stamina is higher than " + ai.healthPercentage * 100 + "%, use ");

                                    ml.move = EditorGUILayout.ObjectField(ml.move, typeof(s_move), false) as s_move;
                                    ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                    EditorGUILayout.LabelField("On Party member?");
                                    ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                    break;
                            }
                            EditorGUILayout.LabelField("Always use [NO MOVE SELECTED]");
                            ai.turnCounters = (charAI.TURN_COUNTER)EditorGUILayout.EnumPopup(ai.turnCounters);
                            switch (ai.turnCounters)
                            {
                                case charAI.TURN_COUNTER.TURN_COUNTER_EQU:
                                    EditorGUILayout.LabelField("Turn counter");
                                    ai.number1 = EditorGUILayout.IntField(ai.number1);
                                    break;

                                case charAI.TURN_COUNTER.ROUND_COUNTER_EQU:
                                    EditorGUILayout.LabelField("Round counter");
                                    ai.number2 = EditorGUILayout.IntField(ai.number2);
                                    break;

                                case charAI.TURN_COUNTER.ROUND_TURN_COUNTER_EQU:
                                    EditorGUILayout.LabelField("Turn counter");
                                    ai.number1 = EditorGUILayout.IntField(ai.number1);
                                    EditorGUILayout.Space();
                                    EditorGUILayout.LabelField("Round counter");
                                    ai.number2 = EditorGUILayout.IntField(ai.number2);
                                    break;
                            }
                            EditorGUILayout.Space();
                            #endregion
                            EditorGUILayout.LabelField(blurb);
                            aiBoolList[ind] = EditorGUILayout.Toggle(aiBoolList[ind]);
                        }
                        else {
                            EditorGUILayout.LabelField("Move not set - set it in the 'Moves' menu!");
                        }
                }

                switch (ai.conditions)
                    {

                        case charAI.CONDITIONS.TARGET_PARTY_HP:
                            break;
                    }
                    */
                }
                else {
                    charaData.aiPages = new o_battleCharDataN.ai_page[1];
                    charaData.aiPages[0].ai = new charAI[charaData.moveLearn.Count];
                }
                break;
            #endregion

            case 5:
                base.OnInspectorGUI();
                break;
        }
    }
}
/*
superTab = GUILayout.Toolbar(superTab, new string[] { "Characters", "Moves", "Items" });
switch (superTab) {

    #region CHARACTERS
    case 0:

        tabChar = GUILayout.Toolbar(tabChar, new string[] { "Overview", "Stats", "Moves", "Elements", "AI" });
        switch (tabChar)
        {
            case 0:
                EditorGUILayout.LabelField("Simulated stats based on level");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Name: " + charaData.name);
                charaData.level = (int)EditorGUILayout.Slider(charaData.level, 1, 100);
                {
                    int tempHPMin = charaData.maxHitPointsB;
                    int tempSPMin = charaData.maxSkillPointsB;
                    int tempHPMax = charaData.maxHitPointsB;
                    int tempSPMax = charaData.maxSkillPointsB;
                    int tempStr = charaData.strengthB;
                    int tempVit = charaData.vitalityB;
                    int tempDx = charaData.dexterityB;
                    int tempLuc = charaData.luckB;

                    for (int i = 1; i < charaData.level; i++)
                    {
                        if (i % charaData.strengthGT == 0)
                            tempStr++;
                        if (i % charaData.vitalityGT == 0)
                            tempVit++;
                        if (i % charaData.dexterityGT == 0)
                            tempDx++;
                        if (i % charaData.luckGT == 0)
                            tempLuc++;

                        tempHPMin += charaData.maxHitPointsGMin;
                        tempSPMin += charaData.maxSkillPointsGMin;

                        tempHPMax += charaData.maxHitPointsGMax;
                        tempSPMax += charaData.maxSkillPointsGMax;
                        //tempHP += Random.Range(data.maxHitPointsGMin, data.maxHitPointsGMax);
                        //tempSP += Random.Range(data.maxSkillPointsGMin, data.maxSkillPointsGMax);
                    }
                    EditorGUILayout.LabelField("Health (HP): " + tempHPMin + " - " + tempHPMax);
                    EditorGUILayout.LabelField("Stamina (SP): " + tempSPMin + " - " + tempSPMax);
                    EditorGUILayout.LabelField("Strength: " + tempStr);
                    EditorGUILayout.LabelField("Vitality: " + tempVit);
                    EditorGUILayout.LabelField("Dexterity: " + tempDx);
                    EditorGUILayout.LabelField("Luck: " + tempLuc);
                }
                break;

            case 1:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name: ");
                charaData.name = EditorGUILayout.TextArea(charaData.name);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Base stats: ");
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Health: ");
                charaData.maxHitPointsB = EditorGUILayout.IntField(charaData.maxHitPointsB);
                EditorGUILayout.LabelField("Growth min: ");
                charaData.maxHitPointsGMin = EditorGUILayout.IntField(charaData.maxHitPointsGMin);
                EditorGUILayout.LabelField("Growth max: ");
                charaData.maxHitPointsGMax = EditorGUILayout.IntField(charaData.maxHitPointsGMax);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Stamina: ");
                charaData.maxSkillPointsB = EditorGUILayout.IntField(charaData.maxSkillPointsB);
                EditorGUILayout.LabelField("Growth min: ");
                charaData.maxSkillPointsGMin = EditorGUILayout.IntField(charaData.maxSkillPointsGMin);
                EditorGUILayout.LabelField("Growth max: ");
                charaData.maxSkillPointsGMax = EditorGUILayout.IntField(charaData.maxSkillPointsGMax);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Strength: ");
                charaData.strengthB = EditorGUILayout.IntField(charaData.strengthB);
                EditorGUILayout.LabelField("Growth turns: ");
                charaData.strengthGT = EditorGUILayout.IntField(charaData.strengthGT);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Vitality: ");
                charaData.vitalityB = EditorGUILayout.IntField(charaData.vitalityB);
                EditorGUILayout.LabelField("Growth turns: ");
                charaData.vitalityGT = EditorGUILayout.IntField(charaData.vitalityGT);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Dexterity: ");
                charaData.dexterityB = EditorGUILayout.IntField(charaData.dexterityB);
                EditorGUILayout.LabelField("Growth turns: ");
                charaData.dexterityGT = EditorGUILayout.IntField(charaData.dexterityGT);
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Luck: ");
                charaData.luckB = EditorGUILayout.IntField(charaData.luckB);
                EditorGUILayout.LabelField("Growth turns: ");
                charaData.luckGT = EditorGUILayout.IntField(charaData.luckGT);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Base experience: ");
                charaData.expB = EditorGUILayout.IntField(charaData.expB);
                EditorGUILayout.Space();
                break;

            case 2:

                break;

            case 3:

                break;

            case 4:
                //o_battleCharData bo = new o_battleCharData();
                //bo.characterAI[];
                if (characterAI != null)
                {
                    foreach (charAI ai in charaData.character_AI)
                    {

                        EditorGUILayout.BeginHorizontal();
                        ai.conditions = (charAI.CONDITIONS)EditorGUILayout.EnumPopup(ai.conditions);
                        switch (ai.conditions)
                        {

                            case charAI.CONDITIONS.ALWAYS:
                                EditorGUILayout.LabelField("Always use ");
                                ai.moveName = EditorGUILayout.TextField(ai.moveName);
                                EditorGUILayout.LabelField("On Party member?");
                                ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                break;
                            case charAI.CONDITIONS.USER_PARTY_HP_LOWER:
                                EditorGUILayout.LabelField("If party member's health is lower than " + ai.healthPercentage * 100 + "%, use ");
                                ai.moveName = EditorGUILayout.TextField(ai.moveName);
                                ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                EditorGUILayout.LabelField("On Party member?");
                                ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                break;
                            case charAI.CONDITIONS.TARGET_PARTY_HP_HIGHER:
                                EditorGUILayout.LabelField("If target's health is higher than " + ai.healthPercentage * 100 + "%, use ");
                                ai.moveName = EditorGUILayout.TextField(ai.moveName);
                                ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                EditorGUILayout.LabelField("On Party member?");
                                ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                break;
                            case charAI.CONDITIONS.USER_PARTY_HP_HIGHER:
                                EditorGUILayout.LabelField("If party member's health is higher than " + ai.healthPercentage * 100 + "%, use ");
                                ai.moveName = EditorGUILayout.TextField(ai.moveName);
                                ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                EditorGUILayout.LabelField("On Party member?");
                                ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                break;
                            case charAI.CONDITIONS.TARGET_PARTY_HP_LOWER:
                                EditorGUILayout.LabelField("If target's health is lower than " + ai.healthPercentage * 100 + "%, use ");
                                ai.moveName = EditorGUILayout.TextField(ai.moveName);
                                ai.healthPercentage = EditorGUILayout.Slider(ai.healthPercentage, 0, 1);
                                EditorGUILayout.LabelField("On Party member?");
                                ai.onParty = EditorGUILayout.Toggle(ai.onParty);
                                break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("Add new AI action"))
                    {
                        characterAI.Add(new charAI());
                    }
                }
                else
                {
                    if (charaData.character_AI != null)
                        characterAI = charaData.character_AI.ToList();
                    else
                        charaData.character_AI = new charAI[1];
                }
                break;
        }

        break;
    #endregion
    #region SKILLS
    case 1:

        if (skilldata == null)
        {
            if (File.Exists("Assets/Data/SkillDatabase.txt"))
            {
                string fil = File.ReadAllText("Assets/Data/SkillDatabase.txt");
                skilldata = JsonUtility.FromJson<s_skillData>(fil).skilldata;
            }
            else
            {
                s_skillData dataJSON = new s_skillData(skilldata);
                string json = JsonUtility.ToJson(dataJSON, false);
                File.WriteAllText("Assets/Data/" + "SkillDatabase." + "txt", json);
            }
        }
        else
        {
            if (moveData != null)
            {
                tabSkill = GUILayout.Toolbar(tabSkill, new string[] { "Overview", "Animations" });
                switch (tabSkill)
                {
                    case 0:
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Move name");
                        moveData.name = EditorGUILayout.TextArea(moveData.name);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Move type");
                        moveData.moveType = (s_move.MOVE_TYPE)EditorGUILayout.EnumPopup(moveData.moveType);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Base power");
                        moveData.power = EditorGUILayout.IntField(moveData.power);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Elemental type");
                        moveData.element = (ELEMENT)EditorGUILayout.EnumPopup(moveData.element);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Inflicts status effect");
                        moveData.status_inflict_chance = EditorGUILayout.Slider(moveData.status_inflict_chance, 0, 1);
                        EditorGUILayout.EndHorizontal();
                        break;

                    case 1:
                        if (moveData.animations != null)
                        {
                            for (int i = 0; i < moveData.animations.Length; i++) {
                                s_actionAnim anim = moveData.animations[i];
                                switch (anim.actionType) {
                                    case s_actionAnim.ACTION_TYPE.WAIT:

                                        break;

                                    case s_actionAnim.ACTION_TYPE.PROJECTILE:

                                        break;

                                }
                            }
                        }
                        break;
                }
                if (GUILayout.Button("Back"))
                {
                    if (EditorUtility.DisplayDialog("Save", "Save current data", "Yes", "No"))
                    {
                        s_skillData dataJSON = new s_skillData(skilldata);
                        string json = JsonUtility.ToJson(dataJSON, false);
                        File.WriteAllText("Assets/Data/" + "SkillDatabase." + "txt", json);
                        moveData = null;
                    }
                    else {
                        moveData = null;
                    }
                }
                if (GUILayout.Button("Save"))
                {
                    if (EditorUtility.DisplayDialog("Save", "Save current data", "Yes", "No"))
                    {
                        s_skillData dataJSON = new s_skillData(skilldata);
                        string json = JsonUtility.ToJson(dataJSON, false);
                        File.WriteAllText("Assets/Data/" + "SkillDatabase." + "txt", json);
                    }
                }
            }
            else
            {
                if (GUILayout.Button("New skill"))
                {
                    skilldata.Add(new s_move());
                }
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                foreach (s_move dat in skilldata)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(dat.name);
                    EditorGUILayout.LabelField(" Type: " + dat.moveType);
                    if (GUILayout.Button("Edit"))
                    {
                        moveData = dat;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
        }
        break;
        #endregion
}
*/
/*
if (GUILayout.Button("Load Character"))
{
    directoryMove = EditorUtility.OpenFilePanel("Save Json move file", "Assets/Data/Characters/", "");
    if (directoryMove != "")
    {
        string fil = File.ReadAllText(directoryMove);
        if (fil != null)
        {
            data = JsonUtility.FromJson<o_battleCharData>(fil);
            characterAI = data.characterAI.ToList();
            isLoadedCharacter = true;
        }
    }
}
*/