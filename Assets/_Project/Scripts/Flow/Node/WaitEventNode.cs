using System.Collections.Generic;
using Game.Flow;
using UnityEngine;

public class WaitEventNode : FlowNode
{
    [Header("Event")]
    [SerializeField]
    private string eventId;

    public override void Enter()
    {
        if (string.IsNullOrWhiteSpace(eventId))
        {
            Debug.LogWarning($"{name} belum memiliki Event Id.");
            Complete(nextNode);
            return;
        }

        FlowEventManager.Instance.Subscribe(eventId, HandleEventRaised);
    }

    private void HandleEventRaised()
    {
        FlowEventManager.Instance.Unsubscribe(eventId, HandleEventRaised);

        Complete(nextNode);
    }
}

// Cara agar memanngil selesai dengan FlowEventManager.Instance.Raise("TaskCompleted"); pada script target