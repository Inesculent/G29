using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    private HashSet<string> defeatedEnemies = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterDefeatedEnemy(string enemyName)
    {
        if (!defeatedEnemies.Contains(enemyName))
        {
            defeatedEnemies.Add(enemyName);
        }
    }

    public bool IsEnemyDefeated(string enemyName)
    {
        return defeatedEnemies.Contains(enemyName);
    }

    public void ResetWorld()
    {
        Debug.Log("Resetting world state...");
        // Let enemies decide what to do during reset
        // Optionally reset world objects or puzzles here
    }
}

