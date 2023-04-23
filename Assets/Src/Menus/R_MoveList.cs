using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Move list")]
public class R_MoveList : R_Default
{
    public List<s_move> moveListRef = new List<s_move>();

    private void OnDisable()
    {
        if (_isReset)
        {
            moveListRef.Clear();
        }
    }

    public s_move PickRandom() {
        return moveListRef[Random.Range(0, moveListRef.Count)];
    }

    public void Clear() {
        moveListRef.Clear();
    }

    public void SetMoves(List<s_move> moves)
    {
        moveListRef= moves;
    }
    public void AddMoves(List<s_move> moves) {
        moveListRef.AddRange(moves);
    }

    public void AddMove(s_move mov) {
        moveListRef.Add(mov);
    }

    public bool ListContains(s_move targ)
    {
        return moveListRef.Contains(targ);
    }

    public void RemoveMove(s_move targ) { 
        moveListRef.Remove(targ);
    }
    public s_move GetMove(string index)
    {
        return moveListRef.Find(x => x.name == index);
    }

    public s_move GetMove(int index)
    {
        return moveListRef[index];
    }
}
