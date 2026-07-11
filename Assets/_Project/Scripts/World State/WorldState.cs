using System;

namespace Game.Flow
{
    [Serializable]
    public class WorldState
    {
        public bool Active = true;

        public bool Destroyed;

        public bool Spawned;

        public bool Completed;
    }
}