using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    [CreateAssetMenu(
        fileName = "Test Flow Node",
        menuName = "Hidden Live/Flow/Test Node")]
    public class TestFlowNode : FlowNode, IFlowOutput
    {
        [SerializeField]
        private string message;

        [SerializeField]
        private float duration = 1f;

        [SerializeField]
        private FlowNode nextNode;

        public override void Enter()
        {
            Debug.Log(message);

            FlowManager.Instance.StartCoroutine(WaitRoutine());
        }

        private IEnumerator WaitRoutine()
        {
            yield return new WaitForSeconds(duration);

            Complete(nextNode);
        }

        public IEnumerable<FlowNode> GetOutputs()
        {
            if (nextNode != null)
                yield return nextNode;
        }

        public void SetNextNode(FlowNode node)
        {
            nextNode = node;
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
}