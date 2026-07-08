using UnityEngine;

namespace Game.Flow
{
    public interface IFlowActivatable
    {
        void Activate();
        void Deactivate();
    }
}