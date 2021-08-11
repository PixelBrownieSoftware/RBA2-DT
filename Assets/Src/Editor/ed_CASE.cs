using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using MagnumFoundation2;

[System.Serializable]
public struct s_characterData {

    public s_characterData(List<o_battleCharData> characterdata) {
        this.characterdata = new List<o_battleCharData>();
        this.characterdata = characterdata;
    }
    public List<o_battleCharData> characterdata;
}
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

    public STAT_DIST_AMOUNT strDist;
    public STAT_DIST_AMOUNT vitDist;
    public STAT_DIST_AMOUNT dexDist;
    public STAT_DIST_AMOUNT agilDist;

    public STAT_DIST_AMOUNT hpDistG;
    public STAT_DIST_AMOUNT spDistG;
    public STAT_DIST_AMOUNT strDistG;
    public STAT_DIST_AMOUNT vitDistG;
    public STAT_DIST_AMOUNT dexDistG;
    public STAT_DIST_AMOUNT agilDistG;

    public List<charAI> characterAI = null;
    Sprite[] sprites;
    int listArray;
    int superTab = 0;
    int tabSkill = 0;
    int tabChar = 0;
    int charAILeng = 0;
    int charAIPageNum = 0;
    s_move moveData = null;
    o_battleCharDataN charaData = null;
    bool isLoadedCharacter = false;
    string directoryMove;
    string element;
    int level = 1;

    //public List<o_ite> itemdata;
    public List<s_move> skilldata;
    public List<o_battleCharData> characterdata;
    Vector2 scrollPos;
    public bool[] aiBoolList;

    public void OnEnable()
    {
        charaData = (o_battleCharDataN)target;
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
                case s_move.MOVE_TYPE.SPECIAL:
                case s_move.MOVE_TYPE.PHYSICAL:
                    lists[i].conditions = charAI.CONDITIONS.ALWAYS;
                    break;

                case s_move.MOVE_TYPE.STATUS:
                    switch (mov.statusType) {
                        case s_move.STATUS_TYPE.HEAL_HEALTH:
                        case s_move.STATUS_TYPE.HEAL_STAMINA:
                            lists[i].isImportant = true;
                            lists[i].onParty = true;
                            lists[i].conditions = charAI.CONDITIONS.USER_PARTY_HP_LOWER;
                            lists[i].healthPercentage = 0.5f;
                            break;
                    }
                    break;
            }
        }
        return lists;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(charaData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        tabChar = GUILayout.Toolbar(tabChar, new string[] { "Overview", "Stats", "Moves", "Elements", "AI" });
        switch (tabChar)
        {
            case 0:
                EditorGUILayout.LabelField("Simulated stats based on level");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Name: " + charaData.name);
                level = (int)EditorGUILayout.Slider(level, 1, 100);
                {
                    int tempHPMin = charaData.maxHitPointsB;
                    int tempSPMin = charaData.maxSkillPointsB;
                    int tempHPMax = charaData.maxHitPointsB;
                    int tempSPMax = charaData.maxSkillPointsB;
                    int tempStr = charaData.strengthB;
                    int tempVit = charaData.vitalityB;
                    int tempDx = charaData.dexterityB;
                    int tempLuc = charaData.luckB;

                    for (int i = 1; i < level; i++)
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
                base.OnInspectorGUI();
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

                    #region SP
                    switch (spDist)
                    {
                        case STAT_DIST_AMOUNT.VERY_LOW:
                            charaData.maxSkillPointsB = Random.Range(BSvlowBoundPTS.x, BSvlowBoundPTS.y);
                            break;
                        case STAT_DIST_AMOUNT.LOW:
                            charaData.maxSkillPointsB = Random.Range(BSlowBoundPTS.x, BSlowBoundPTS.y);
                            break;
                        case STAT_DIST_AMOUNT.AVERAGE:
                            charaData.maxSkillPointsB = Random.Range(BSavgBoundPTS.x, BSavgBoundPTS.y);
                            break;
                        case STAT_DIST_AMOUNT.HIGH:
                            charaData.maxSkillPointsB = Random.Range(BShighBoundPTS.x, BShighBoundPTS.y);
                            break;
                        case STAT_DIST_AMOUNT.VERY_HIGH:
                            charaData.maxSkillPointsB = Random.Range(BSvhighBoundPTS.x, BSvhighBoundPTS.y);
                            break;
                    }
                    switch (spDistG)
                    {
                        case STAT_DIST_AMOUNT.VERY_LOW:
                            charaData.maxSkillPointsGMin = Random.Range(LWvlowBoundPTS.x, LWvlowBoundPTS.y);
                            if (charaData.maxSkillPointsGMin > HGvlowBoundPTS.x)
                            {
                                charaData.maxSkillPointsGMax = Random.Range(charaData.maxSkillPointsGMin, HGvlowBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxSkillPointsGMax = Random.Range(HGvlowBoundPTS.x, HGvlowBoundPTS.y);
                            }
                            break;

                        case STAT_DIST_AMOUNT.LOW:
                            charaData.maxSkillPointsGMin = Random.Range(LWlowBoundPTS.x, LWlowBoundPTS.y);
                            if (charaData.maxSkillPointsGMin > HGlowBoundPTS.x)
                            {
                                charaData.maxSkillPointsGMax = Random.Range(charaData.maxHitPointsGMin, HGlowBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxSkillPointsGMax = Random.Range(HGlowBoundPTS.x, HGlowBoundPTS.y);
                            }
                            break;

                        case STAT_DIST_AMOUNT.AVERAGE:
                            charaData.maxSkillPointsGMin = Random.Range(LWavgBoundPTS.x, LWavgBoundPTS.y);
                            if (charaData.maxSkillPointsGMin > HGavgBoundPTS.x)
                            {
                                charaData.maxSkillPointsGMax = Random.Range(charaData.maxSkillPointsGMin, HGavgBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxSkillPointsGMax = Random.Range(HGavgBoundPTS.x, HGavgBoundPTS.y);
                            }
                            break;

                        case STAT_DIST_AMOUNT.HIGH:
                            charaData.maxSkillPointsGMin = Random.Range(LWhighBoundPTS.x, LWhighBoundPTS.y);
                            if (charaData.maxSkillPointsGMin > HGhighBoundPTS.x)
                            {
                                charaData.maxSkillPointsGMax = Random.Range(charaData.maxSkillPointsGMin, HGhighBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxSkillPointsGMax = Random.Range(HGhighBoundPTS.x, HGhighBoundPTS.y);
                            }
                            break;

                        case STAT_DIST_AMOUNT.VERY_HIGH:
                            charaData.maxSkillPointsGMin = Random.Range(LWvhighBoundPTS.x, LWvhighBoundPTS.y);
                            if (charaData.maxSkillPointsGMin > HGvhighBoundPTS.x)
                            {
                                charaData.maxSkillPointsGMax = Random.Range(charaData.maxSkillPointsGMin, HGvhighBoundPTS.y);
                            }
                            else
                            {
                                charaData.maxSkillPointsGMax = Random.Range(HGvhighBoundPTS.x, HGvhighBoundPTS.y);
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
                strDist = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Strength distribution", strDist);
                dexDist = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Dexterity distribution", dexDist);
                vitDist = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Vitality distribution", vitDist);
                agilDist = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Agility distribution", agilDist);

                EditorGUILayout.LabelField("Growth stats");
                hpDistG = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Health growth distribution", hpDistG);
                spDistG = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Stamina growth distribution", spDistG);
                strDistG = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Strength growth distribution", strDistG);
                dexDistG = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Dexterity growth distribution", dexDistG);
                vitDistG = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Vitality growth distribution", vitDistG);
                agilDistG = (STAT_DIST_AMOUNT)EditorGUILayout.EnumPopup("Agility growth distribution", agilDistG);


                #endregion
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
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Growth min: ");
                charaData.maxHitPointsGMin = EditorGUILayout.IntField(charaData.maxHitPointsGMin);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Growth max: ");
                charaData.maxHitPointsGMax = EditorGUILayout.IntField(charaData.maxHitPointsGMax);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Stamina: ");
                charaData.maxSkillPointsB = EditorGUILayout.IntField(charaData.maxSkillPointsB);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Growth min: ");
                charaData.maxSkillPointsGMin = EditorGUILayout.IntField(charaData.maxSkillPointsGMin);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Growth max: ");
                charaData.maxSkillPointsGMax = EditorGUILayout.IntField(charaData.maxSkillPointsGMax);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Strength: ");
                charaData.strengthB = EditorGUILayout.IntField(charaData.strengthB);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Growth turns: ");
                charaData.strengthGT = EditorGUILayout.IntField(charaData.strengthGT);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Vitality: ");
                charaData.vitalityB = EditorGUILayout.IntField(charaData.vitalityB);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Growth turns: ");
                charaData.vitalityGT = EditorGUILayout.IntField(charaData.vitalityGT);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Dexterity: ");
                charaData.dexterityB = EditorGUILayout.IntField(charaData.dexterityB);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Growth turns: ");
                charaData.dexterityGT = EditorGUILayout.IntField(charaData.dexterityGT);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Luck: ");
                charaData.luckB = EditorGUILayout.IntField(charaData.luckB);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Growth turns: ");
                charaData.luckGT = EditorGUILayout.IntField(charaData.luckGT);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
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
                /*
                elementSliderSelector = (ELEMENT)EditorGUILayout.EnumPopup(elementSliderSelector);

                EditorGUILayout.BeginHorizontal();
                switch (elementSliderSelector)
                {
                    case ELEMENT.NORMAL:
                        element = "Strike";
                        break;
                    case ELEMENT.PEIRCE:
                        element = "Peirce";
                        break;
                    case ELEMENT.FORCE:
                        element = "Force";
                        break;
                    case ELEMENT.FIRE:
                        element = "Fire";
                        break;
                    case ELEMENT.ICE:
                        element = "Ice";
                        break;
                    case ELEMENT.WIND:
                        element = "Wind";
                        break;
                    case ELEMENT.ELECTRIC:
                        element = "Electirc";
                        break;
                    case ELEMENT.EARTH:
                        element = "Earth";
                        break;
                    case ELEMENT.POISON:
                        element = "Poison";
                        break;
                }
                if (elementSliderSelector != ELEMENT.UNKNOWN)
                {
                    EditorGUILayout.LabelField(element + ": ");
                    data.elementTypeCharts[(int)elementSliderSelector] = EditorGUILayout.Slider(data.elementTypeCharts[(int)elementSliderSelector], -1.9f, 2.9f);
                    EditorGUILayout.EndHorizontal();
                }
                for (int i = 0; i < 13; i++)
                {
                    ELEMENT elemen = (ELEMENT)i;
                    string str = "";

                    switch (elemen)
                    {

                        case ELEMENT.NORMAL:
                            str = "Strike";
                            break;
                        case ELEMENT.PEIRCE:
                            str = "Peirce";
                            break;
                        case ELEMENT.PSYCHIC:
                            str = "Psychic";
                            break;
                        case ELEMENT.WATER:
                            str = "Water";
                            break;
                        case ELEMENT.LIGHT:
                            str = "Light";
                            break;
                        case ELEMENT.DARK:
                            str = "Dark";
                            break;
                        case ELEMENT.FORCE:
                            str = "Force";
                            break;
                        case ELEMENT.FIRE:
                            str = "Fire";
                            break;
                        case ELEMENT.ICE:
                            str = "Ice";
                            break;
                        case ELEMENT.WIND:
                            str = "Wind";
                            break;
                        case ELEMENT.ELECTRIC:
                            str = "Electirc";
                            break;
                        case ELEMENT.EARTH:
                            str = "Earth";
                            break;
                        case ELEMENT.POISON:
                            str = "Poison";
                            break;
                    }

                    ///NOTE THAT 
                    ///-0.000001 -> -1 IS REFLECT
                    ///-1.000001 -> -2 IS ABSORB
                    ///THEY ARE CALCULATED BASED ON THEIR .0 POINTS
                    ///THE FULL NUMBERS JUST TELL WHAT TYPE IT IS

                    if (data.elementTypeCharts[i] == 0)
                        EditorGUILayout.LabelField(str + ": " + data.elementTypeCharts[i] + " Immune");
                    else if (data.elementTypeCharts[i] > 0 && data.elementTypeCharts[i] < 1)
                        EditorGUILayout.LabelField(str + ": " + data.elementTypeCharts[i] + " Resistant");
                    else if (data.elementTypeCharts[i] >= 1 && data.elementTypeCharts[i] < 2)
                        EditorGUILayout.LabelField(str + ": " + data.elementTypeCharts[i] + "");
                    else if (data.elementTypeCharts[i] >= 2 && data.elementTypeCharts[i] < 3)
                        EditorGUILayout.LabelField(str + ": " + data.elementTypeCharts[i] + " Weak");
                    else if (data.elementTypeCharts[i] < 0 && data.elementTypeCharts[i] > -1)
                        EditorGUILayout.LabelField(str + ": " + data.elementTypeCharts[i] + " Reflect");
                    else if (data.elementTypeCharts[i] <= 2 && data.elementTypeCharts[i] > -3)
                        EditorGUILayout.LabelField(str + ": " + data.elementTypeCharts[i] + " Absorb");

                }
                EditorGUILayout.Space();

                actionSliderSelector = (ACTION_TYPE)EditorGUILayout.EnumPopup(actionSliderSelector);
                switch (actionSliderSelector)
                {
                    case ACTION_TYPE.COMFORT:
                        element = "Comfort";
                        break;

                    case ACTION_TYPE.FLIRT:
                        element = "Flirt";
                        break;

                    case ACTION_TYPE.INTELLECT:
                        element = "Intellect";
                        break;

                    case ACTION_TYPE.PLAYFUL:
                        element = "Playful";
                        break;

                    case ACTION_TYPE.THREAT:
                        element = "Threat";
                        break;

                }
                if (actionSliderSelector != ACTION_TYPE.NONE)
                {
                    EditorGUILayout.LabelField(element + ": ");
                    data.actionTypeCharts[(int)actionSliderSelector] = EditorGUILayout.Slider(data.actionTypeCharts[(int)actionSliderSelector], -1.9f, 2.9f);
                    EditorGUILayout.EndHorizontal();
                }
                for (int i = 0; i < 5; i++)
                {
                    ACTION_TYPE elemen = (ACTION_TYPE)i;
                    string str = "";

                    switch (elemen)
                    {

                        case ACTION_TYPE.COMFORT:
                            str = "Comfort";
                            break;

                        case ACTION_TYPE.FLIRT:
                            str = "Flirt";
                            break;

                        case ACTION_TYPE.INTELLECT:
                            str = "Intellect";
                            break;

                        case ACTION_TYPE.PLAYFUL:
                            str = "Playful";
                            break;

                        case ACTION_TYPE.THREAT:
                            str = "Threat";
                            break;
                    }

                    ///NOTE THAT 
                    ///-0.000001 -> -1 IS REFLECT
                    ///-1.000001 -> -2 IS ABSORB
                    ///THEY ARE CALCULATED BASED ON THEIR .0 POINTS
                    ///THE FULL NUMBERS JUST TELL WHAT TYPE IT IS

                    if (data.actionTypeCharts[i] == 0)
                        EditorGUILayout.LabelField(str + ": " + data.actionTypeCharts[i] + " Immune");
                    else if (data.actionTypeCharts[i] > 0 && data.actionTypeCharts[i] < 1)
                        EditorGUILayout.LabelField(str + ": " + data.actionTypeCharts[i] + " Resistant");
                    else if (data.actionTypeCharts[i] >= 1 && data.actionTypeCharts[i] < 2)
                        EditorGUILayout.LabelField(str + ": " + data.actionTypeCharts[i] + "");
                    else if (data.actionTypeCharts[i] >= 2 && data.actionTypeCharts[i] < 3)
                        EditorGUILayout.LabelField(str + ": " + data.actionTypeCharts[i] + " Weak");
                    else if (data.actionTypeCharts[i] < 0 && data.actionTypeCharts[i] > -1)
                        EditorGUILayout.LabelField(str + ": " + data.actionTypeCharts[i] + " Reflect");
                    else if (data.actionTypeCharts[i] <= 2 && data.actionTypeCharts[i] > -3)
                        EditorGUILayout.LabelField(str + ": " + data.actionTypeCharts[i] + " Absorb");

                }
                */
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
