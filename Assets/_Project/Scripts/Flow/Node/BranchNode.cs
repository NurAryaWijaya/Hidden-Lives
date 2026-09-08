using System.Collections.Generic;
using Game.Flow;
using Game.Localization;
using UnityEngine;

public class BranchNode : FlowNode
{
    [Header("Branch")]
    [SerializeField]
    private FlowNode trueNode;

    [SerializeField]
    private FlowNode falseNode;

    [Header("Localization")]
    [SerializeField]
    private LocalizedBranchData branchData;

    public override int OutputCount => 2;

    public override void Enter()
    {
        if (branchData == null)
        {
            Debug.LogWarning($"BranchNode '{name}' belum memiliki LocalizedBranchData.");
            return;
        }

        GameStateManager.Instance.SetState(GameState.Branch);

        BranchManager.Instance.BranchSelected += OnSelected;

        BranchManager.Instance.Show(
            branchData.TrueText,
            branchData.FalseText);
    }

    private void OnSelected(bool result)
    {
        BranchManager.Instance.BranchSelected -= OnSelected;

        GameStateManager.Instance.SetState(GameState.Exploration);

        Complete(result ? trueNode : falseNode);
    }

    public override IEnumerable<FlowNode> GetOutputs()
    {
        if (trueNode != null)
            yield return trueNode;

        if (falseNode != null)
            yield return falseNode;
    }

    public override void SetOutput(int index, FlowNode node)
    {
        switch (index)
        {
            case 0:
                trueNode = node;
                break;

            case 1:
                falseNode = node;
                break;
        }
    }

    public override void RemoveOutput(FlowNode node)
    {
        if (trueNode == node)
            trueNode = null;

        if (falseNode == node)
            falseNode = null;
    }

    public override string GetOutputName(int index)
    {
        return index switch
        {
            0 => "True",
            1 => "False",
            _ => string.Empty
        };
    }
}