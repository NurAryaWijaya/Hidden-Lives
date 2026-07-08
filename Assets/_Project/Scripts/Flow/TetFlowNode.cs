using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    [CreateAssetMenu(
        fileName = "Test Flow Node",
        menuName = "Hidden Live/Flow/Test Node")]
    public class TestFlowNode : FlowNode
    {
        [SerializeField]
        private string message;

        [SerializeField]
        private float duration = 1f;

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

        public void SetNextNode(FlowNode node)
        {
            nextNode = node;
        }
    }
}