using System.Collections.Generic;

namespace Game.Flow
{
    public class WorldStateSnapshot
    {
        public Dictionary<string, WorldState> States = new();

        public HashSet<string> CompletedCutscenes = new();
    }
}