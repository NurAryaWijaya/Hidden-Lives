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

            CheckpointInfo info = new()
            {
                CheckpointId = checkpointId,
                SceneName = SceneManager.GetActiveScene().name,
                GraphId = FlowManager.Instance.CurrentGraph.GraphId,
                NodeId = NodeId,
                Snapshot = WorldStateManager.Instance.CreateSnapshot()
            };

            CheckpointManager.Instance.Register(info);

            SaveManager.Instance.SaveCheckpoint(info);

            Complete(nextNode);
        }
    }
}