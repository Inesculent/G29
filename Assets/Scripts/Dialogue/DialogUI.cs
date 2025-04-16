using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;  // Reference to the dialogue text component.
    [SerializeField] private Button[] optionButtons;         // Array of option buttons.

    private DialogueNode currentNode;
    private EnemyDialogueTrigger currentEnemyDialogue;  // Reference to the enemy that initiated the dialogue.

    // Starts the dialogue and stores a reference to the enemy for callbacks.
    public void StartDialogue(DialogueNode startingNode, EnemyDialogueTrigger enemyDialogueTrigger)
    {
        // Unlock and show the cursor for UI interaction.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentEnemyDialogue = enemyDialogueTrigger;
        currentNode = startingNode;
        Debug.Log("Dialogue started: " + currentNode.dialogueText);
        DisplayCurrentNode();
    }

    // Displays the current node's dialogue text and sets up the option buttons.
    private void DisplayCurrentNode()
    {
        dialogueText.text = currentNode.dialogueText;
        Debug.Log("Current node text: " + currentNode.dialogueText);

        // If no options exist (i.e. terminal node), show a default "Continue" button.
        if (currentNode.options == null || currentNode.options.Length == 0)
        {
            // Disable all buttons first.
            foreach (Button btn in optionButtons)
                btn.gameObject.SetActive(false);
            
            if (optionButtons.Length > 0)
            {
                optionButtons[0].gameObject.SetActive(true);
                TextMeshProUGUI btnText = optionButtons[0].GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = "Continue";
                }
            }
        }
        else
        {
            // If options exist, set up each button accordingly.
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < currentNode.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    TextMeshProUGUI btnText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (btnText != null)
                    {
                        btnText.text = currentNode.options[i].optionText;
                    }
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    // Called when an option or the continue button is clicked.
    public void OnOptionButtonClicked(int index)
    {
        // If there are no options (terminal node), treat the click as a "continue" command.
        if (currentNode.options == null || currentNode.options.Length == 0)
        {
            Debug.Log("Continue button clicked on terminal node, ending dialogue.");
            EndDialogue();
            return;
        }

        Debug.Log($"Button {index} clicked!");
        if (index < currentNode.options.Length)
        {
            DialogueOption selectedOption = currentNode.options[index];
            Debug.Log($"Option selected: \"{selectedOption.optionText}\"");

            // If a next node is assigned, continue the dialogue.
            if (selectedOption.nextNode != null)
            {
                currentNode = selectedOption.nextNode;
                DisplayCurrentNode();
            }
            else
            {
                Debug.Log("No next node assigned to this option, ending dialogue.");
                EndDialogue();
            }
        }
        else
        {
            Debug.LogWarning("Button index exceeds available dialogue options.");
        }
    }

    // Ends the dialogue, resets the cursor, and calls the corresponding enemy callback based on outcome.
    private void EndDialogue()
    {
    Debug.Log("Ending dialogue. Final node text: " + currentNode.dialogueText);
    gameObject.SetActive(false);

    // Re-lock and hide the cursor.
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    // Outcome is decided by the isFailNode flag
    if (currentNode.isFailNode)
    {
        Debug.Log("Determined outcome: FAILURE.");
        currentEnemyDialogue?.OnDialogueFailure();
    }
    else
    {
        Debug.Log("Determined outcome: SUCCESS.");
        currentEnemyDialogue?.OnDialogueSuccess();
    }
}

}
