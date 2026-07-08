using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    public class FlowEventManager : MonoBehaviour
    {
        public static FlowEventManager Instance { get; private set; }

        private readonly Dictionary<string, Action> events =
            new Dictionary<string, Action>();
        private readonly Dictionary<string, bool> eventResults = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Subscribe(string eventId, Action callback)
        {
            if (!events.ContainsKey(eventId))
            {
                events[eventId] = callback;
            }
            else
            {
                events[eventId] += callback;
            }
        }

        public void Unsubscribe(string eventId, Action callback)
        {
            if (!events.ContainsKey(eventId))
                return;

            events[eventId] -= callback;

            if (events[eventId] == null)
            {
                events.Remove(eventId);
            }
        }

        public void Raise(string eventId)
        {
            if (events.TryGetValue(eventId, out Action callback))
            {
                callback?.Invoke();
            }
        }
    }
}