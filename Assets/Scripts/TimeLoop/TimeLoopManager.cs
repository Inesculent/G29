using System.Collections;
using UnityEngine;

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance;

    [Tooltip("Reference object in the scene that defines the respawn position.")]
    public Transform respawnTransform; 

    public float respawnOffset = 2f;
    public float loopCooldown = 1.5f;

    public int loopCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TriggerTimeLoop()
    {
        if (respawnTransform == null)
        {
            Debug.LogError("Respawn Transform not assigned in the inspector.");
            return;
        }

        loopCount++;
        Debug.Log($"Time Loop Triggered. Current Loop Count: {loopCount}");
        UIManager.Instance?.UpdateLoopCounter(loopCount);
        StartCoroutine(RespawnPlayer());
        
    }

    private IEnumerator RespawnPlayer()
    {
        UIManager.Instance?.FadeOut(); // Fade to black
        yield return new WaitForSeconds(loopCooldown);

        Vector3 respawnPosition = GetSafeRespawnPosition(respawnTransform.position);
        PlayerController.Instance.RespawnAt(respawnPosition);

        UIManager.Instance?.FadeIn(); // Fade back in
        StateManager.Instance?.ResetWorld();
    }

    private Vector3 GetSafeRespawnPosition(Vector3 originPosition)
    {
        Vector3 respawnPosition = originPosition + new Vector3(respawnOffset, 0, respawnOffset);
        if (Physics.Raycast(respawnPosition + Vector3.up * 2, Vector3.down, out RaycastHit hit, 4f))
        {
            return hit.point;
        }
        return respawnPosition;
    }
}

