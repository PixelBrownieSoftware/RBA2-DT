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

    public s_enemyGroup GetGroup(string group) {
        return groupList.Find(x => x.name == group);
    }
    public void RemoveGroup(s_enemyGroup group)
    {
        groupList.Remove(group);
    }

    public void Clear() {
        groupList.Clear();
    }

    public void AddGroup(s_enemyGroup group) {
        if (groupList == null)
            groupList = new List<s_enemyGroup>();
        groupList.Add(group);
    }
}
