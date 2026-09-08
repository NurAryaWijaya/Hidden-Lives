using Game.Flow;
using UnityEngine;

public class TriggerConflict : MonoBehaviour, IFlowActivatable
{
    [SerializeField] private Collider box;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FlowEventManager.Instance.Raise("ConflictTriggered");
            Deactivate();
        }
    }

    void Start()
    {
        box.enabled = false;
    }

    public void Activate()
    {
        box.enabled = true;
    }

    public void Deactivate()
    {
        box.enabled = false;
    }
}
