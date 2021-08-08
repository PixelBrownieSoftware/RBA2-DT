using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class o_weapon : s_move
{
    public enum WEAPON_TYPE { 
        FIST,
        SWORD,
        GUN,
        STAFF
    }

    public WEAPON_TYPE weaponType;

}
