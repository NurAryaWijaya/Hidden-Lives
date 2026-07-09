using UnityEngine;
using Game.Flow;

public class CutsceneNode : FlowNode
{
    [SerializeField]
    private string flowId;

    private TimelineCutsceneController controller;

    public override void Enter()
    {
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