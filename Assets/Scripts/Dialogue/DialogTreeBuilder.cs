using UnityEngine;

public class DialogueTreeBuilder : MonoBehaviour
{
    [HideInInspector]
    public DialogueNode startingNode;

    void Awake()
    {
        BuildDialogueTree();
    }

    void BuildDialogueTree()
    {
        // Create a generic fail node used for wrong choices.
        DialogueNode failNode = new DialogueNode();
        failNode.dialogueText = "You failed the dialogue challenge.";
        failNode.options = new DialogueOption[0];
        failNode.isFailNode = true;

        // --- Node 0: Initial Interaction ---
        DialogueNode node0 = new DialogueNode();
        node0.dialogueText = "Where do you hail from stranger?";
        DialogueOption option0_1 = new DialogueOption();
        option0_1.optionText = "From the town over yonder";
        DialogueOption option0_2 = new DialogueOption();
        option0_2.optionText = "Memory’s a funny thing ain’t it? I have no clue";
        node0.options = new DialogueOption[] { option0_1, option0_2 };

        // --- Node 1: Geographical Confirmation ---
        DialogueNode node1 = new DialogueNode();
        node1.dialogueText = "Milwood? That’s quite a ways from here.";
        DialogueOption option1_1 = new DialogueOption();
        option1_1.optionText = "Yes";  // (You might set a flag here externally)
        DialogueOption option1_2 = new DialogueOption();
        option1_2.optionText = "No, I’m from Starne";  // (Set a flag externally)
        node1.options = new DialogueOption[] { option1_1, option1_2 };

        // --- Node 2: Memory Failure (Failure Node) ---
        DialogueNode node2 = new DialogueNode();
        node2.dialogueText = "No clue you say? If memory fails you, then civilities are impossible. Perhaps a battle might jog your memory.";
        node2.options = new DialogueOption[0];
        node2.isFailNode = true;  // Mark this node as a failure

        // --- Node 3: Reason for Visiting ---
        DialogueNode node3 = new DialogueNode();
        node3.dialogueText = "We don’t get your folk around here often. What brings you here?";
        DialogueOption option3_1 = new DialogueOption();
        option3_1.optionText = "I am a traveling salesman, looking to pawn my exotic wares.";
        DialogueOption option3_2 = new DialogueOption();
        option3_2.optionText = "I am a humble forager, looking to cure my wife’s illness";
        node3.options = new DialogueOption[] { option3_1, option3_2 };

        // --- Node 4: Salesman Failure (Failure Node) ---
        DialogueNode node4 = new DialogueNode();
        node4.dialogueText = "Where are these wares you speak of?";
        node4.options = new DialogueOption[0];
        node4.isFailNode = true;  // Salesman path fails because you have nothing

        // --- Node 5: Foraging Options ---
        DialogueNode node5 = new DialogueNode();
        node5.dialogueText = "Foraging eh? Which one of these might cure a cold?";
        DialogueOption option5_1 = new DialogueOption();
        option5_1.optionText = "Elderberry"; // Correct option
        DialogueOption option5_2 = new DialogueOption();
        option5_2.optionText = "Hemlock";     // Fail option
        DialogueOption option5_3 = new DialogueOption();
        option5_3.optionText = "Oleander";     // Fail option
        node5.options = new DialogueOption[] { option5_1, option5_2, option5_3 };
        // Set failure outcomes:
        option5_2.nextNode = failNode;
        option5_3.nextNode = failNode;

        // --- Node 6: Burn Treatment Options ---
        DialogueNode node6 = new DialogueNode();
        node6.dialogueText = "Ah, the time-honored remedy indeed! Its flavor is certainly complex. If you are who you claim to be, how might you dress a burn?";
        DialogueOption option6_1 = new DialogueOption();
        option6_1.optionText = "I’d dress it with aloe";   // Correct option
        DialogueOption option6_2 = new DialogueOption();
        option6_2.optionText = "I’d dress it with nettles"; // Fail option
        DialogueOption option6_3 = new DialogueOption();
        option6_3.optionText = "I’d dress it with hogweed";  // Fail option
        node6.options = new DialogueOption[] { option6_1, option6_2, option6_3 };
        option6_2.nextNode = failNode;
        option6_3.nextNode = failNode;

        // --- Node 7: Final Geographical Confirmation ---
        DialogueNode node7 = new DialogueNode();
        node7.dialogueText = "Very well, it seems you are truly as you say. What town were you from again?";
        DialogueOption option7_1 = new DialogueOption();
        option7_1.optionText = "Starne";   // Correct option depending on earlier flag
        DialogueOption option7_2 = new DialogueOption();
        option7_2.optionText = "Milwood";  // Correct option depending on flag
        DialogueOption option7_3 = new DialogueOption();
        option7_3.optionText = "Bogdon";   // Fail option
        DialogueOption option7_4 = new DialogueOption();
        option7_4.optionText = "Trier";     // Fail option
        node7.options = new DialogueOption[] { option7_1, option7_2, option7_3, option7_4 };
        option7_3.nextNode = failNode;
        option7_4.nextNode = failNode;

        // --- Node 8: Terminal Success Node ---
        DialogueNode node8 = new DialogueNode();
        node8.dialogueText = "Ah yes, I’ve been looking to visit one day. I bid you well forager.";
        node8.options = new DialogueOption[0];
        node8.isFailNode = false;  // This is a success outcome

        // --- Wiring Up the Tree ---
        option0_1.nextNode = node1;
        option0_2.nextNode = node2;  // Memory failure

        option1_1.nextNode = node3;
        option1_2.nextNode = node3;

        option3_1.nextNode = node4;  // Salesman fails
        option3_2.nextNode = node5;  // Forager proceeds

        option5_1.nextNode = node6;
        // Options 5_2 and 5_3 already lead to failNode.

        option6_1.nextNode = node7;
        // Options 6_2 and 6_3 already lead to failNode.

        option7_1.nextNode = node8;
        option7_2.nextNode = node8;
        // Options 7_3 and 7_4 already lead to failNode.

        // Set the starting node.
        startingNode = node0;
    }
}
