using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
public void RespawnAt(Vector3 respawnPosition)
{
    transform.position = respawnPosition;
    
    // Reset player's health when they respawn
    GetComponent<Health>().ResetHealth();
}

    public void Die()
    {
        if (TimeLoopManager.Instance != null) // Ensure Instance is valid
        {
            TimeLoopManager.Instance.TriggerTimeLoop(transform.position);
        }
        else
        {
            Debug.LogError("TimeLoopManager Instance is null!");
        }
    }
}
