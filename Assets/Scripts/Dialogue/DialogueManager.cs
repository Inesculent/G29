using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public DialogueUI dialogueUI; // Assigned in the Inspector.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional.
        }
        else
        {
            Destroy(gameObject);
        }
        if (dialogueUI == null)
        {
            dialogueUI = FindObjectOfType<DialogueUI>();
        }
    }

    // Now also accepts an EnemyDialogueTrigger parameter.
    public void StartDialogue(DialogueTreeBuilder treeBuilder, EnemyDialogueTrigger enemyDialogueTrigger)
    {
        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI not assigned in DialogueManager.");
            return;
        }
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.StartDialogue(treeBuilder.startingNode, enemyDialogueTrigger);
    }
}
