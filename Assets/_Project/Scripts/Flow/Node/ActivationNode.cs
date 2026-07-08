using System.Collections.Generic;
using Game.Flow;
using UnityEngine;

public class ActivationNode : FlowNode
{
    [Header("Target")]
    [SerializeField]
    private string flowId;

    [Header("Activation")]
    [SerializeField]
    private bool activate = true;

    public override void Enter()
    {
        IFlowActivatable target =
            FlowRegistry.Instance.Get<IFlowActivatable>(flowId);

        if (target == null)
        {
            Debug.LogWarning($"Flow target '{flowId}' tidak ditemukan.");
            Complete(nextNode);
            return;
        }

        if (activate)
            target.Activate();
        else
            target.Deactivate();

        Complete(nextNode);
    }
}