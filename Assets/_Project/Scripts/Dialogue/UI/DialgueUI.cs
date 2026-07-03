using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField]
        private DialogueCanvas dialogueCanvas;

        private void Awake()
        {
            dialogueCanvas.Hide();
        }

        private void Start()
        {
            DialogueManager.Instance.DialogueStarted += OnDialogueStarted;
            DialogueManager.Instance.DialogueNodeChanged += OnDialogueNodeChanged;
            DialogueManager.Instance.DialogueEnded += OnDialogueEnded;
        }

        private void OnDestroy()
        {
            if (DialogueManager.Instance == null)
            {
                return;
            }

            DialogueManager.Instance.DialogueStarted -= OnDialogueStarted;
            DialogueManager.Instance.DialogueNodeChanged -= OnDialogueNodeChanged;
            DialogueManager.Instance.DialogueEnded -= OnDialogueEnded;
        }

        private void OnDialogueStarted()
        {
            dialogueCanvas.Show();
        }

        private void OnDialogueNodeChanged(DialogueNode node)
        {
            dialogueCanvas.SetSpeaker(node.SpeakerName);
            dialogueCanvas.SetDialogue(node.DialogueText);
        }

        private void OnDialogueEnded()
        {
            dialogueCanvas.Hide();
        }
    }
}