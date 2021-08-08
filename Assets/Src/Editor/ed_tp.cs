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