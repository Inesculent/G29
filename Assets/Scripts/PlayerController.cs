using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Health healthComponent;
    private PlayerMovements playerMovements;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        healthComponent = GetComponent<Health>();
        playerMovements = GetComponent<PlayerMovements>(); // Reference to movement script
    }

    public void RespawnAt(Vector3 position)
    {
        Debug.Log("Respawning player...");

        // Disable movement temporarily
        if (playerMovements != null)
        {
            playerMovements.SetCanMove(false);
        }

        // Move the player to respawn location
        transform.position = position;

        // Restore health
        healthComponent.ResetHealth();

        // Re-enable movement after short delay
        Invoke(nameof(ReenableMovement), 0.1f);
    }

    private void ReenableMovement()
    {
        if (playerMovements != null)
        {
            playerMovements.SetCanMove(true);
        }

        // Sync camera orientation post-respawn
        FindObjectOfType<PlayerCam>()?.SyncCameraRotation();

        Debug.Log("Movement re-enabled after respawn.");
    }
}
