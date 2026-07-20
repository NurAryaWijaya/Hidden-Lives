using UnityEngine;

namespace Game.Flow
{
    public class SpawnNode : FlowNode
    {
        [Header("Spawn")]
        [SerializeField]
        private string spawnId;

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 rotation;

        [SerializeField]
        private Transform parent;

        public override void Enter()
        {
            Register();

            SpawnManager.Instance.Spawn(
                spawnId,
                prefab,
                position,
                Quaternion.Euler(rotation),
                parent);

            WorldStateManager.Instance.SetSpawned(spawnId);

            Complete(nextNode);
        }

        public void Register()
        {
            SpawnManager.Instance.Register(
                spawnId,
                prefab,
                position,
                Quaternion.Euler(rotation),
                parent);
        }
    }
}