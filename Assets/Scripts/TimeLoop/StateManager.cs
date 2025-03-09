using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    private HashSet<string> collectedItems = new HashSet<string>();
    private HashSet<string> defeatedEnemies = new HashSet<string>();
    private HashSet<string> interactedNPCs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterItem(string itemID) => collectedItems.Add(itemID);
    public bool HasItem(string itemID) => collectedItems.Contains(itemID);

    public void RegisterDefeatedEnemy(string enemyID) => defeatedEnemies.Add(enemyID);
    public bool IsEnemyDefeated(string enemyID) => defeatedEnemies.Contains(enemyID);

    public void RegisterNPCInteraction(string npcID) => interactedNPCs.Add(npcID);
    public bool HasInteractedWithNPC(string npcID) => interactedNPCs.Contains(npcID);

    public void ResetWorld()
    {
        foreach (ResettableNPC npc in FindObjectsOfType<ResettableNPC>())
        {
            if (!interactedNPCs.Contains(npc.npcID))
                npc.ResetState();
        }

        foreach (RespawnableEnemy enemy in FindObjectsOfType<RespawnableEnemy>())
        {
            if (!defeatedEnemies.Contains(enemy.enemyID))
                enemy.Respawn();
        }

        foreach (CollectibleItem item in FindObjectsOfType<CollectibleItem>())
        {
            if (!collectedItems.Contains(item.itemID))
                item.Respawn();
        }
    }
}
