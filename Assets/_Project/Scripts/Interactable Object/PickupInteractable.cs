using Unity.Multiplayer.PlayMode;
using UnityEngine;

public enum InteractableType
{
    None,
    Testing,
}

public enum CarryState
{
    World,
    Carried,
    Placed
}

[RequireComponent(typeof(Rigidbody))]
public class PickupInteractable : Interactable
{
    [Header("Pickup")]

    [SerializeField]
    private InteractableType interactableType;
    [SerializeField]
    private bool canBeRetrieved = true;

    public bool CanBeRetrieved => canBeRetrieved;
    public InteractableType Type => interactableType;
    public CarryState State { get; private set; }

    private int interactableLayer;
    private int heldLayer;
    private bool retrieved = true;
    private Rigidbody rb;
    private Collider[] colliders;
    private PlaceInteractable currentPlace;

    public bool IsPickedUp { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        interactableLayer = LayerMask.NameToLayer("Interactable");
        heldLayer = LayerMask.NameToLayer("HeldObject");

        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }

    public override void Interact(PlayerInteractor player)
    {
        if (IsPickedUp)
            return;
        if (!retrieved)
            return;

        player.PlayerCarry.Pickup(this);
    }

    public void OnPickup(Transform holdPoint)
    {
        SetLayerRecursively(transform, heldLayer);

        if (currentPlace != null)
        {
            currentPlace.RemoveObject();

            currentPlace = null;
        }
        IsPickedUp = true;

        Hide();

        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        rb.isKinematic = true;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDrop()
    {
        SetLayerRecursively(transform, interactableLayer);

        IsPickedUp = false;

        transform.SetParent(null);

        rb.isKinematic = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnPlace(Transform placePoint, PlaceInteractable place)
    {
        SetLayerRecursively(transform, interactableLayer);

        currentPlace = place;
        IsPickedUp = false;

        transform.SetParent(placePoint);

        transform.localPosition = Vector3.zero;

        transform.localRotation = Quaternion.identity;

        rb.isKinematic = true;

        retrieved = canBeRetrieved;

        if (canBeRetrieved)
        {
            SetLayerRecursively(transform, interactableLayer);
        }
        else
        {
            SetLayerRecursively(transform, heldLayer);
        }
    }

    public override bool CanInteract(PlayerInteractor player)
    {
        return !IsPickedUp;
    }

    private void SetLayerRecursively(Transform root, int layer)
    {
        root.gameObject.layer = layer;

        foreach (Transform child in root)
        {
            SetLayerRecursively(child, layer);
        }
    }
}