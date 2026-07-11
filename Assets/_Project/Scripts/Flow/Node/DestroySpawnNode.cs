using UnityEngine;

namespace Game.Flow
{
    public class DestroySpawnNode : FlowNode
    {
        [SerializeField]
        private string spawnId;

        public override void Enter()
        {
            SpawnManager.Instance.Destroy(spawnId);

            Complete(nextNode);
        }
    }
}