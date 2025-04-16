using UnityEngine;

public class EnemyDialogueTrigger : MonoBehaviour
{
    public EnemyCombatController combatController; // Optional.
    public EnemyAI enemyAI;
    public DialogueTreeBuilder dialogueTree;  // Assign this in the Inspector.

    private bool dialogueStarted = false;

    void Start()
    {
        if (enemyAI != null)
            enemyAI.enabled = false;

        if (DialogueManager.Instance == null)
            Debug.LogError("DialogueManager instance not found in the scene.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (dialogueStarted || !other.CompareTag("Player"))
            return;

        if (dialogueTree == null)
        {
            Debug.LogError("Dialogue tree reference missing on EnemyDialogueTrigger.");
            return;
        }

        dialogueStarted = true;
        Debug.Log("Enemy dialogue triggered.");
        DialogueManager.Instance.StartDialogue(dialogueTree, this);
    }

    // Called when dialogue is successful: enemy disappears.
    public void OnDialogueSuccess()
    {
        Debug.Log("OnDialogueSuccess called: Disabling enemy.");
        Transform enemyRoot = transform.parent != null ? transform.parent : transform;
        enemyRoot.gameObject.SetActive(false);
    }

    // Called when dialogue fails: enemy AI resumes, and enemy becomes aggressive.
    public void OnDialogueFailure()
    {
        Debug.Log("OnDialogueFailure called: Enemy AI resumes attack.");
        if (enemyAI != null)
        {
            enemyAI.enabled = true;
            Debug.Log("Enemy AI has been enabled.");
        }

        // Force aggro by accessing the Aggro component.
        Aggro aggro = GetComponent<Aggro>();
        if (aggro != null)
        {
            aggro.ForceAggro();
            Debug.Log("Enemy aggro has been forced.");
        }
        else
        {
            Debug.LogWarning("Aggro component not found on enemy.");
        }

        // Optionally re-enable enemy visuals.
        Transform model = transform.parent != null
            ? transform.parent.Find("Enemy Model")
            : transform.Find("Enemy Model");

        if (model != null)
        {
            model.gameObject.SetActive(true);
            Debug.Log("Enemy model re-enabled.");
        }
    }
}
