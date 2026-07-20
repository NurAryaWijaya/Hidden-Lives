using Game.Flow;
using UnityEngine;

public class DoorHouse2 : Interactable, IFlowActivatable
{
    [Header("Door Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private Collider doorCollider;

    public bool IsOpened { get; private set; }

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    private void Start()
    {
        closedRotation = transform.localRotation;
        openedRotation = closedRotation * Quaternion.Euler(0f, 0f, openAngle);

        // Pastikan collider aktif saat awal
        if (doorCollider != null)
            doorCollider.enabled = true;
    }

    private void Update()
    {
        Quaternion target = IsOpened ? openedRotation : closedRotation;

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            target,
            openSpeed * Time.deltaTime
        );
    }

    public override void Interact(PlayerInteractor player)
    {
        IsOpened = !IsOpened;

        // Aktif/nonaktif collider sesuai status pintu
        if (doorCollider != null)
            doorCollider.enabled = !IsOpened;

        // Menunggu kondisi pemain menutup pintu di chapter 1
        if (IsOpened)
        {
            FlowEventManager.Instance.Raise("DoorHouse2_Opened");
        }
    }

    public void CloseDoor()
    {
        if (!IsOpened)
            return;

        IsOpened = false;

        if (doorCollider != null)
            doorCollider.enabled = true;

        FlowEventManager.Instance.Raise("Door_Closed_After_Take_Phone");

        Deactivate();
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void Activate()
    {
        int interactableLayer = LayerMask.NameToLayer("Interactable");

        if (interactableLayer == -1)
        {
            return;
        }

        SetLayerRecursively(gameObject, interactableLayer);
    }

    public void Deactivate()
    {
        int heldObjectLayer = LayerMask.NameToLayer("HeldObject");

        if (heldObjectLayer == -1)
        {
            return;
        }

        SetLayerRecursively(gameObject, heldObjectLayer);
    }
}