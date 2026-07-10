using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Flow
{
    public class CheckpointNode : FlowNode
    {
        [SerializeField]
        private string checkpointId;

        public override void Enter()
        {
            CheckpointManager.Instance.Unlock(
                checkpointId,
                SceneManager.GetActiveScene().name,
                FlowManager.Instance.CurrentGraph,
                this);

            Complete(nextNode);
        }
    }
}