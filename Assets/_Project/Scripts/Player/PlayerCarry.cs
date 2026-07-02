using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [Header("Carry")]

    [SerializeField] private Transform holdPoint;
    [SerializeField] private Collider playerCollider;

    public PickupInteractable CurrentItem { get; private set; }

    public bool IsCarrying => CurrentItem != null;

    public bool Pickup(PickupInteractable item)
    {
        if (item == null)
            return false;

        if (IsCarrying)
            return false;

        IgnorePlayerCollision(item, true);

        CurrentItem = item;
        CurrentItem.OnPickup(holdPoint);

        return true;
    }

    public bool Drop()
    {
        if (!IsCarrying)
            return false;

        CurrentItem.OnDrop();

        IgnorePlayerCollision(CurrentItem, false);

        CurrentItem = null;

        return true;
    }

    public bool Place(PlaceInteractable place)
    {
        if (!IsCarrying)
            return false;

        if (place == null)
            return false;

        if (!place.PlaceObject(CurrentItem))
            return false;

        CurrentItem = null;

        return true;
    }

    private void IgnorePlayerCollision(PickupInteractable item, bool ignore)
    {
        Collider[] itemColliders = item.GetComponentsInChildren<Collider>();

        foreach (Collider itemCollider in itemColliders)
        {
            Physics.IgnoreCollision(playerCollider, itemCollider, ignore);
        }
    }
}