using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(S_StatusEffect))]
public class Ed_Status : Editor
{
    int tab = 0;
    S_StatusEffect data;
    string[] menuSelectOptions;
    const int buffAffectVal = 6;
    private S_Element[] elementList;
    private List<S_Element> inflict;
    private List<S_StatusEffect.s_statusReplace> statusReplace;

    private void OnEnable()
    {
        string[] fileNames = EditorAssetHelper.GetFileNames("S_Element", "Assets/Data/Elements/");
        elementList = new S_Element[fileNames.Length];
        for (int i = 0; i < fileNames.Length; i++) {
            elementList[i] = AssetDatabase.LoadAssetAtPath<S_Element>("Assets/Data/Elements/" + fileNames[i]+ ".asset");
        }
    }

    public override void OnInspectorGUI()
    {
        data = (S_StatusEffect)target;
        if (data != null)
        {
            menuSelectOptions = new string[] { "Overview", "Properties", "Stats", "Elements", "Status effect", "Raw data" };
            tab = GUILayout.Toolbar(tab, menuSelectOptions);
            switch (menuSelectOptions[tab])
            {
                case "Overview":
                    break;
                case "Status effect":
                    EditorGUILayout.LabelField("Replacements", GUILayout.Width(120f));
                    statusReplace = data.statusReplace.ToList();
                    if (GUILayout.Button("Add replace status"))
                    {
                        statusReplace.Add(new S_StatusEffect.s_statusReplace());
                        data.statusReplace = statusReplace.ToArray();
                    }
                    if (data.statusReplace != null)
                    {
                        for (int i = 0; i < statusReplace.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Replace ", GUILayout.Width(120f));
                            data.statusReplace[i].toReplace = EditorGUILayout.ObjectField(data.statusReplace[i].toReplace, typeof(S_StatusEffect), false) as S_StatusEffect;
                            EditorGUILayout.LabelField(" with ", GUILayout.Width(120f));
                            data.statusReplace[i].replace = EditorGUILayout.ObjectField(data.statusReplace[i].replace, typeof(S_StatusEffect), false) as S_StatusEffect;
                            if (GUILayout.Button("-"))
                            {
                                statusReplace.Remove(data.statusReplace[i]);
                                data.statusReplace = statusReplace.ToArray();
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    break;
                case "Properties":
                    EditorGUILayout.LabelField("Remove on round", GUILayout.Width(120f));
                    data.removeOnEndRound = EditorGUILayout.Toggle(data.removeOnEndRound);
                    if (!data.removeOnEndRound)
                    {
                        EditorGUILayout.LabelField("Duration", GUILayout.Width(120f));
                        data.minDuration = EditorGUILayout.IntSlider(data.minDuration, 1, data.maxDuration);
                        data.maxDuration = EditorGUILayout.IntSlider(data.maxDuration, data.minDuration, 10);
                    }
                    EditorGUILayout.LabelField("Restriction type", GUILayout.Width(120f));
                    data.restriction = (S_StatusEffect.RESTRICTION)EditorGUILayout.EnumPopup(data.restriction);
                    EditorGUILayout.LabelField("Variable change", GUILayout.Width(120f));
                    data.variableChange = (S_StatusEffect.VARIABLE_CHANGE)EditorGUILayout.EnumPopup(data.variableChange);
                    if (data.variableChange != S_StatusEffect.VARIABLE_CHANGE.NONE) {
                        data.regenPercentage = EditorGUILayout.Slider(data.regenPercentage, -1, 1);
                        data.regenPercentage = Mathf.Round(data.regenPercentage * 100f) / 100f;
                    }
                    break;
                case "Elements":
                    inflict = data.criticalOnHit.ToList();
                    for (int i = 0; i < elementList.Length; i++)
                    {
                        S_Element curEl = elementList[i];
                        EditorGUILayout.BeginHorizontal();
                        GUI.color = curEl.elementColour;
                        EditorGUILayout.LabelField(curEl.name, GUILayout.Width(120f));
                        GUI.color = Color.white;
                        if (inflict.Contains(curEl))
                        {
                            GUI.backgroundColor = Color.white;
                            if (GUILayout.Button(""))
                            {
                                inflict.Remove(curEl);
                                data.criticalOnHit = inflict.ToArray();
                            }
                            GUI.backgroundColor = Color.white;
                        }
                        else
                        {
                            GUI.backgroundColor = Color.grey;
                            if (GUILayout.Button(""))
                            {
                                inflict.Add(curEl);
                                data.criticalOnHit = inflict.ToArray();
                            }
                            GUI.backgroundColor = Color.white;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    break;

                case "Stats":
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Strength", GUILayout.Width(70f));
                    data.strAffect = EditorGUILayout.IntSlider(data.strAffect, -buffAffectVal, buffAffectVal);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Vitality", GUILayout.Width(70f));
                    data.vitAffect = EditorGUILayout.IntSlider(data.vitAffect, -buffAffectVal, buffAffectVal);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Magic", GUILayout.Width(70f));
                    data.magAffect = EditorGUILayout.IntSlider(data.magAffect, -buffAffectVal, buffAffectVal);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Dexterity", GUILayout.Width(70f));
                    data.dexAffect = EditorGUILayout.IntSlider(data.dexAffect, -buffAffectVal, buffAffectVal);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Agility", GUILayout.Width(70f));
                    data.agiAffect = EditorGUILayout.IntSlider(data.agiAffect, -buffAffectVal, buffAffectVal);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Luck", GUILayout.Width(70f));
                    data.lucAffect = EditorGUILayout.IntSlider(data.lucAffect, -buffAffectVal, buffAffectVal);
                    EditorGUILayout.EndHorizontal();
                    break;
                case "Raw data":
                    base.OnInspectorGUI();
                    break;
            }
        }
    }
}
