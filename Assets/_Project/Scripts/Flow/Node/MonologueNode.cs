using Game.Dialogue;
using Game.Flow;
using Game.Localization;
using UnityEngine;

public class MonologueNode : FlowNode
{
    [SerializeField]
    private LocalizedDialogueData dialogue;

    public override void Enter()
    {
        if (dialogue == null)
        {
            Complete(nextNode);
            return;
        }

        DialogueData dialogueData = dialogue.GetDialogue();

        if (dialogueData == null)
        {
            Debug.LogWarning("MonologueNode : DialogueData tidak ditemukan.");
            Complete(nextNode);
            return;
        }

        DialogueManager.Instance.DialogueEnded += HandleMonologueEnded;

        DialogueManager.Instance.StartDialogue(dialogueData);

        GameStateManager.Instance.SetState(GameState.Dialogue);
    }

    private void HandleMonologueEnded()
    {
        DialogueManager.Instance.DialogueEnded -= HandleMonologueEnded;

        GameStateManager.Instance.SetState(GameState.Exploration);

        Complete(nextNode);
    }

    public override void Exit()
    {
        DialogueManager.Instance.DialogueEnded -= HandleMonologueEnded;
    }
}