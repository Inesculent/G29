using UnityEngine;

public class EnemyReset : MonoBehaviour
{
    private Health healthComponent;

    private void Awake()
    {
        healthComponent = GetComponent<Health>();
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
        if (StateManager.Instance.IsEnemyDefeated(gameObject.name))
        {
            gameObject.SetActive(false); // Stay dead
        }
        else
        {
            gameObject.SetActive(true); // Ensure active
            healthComponent.ResetHealth(); // ✅ Also resets the health bar
        }
    }
}


