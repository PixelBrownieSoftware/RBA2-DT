using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Enemy weakness")]
public class S_EnemyWeaknessReveal : ScriptableObject
{
    private void OnEnable()
    {
        enemyElementWeaknesses.Clear();
    }
    private void OnDisable()
    {
        enemyElementWeaknesses.Clear();
    }
    public Dictionary<o_battleCharDataN, HashSet<S_Element>> enemyElementWeaknesses = new Dictionary<o_battleCharDataN, HashSet<S_Element>>();

    public string[] getAllEnemyWeaknesses(o_battleCharDataN chara) {
        List<string> weaknesses = new List<string>();
        foreach (var weakness in enemyElementWeaknesses[chara]) {
            weaknesses.Add(weakness.name);
        }
        return weaknesses.ToArray();
    }

   public bool EnemyWeaknessExists(o_battleCharDataN enemy, S_Element element)
    {
        if (enemyElementWeaknesses != null)
            return false;
        bool enemyExists = enemyElementWeaknesses.ContainsKey(enemy);
        if (!enemyExists)
            return false;
        /*
        bool elementListExists = enemyElementWeaknesses[enemy] != null;
        if (!elementListExists)
            return false;
        */
        bool elementExists = enemyElementWeaknesses[enemy].Contains(element);
        return elementExists;
    }

    public void AddElementWeakness(o_battleCharDataN enemy, S_Element element)
    {
        if (enemyElementWeaknesses != null)
            enemyElementWeaknesses = new Dictionary<o_battleCharDataN, HashSet<S_Element>>();
        bool enemyExists = enemyElementWeaknesses.ContainsKey(enemy);
        if (!enemyExists)
            enemyElementWeaknesses.Add(enemy, new HashSet<S_Element>());
        bool elementExists = enemyElementWeaknesses[enemy].Contains(element);
        if (!elementExists) {
            enemyElementWeaknesses[enemy].Add(element);
        }
    }
}
