using System.Collections;
using UnityEngine;

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance;
    
    public float respawnOffset = 2f;
    public float loopCooldown = 1.5f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TriggerTimeLoop(Vector3 deathPosition)
    {
        Vector3 respawnPosition = deathPosition + new Vector3(respawnOffset, 0, respawnOffset);
        StartCoroutine(RespawnPlayer(respawnPosition));
    }

    private IEnumerator RespawnPlayer(Vector3 respawnPosition)
    {
        UIManager.Instance.FadeOut(); // Fade to black
        yield return new WaitForSeconds(loopCooldown);

        PlayerController.Instance.RespawnAt(respawnPosition);

        UIManager.Instance.FadeIn(); // Fade back in
        StateManager.Instance.ResetWorld();
    }
}
