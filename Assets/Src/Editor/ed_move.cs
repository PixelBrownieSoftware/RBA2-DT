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

    public void DrawReqGUITool(ref s_move.moveRequirement req)
    {
        req.type = (s_move.moveRequirement.MOVE_REQ_TYPE)EditorGUILayout.EnumPopup(req.type);
        switch (req.type)
        {
            case s_move.moveRequirement.MOVE_REQ_TYPE.ELEMENTAL:
                req.element = (ELEMENT)EditorGUILayout.EnumPopup(req.element);
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
                str = "A " + req.element.ToString() + " type skill";
                break;

            case s_move.moveRequirement.MOVE_REQ_TYPE.SPECIFIC:
                str = req.move.name;
                break;
        }
        EditorGUILayout.LabelField(str);
    }

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

                    switch (data.element) {
                        case ELEMENT.FIRE:
                            GUI.color = Color.red;
                            break;
                        case ELEMENT.ICE:
                            GUI.color = Color.cyan;
                            break;
                    }
                    EditorGUILayout.LabelField(data.element.ToString());
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Type");
                    switch (data.moveType) {
                        case s_move.MOVE_TYPE.PHYSICAL:
                            GUI.color = Color.grey;
                            break;
                        case s_move.MOVE_TYPE.SPECIAL:
                            GUI.color = Color.blue;
                            break;
                    }
                    EditorGUILayout.LabelField(data.moveType.ToString());
                    GUI.color = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("Base damage: " + data.power);
                    string requirements = "Stat requirements: ";
                    if (data.strReq > 0)
                        requirements += " Str - " + data.strReq + " ";
                    if (data.vitReq > 0)
                        requirements += " Vit - " + data.vitReq + " ";
                    if (data.dxReq > 0)
                        requirements += " Dex - " + data.dxReq + " ";
                    if (data.agiReq > 0)
                        requirements += " Agi - " + data.agiReq + " ";
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
                    break;

                case 3:
                    base.OnInspectorGUI();
                    break;
            }
        }
    }
}
