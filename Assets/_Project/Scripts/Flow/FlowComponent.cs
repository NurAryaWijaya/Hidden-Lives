using UnityEngine;

namespace Game.Flow
{
    public class FlowComponent : MonoBehaviour
    {
        [SerializeField]
        private string id;
        public string Id => id;
    }
}