using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialogue
{
    [CreateAssetMenu(
        fileName = "Dialogue",
        menuName = "Game/Dialogue/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [Header("Dialogue")]

        [SerializeField]
        private string dialogueID;

        [SerializeField]
        private int startingNodeIndex = 0;

        [SerializeField]
        private List<DialogueNode> nodes = new();

        public string DialogueID => dialogueID;

        public int StartingNodeIndex => startingNodeIndex;

        public IReadOnlyList<DialogueNode> Nodes => nodes;

        public DialogueNode GetNode(int index)
        {
            if (index < 0 || index >= nodes.Count)
            {
                return null;
            }

            return nodes[index];
        }
    }
}