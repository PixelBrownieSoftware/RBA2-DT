using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Integer")]
public class R_Int : R_Default
{
    public int integer;
    [SerializeField]
    private int defaultInteger = 0;

    public void Set(int num) {
        integer = num;
    }
    private void OnEnable()
    {
        if (_isReset)
        {
            integer = defaultInteger;
        }
    }
}
