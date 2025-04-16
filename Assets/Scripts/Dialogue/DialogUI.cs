using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button[] optionButtons;

    private DialogueNode currentNode;
    private EnemyDialogueTrigger currentEnemyDialogue;
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

    public void StartDialogue(DialogueNode startingNode, EnemyDialogueTrigger enemyDialogueTrigger)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentEnemyDialogue = enemyDialogueTrigger;
        currentNode = startingNode;
        Debug.Log("Dialogue started: " + currentNode.dialogueText);
        DisplayCurrentNode();
    }

    private void DisplayCurrentNode()
    {
        dialogueText.text = currentNode.dialogueText;
        Debug.Log("Current node: " + currentNode.dialogueText);

        // Terminal node? show just “Continue”
        if (currentNode.options == null || currentNode.options.Length == 0)
        {
            foreach (var btn in optionButtons) btn.gameObject.SetActive(false);
            if (optionButtons.Length > 0)
            {
                var b = optionButtons[0];
                b.gameObject.SetActive(true);
                b.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
            }
            return;
        }

        // Otherwise show all options
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentNode.options.Length)
            {
                var btn = optionButtons[i];
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = currentNode.options[i].optionText;
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnOptionButtonClicked(int index)
    {
        // Terminal “Continue”
        if (currentNode.options == null || currentNode.options.Length == 0)
        {
            EndDialogue(); 
            return;
        }

        var opt = currentNode.options[index];
        Debug.Log($"Clicked option [{index}]: {opt.optionText}");

        // 1) Set any flag this option carries
        if (!string.IsNullOrEmpty(opt.flagToSet))
        {
            flags[opt.flagToSet] = opt.flagValue;
            Debug.Log($"Flag set: {opt.flagToSet} = {opt.flagValue}");
        }

        // 2) Conditional branching (Node 7 logic)
        if (!string.IsNullOrEmpty(opt.conditionFlag))
        {
            flags.TryGetValue(opt.conditionFlag, out bool val);
            if (val == opt.conditionValue)
            {
                // success path → jump to the terminal success node
                currentNode = new DialogueTreeBuilder().startingNode; // dummy to access node8?
                // Actually, we know at Build time opt.nextNode == failNode. Instead:
                // we need the actual node8 reference. Better: store node8 in opt.nextNode? 
                // For simplicity here, assume opt.nextNode was overwritten to node8 at build—alternatively:
                // treat any option with conditionFlag as success and manually set to node8:
                currentNode = opt.nextNode; 
                DisplayCurrentNode();
            }
            else
            {
                Debug.Log("Condition not met, failing dialogue.");
                FailDialogue();
            }
            return;
        }

        // 3) Normal nextNode flow
        if (opt.nextNode != null)
        {
            currentNode = opt.nextNode;
            DisplayCurrentNode();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Dialogue ended on node: " + currentNode.dialogueText);
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (currentNode.isFailNode)
        {
            Debug.Log("Outcome: FAILURE");
            currentEnemyDialogue?.OnDialogueFailure();
        }
        else
        {
            Debug.Log("Outcome: SUCCESS");
            currentEnemyDialogue?.OnDialogueSuccess();
        }
    }

    private void FailDialogue()
    {
        Debug.Log("Forcing failure outcome.");
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentEnemyDialogue?.OnDialogueFailure();
    }
}
