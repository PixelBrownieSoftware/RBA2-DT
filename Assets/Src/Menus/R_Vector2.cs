using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Vector2")]
public class R_Vector2 : R_Default
{
    public Vector2 vector2;
    public void Set(Vector2 v) {
        vector2 = v;
    }
}
