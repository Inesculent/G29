using System;
using UnityEngine;

[Serializable]
public class DialogueOption
{
    public string optionText;
    public DialogueNode nextNode;  // Reference to the next dialogue node.
}

[Serializable]
public class DialogueNode
{
    [TextArea]
    public string dialogueText;    // What is displayed for this node.
    public DialogueOption[] options; // Options that the player can choose.

    // Checks to see if it's a fail node
    public bool isFailNode;
}
