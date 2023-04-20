using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(s_move))]
public class ed_move : Editor
{
    int tab = 0;
    s_move data;
    const int requirmentMax = 20;
    const int buffMax = 5;

    /*
    public void DrawReqGUITool(ref s_move.moveRequirement req)
    {
        req.type = (s_move.moveRequirement.MOVE_REQ_TYPE)EditorGUILayout.EnumPopup(req.type);
        switch (req.type)
        {
            case s_move.moveRequirement.MOVE_REQ_TYPE.ELEMENTAL:
               // req.element = (ELEMENT)EditorGUILayout.EnumPopup(req.element);
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.SPECIFIC:
                req.move = EditorGUILayout.ObjectField(req.move, typeof(s_move), false) as s_move;
                break;
        }
        EditorGUILayout.Space();
    }

    public void DrawReqGUI(s_move.moveRequirement req) {
        string str = "";
        switch (req.type) {
            case s_move.moveRequirement.MOVE_REQ_TYPE.BUFF:
                str = "A buff skill";
                break;
            case s_move.moveRequirement.MOVE_REQ_TYPE.DEBUFF:
                str = "A debuff skill";
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.ELEMENTAL:
                if(req.element != null)
                    str = "A " + req.element.ToString() + " type skill";
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.SPECIFIC:
                str = req.move.name;
                break;
        }
        EditorGUILayout.LabelField(str);
    }
    */
    public override void OnInspectorGUI()
    {
        data = (s_move)target;
        if (data != null)
        {
            tab = GUILayout.Toolbar(tab, new string[] { "Overview", "Properties", "Animation", "Raw data" });
            switch (tab)
            {
                case 0:
                    EditorGUILayout.LabelField("Name: "  + data.name);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Element");

                    /*
                    switch (data.element) {
                        case ELEMENT.FIRE:
                            GUI.color = Color.red;
                            break;
                        case ELEMENT.ICE:
                            GUI.color = Color.cyan;
                            break;
                    }
                    */
                    if(data.element != null)
                        EditorGUILayout.LabelField(data.element.ToString());
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Type");
                    /*
                    switch (data.moveType) {
                        case s_move.MOVE_TYPE.PHYSICAL:
                            GUI.color = Color.grey;
                            break;
                        case s_move.MOVE_TYPE.SPECIAL:
                            GUI.color = Color.blue;
                            break;
                    }
                    */
                    EditorGUILayout.LabelField(data.moveType.ToString());
                    GUI.color = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("Base damage: " + data.power);
                    string requirements = "Stat requirements: ";
                    /*
                    if (data.strReq > 0)
                        requirements += " Str - " + data.strReq + " ";
                    if (data.vitReq > 0)
                        requirements += " Vit - " + data.vitReq + " ";
                    if (data.dxReq > 0)
                        requirements += " Dex - " + data.dxReq + " ";
                    if (data.agiReq > 0)
                        requirements += " Agi - " + data.agiReq + " ";
                    */
                    EditorGUILayout.LabelField(requirements);
                    EditorGUILayout.BeginHorizontal();
                    string tech = "";
                    /*
                    switch (data.comboType)
                    {
                        case s_move.MOVE_QUANITY_TYPE.MONO_TECH:
                            tech = "Single tech";
                            break;

                        case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:
                            tech = "Dual tech";
                            break;

                        case s_move.MOVE_QUANITY_TYPE.TRIPLE_TECH:
                            tech = "Triple tech";
                            break;

                        case s_move.MOVE_QUANITY_TYPE.QUAD_TECH:
                            tech = "Quad tech";
                            break;

                        case s_move.MOVE_QUANITY_TYPE.PENTA_TECH:
                            tech = "Penta tech";
                            break;
                    }
                    */
                    EditorGUILayout.LabelField(tech);
                    GUI.color = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    /*
                    if (data.moveRequirements != null)
                    {
                        foreach (var cmb in data.moveRequirements)
                        {
                            switch (cmb.comboType)
                            {
                                case s_move.MOVE_QUANITY_TYPE.DUAL_TECH:
                                    DrawReqGUI(cmb.Req1);
                                    DrawReqGUI(cmb.Req2);
                                    break;

                                case s_move.MOVE_QUANITY_TYPE.TRIPLE_TECH:
                                    DrawReqGUI(cmb.Req1);
                                    DrawReqGUI(cmb.Req2);
                                    DrawReqGUI(cmb.Req3);
                                    break;

                                case s_move.MOVE_QUANITY_TYPE.QUAD_TECH:
                                    DrawReqGUI(cmb.Req1);
                                    DrawReqGUI(cmb.Req2);
                                    DrawReqGUI(cmb.Req3);
                                    DrawReqGUI(cmb.Req4);
                                    break;

                                case s_move.MOVE_QUANITY_TYPE.PENTA_TECH:
                                    DrawReqGUI(cmb.Req1);
                                    DrawReqGUI(cmb.Req2);
                                    DrawReqGUI(cmb.Req3);
                                    DrawReqGUI(cmb.Req4);
                                    DrawReqGUI(cmb.Req5);
                                    break;
                            }
                        }
                    }
                    */
                    break;

                case 1:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Power:");
                    data.power = EditorGUILayout.IntField(data.power);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Cost:");
                    data.cost = EditorGUILayout.IntField(data.cost);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    /*
                    EditorGUILayout.LabelField("Element:");
                    data.element = (ELEMENT)EditorGUILayout.EnumPopup(data.element);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    */
                    EditorGUILayout.LabelField("Type:");
                    data.moveType = (s_move.MOVE_TYPE)EditorGUILayout.EnumPopup(data.moveType);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    /*
                    switch (data.moveType) {
                        case s_move.MOVE_TYPE.STATUS:
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Heal health?");
                            data.healHealth = EditorGUILayout.Toggle(data.healHealth);
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Heal stamina?");
                            data.healStamina = EditorGUILayout.Toggle(data.healStamina);
                            EditorGUILayout.Space();
                            EditorGUILayout.EndHorizontal();
                            break;
                    }
                    */
                    EditorGUILayout.LabelField("Status stuff:");
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Strength:");
                    data.strBuff = EditorGUILayout.IntSlider(data.strBuff, -buffMax, buffMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Dexterity:");
                    data.dexBuff = EditorGUILayout.IntSlider(data.dexBuff, -buffMax, buffMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Vitality:");
                    data.vitBuff = EditorGUILayout.IntSlider(data.vitBuff, -buffMax, buffMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Agility:");
                    data.agiBuff = EditorGUILayout.IntSlider(data.agiBuff, -buffMax, buffMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Luck:");
                    data.lucBuff = EditorGUILayout.IntSlider(data.lucBuff, -buffMax, buffMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Intelligence:");
                    data.intBuff = EditorGUILayout.IntSlider(data.intBuff, -buffMax, buffMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Element:");
                    //data.element = (ELEMENT)EditorGUILayout.EnumPopup(data.element);
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Strength requirment:");
                    data.strReq = EditorGUILayout.IntSlider(data.strReq, 0, requirmentMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Vitality requirment:");
                    data.vitReq = EditorGUILayout.IntSlider(data.vitReq, 0, requirmentMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Dexterity requirment:");
                    data.dxReq = EditorGUILayout.IntSlider(data.dxReq, 0, requirmentMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Agility requirment:");
                    data.agiReq = EditorGUILayout.IntSlider(data.agiReq, 0, requirmentMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Intelligence requirment:");
                    data.intReq = EditorGUILayout.IntSlider(data.intReq, 0, requirmentMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Luck requirment:");
                    data.lucReq = EditorGUILayout.IntSlider(data.lucReq, 0, requirmentMax);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    break;

                case 3:
                    base.OnInspectorGUI();
                    break;
            }
        }
    }
}
