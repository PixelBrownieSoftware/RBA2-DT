using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class o_locSpr : MonoBehaviour
{
    o_locationOverworld loc;
    public void Update()
    {
        if (loc == null)
            loc = GetComponent<o_locationOverworld>();
        else
            loc.ChangeSprite();
    }

}
