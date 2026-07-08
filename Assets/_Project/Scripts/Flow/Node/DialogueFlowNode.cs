using Game.Dialogue;
using Game.Flow;
using UnityEngine;

public class DialogueFlowNode : FlowNode
{
    [SerializeField]
    private string flowId;

    private DialogueInteractable dialogueInteractable;

    public override void Enter()
    {
        dialogueInteractable =  FlowRegistry.Instance.Get<DialogueInteractable>(flowId);

        if (dialogueInteractable == null)
        {
            Debug.LogWarning($"DialogueInteractable dengan Id '{flowId}' tidak ditemukan.");
            Complete(nextNode);
            return;
        }

        dialogueInteractable.Activate();

        DialogueManager.Instance.DialogueEnded += HandleDialogueEnded;
    }

    private void HandleDialogueEnded()
    {
        DialogueManager.Instance.DialogueEnded -= HandleDialogueEnded;

        dialogueInteractable.Deactivate();

        Complete(nextNode);
    }
}
