using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Move list")]
public class R_MoveList : R_Default
{
    public List<s_move> moveListRef = new List<s_move>();

    public s_move PickRandom() { 
        return moveListRef[Random.Range(0, moveListRef.Count)];
    }

    public bool ListContains(s_move targ)
    {
        return moveListRef.Contains(targ);
    }

    public void RemoveMove(s_move targ) { 
        moveListRef.Remove(targ);
    }

    public s_move GetChracter(int index)
    {
        return moveListRef[index];
    }
}
