using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Enemy group list")]
public class R_EnemyGroupList : R_Default
{
    public List<s_enemyGroup> groupList = new List<s_enemyGroup>();
    [SerializeField]
    private List<s_enemyGroup> groupListDefault = new List<s_enemyGroup>();

    private void OnEnable()
    {
        if (_isReset)
        {
            groupList.AddRange(groupListDefault);
        }
    }

    private void OnDisable()
    {
        if (_isReset)
        {
            groupList.Clear();
        }
    }
    public void RemoveGroup(s_enemyGroup group)
    {
        groupList.Remove(group);
    }

    public void AddGroup(s_enemyGroup group) {
        groupList.Add(group);
    }
}
