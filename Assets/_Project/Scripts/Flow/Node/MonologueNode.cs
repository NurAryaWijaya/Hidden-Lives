using Game.Dialogue;
using Game.Flow;
using UnityEngine;

public class MonologueNode : FlowNode
{
    [SerializeField]
    private DialogueData dialogueData;

    public override void Enter()
    {
        if (dialogueData == null)
        {
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
}

