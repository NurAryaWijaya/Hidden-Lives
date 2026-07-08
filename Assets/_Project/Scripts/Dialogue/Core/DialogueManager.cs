using System;
using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        private DialogueSession session;

        public event Action DialogueStarted;
        public event Action<DialogueNode> DialogueNodeChanged;
        public event Action<bool> OnDialogueModeChanged;
        public event Action DialogueEnded;

        public Transform CurrentFocusPoint { get; private set; }

        public bool IsDialogueRunning => session != null && session.IsRunning;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            session = new DialogueSession();
        }

        public void StartDialogue(DialogueData dialogue, Transform focusPoint)
        {
            GameStateManager.Instance.SetState(GameState.Dialogue);
            if (IsDialogueRunning)
            {
                Debug.LogWarning("DialogueManager: A dialogue is already running.");
                return;
            }

            CurrentFocusPoint = focusPoint;

            DialogueNode firstNode = session.StartDialogue(dialogue);

            if (firstNode == null)
            {
                return;
            }

            OnDialogueModeChanged?.Invoke(true);
            DialogueStarted?.Invoke();
            DialogueNodeChanged?.Invoke(firstNode);
        }

        // Monolog
        public void StartDialogue(DialogueData dialogueData)
        {
            StartDialogue(dialogueData, null);
        }

        public void ContinueDialogue()
        {
            if (!IsDialogueRunning)
            {
                return;
            }

            DialogueNode nextNode = session.MoveNext();

            if (nextNode == null)
            {
                EndDialogue();
                return;
            }

            DialogueNodeChanged?.Invoke(nextNode);
        }

        public void EndDialogue()
        {
            if (!IsDialogueRunning)
            {
                return;
            }

            session.EndDialogue();

            CurrentFocusPoint = null;

            OnDialogueModeChanged?.Invoke(false);
            DialogueEnded?.Invoke();

            if (GameStateManager.Instance.IsState(GameState.Dialogue))
            {
                GameStateManager.Instance.SetState(GameState.Exploration);
            }
        }
    }
}