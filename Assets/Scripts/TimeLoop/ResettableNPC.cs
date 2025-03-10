using UnityEngine;

public class ResettableNPC : MonoBehaviour
{
    public string npcID;

    public void ResetState()
    {
        Debug.Log($"NPC {npcID} reset to default state.");
    }
}
/*How to Attach
Select an NPC GameObject, add ResettableNPC.cs, and give it a unique NPC ID.
Select an Enemy GameObject, add RespawnableEnemy.cs, and give it a unique Enemy ID.
Select a Collectible GameObject, add CollectibleItem.cs, and give it a unique Item ID.
*/