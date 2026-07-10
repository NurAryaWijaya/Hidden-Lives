using Game.Flow;
using UnityEngine;

public class NextGraphNode : FlowNode
{
    [SerializeField]
    private FlowGraph nextGraph;

    public override int OutputCount => 0;

    public override void Enter()
    {
        if (nextGraph == null)
        {
            Debug.LogWarning("Next Graph belum diisi.");
            Complete(null);
            return;
        }

        FlowManager.Instance.StartFlow(nextGraph);
    }
}
