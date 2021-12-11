using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public ELEMENT_WEAKNESS this[ELEMENT element] {
        get {
            switch (element) {
                case ELEMENT.STRIKE:
                    return strike;
                case ELEMENT.PEIRCE:
                    return peirce;
                case ELEMENT.FIRE:
                    return fire;
                case ELEMENT.ICE:
                    return ice;
                case ELEMENT.WATER:
                    return water;
                case ELEMENT.ELECTRIC:
                    return electric;

            }
            return none;
        }
    }
    ELEMENT_WEAKNESS none;
    public ELEMENT_WEAKNESS strike;
    public ELEMENT_WEAKNESS peirce;

    public ELEMENT_WEAKNESS fire;
    public ELEMENT_WEAKNESS ice;
    public ELEMENT_WEAKNESS water;
    public ELEMENT_WEAKNESS electric;
    public ELEMENT_WEAKNESS wind;
    public ELEMENT_WEAKNESS earth;
    public ELEMENT_WEAKNESS psychic;
    public ELEMENT_WEAKNESS light;
    public ELEMENT_WEAKNESS dark;
    public ELEMENT_WEAKNESS bio;
}

[CreateAssetMenu(fileName = "Elements", menuName = "Element holder")]
public class s_element : ScriptableObject
{
    public string[] elements;
    
}
