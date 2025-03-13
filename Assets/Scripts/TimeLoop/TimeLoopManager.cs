using System.Collections;
using UnityEngine;

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance;

    [Tooltip("Reference object in the scene that defines the respawn position.")]
    public Transform respawnTransform; 

    public float respawnOffset = 2f;
    public float loopCooldown = 1.5f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Now call this function without parameters.
    public void TriggerTimeLoop()
    {
        if (respawnTransform == null)
        {
            Debug.LogError("Respawn Transform not assigned in the inspector.");
            return;
        }
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        UIManager.Instance?.FadeOut(); // Fade to black
        yield return new WaitForSeconds(loopCooldown);

        // Calculate a safe respawn position using the object's position.
        Vector3 respawnPosition = GetSafeRespawnPosition(respawnTransform.position);
        PlayerController.Instance.RespawnAt(respawnPosition); // Move player

        UIManager.Instance?.FadeIn(); // Fade back in
        StateManager.Instance?.ResetWorld();
    }

    private Vector3 GetSafeRespawnPosition(Vector3 originPosition)
    {
        Vector3 respawnPosition = originPosition + new Vector3(respawnOffset, 0, respawnOffset);
        if (Physics.Raycast(respawnPosition + Vector3.up * 2, Vector3.down, out RaycastHit hit, 4f))
        {
            return hit.point; // Snap to the ground level
        }
        return respawnPosition;
    }
}