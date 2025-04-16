using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemID;

    public void Respawn()
    {
        gameObject.SetActive(true);
    }
}
/*How to Attach
Select an NPC GameObject, add ResettableNPC.cs, and give it a unique NPC ID.
Select an Enemy GameObject, add RespawnableEnemy.cs, and give it a unique Enemy ID.
Select a Collectible GameObject, add CollectibleItem.cs, and give it a unique Item ID.
*/