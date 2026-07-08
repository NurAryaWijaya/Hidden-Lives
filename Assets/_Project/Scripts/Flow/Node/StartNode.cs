using Game.Flow;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : FlowNode
{
    public override void Enter()
    {
        Complete(nextNode);
    }
}
