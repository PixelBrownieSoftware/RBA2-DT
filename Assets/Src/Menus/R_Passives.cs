using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Passives")]
public class R_Passives : R_Default
{
    public List<S_Passive> passives = new List<S_Passive>();

    public S_Passive PickRandom()
    {
        return passives[Random.Range(0, passives.Count)];
    }

    public void Clear()
    {
        passives.Clear();
    }

    public void SetMoves(List<S_Passive> moves)
    {
        passives = moves;
    }
    public void AddMoves(List<S_Passive> moves)
    {
        passives.AddRange(moves);
    }

    public void AddMove(S_Passive mov)
    {
        passives.Add(mov);
    }

    public bool ListContains(S_Passive targ)
    {
        return passives.Contains(targ);
    }

    public void RemoveMove(S_Passive targ)
    {
        passives.Remove(targ);
    }
    public S_Passive GetPassive(string index)
    {
        return passives.Find(x => x.name == index);
    }

    public S_Passive GetPassive(int index)
    {
        return passives[index];
    }
}
