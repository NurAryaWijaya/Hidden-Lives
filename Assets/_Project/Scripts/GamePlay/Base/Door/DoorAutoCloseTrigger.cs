using UnityEngine;

public class DoorAutoCloseTrigger : MonoBehaviour
{
    [SerializeField] private DoorHouse2 door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.CloseDoor();
        }
    }
}