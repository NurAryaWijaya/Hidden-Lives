using Game.Flow;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : FlowNode, IFlowOutput
{
    [SerializeField]
    private FlowNode nextNode;

    public override void Enter()
    {
        Complete(nextNode);
    }

    public IEnumerable<FlowNode> GetOutputs()
    {
        if (nextNode != null)
            yield return nextNode;
    }

    public void SetOutput(int index, FlowNode node)
    {
        if (index == 0)
        {
            nextNode = node;
        }
    }

    public void RemoveOutput(FlowNode node)
    {
        if (nextNode == node)
        {
            nextNode = null;
        }
    }
}
