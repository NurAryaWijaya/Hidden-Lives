using UnityEngine;
using Game.Flow;

public class EndNode : FlowNode
{
    public override int OutputCount => 0;

    public override void Enter()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
        Complete(null);
    }
}
