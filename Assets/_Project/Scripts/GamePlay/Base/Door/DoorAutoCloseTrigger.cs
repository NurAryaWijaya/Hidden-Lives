using UnityEngine;
using Game.Flow;


public class DoorAutoCloseTrigger : MonoBehaviour, IFlowActivatable
{
    [SerializeField] private DoorHouse2 door;
    [SerializeField] private Collider colliderTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.CloseDoor();
        }
    }

    public void Activate()
    {
        colliderTrigger.enabled = true;
    }
    public void Deactivate() 
    {
        colliderTrigger.enabled = false;
    }
}