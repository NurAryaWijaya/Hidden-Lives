using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueSession
    {
        private DialogueData currentDialogue;
        private int currentNodeIndex = -1;

        public DialogueNode CurrentNode
        {
            get
            {
                if (currentDialogue == null)
                {
                    return null;
                }

                return currentDialogue.GetNode(currentNodeIndex);
            }
        }

        public bool IsRunning => currentDialogue != null;

        public DialogueNode StartDialogue(DialogueData dialogue)
        {
            if (dialogue == null)
            {
                Debug.LogWarning("DialogueSession: DialogueData is null.");
                return null;
            }

            currentDialogue = dialogue;
            currentNodeIndex = dialogue.StartingNodeIndex;

            return CurrentNode;
        }

        public DialogueNode MoveNext()
        {
            if (currentDialogue == null)
            {
                return null;
            }

            DialogueNode currentNode = CurrentNode;

            if (currentNode == null)
            {
                EndDialogue();
                return null;
            }

            if (currentNode.NextNodeIndex < 0)
            {
                return null;
            }

            currentNodeIndex = currentNode.NextNodeIndex;

            return CurrentNode;
        }

        public void EndDialogue()
        {
            currentDialogue = null;
            currentNodeIndex = -1;
        }
    }
}