using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Elements", menuName = "Element holder")]
public class S_Element : ScriptableObject
{
    public float strength;
    public float vitality;
    public float intelligence;
    public float agility;
    public float dexterity;
    public float luck;

    public bool isMagic = true;

    public effect[] statusInflict;
    [System.Serializable]
    public struct effect
    {
        public S_StatusEffect statusEffect;
        public float chance;
        public bool add_remove;
    }
    //For cosmetics
    public Color elementColour = Color.white;
}
