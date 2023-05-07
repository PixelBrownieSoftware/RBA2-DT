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
    public override void OnInspectorGUI()
    {
        data = (S_StatusEffect)target;
        if (data != null)
        {
            tab = GUILayout.Toolbar(tab, new string[] { "Overview", "Properties", "Stats", "Raw data" });
            switch (tab)
            {
                case 3:
                    base.OnInspectorGUI();
                    break;
            }
        }
    }
}
