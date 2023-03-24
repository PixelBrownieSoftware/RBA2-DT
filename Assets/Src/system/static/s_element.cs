using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
[System.Serializable]
public class element_affinity
{
    public Tuple<int,int> this[ELEMENT element]
    {
        get
        {
            switch (element)
            {
                case ELEMENT.STRIKE:
                    break;
                case ELEMENT.PEIRCE:
                    break;
                case ELEMENT.FIRE:
                    break;
                case ELEMENT.ICE:
                    break;
                case ELEMENT.WATER:
                    break;
                case ELEMENT.ELECTRIC:
                    break;
            }
            return new Tuple<int, int>(0,0);
        }
    }
    public int CalculateTotal() {
        return strike +
            peirce +
            fire +
            ice +
            water +
            electric +
            wind +
            earth +
            psychic +
            light +
            dark +
            heal +
            support;
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
[System.Serializable]
public struct element_weaknesses
{
    public float this[ELEMENT element]
    {
        get
        {
            if (weaknessChart != null) {
                if (weaknessChart.Length > 0)
                {
                    foreach (var weak in weaknessChart)
                    {
                        if (weak.element == element)
                            return weak.weakness;
                    }
                }
                else
                    return 1f;
            }
            return 1f;
        }
    }
    public weaknesses[] weaknessChart;

    [System.Serializable]
    public struct weaknesses
    {
        public ELEMENT element;
        public float weakness;
    }
}

[CreateAssetMenu(fileName = "Elements", menuName = "Element holder")]
public class s_element : ScriptableObject
{
    public string[] elements;
    
}
*/