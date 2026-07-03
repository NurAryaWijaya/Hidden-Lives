using System;
using UnityEngine;

namespace Game.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        [SerializeField]
        private string nodeID;

        [SerializeField]
        private string speakerName;

        [TextArea(3, 6)]
        [SerializeField]
        private string dialogueText;

        [SerializeField]
        private int nextNodeIndex = -1;

        public string NodeID => nodeID;
        public string SpeakerName => speakerName;
        public string DialogueText => dialogueText;
        public int NextNodeIndex => nextNodeIndex;
    }
}