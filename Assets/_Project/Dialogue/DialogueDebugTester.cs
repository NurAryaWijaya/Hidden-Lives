using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Dialogue
{
    public class DialogueDebugTester : MonoBehaviour
    {
        [SerializeField]
        private DialogueData dialogueData;

        private void Start()
        {
            DialogueManager.Instance.DialogueStarted += OnDialogueStarted;
            DialogueManager.Instance.DialogueNodeChanged += OnDialogueNodeChanged;
            DialogueManager.Instance.DialogueEnded += OnDialogueEnded;
        }

        private void OnDisable()
        {
            if (DialogueManager.Instance == null)
            {
                return;
            }

            DialogueManager.Instance.DialogueStarted -= OnDialogueStarted;
            DialogueManager.Instance.DialogueNodeChanged -= OnDialogueNodeChanged;
            DialogueManager.Instance.DialogueEnded -= OnDialogueEnded;
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
            {
                DialogueManager.Instance.StartDialogue(dialogueData, null);
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                DialogueManager.Instance.ContinueDialogue();
            }
        }

        private void OnDialogueStarted()
        {
            Debug.Log("Dialogue Started");
        }

        private void OnDialogueNodeChanged(DialogueNode node)
        {
            Debug.Log($"{node.SpeakerName} : {node.DialogueText}");
        }

        private void OnDialogueEnded()
        {
            Debug.Log("Dialogue Ended");
        }
    }
}