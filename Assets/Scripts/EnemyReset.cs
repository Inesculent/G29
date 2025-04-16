using UnityEngine;

public class EnemyReset : MonoBehaviour
{
    private Health healthComponent;
    private EnemyIdentity identity;

    private void Awake()
    {
        healthComponent = GetComponent<Health>();
        identity = GetComponent<EnemyIdentity>();

        if (identity == null)
        {
            Debug.LogError($"EnemyReset on {gameObject.name} is missing EnemyIdentity!");
        }
    }

    private void OnEnable()
    {
        if (TimeLoopManager.Instance != null)
        {
            TimeLoopManager.Instance.loopResetEvent += HandleLoopReset;
        }
    }

    private void OnDisable()
    {
        if (TimeLoopManager.Instance != null)
        {
            TimeLoopManager.Instance.loopResetEvent -= HandleLoopReset;
        }
    }

    private void HandleLoopReset()
    {
        Debug.Log($"[{gameObject.name}] Reset handler called. ID: {identity?.UniqueID}");

        if (identity == null) return;

        if (StateManager.Instance.IsEnemyDefeated(identity.UniqueID))
        {
            Debug.Log($"[{gameObject.name}] staying dead.");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"[{gameObject.name}] restoring.");
            gameObject.SetActive(true);
            healthComponent.ResetHealth();
        }
    }
}
