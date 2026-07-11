using UnityEngine;

namespace Game.Flow
{
    public class DestroyNode : FlowNode
    {
        [Header("Destroy")]

        [SerializeField]
        private string flowId;

        public override void Enter()
        {
            FlowComponent component = FlowRegistry.Instance.Get(flowId);

            if (component != null)
            {
                component.gameObject.SetActive(false);
            }

            WorldStateManager.Instance.SetDestroyed(flowId);

            Complete(nextNode);
        }
    }
}