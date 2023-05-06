using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Colour")]
public class R_Colour : R_Default
{
    public Color colour;
    public void Set(Color colour) {
        this.colour = colour;
    }
}
