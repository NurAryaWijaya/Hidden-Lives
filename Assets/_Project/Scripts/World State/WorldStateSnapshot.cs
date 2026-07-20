using System.Collections.Generic;

namespace Game.Flow
{
    [System.Serializable]
    public class WorldStateRecord
    {
        public string Id;

        public WorldState State;
    }

    [System.Serializable]
    public class WorldStateSnapshot
    {
        public List<WorldStateRecord> States = new();

        public List<string> CompletedCutscenes = new();
    }
}