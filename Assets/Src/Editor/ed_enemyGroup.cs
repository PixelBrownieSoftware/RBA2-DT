using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(s_enemyGroup))]
[CanEditMultipleObjects]
public class ed_enemyGroup : Editor
{
    int lastTab = 0;
    int tab = 0;
    string[] menuSelectOptions;
    bool[] displayCharacters;
    bool[] displayCharactersExtraSkills;
    bool[] displayCharactersExtraPassives;
    bool[] displayCharactersSummon;
    bool[] displayCharactersSummonExtraSkills;
    bool[] displayCharactersSummonExtraPassives;
    s_enemyGroup enGroupData;
    public void SetLevel(ref s_enemyGroup.s_groupMember currentChar) {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level ");
        currentChar.levType = (s_enemyGroup.s_groupMember.LEVEL_TYPE)EditorGUILayout.EnumPopup(currentChar.levType, GUILayout.Width(105f));
        switch (currentChar.levType)
        {
            case s_enemyGroup.s_groupMember.LEVEL_TYPE.FIXED:
                currentChar.level = EditorGUILayout.IntSlider(currentChar.level, 1, 50);
                currentChar.maxLevel = currentChar.level;
                break;
            case s_enemyGroup.s_groupMember.LEVEL_TYPE.RANDOM:
                currentChar.level = EditorGUILayout.IntSlider(currentChar.level, 1, 50);
                currentChar.maxLevel = EditorGUILayout.IntSlider(currentChar.maxLevel, 1, 50);
                currentChar.level = Mathf.Clamp(currentChar.level, 1, currentChar.maxLevel);
                break;
        }
        EditorGUILayout.EndHorizontal();
    }
    public void DrawCharacterStatus(
        ref s_enemyGroup.s_groupMember currentChar,
        ref bool displayChar,
        ref bool displayCharSkill,
        ref bool displayCharPassive,
        ref bool[] displayCharList,
        ref bool[] displayCharSkillList,
        ref bool[] displayCharPassivList,
        ref s_enemyGroup.s_groupMember[] groupMembers
        )
    {
        o_battleCharDataN charaSetter = currentChar.memberDat;
        string charaFoldoutDisplay = "None (Set character)";
        if (charaSetter != null)
        {
            charaFoldoutDisplay = charaSetter.name;
        }
        if (displayCharList != null)
        {
            EditorGUILayout.BeginHorizontal();
            displayChar = EditorGUILayout.Foldout(displayChar, charaFoldoutDisplay);
            if (GUILayout.Button("-", GUILayout.Width(20f)))
            {
                List<s_enemyGroup.s_groupMember> members = new List<s_enemyGroup.s_groupMember>();
                members = groupMembers.ToList();
                members.Remove(currentChar);
                groupMembers = members.ToArray();
                displayCharList = new bool[groupMembers.Length];
                displayCharSkillList = new bool[groupMembers.Length];
                displayCharPassivList = new bool[groupMembers.Length];
                Repaint();
                return;
            }
            EditorGUILayout.EndHorizontal();
        }
        else {
            displayChar = true;
        }
        if (displayChar)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.Space();
            currentChar.memberDat = EditorGUILayout.ObjectField(currentChar.memberDat, typeof(o_battleCharDataN), false) as o_battleCharDataN;
            SetLevel(ref currentChar);
            EditorGUILayout.BeginHorizontal();
            displayCharSkill = EditorGUILayout.Foldout(displayCharSkill, "Extra skills");
            if (GUILayout.Button("+", GUILayout.Width(130f)))
            {
                List<s_move> moves = new List<s_move>();
                moves = currentChar.extraSkills.ToList();
                moves.Add(new s_move());
                currentChar.extraSkills = moves.ToArray();
                Repaint();
                return;
            }
            EditorGUILayout.EndHorizontal();
            if (displayCharSkill)
            {
                EditorGUI.indentLevel++;
                for (int mI = 0; mI < currentChar.extraSkills.Length; mI++)
                {
                    EditorGUILayout.BeginHorizontal();
                    currentChar.extraSkills[mI] = EditorGUILayout.ObjectField(currentChar.extraSkills[mI], typeof(s_move), false) as s_move;
                    if (GUILayout.Button("-", GUILayout.Width(20f)))
                    {
                        EditorGUILayout.BeginHorizontal();
                        List<s_move> moves = new List<s_move>();
                        moves = currentChar.extraSkills.ToList();
                        moves.RemoveAt(mI);
                        currentChar.extraSkills = moves.ToArray();
                        Repaint();
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.BeginHorizontal();
            displayCharPassive = EditorGUILayout.Foldout(displayCharPassive, "Passives");
            if (GUILayout.Button("+", GUILayout.Width(130f)))
            {
                List<S_Passive> moves = new List<S_Passive>();
                moves = currentChar.passives.ToList();
                moves.Add(new S_Passive());
                currentChar.passives = moves.ToArray();
                Repaint();
                return;
            }
            EditorGUILayout.EndHorizontal();
            if (displayCharPassive)
            {
                EditorGUI.indentLevel++;
                for (int mI = 0; mI < currentChar.passives.Length; mI++)
                {
                    EditorGUILayout.BeginHorizontal();
                    currentChar.passives[mI] = EditorGUILayout.ObjectField(currentChar.passives[mI], typeof(S_Passive), false) as S_Passive;
                    if (GUILayout.Button("-", GUILayout.Width(35f)))
                    {
                        List<S_Passive> moves = new List<S_Passive>();
                        moves = currentChar.passives.ToList();
                        moves.RemoveAt(mI);
                        currentChar.passives = moves.ToArray();
                        Repaint();
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.Space();
    }

    public override void OnInspectorGUI() {
        enGroupData = (s_enemyGroup)target;
        if (enGroupData != null)
        {
            List<string> optionsMenuList = new List<string>() { "Overview", "Enemies", "Enemy summonables"};
            
            if (enGroupData.fixedPlayers) {
                optionsMenuList.Add("Players");
            }
            if (enGroupData.guestInvolved)
            {
                optionsMenuList.Add("Guest");
            }
            optionsMenuList.Add("Raw data");
            menuSelectOptions = optionsMenuList.ToArray();
            tab = GUILayout.Toolbar(tab, menuSelectOptions);
            switch (menuSelectOptions[tab]) {
                case "Overview":
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Fixed players? ");
                        enGroupData.fixedPlayers = EditorGUILayout.Toggle(enGroupData.fixedPlayers);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Has guest? ");
                        enGroupData.guestInvolved = EditorGUILayout.Toggle(enGroupData.guestInvolved);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Fleeable? ");
                        enGroupData.fleeable = EditorGUILayout.Toggle(enGroupData.fleeable);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Complete once? ");
                        enGroupData.perishIfDone = EditorGUILayout.Toggle(enGroupData.perishIfDone);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                        foreach (var chara in enGroupData.members)
                        {
                            string display = "";
                            if (chara.memberDat != null)
                            {
                                display = chara.memberDat.name;
                                switch (chara.levType)
                                {
                                    case s_enemyGroup.s_groupMember.LEVEL_TYPE.FIXED:
                                        display += " Level " + chara.level;
                                        break;
                                    case s_enemyGroup.s_groupMember.LEVEL_TYPE.RANDOM:
                                        display += " Level " + chara.level + " - " + chara.maxLevel;
                                        break;
                                }
                            }
                            else
                                display = "None (Set character)";
                            EditorGUILayout.LabelField(display);
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Branches ");
                        if (GUILayout.Button("+", GUILayout.Width(20f)))
                        {
                            List<s_enemyGroup> groups = enGroupData.branches.ToList();
                            groups.Add(new s_enemyGroup());
                            enGroupData.branches = groups.ToArray();
                            Repaint();
                        }
                        EditorGUILayout.EndHorizontal();
                        for (int i = 0; i < enGroupData.branches.Length; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            enGroupData.branches[i] = EditorGUILayout.ObjectField(enGroupData.branches[i], typeof(s_enemyGroup), false) as s_enemyGroup;
                            if (GUILayout.Button("-", GUILayout.Width(20f)))
                            {
                                List<s_enemyGroup> groups = enGroupData.branches.ToList();
                                groups.RemoveAt(i);
                                enGroupData.branches = groups.ToArray();
                                Repaint();
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Unlock characters");
                        if (GUILayout.Button("+", GUILayout.Width(20f)))
                        {
                            List<o_battleCharDataN> characters = enGroupData.unlockCharacters.ToList();
                            characters.Add(new o_battleCharDataN());
                            enGroupData.unlockCharacters = characters.ToArray();
                            Repaint();
                        }
                        EditorGUILayout.EndHorizontal();
                        if (enGroupData.unlockCharacters != null)
                        {
                            for (int i = 0; i < enGroupData.unlockCharacters.Length; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                enGroupData.unlockCharacters[i] = EditorGUILayout.ObjectField(enGroupData.unlockCharacters[i], typeof(o_battleCharDataN), false) as o_battleCharDataN;
                                if (GUILayout.Button("-", GUILayout.Width(20f)))
                                {
                                    List<o_battleCharDataN> characters = enGroupData.unlockCharacters.ToList();
                                    characters.RemoveAt(i);
                                    enGroupData.unlockCharacters = characters.ToArray();
                                    Repaint();
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                    break;
                case "Enemies":
                    {
                        if (tab != lastTab)
                        {
                            if (displayCharacters == null)
                                displayCharacters = new bool[enGroupData.members.Length];
                            if (displayCharactersExtraSkills == null)
                                displayCharactersExtraSkills = new bool[enGroupData.members.Length];
                            if (displayCharactersExtraPassives == null)
                                displayCharactersExtraPassives = new bool[enGroupData.members.Length];

                            if (displayCharacters.Length == 0)
                                displayCharacters = new bool[enGroupData.members.Length];
                            if (displayCharactersExtraSkills.Length == 0)
                                displayCharactersExtraSkills = new bool[enGroupData.members.Length];
                            if (displayCharactersExtraPassives.Length == 0)
                                displayCharactersExtraPassives = new bool[enGroupData.members.Length];

                            if (displayCharactersSummon == null)
                                displayCharactersSummon = new bool[enGroupData.members_pre_summon.Length];
                            if (displayCharactersSummonExtraSkills == null)
                                displayCharactersSummonExtraSkills = new bool[enGroupData.members_pre_summon.Length];
                            if (displayCharactersSummonExtraPassives == null)
                                displayCharactersSummonExtraPassives = new bool[enGroupData.members_pre_summon.Length];

                            if (displayCharactersSummon.Length == 0)
                                displayCharactersSummon = new bool[enGroupData.members_pre_summon.Length];
                            if (displayCharactersSummonExtraSkills.Length == 0)
                                displayCharactersSummonExtraSkills = new bool[enGroupData.members_pre_summon.Length];
                            if (displayCharactersSummonExtraPassives.Length == 0)
                                displayCharactersSummonExtraPassives = new bool[enGroupData.members_pre_summon.Length];
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Enemies", GUILayout.Width(55f));
                        if (GUILayout.Button("+", GUILayout.Width(20f)))
                        {
                            List<s_enemyGroup.s_groupMember> members = enGroupData.members.ToList();
                            members.Add(new s_enemyGroup.s_groupMember());
                            enGroupData.members = members.ToArray();
                            displayCharacters = new bool[enGroupData.members.Length];
                            displayCharactersExtraSkills = new bool[enGroupData.members.Length];
                            displayCharactersExtraPassives = new bool[enGroupData.members.Length];
                            Repaint();
                        }
                        EditorGUI.indentLevel++;
                        EditorGUILayout.EndHorizontal();
                        for (int i = 0; i < enGroupData.members.Length; i++)
                        {
                            s_enemyGroup.s_groupMember currentChar = enGroupData.members[i];

                            DrawCharacterStatus(ref currentChar,
                                ref displayCharacters[i],
                                ref displayCharactersExtraSkills[i],
                                ref displayCharactersExtraPassives[i],
                                ref displayCharacters,
                                ref displayCharactersExtraSkills,
                                ref displayCharactersExtraPassives,
                                ref enGroupData.members);
                        }
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;

                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Pre-summon");
                        if (GUILayout.Button("Add Enemy pre summon"))
                        {
                            List<s_enemyGroup.s_groupMember> members = new List<s_enemyGroup.s_groupMember>();
                            members = enGroupData.members_pre_summon.ToList();
                            members.Add(new s_enemyGroup.s_groupMember());
                            enGroupData.members_pre_summon = members.ToArray();
                            displayCharactersSummon = new bool[enGroupData.members_pre_summon.Length];
                            displayCharactersSummonExtraSkills = new bool[enGroupData.members_pre_summon.Length];
                            displayCharactersSummonExtraPassives = new bool[enGroupData.members_pre_summon.Length];
                            Repaint();
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                        for (int i = 0; i < enGroupData.members_pre_summon.Length; i++)
                        {
                            s_enemyGroup.s_groupMember currentChar = enGroupData.members_pre_summon[i];
                            DrawCharacterStatus(ref currentChar,
                                ref displayCharactersSummon[i],
                                ref displayCharactersSummonExtraSkills[i],
                                ref displayCharactersSummonExtraPassives[i],
                                ref displayCharactersSummon,
                                ref displayCharactersSummonExtraSkills,
                                ref displayCharactersSummonExtraPassives,
                                ref enGroupData.members_pre_summon);
                            EditorGUILayout.Space();
                        }
                        EditorGUILayout.Space();
                    }
                    break;
                case "Players":
                    {
                        if (tab != lastTab)
                        {
                            if (displayCharacters == null)
                                displayCharacters = new bool[enGroupData.members_Player.Length];
                            if (displayCharactersExtraSkills == null)
                                displayCharactersExtraSkills = new bool[enGroupData.members_Player.Length];
                            if (displayCharactersExtraPassives == null)
                                displayCharactersExtraPassives = new bool[enGroupData.members_Player.Length];

                            if (displayCharacters.Length == 0)
                                displayCharacters = new bool[enGroupData.members_Player.Length];
                            if (displayCharactersExtraSkills.Length == 0)
                                displayCharactersExtraSkills = new bool[enGroupData.members_Player.Length];
                            if (displayCharactersExtraPassives.Length == 0)
                                displayCharactersExtraPassives = new bool[enGroupData.members_Player.Length];

                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Enemies", GUILayout.Width(55f));
                        if (GUILayout.Button("+", GUILayout.Width(20f)))
                        {
                            List<s_enemyGroup.s_groupMember> members = enGroupData.members_Player.ToList();
                            members.Add(new s_enemyGroup.s_groupMember());
                            enGroupData.members_Player = members.ToArray();
                            displayCharacters = new bool[enGroupData.members_Player.Length];
                            displayCharactersExtraSkills = new bool[enGroupData.members_Player.Length];
                            displayCharactersExtraPassives = new bool[enGroupData.members_Player.Length];
                            Repaint();
                        }
                        EditorGUI.indentLevel++;
                        EditorGUILayout.EndHorizontal();
                        for (int i = 0; i < enGroupData.members_Player.Length; i++)
                        {
                            s_enemyGroup.s_groupMember currentChar = enGroupData.members_Player[i];

                            DrawCharacterStatus(ref currentChar,
                                ref displayCharacters[i],
                                ref displayCharactersExtraSkills[i],
                                ref displayCharactersExtraPassives[i],
                                ref displayCharacters,
                                ref displayCharactersExtraSkills,
                                ref displayCharactersExtraPassives,
                                ref enGroupData.members_Player);
                        }
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;

                        EditorGUILayout.Space();
                    }
                    break;

                case "Enemy summonables":
                    {
                        if (tab != lastTab)
                        {
                            if (displayCharacters == null)
                                displayCharacters = new bool[enGroupData.members_summonable.Length];
                            if (displayCharactersExtraSkills == null)
                                displayCharactersExtraSkills = new bool[enGroupData.members_summonable.Length];
                            if (displayCharactersExtraPassives == null)
                                displayCharactersExtraPassives = new bool[enGroupData.members_summonable.Length];

                            if (displayCharacters.Length == 0)
                                displayCharacters = new bool[enGroupData.members_summonable.Length];
                            if (displayCharactersExtraSkills.Length == 0)
                                displayCharactersExtraSkills = new bool[enGroupData.members_summonable.Length];
                            if (displayCharactersExtraPassives.Length == 0)
                                displayCharactersExtraPassives = new bool[enGroupData.members_summonable.Length];
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Summonables", GUILayout.Width(55f));
                        if (GUILayout.Button("+", GUILayout.Width(20f)))
                        {
                            List<s_enemyGroup.s_groupMember> members = enGroupData.members_summonable.ToList();
                            members.Add(new s_enemyGroup.s_groupMember());
                            enGroupData.members_summonable = members.ToArray();
                            displayCharacters = new bool[enGroupData.members_summonable.Length];
                            displayCharactersExtraSkills = new bool[enGroupData.members_summonable.Length];
                            displayCharactersExtraPassives = new bool[enGroupData.members_summonable.Length];
                            Repaint();
                            break;
                        }
                        EditorGUI.indentLevel++;
                        EditorGUILayout.EndHorizontal();
                        if (enGroupData.members_summonable != null)
                        {
                            for (int i = 0; i < enGroupData.members_summonable.Length; i++)
                            {
                                s_enemyGroup.s_groupMember currentChar = enGroupData.members_summonable[i];
                                DrawCharacterStatus(ref currentChar,
                                    ref displayCharacters[i],
                                    ref displayCharactersExtraSkills[i],
                                    ref displayCharactersExtraPassives[i],
                                    ref displayCharacters,
                                    ref displayCharactersExtraSkills,
                                    ref displayCharactersExtraPassives,
                                    ref enGroupData.members_summonable);
                            }
                        }
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    break;

                case "Guest":
                    {
                        s_enemyGroup.s_groupMember currentChar = enGroupData.member_Guest;
                        bool displayChar = true;
                        bool[] displayStuff = null;
                        s_enemyGroup.s_groupMember[] dummyDisplay = null;
                        DrawCharacterStatus(ref currentChar,
                            ref displayChar,
                            ref displayChar,
                            ref displayChar,
                            ref displayStuff,
                            ref displayStuff,
                            ref displayStuff,
                            ref dummyDisplay);
                    }
                    break;

                case "Raw data":
                    Repaint();
                    base.OnInspectorGUI();
                    break;
            }
            lastTab = tab;
        }
    }
}