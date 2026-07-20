using Game.Flow;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public enum CarryState
{
    World,
    Carried,
    Placed
}

[RequireComponent(typeof(Rigidbody))]
public class PickupInteractable : Interactable, IFlowActivatable
{
    [Header("Pickup")]

    [SerializeField]
    private InteractableType interactableType;
    [SerializeField]
    private bool canBeRetrieved = true;
    [SerializeField]
    private bool interactionEnabled;

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

    public virtual void OnPickup(Transform holdPoint)
    {
        SetLayerRecursively(transform, heldLayer);
        SetCollidersEnabled(false);

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
        SetCollidersEnabled(true);

        IsPickedUp = false;

        transform.SetParent(null);

        rb.isKinematic = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public virtual void OnPlace(Transform placePoint, PlaceInteractable place)
    {
        SetLayerRecursively(transform, interactableLayer);
        SetCollidersEnabled(true);

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
        return interactionEnabled &&
               !IsPickedUp;
    }

    public void Activate()
    {
        interactionEnabled = true;
    }

    public void Deactivate()
    {
        interactionEnabled = false;
    }

    private void SetLayerRecursively(Transform root, int layer)
    {
        root.gameObject.layer = layer;

        foreach (Transform child in root)
        {
            SetLayerRecursively(child, layer);
        }
    }

    private void SetCollidersEnabled(bool enabled)
    {
        foreach (var col in colliders)
        {
            col.enabled = enabled;
        }
    }
}

//Kalau mau overide contoh base.OnPlace(placePoint, place);