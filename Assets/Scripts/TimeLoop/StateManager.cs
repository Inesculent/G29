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

    public void RegisterDefeatedEnemy(string enemyID)
    {
        if (!defeatedEnemies.Contains(enemyID))
        {
            Debug.Log($"Registering defeated enemy: {enemyID}");
            defeatedEnemies.Add(enemyID);
        }
        else
        {
            Debug.LogWarning($"Enemy {enemyID} was already marked as defeated.");
        }
    }

    public bool IsEnemyDefeated(string enemyID)
    {
        bool result = defeatedEnemies.Contains(enemyID);
        Debug.Log($"Checking if enemy {enemyID} is defeated: {result}");
        return result;
    }

    public void ResetWorld()
    {
        Debug.Log("Resetting world state...");
    }
}
