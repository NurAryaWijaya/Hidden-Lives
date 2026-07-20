using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    public class WorldStateManager : MonoBehaviour
    {
        public static WorldStateManager Instance { get; private set; }

        private readonly Dictionary<string, WorldState> states = new();

        private readonly HashSet<string> completedCutscenes = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #region State

        public void Save(string id, WorldState state)
        {
            states[id] = state;
        }

        public bool TryGet(string id, out WorldState state)
        {
            return states.TryGetValue(id, out state);
        }

        public bool Has(string id)
        {
            return states.ContainsKey(id);
        }

        public void Remove(string id)
        {
            states.Remove(id);
        }

        public void Clear()
        {
            states.Clear();
            completedCutscenes.Clear();
        }

        #endregion

        #region Helper

        private WorldState GetOrCreate(string id)
        {
            if (!states.TryGetValue(id, out WorldState state))
            {
                state = new WorldState();
                states.Add(id, state);
            }

            return state;
        }

        public void SetActive(string id, bool value)
        {
            GetOrCreate(id).Active = value;
        }

        public bool IsActive(string id)
        {
            return !states.TryGetValue(id, out WorldState state)
                || state.Active;
        }

        public void SetDestroyed(string id, bool value = true)
        {
            GetOrCreate(id).Destroyed = value;
        }

        public bool IsDestroyed(string id)
        {
            return states.TryGetValue(id, out WorldState state)
                && state.Destroyed;
        }

        public void SetSpawned(string id, bool value = true)
        {
            GetOrCreate(id).Spawned = value;
        }

        public bool IsSpawned(string id)
        {
            return states.TryGetValue(id, out WorldState state)
                && state.Spawned;
        }

        public void SetCompleted(string id, bool value = true)
        {
            GetOrCreate(id).Completed = value;
        }

        public bool IsCompleted(string id)
        {
            return states.TryGetValue(id, out WorldState state)
                && state.Completed;
        }

        #endregion

        #region Cutscene

        public void CompleteCutscene(string id)
        {
            completedCutscenes.Add(id);
        }

        public bool IsCutsceneCompleted(string id)
        {
            return completedCutscenes.Contains(id);
        }

        #endregion

        #region Apply

        public void Apply()
        {
            foreach ((string id, WorldState state) in states)
            {
                FlowComponent component =
                    FlowRegistry.Instance.Get<FlowComponent>(id);

                if (component == null)
                    continue;

                component.gameObject.SetActive(state.Active);
            }

            SpawnManager.Instance.Restore();
            RestoreCompletedCutscenes();
        }

        #endregion

        public WorldStateSnapshot CreateSnapshot()
        {
            WorldStateSnapshot snapshot = new();

            foreach ((string id, WorldState state) in states)
            {
                snapshot.States.Add(new WorldStateRecord
                {
                    Id = id,
                    State = new WorldState
                    {
                        Active = state.Active,
                        Destroyed = state.Destroyed,
                        Spawned = state.Spawned,
                        Completed = state.Completed
                    }
                });
            }

            snapshot.CompletedCutscenes.AddRange(completedCutscenes);

            return snapshot;
        }

        public void RestoreSnapshot(WorldStateSnapshot snapshot)
        {
            states.Clear();

            foreach (WorldStateRecord record in snapshot.States)
            {
                states.Add(record.Id, record.State);
            }

            completedCutscenes.Clear();

            foreach (string id in snapshot.CompletedCutscenes)
            {
                completedCutscenes.Add(id);
            }
        }

        private void RestoreCompletedCutscenes()
        {
            TimelineCutsceneController[] cutscenes =
                FindObjectsByType<TimelineCutsceneController>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None);

            foreach (TimelineCutsceneController controller in cutscenes)
            {
                if (!IsCutsceneCompleted(controller.Id))
                    continue;

                controller.RestoreCompleted();
            }
        }
    }
}