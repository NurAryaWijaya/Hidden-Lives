using Game.Flow;
using UnityEngine;

public class PhoneTaken : Interactable, IFlowActivatable
{
    private bool isPhoneTaken = false;

    public bool IsPhoneTaken => isPhoneTaken;
    public override void Interact(PlayerInteractor player)
    {
        isPhoneTaken = true;
        Debug.Log("Phone Interacted!");
        FlowEventManager.Instance.Raise("Phone_Taken");
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
