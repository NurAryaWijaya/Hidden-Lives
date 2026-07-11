using UnityEngine;

namespace Game.Flow
{
    public class MoveNode : FlowNode
    {
        [Header("Move")]
        [SerializeField]
        private string flowId;

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 rotation;

        public override void Enter()
        {
            if (string.IsNullOrWhiteSpace(flowId))
            {
                Debug.LogWarning("MoveNode : Flow Id belum diisi.");
                Complete(nextNode);
                return;
            }

            FlowComponent component =
                FlowRegistry.Instance.Get<FlowComponent>(flowId);

            if (component == null)
            {
                Debug.LogWarning(
                    $"MoveNode : Flow Id '{flowId}' tidak ditemukan.");

                Complete(nextNode);
                return;
            }

            PlayerMovement movement =
                component.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                movement.Teleport(
                    position,
                    Quaternion.Euler(rotation));
            }
            else
            {
                component.transform.SetPositionAndRotation(
                    position,
                    Quaternion.Euler(rotation));
            }

            Complete(nextNode);
        }
    }
}