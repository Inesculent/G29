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
        // Generic fail node
        DialogueNode failNode = new DialogueNode
        {
            dialogueText = "You failed the dialogue challenge.",
            options = new DialogueOption[0],
            isFailNode = true
        };

        // Node 0
        DialogueNode node0 = new DialogueNode
        {
            dialogueText = "Where do you hail from stranger?"
        };
        var option0_1 = new DialogueOption { optionText = "From the town over yonder" };
        var option0_2 = new DialogueOption { optionText = "Memory’s a funny thing ain’t it? I have no clue" };
        node0.options = new[] { option0_1, option0_2 };

        // Node 1: set flags when chosen
        DialogueNode node1 = new DialogueNode
        {
            dialogueText = "Milwood? That’s quite a ways from here."
        };
        var option1_1 = new DialogueOption
        {
            optionText = "Yes",
            flagToSet = "isMilwood",
            flagValue = true
        };
        var option1_2 = new DialogueOption
        {
            optionText = "No, I’m from Starne",
            flagToSet = "isStarne",
            flagValue = true
        };
        node1.options = new[] { option1_1, option1_2 };

        // Node 2: memory failure
        DialogueNode node2 = new DialogueNode
        {
            dialogueText = "No clue you say? If memory fails you, then civilities are impossible. Perhaps a battle might jog your memory.",
            options = new DialogueOption[0],
            isFailNode = true
        };

        // Node 3
        DialogueNode node3 = new DialogueNode
        {
            dialogueText = "We don’t get your folk around here often. What brings you here?"
        };
        var option3_1 = new DialogueOption { optionText = "I am a traveling salesman, looking to pawn my exotic wares." };
        var option3_2 = new DialogueOption { optionText = "I am a humble forager, looking to cure my wife’s illness" };
        node3.options = new[] { option3_1, option3_2 };

        // Node 4: salesman failure
        DialogueNode node4 = new DialogueNode
        {
            dialogueText = "Where are these wares you speak of?",
            options = new DialogueOption[0],
            isFailNode = true
        };

        // Node 5: foraging
        DialogueNode node5 = new DialogueNode
        {
            dialogueText = "Foraging eh? Which one of these might cure a cold?"
        };
        var option5_1 = new DialogueOption { optionText = "Elderberry" };
        var option5_2 = new DialogueOption { optionText = "Hemlock", nextNode = failNode };
        var option5_3 = new DialogueOption { optionText = "Oleander", nextNode = failNode };
        node5.options = new[] { option5_1, option5_2, option5_3 };

        // Node 6: burn treatment
        DialogueNode node6 = new DialogueNode
        {
            dialogueText = "… how might you dress a burn?"
        };
        var option6_1 = new DialogueOption { optionText = "I’d dress it with aloe" };
        var option6_2 = new DialogueOption { optionText = "I’d dress it with nettles", nextNode = failNode };
        var option6_3 = new DialogueOption { optionText = "I’d dress it with hogweed", nextNode = failNode };
        node6.options = new[] { option6_1, option6_2, option6_3 };

        // Node 7: final geography check — use conditionFlag to decide success
        DialogueNode node7 = new DialogueNode
        {
            dialogueText = "What town were you from again?"
        };
        var option7_1 = new DialogueOption
        {
            optionText = "Starne",
            conditionFlag = "isStarne",
            conditionValue = true,
            nextNode = failNode  // placeholder; will be overridden on success by UI logic
        };
        var option7_2 = new DialogueOption
        {
            optionText = "Milwood",
            conditionFlag = "isMilwood",
            conditionValue = true,
            nextNode = failNode
        };
        var option7_3 = new DialogueOption { optionText = "Bogdon", nextNode = failNode };
        var option7_4 = new DialogueOption { optionText = "Trier",  nextNode = failNode };
        node7.options = new[] { option7_1, option7_2, option7_3, option7_4 };

        // Node 8: success
        DialogueNode node8 = new DialogueNode
        {
            dialogueText = "Ah yes, I’ve been looking to visit one day. I bid you well forager.",
            options = new DialogueOption[0],
            isFailNode = false
        };

        // Wire up static nextNode links
        option0_1.nextNode = node1;
        option0_2.nextNode = node2;

        option1_1.nextNode = node3;
        option1_2.nextNode = node3;

        option3_1.nextNode = node4;
        option3_2.nextNode = node5;

        option5_1.nextNode = node6;

        option6_1.nextNode = node7;

        // For Node 7 we leave nextNode=failNode; success is handled by condition check in UI,
        // so we override nextNode at runtime if condition passes.

        // Finally set the tree root
        startingNode = node0;
    }
}
