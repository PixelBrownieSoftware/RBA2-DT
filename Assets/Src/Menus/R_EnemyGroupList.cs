using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Enemy group list")]
public class R_EnemyGroupList : R_Default
{
    public List<s_enemyGroup> groupList = new List<s_enemyGroup>();
}
