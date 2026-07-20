using UnityEngine;
using Game.Flow;

public class LightActivatedManager : MonoBehaviour, IFlowActivatable
{
    [SerializeField] private GameObject[] switchObjects;

    private int interactableLayer;
    private int heldObjectLayer;

    private void Awake()
    {
        interactableLayer = LayerMask.NameToLayer("Interactable");
        heldObjectLayer = LayerMask.NameToLayer("HeldObject");
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    private void SetAllSwitchLayer(int layer)
    {
        foreach (GameObject obj in switchObjects)
        {
            if (obj != null)
            {
                SetLayerRecursively(obj, layer);
            }
        }
    }

    public void Activate()
    {
        if (interactableLayer == -1)
            return;

        SetAllSwitchLayer(interactableLayer);
    }

    public void Deactivate()
    {
        if (heldObjectLayer == -1)
            return;

        SetAllSwitchLayer(heldObjectLayer);
    }
}