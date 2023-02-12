using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/TextArray")]
public class R_TextArray : R_Default
{
    public string[] _textArray;
    public string PickRandom() { 
        return _textArray[Random.Range(0, _textArray.Length)];
    }
}