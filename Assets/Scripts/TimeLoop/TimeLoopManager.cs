using System.Collections;
using UnityEngine;

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance { get; private set; }

    public float respawnOffset = 2f;
    public float loopCooldown = 1.5f;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerTimeLoop(Vector3 deathPosition)
    {
        Vector3 respawnPosition = deathPosition + new Vector3(respawnOffset, 0, respawnOffset);
        StartCoroutine(RespawnPlayer(respawnPosition));
    }

    private IEnumerator RespawnPlayer(Vector3 respawnPosition)
    {
        yield return new WaitForSeconds(loopCooldown);
        PlayerController.Instance.RespawnAt(respawnPosition);
        StateManager.Instance.ResetWorld();
    }
}
