using UnityEngine;
using Game.Flow;

using System;

public class CobekHeld : Interactable, IFlowActivatable
{
    //[SerializeField]
    //private GameObject cobekSeasioning;

    public override void Interact(PlayerInteractor player)
    {
        FlowEventManager.Instance.Raise("Cobek_Held_Interacted");
        //cobekSeasioning?.SetActive(true);
    }


    // Aktivasi
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
