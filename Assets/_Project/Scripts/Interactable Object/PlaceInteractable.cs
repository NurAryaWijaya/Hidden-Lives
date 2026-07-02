using UnityEngine;

public class PlaceInteractable : Interactable
{
    [Header("Placement")]
    [SerializeField] private InteractableType acceptedType;
    [SerializeField] private Transform placePoint;

    private PickupInteractable placedObject;
    public bool HasObject => placedObject != null;

    public override void Interact(PlayerInteractor player)
    {
        // TEMPAT SUDAH TERISI
        if (HasObject)
        {
            return;
        }

        // PLAYER TIDAK MEMBAWA APA-APA

        PickupInteractable carried = player.PlayerCarry.CurrentItem;

        if (carried == null)
            return;

        // TYPE TIDAK SESUAI
        if (carried.Type != acceptedType)
            return;

        player.PlayerCarry.Place(this);
    }

    public bool PlaceObject(PickupInteractable item)
    {
        
        if (item == null)
            return false;

        // Jika tempat sudah terisi
        if (placedObject != null)
            return false;

        placedObject = item;

        item.OnPlace(placePoint, this);

        return true;
    }

    public PickupInteractable RemoveObject()
    {
        if (!HasObject)
            return null;

        PickupInteractable item = placedObject;

        placedObject = null;

        return item;
    }

    public override bool CanInteract(PlayerInteractor player)
    {
        // Sudah terisi
        if (HasObject)
        {
            return false; //placedObject.CanBeRetrieved;
        }

        PickupInteractable carried = player.PlayerCarry.CurrentItem;

        if (carried == null)
            return false;

        return carried.Type == acceptedType;
    }
}