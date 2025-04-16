using System;
using UnityEngine;

[Serializable]
public class DialogueOption
{
    public string optionText;
    public DialogueNode nextNode;

    // New: when this option is chosen, set flags[flagToSet] = flagValue
    public string flagToSet;
    public bool flagValue;

    // New: to use on a terminal branching node (Node 7)
    //   if flags[conditionFlag] == conditionValue → follow nextNode
    //   else → immediate failure
    public string conditionFlag;
    public bool conditionValue;
}

[Serializable]
public class DialogueNode
{
    [TextArea]
    public string dialogueText;
    public DialogueOption[] options;
    public bool isFailNode;
}
