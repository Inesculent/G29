using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Health healthComponent;
    private Rigidbody rb;
    private PlayerMovements playerMovement;
    private CharacterController characterController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        healthComponent = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovements>();
        characterController = GetComponent<CharacterController>();
    }

    public void RespawnAt(Vector3 position)
    {
        Debug.Log("Respawning player...");

        // Disable movement and character controller
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (characterController != null)
            characterController.enabled = false;

        // Move the player to respawn location
        transform.position = position;

        // Reset health
        healthComponent.ResetHealth();

        // Re-enable movement and physics after a short delay
        Invoke(nameof(ReenableMovement), 0.1f);
    }

    private void ReenableMovement()
    {
        if (characterController != null)
            characterController.enabled = true;

        if (rb != null)
            rb.isKinematic = false;

        if (playerMovement != null)
            playerMovement.enabled = true;

        Debug.Log("Movement re-enabled after respawn.");

        // Optional: re-sync camera orientation
        PlayerCam cam = GetComponentInChildren<PlayerCam>();
        if (cam != null)
        {
            cam.SyncCameraRotation();
        }
    }
}

