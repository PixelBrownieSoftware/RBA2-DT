using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Registers/Boolean")]
public class R_Boolean : R_Default
{
    public bool boolean;
    [SerializeField]
    private bool defaultBoolean;

    private void OnEnable()
    {
        if (_isReset)
        {
            boolean = defaultBoolean;
        }
    }
}

