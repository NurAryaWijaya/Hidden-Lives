using Game.Cutscene;
using Game.Flow;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    [CreateAssetMenu(
        fileName = "Timeline Cutscene Branch Node",
        menuName = "Game/Flow/Nodes/Timeline Cutscene Branch")]
    public class TimelineCutsceneBranchNode : CutsceneNode
    {
        [Header("Branch")]

        [SerializeField]
        private FlowNode trueNode;

        [SerializeField]
        private FlowNode falseNode;

        protected override void HandleFinished()
        {
            controller.Finished -= HandleFinished;

            WorldStateManager.Instance.CompleteCutscene(flowId);

            GameStateManager.Instance.SetState(GameState.Exploration);

            TimelineBranchSignal branchSignal =
                FlowRegistry.Instance.Get<TimelineBranchSignal>(flowId);

            if (branchSignal == null)
            {
                Debug.LogWarning($"TimelineBranchSignal '{flowId}' tidak ditemukan.");

                Complete(nextNode);
                return;
            }

            Complete(branchSignal.Result ? trueNode : falseNode);
        }

        public override int OutputCount => 2;

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
}