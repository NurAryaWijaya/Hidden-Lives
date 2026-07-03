using TMPro;
using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueCanvas : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField]
        private GameObject dialoguePanel;

        [Header("UI")]
        [SerializeField]
        private TMP_Text speakerNameText;

        [SerializeField]
        private TMP_Text dialogueText;

        public void Show()
        {
            dialoguePanel.SetActive(true);
        }

        public void Hide()
        {
            dialoguePanel.SetActive(false);
        }

        public void SetSpeaker(string speakerName)
        {
            speakerNameText.text = speakerName;
        }

        public void SetDialogue(string text)
        {
            dialogueText.text = text;
        }
    }
}