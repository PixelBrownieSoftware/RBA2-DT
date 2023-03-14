using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.System;
using MagnumFoundation2.Objects;

[System.Serializable]
public class s_battleEvent
{
    //public List<s_bEvent> events;
    public ev_script eventScript;
    public enum B_COND
    {
        HEALTH,
        STAMINA,
        TURNS_ELAPSED,
        MOVE_USED
    }
    public enum B_CHECK_COND
    {
        PER_TURN,
        ON_START
    }
    public B_COND battleCond;
    public B_CHECK_COND battleCheckCond;
    public bool enabled = true;
    public int int0;
}

[CreateAssetMenu(fileName = "New enemy group", menuName = "Battle group")]
public class s_enemyGroup : ScriptableObject
{
    [System.Serializable]
    public class s_groupMember{
        public o_battleCharDataN memberDat;
        public enum LEVEL_TYPE {
            RANDOM,
            FIXED
        }
        public LEVEL_TYPE levType;
        public int level;
        public int maxLevel;
        public s_move[] extraSkills;
        public s_passive[] passives;
        public o_weapon[] weaponSelections;
    }
    public bool fleeable = true;
    public bool fixedPlayers = false;
    public bool guestInvolved = false;
    public bool perishIfDone = false;
    public string sceneToGoTo;
    public s_groupMember[] members;
    public s_groupMember[] members_summonable;
    public s_groupMember[] members_pre_summon;
    public s_groupMember[] members_Player;
    public s_groupMember member_Guest;
    public s_battleEvent[] events;

    public o_battleCharDataN[] unlockCharacters;

    public s_enemyGroup[] branches;
    public Shop_item[] shopItems;

    public Sprite bg1;
    public Sprite bg2;
    public Material material1;
    public Material material2;
}
