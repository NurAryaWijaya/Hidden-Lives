using UnityEngine;
using Game.Flow;

public class CutsceneNode : FlowNode
{
    [SerializeField]
    private string flowId;

    private TimelineCutsceneController controller;

    public override void Enter()
    {
        if (WorldStateManager.Instance.IsCutsceneCompleted(flowId))
        {
            Complete(nextNode);
            return;
        }

        controller = FlowRegistry.Instance.Get<TimelineCutsceneController>(flowId);

        if (controller == null)
        {
            Debug.LogWarning($"Timeline Cutscene '{flowId}' tidak ditemukan.");
            Complete(nextNode);
            return;
        }

        controller.Finished += HandleFinished;

        controller.Play();

        GameStateManager.Instance.SetState(GameState.Cutscene);
    }

    private void HandleFinished()
    {
        controller.Finished -= HandleFinished;

        WorldStateManager.Instance.CompleteCutscene(flowId);

        GameStateManager.Instance.SetState(GameState.Exploration);

        Complete(nextNode);
    }

    public override void Exit()
    {
        if (controller == null)
            return;

        controller.Finished -= HandleFinished;
    }
}